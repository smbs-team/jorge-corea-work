namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches.InteractiveCharts;
    using CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that imports the RScript post process.
    /// </summary>
    public class ImportRScriptPostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRScriptPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportRScriptPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the RScript post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The post process id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportRScriptPostProcessAsync(DatasetPostProcessData datasetPostProcessData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertZero(datasetPostProcessData.DatasetPostProcessId, nameof(DatasetPostProcess), nameof(datasetPostProcessData.DatasetPostProcessId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.DatasetId, nameof(datasetPostProcessData.DatasetId));
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.PostProcessName, nameof(datasetPostProcessData.PostProcessName));
            InputValidationHelper.AssertShouldBeGreaterThan(datasetPostProcessData.RscriptModelId ?? 0, 0, nameof(datasetPostProcessData.RscriptModelId));

            List<Guid> secondaryDatasetIds = new List<Guid>();
            List<Guid> datasetIds = new List<Guid>();
            datasetIds.Add(datasetPostProcessData.DatasetId);
            if (datasetPostProcessData.SecondaryDatasets?.Length > 0)
            {
                datasetIds.AddRange(datasetPostProcessData.SecondaryDatasets);
                secondaryDatasetIds.AddRange(datasetPostProcessData.SecondaryDatasets);
            }

            var datasets = await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                dbContext,
                datasetIds.ToArray(),
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: false,
                includeDatasetUserClientState: false);

            var primaryDataset = datasets.FirstOrDefault(d => d.DatasetId == datasetPostProcessData.DatasetId);

            InputValidationHelper.AssertEntityExists(primaryDataset, nameof(Dataset), datasetPostProcessData.DatasetId);
            InputValidationHelper.AssertNotEmpty(datasetPostProcessData.CustomSearchExpressions, nameof(datasetPostProcessData.CustomSearchExpressions));
            InputValidationHelper.AssertEmpty(datasetPostProcessData.ExceptionPostProcessRules, nameof(datasetPostProcessData.ExceptionPostProcessRules));

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(primaryDataset, dbContext);

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            projectBusinessLogic.ValidateImportPostProcess(datasetPostProcessData, datasets);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(primaryDataset.UserId, primaryDataset.ParentFolder, primaryDataset.IsLocked, userProject, "ImportRScriptPostProcess");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            var model = await dbContext.RscriptModel.Where(m => m.RscriptModelId == datasetPostProcessData.RscriptModelId).FirstOrDefaultAsync();
            InputValidationHelper.AssertEntityExists(model, nameof(RscriptModel), datasetPostProcessData.RscriptModelId);

            List<DatasetPostProcess> primaryAndSecondariesPostProcesses =
                await DatasetHelper.LoadPrimaryAndSecondariesPostProcessesAsync(userProject, primaryDataset, datasetPostProcessData, DatasetPostProcessType.RScriptPostProcess, dbContext);
            List<DatasetPostProcess> importedPostProcesses = new List<DatasetPostProcess>();

            DatasetPostProcess primaryDatasetPostProcess = null;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    foreach (var currentDataset in datasets)
                    {
                        datasetPostProcessData.DatasetId = currentDataset.DatasetId;

                        var datasetPostProcess = primaryAndSecondariesPostProcesses.FirstOrDefault(dpp => dpp.DatasetId == currentDataset.DatasetId);

                        if (datasetPostProcess != null)
                        {
                            primaryAndSecondariesPostProcesses.Remove(datasetPostProcess);
                        }

                        datasetPostProcess = await this.ImportRScriptPostProcessAsync(primaryDataset, datasetPostProcessData,  datasetPostProcess, dbContext);

                        datasetPostProcess.PrimaryDatasetPostProcess = primaryDatasetPostProcess;
                        if (primaryDatasetPostProcess == null)
                        {
                            primaryDatasetPostProcess = datasetPostProcess;
                        }

                        importedPostProcesses.Add(datasetPostProcess);
                    }

                    DeleteDatasetPostProcessService service = new DeleteDatasetPostProcessService(this.ServiceContext);
                    CloudBlobContainer blobContainer =
                        await this.ServiceContext.CloudStorageProvider.GetCloudBlobContainer(ImportExceptionPostProcessService.RScriptBlobContainerName, this.ServiceContext.AppCredential);

                    // Remove unused post processes.
                    await service.DeleteDatasetPostProcessesAsync(primaryAndSecondariesPostProcesses, dbContext, bypassPostProcessBundleCheck: true, checkPostProcessStackOnPivot: false);
                    await service.DeleteDependenciesAsync(primaryAndSecondariesPostProcesses, blobContainer);

                    await dbContext.ValidateAndSaveChangesAsync();
                    datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                    return (datasetState, datasetPostProcessState);
                },
                primaryDataset,
                isRootLock: false,
                primaryDataset.DataSetState,
                primaryDataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);

            return new IdResult(primaryDatasetPostProcess.DatasetPostProcessId);
        }

        /// <summary>
        /// Imports the RScript post process.
        /// </summary>
        /// <param name="primaryDataset">The primary dataset for validations.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The post process.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<DatasetPostProcess> ImportRScriptPostProcessAsync(
            Dataset primaryDataset,
            DatasetPostProcessData datasetPostProcessData,
            DatasetPostProcess datasetPostProcess,
            CustomSearchesDbContext dbContext)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            IEnumerable<string> columnsToValidate = null;

            if (datasetPostProcess != null)
            {
                columnsToValidate = CustomSearchExpressionEvaluator.GetCalculatedColumnNames(datasetPostProcess);
                dbContext.CustomSearchExpression.RemoveRange(datasetPostProcess.CustomSearchExpression);
                datasetPostProcess.CustomSearchExpression.Clear();
                dbContext.DatasetPostProcess.Update(datasetPostProcess);
            }
            else
            {
                datasetPostProcess = new DatasetPostProcess()
                {
                    DatasetId = datasetPostProcessData.DatasetId,
                    PostProcessName = datasetPostProcessData.PostProcessName,
                    PostProcessRole = datasetPostProcessData.PostProcessRole,
                    PostProcessType = DatasetPostProcessType.RScriptPostProcess.ToString(),
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    ExecutionOrder = int.MaxValue,

                    // Priority is only supported on creation. Priority updates could break column expressions.
                    Priority = datasetPostProcessData.Priority
                };

                dbContext.DatasetPostProcess.Add(datasetPostProcess);
            }

            datasetPostProcess.IsDirty = true;
            datasetPostProcess.PostProcessDefinition = datasetPostProcessData.PostProcessDefinition;
            datasetPostProcess.RscriptModelId = datasetPostProcessData.RscriptModelId;
            datasetPostProcess.LastModifiedBy = userId;
            datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;

            for (int i = 0; i < datasetPostProcessData.CustomSearchExpressions.Length; i++)
            {
                var customSearchExpressionData = datasetPostProcessData.CustomSearchExpressions[i];
                CustomSearchExpressionRoleType roleType =
                    InputValidationHelper.ValidateEnum<CustomSearchExpressionRoleType>(customSearchExpressionData.ExpressionRole, nameof(customSearchExpressionData.ExpressionRole));

                if (primaryDataset.DatasetId == datasetPostProcessData.DatasetId ||
                    (roleType == CustomSearchExpressionRoleType.CalculatedColumnPostCommit) ||
                    (roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent) ||
                    (roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent) ||
                    (roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit))
                {
                    CustomSearchExpression customSearchExpression = new CustomSearchExpression()
                    {
                        ColumnName = customSearchExpressionData.ColumnName,
                        ExpressionRole = customSearchExpressionData.ExpressionRole,
                        ExpressionType = customSearchExpressionData.ExpressionType,
                        OwnerType = CustomSearchExpressionOwnerType.DatasetPostProcess.ToString(),
                        Script = customSearchExpressionData.Script,
                        ExecutionOrder = i
                    };

                    datasetPostProcess.CustomSearchExpression.Add(customSearchExpression);
                    customSearchExpression.DatasetPostProcess = datasetPostProcess;
                }
            }

            ImportRScriptModelService.ValidateExpressions(datasetPostProcess.CustomSearchExpression);

            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                datasetPostProcess.CustomSearchExpression.ToList(),
                this.ServiceContext,
                primaryDataset,
                DatasetHelper.GetPreviousDatasetPostProcess(primaryDataset, datasetPostProcess),
                datasetPostProcess,
                chartTypeContext: null,
                throwOnFail: true);

            // Validates if the expressions can be removed.
            if ((columnsToValidate != null) && (columnsToValidate.Count() > 0))
            {
                var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, primaryDataset);
                CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(primaryDataset, dbColumns, columnsToValidate);
            }

            return datasetPostProcess;
        }

        /// <summary>
        /// Imports the exception post process.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public async Task<DatasetPostProcessData> GetRScriptPostProcess(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            var query = dbContext.DatasetPostProcess as IQueryable<DatasetPostProcess>;
            DatasetPostProcess datasetPostProcess =
                await query.Where(d => d.DatasetId == datasetId).Include(d => d.CustomSearchExpression).FirstOrDefaultAsync();

            DatasetPostProcessData datasetPostProcessData =
                new DatasetPostProcessData(datasetPostProcess, ModelInitializationType.FullObject, userDetails: null);

            CustomSearchExpression[] customSearchExpressions = datasetPostProcess.CustomSearchExpression.ToArray();
            datasetPostProcessData.CustomSearchExpressions = new CustomSearchExpressionData[customSearchExpressions.Length];
            for (int j = 0; j < customSearchExpressions.Length; j++)
            {
                var item = customSearchExpressions[j];
                datasetPostProcessData.CustomSearchExpressions[j] =
                    new CustomSearchExpressionData(item, ModelInitializationType.FullObject);
            }

            return datasetPostProcessData;
        }
    }
}
