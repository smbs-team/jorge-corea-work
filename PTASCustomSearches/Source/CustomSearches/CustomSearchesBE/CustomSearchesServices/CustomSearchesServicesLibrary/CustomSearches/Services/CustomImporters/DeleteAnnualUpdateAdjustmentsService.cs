namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes the annual update adjustments post processes.
    /// </summary>
    public class DeleteAnnualUpdateAdjustmentsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAnnualUpdateAdjustmentsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteAnnualUpdateAdjustmentsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the annual update adjustments post processes.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task DeleteAnnualUpdateAdjustmentsAsync(
            Guid datasetId,
            CustomSearchesDbContext dbContext,
            ILogger logger)
        {
            var userProject =
                await (from up in dbContext.UserProject
                       join upd in dbContext.UserProjectDataset
                            on up.UserProjectId equals upd.UserProjectId
                       join d in dbContext.Dataset
                            on upd.DatasetId equals d.DatasetId
                       where upd.OwnsDataset == true && upd.DatasetId == datasetId
                       select up).
                       Include(up => up.ProjectType).
                    FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, nameof(userProject), datasetId, $"The dataset '{datasetId}' doesn't belong to a user project.");

            var datasetPostProcesses =
                await (from up in dbContext.UserProject
                       join upd in dbContext.UserProjectDataset
                            on up.UserProjectId equals upd.UserProjectId
                       join d in dbContext.Dataset
                            on upd.DatasetId equals d.DatasetId
                       join dpp in dbContext.DatasetPostProcess
                            on d.DatasetId equals dpp.DatasetId
                       where upd.OwnsDataset == true &&
                        upd.UserProjectId == userProject.UserProjectId &&
                        AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder.Contains(dpp.PostProcessRole)
                       select dpp).
                    ToListAsync();

            InputValidationHelper.AssertNotEmpty(datasetPostProcesses, nameof(datasetPostProcesses), $"The dataset '{datasetId}' doesn't have annual update adjustments post processes.");

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);

            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId);

            var primaryDatasetPostProcess = datasetPostProcesses.Where(dpp => dpp.DatasetId == datasetId).FirstOrDefault();
            var primaryDataset = primaryDatasetPostProcess.Dataset;

            List<Guid> datasetIds = new List<Guid>();
            datasetIds.Add(pivotDataset.DatasetId);
            datasetIds.AddRange(datasetPostProcesses.Where(dpp => dpp.DatasetId != pivotDataset.DatasetId).Select(dpp => dpp.DatasetId));

            List<Dataset> datasets = await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                dbContext,
                datasetIds.ToArray(),
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: true,
                includeDatasetUserClientState: false);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(primaryDataset.UserId, primaryDataset.ParentFolder, primaryDataset.IsLocked, userProject, "DeleteAnnualUpdateAdjustments");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    CloudBlobContainer blobContainer =
                        await this.ServiceContext.CloudStorageProvider.GetCloudBlobContainer(ImportExceptionPostProcessService.RScriptBlobContainerName, this.ServiceContext.AppCredential);
                    DeleteDatasetPostProcessService service = new DeleteDatasetPostProcessService(this.ServiceContext);

                    int annualUpdateAdjustmentsLength = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder.Count;
                    for (int i = annualUpdateAdjustmentsLength - 1; i >= 0; i--)
                    {
                        string roleName = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder[i];
                        var currentPostProcesses =
                            datasetPostProcesses.Where(dpp => dpp.PostProcessRole.Trim().ToLower() == roleName).OrderBy(dpp => dpp.DatasetId == pivotDataset.DatasetId).ToList();

                        await service.DeleteDatasetPostProcessesAsync(currentPostProcesses, dbContext, bypassPostProcessBundleCheck: true, checkPostProcessStackOnPivot: true);
                    }

                    await service.DeleteDependenciesAsync(datasetPostProcesses, blobContainer);

                    await dbContext.SaveChangesAsync();

                    // Executes all dataset post processes regenerating only the view.
                    DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData
                    {
                        OnlyView = true,
                        DatasetId = datasetId
                    };

                    ExecuteDatasetPostProcessService executeService = new ExecuteDatasetPostProcessService(this.ServiceContext);
                    await executeService.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"DeleteDatasetPostProcess: {message}"); });

                    datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                    return (datasetState, datasetPostProcessState);
                },
                pivotDataset,
                isRootLock: false,
                pivotDataset.DataSetState,
                pivotDataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);
        }
    }
}
