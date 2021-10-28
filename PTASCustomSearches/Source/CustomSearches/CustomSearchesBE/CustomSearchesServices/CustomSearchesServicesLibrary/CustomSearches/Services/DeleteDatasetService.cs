namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes a dataset and all its child objects (including charts and DB tables).
    /// </summary>
    public class DeleteDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a dataset and all its child objects (including charts and DB tables).
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter dataset or it is used by a project.</exception>
        public async Task DeleteDatasetWithLockAsync(Guid datasetId, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            Dataset dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                dbContext,
                datasetId,
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: true,
                includeUserProject: false,
                includeDatasetUserClientState: true);

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            List<int> postProcessIds = (from pp in dataset.DatasetPostProcess select pp.DatasetPostProcessId).ToList();
            var inversePrimaryPostProcessesCount =
                 await (from dpp in dbContext.DatasetPostProcess
                        where dpp.PrimaryDatasetPostProcessId != null && postProcessIds.Contains((int)dpp.PrimaryDatasetPostProcessId)
                        select dpp).CountAsync();

            if (inversePrimaryPostProcessesCount > 0)
            {
                throw new CustomSearchesRequestBodyException(
                    $"The dataset shouldn't contain primary post processes.",
                    innerException: null);
            }

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            if (userProject != null)
            {
                ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
                var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
                var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId);

                if (pivotDataset.DatasetId == datasetId)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"The base dataset can't be deleted from the user project.",
                        innerException: null);
                }
            }

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "DeleteDataset");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    await this.DeleteDataset(dataset, deleteDatasetRow: true, dbContext, blobContainer);
                    await dbContext.SaveChangesAsync();
                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext,
                allowFailed: true);
        }

        /// <summary>
        /// Deletes a dataset and all its child objects (including charts and DB tables).
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="deleteDatasetRow">Value indicating whether should be deleted the dataset row.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteDataset(Dataset dataset, bool deleteDatasetRow, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            if (dataset.InverseSourceDataset.Count > 0)
            {
                throw new CustomSearchesConflictException($"Dataset with id '{dataset.DatasetId}' is being used by a project.", null);
            }

            using (var gisDbContext = this.ServiceContext.GisDbContextFactory.Create())
            {
                var mapRendererIdHashSet =
                    (await gisDbContext.MapRenderer.Where(m => m.DatasetId == dataset.DatasetId).Select(d => d.MapRendererId).ToListAsync()).ToHashSet();
                if (mapRendererIdHashSet.Count > 0)
                {
                    string mapRendererIds = string.Join(", ", mapRendererIdHashSet.Select(v => $"'{v}'"));
                    throw new CustomSearchesConflictException(
                        $"Dataset with id '{dataset.DatasetId}' is being used by the following map renderer(s): {mapRendererIds}.",
                        null);
                }
            }

            dbContext.DatasetUserClientState.RemoveRange(dataset.DatasetUserClientState);
            dbContext.CustomSearchExpression.RemoveRange(dataset.CustomSearchExpression);

            if (dataset.DatasetPostProcess != null)
            {
                var continuationToken = new BlobContinuationToken();
                do
                {
                    string folderPath = $"{GetDatasetFileService.BlobResultsFolderName}/{dataset.DatasetId}".ToLower();
                    var result = await blobContainer.ListBlobsSegmentedAsync(
                        folderPath,
                        useFlatBlobListing: true,
                        BlobListingDetails.None,
                        maxResults: null,
                        continuationToken,
                        options: null,
                        operationContext: null);

                    continuationToken = result.ContinuationToken;
                    await Task.WhenAll(result.Results
                        .Select(item => (item as CloudBlob)?.DeleteIfExistsAsync())
                        .Where(task => task != null));
                }
                while (continuationToken != null);

                foreach (var datasetPostProcess in dataset.DatasetPostProcess)
                {
                    dbContext.CustomSearchExpression.RemoveRange(datasetPostProcess.CustomSearchExpression);

                    if (datasetPostProcess.ExceptionPostProcessRule != null)
                    {
                        foreach (var exceptionPostProcessRule in datasetPostProcess.ExceptionPostProcessRule)
                        {
                            dbContext.CustomSearchExpression.RemoveRange(exceptionPostProcessRule.CustomSearchExpression);
                        }

                        datasetPostProcess.PrimaryDatasetPostProcess = null;
                        dbContext.ExceptionPostProcessRule.RemoveRange(datasetPostProcess.ExceptionPostProcessRule);
                    }
                }

                dbContext.DatasetPostProcess.RemoveRange(dataset.DatasetPostProcess);
            }

            if (dataset.InteractiveChart != null)
            {
                foreach (var interactiveChart in dataset.InteractiveChart)
                {
                    dbContext.CustomSearchExpression.RemoveRange(interactiveChart.CustomSearchExpression);
                }

                dbContext.InteractiveChart.RemoveRange(dataset.InteractiveChart);
            }

            Folder folder = dataset.ParentFolder;
            if (folder != null)
            {
                folder.Dataset.Remove(dataset);
                CustomSearchFolderType folderType = Enum.Parse<CustomSearchFolderType>(folder.FolderType, ignoreCase: true);
                DatasetFolderManager folderManager = new DatasetFolderManager(folderType, dataset.UserId, dbContext);
                await folderManager.RemovesUnusedFoldersAsync(folder);
            }

            if (dataset.UserProjectDataset.Count > 0)
            {
                dbContext.UserProjectDataset.RemoveRange(dataset.UserProjectDataset);
            }

            await DbTransientRetryPolicy.DropDatasetTablesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                dataset);

            if (deleteDatasetRow)
            {
                dbContext.Dataset.Remove(dataset);
            }
        }
    }
}
