namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches.InteractiveCharts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes an Interactive Chart.
    /// </summary>
    public class DeleteDatasetPostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDatasetPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteDatasetPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a dataset post process.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or postprocess was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset locked.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeletePostProcessAndSecondariesWithLockAsync(
            int datasetPostProcessId,
            CustomSearchesDbContext dbContext,
            CloudBlobContainer blobContainer,
            ILogger logger)
        {
            DatasetPostProcess datasetPostProcess = await
                (from dp in dbContext.DatasetPostProcess where dp.DatasetPostProcessId == datasetPostProcessId select dp).
                Include(dp => dp.Dataset).
                    ThenInclude(d => d.ParentFolder).
                Include(dp => dp.PrimaryDatasetPostProcess).
                Include(dp => dp.InversePrimaryDatasetPostProcess).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(datasetPostProcess, "Dataset post process", datasetPostProcessId);

            if (datasetPostProcess.PrimaryDatasetPostProcessId != null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"The delete should be over the primary dataset post process.",
                    innerException: null);
            }

            Dataset dataset = datasetPostProcess.Dataset;
            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), datasetPostProcess.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "DeleteDatasetPostProcess");
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            List<DatasetPostProcess> datasetPostProcesses = new List<DatasetPostProcess>();
            datasetPostProcesses.Add(datasetPostProcess);
            datasetPostProcesses.AddRange(datasetPostProcess.InversePrimaryDatasetPostProcess);

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetPostProcess.DatasetId);

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

            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    await this.DeleteDatasetPostProcessesAsync(datasetPostProcesses, dbContext, bypassPostProcessBundleCheck: false, checkPostProcessStackOnPivot: true);
                    await this.DeleteDependenciesAsync(datasetPostProcesses, blobContainer);

                    await dbContext.SaveChangesAsync();

                    // Executes all dataset post processes regenerating only the view.
                    DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData
                    {
                        OnlyView = true,
                        DatasetId = dataset.DatasetId
                    };

                    ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
                    await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { logger.LogInformation($"DeleteDatasetPostProcess: {message}"); });

                    datasetPostProcessState = DatasetHelper.CalculateDatasetPostProcessState(dataset.DatasetPostProcess);
                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);
        }

        /// <summary>
        /// Deletes the dataset post processes.
        /// </summary>
        /// <param name="datasetPostProcesses">The dataset post processes.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        /// <param name="checkPostProcessStackOnPivot">Value indicating whether the stack check is over the current dataset or should be over the pivot.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or postprocess was not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset locked.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteDatasetPostProcessesAsync(
            List<DatasetPostProcess> datasetPostProcesses,
            CustomSearchesDbContext dbContext,
            bool bypassPostProcessBundleCheck,
            bool checkPostProcessStackOnPivot)
        {
            var datasetIds = datasetPostProcesses.Select(dpp => dpp.DatasetId);
            if (datasetIds.Count() > 0)
            {
                await DatasetHelper.LoadDatasetsWithDependenciesAsync(
                    dbContext,
                    datasetIds.ToArray(),
                    includeRelatedExpressions: true,
                    includeParentFolder: true,
                    includeInverseSourceDatasets: false,
                    includeUserProject: false,
                    includeDatasetUserClientState: false);
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            foreach (var currentPostProcess in datasetPostProcesses)
            {
                await this.DeleteDatasetPostProcessAsync(currentPostProcess, currentPostProcess.Dataset, userId, dbContext, bypassPostProcessBundleCheck, checkPostProcessStackOnPivot);
            }
        }

        /// <summary>
        /// Deletes a dataset post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="userId">The user id.</param
        /// <param name="dbContext">The database context.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        /// <param name="checkPostProcessStackOnPivot">Value indicating whether the stack check is over the current dataset or should be over the pivot.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteDatasetPostProcessAsync(
            DatasetPostProcess datasetPostProcess,
            Dataset dataset,
            Guid userId,
            CustomSearchesDbContext dbContext,
            bool bypassPostProcessBundleCheck,
            bool checkPostProcessStackOnPivot)
        {
            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);
            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            await projectBusinessLogic.ValidateDeletePostProcessAsync(datasetPostProcess, bypassPostProcessBundleCheck, checkPostProcessStackOnPivot);

            var columnsToValidate = CustomSearchExpressionEvaluator.GetCalculatedColumnNames(datasetPostProcess);
            int datasetPostProcessPriority = datasetPostProcess.Priority;

            foreach (var rule in datasetPostProcess.ExceptionPostProcessRule)
            {
                dbContext.CustomSearchExpression.RemoveRange(rule.CustomSearchExpression);
            }

            datasetPostProcess.PrimaryDatasetPostProcess = null;
            dbContext.CustomSearchExpression.RemoveRange(datasetPostProcess.CustomSearchExpression);
            dbContext.ExceptionPostProcessRule.RemoveRange(datasetPostProcess.ExceptionPostProcessRule);
            dbContext.DatasetPostProcess.Remove(datasetPostProcess);

            foreach (var currentDatasetPostProcess in dataset.DatasetPostProcess)
            {
                if ((currentDatasetPostProcess.IsDirty == false) && (currentDatasetPostProcess.Priority > datasetPostProcessPriority))
                {
                    currentDatasetPostProcess.LastModifiedBy = userId;
                    currentDatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                    currentDatasetPostProcess.IsDirty = true;
                    dbContext.DatasetPostProcess.Update(currentDatasetPostProcess);
                }
            }

            // Validates if the expressions can be removed.
            dataset.DatasetPostProcess.Remove(datasetPostProcess);
            if (dataset.DatasetPostProcess.Count == 0)
            {
                dataset.DataSetPostProcessState = DatasetPostProcessStateType.NotProcessed.ToString();
            }

            var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

            CustomSearchExpressionEvaluator.AssertExpressionReferencesToColumnsAreValid(dataset, dbColumns, columnsToValidate);
        }

        /// <summary>
        /// Drops dataset post process database tables and rscript files.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteDependenciesAsync(DatasetPostProcess datasetPostProcess, CloudBlobContainer blobContainer)
        {
            await this.DropPostProcessTablesAsync(datasetPostProcess.Dataset, datasetPostProcess.DatasetPostProcessId);
            await this.RemovePostProcessRScriptFilesAsync(datasetPostProcess.Dataset, datasetPostProcess, blobContainer);
        }

        /// <summary>
        /// Drops dataset post processes tables and rscript files.
        /// </summary>
        /// <param name="datasetPostProcesses">The dataset post processes.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteDependenciesAsync(List<DatasetPostProcess> datasetPostProcesses, CloudBlobContainer blobContainer)
        {
            foreach (var datasetPostProcess in datasetPostProcesses)
            {
                await this.DeleteDependenciesAsync(datasetPostProcess, blobContainer);
            }
        }

        /// <summary>
        /// Drops dataset post process tables in database.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DropPostProcessTablesAsync(Dataset dataset, int datasetPostProcessId)
        {
            string postprocessTable = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(dataset, datasetPostProcessId);
            string datasetPostProcessView = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess: true);
            string script = $"IF OBJECT_ID('{postprocessTable}') IS NOT NULL BEGIN DROP TABLE {postprocessTable} END;\n";
            script += DatasetHelper.GetDeleteBackendUpdateScript(datasetPostProcessId);

            if (dataset.DatasetPostProcess.Count == 0)
            {
                script += $"IF OBJECT_ID('{datasetPostProcessView}') IS NOT NULL BEGIN DROP VIEW {datasetPostProcessView} END;\n";
            }

            await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                parameters: null);
        }

        /// <summary>
        /// Removes post process rscript files.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="blobContainer">The data database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RemovePostProcessRScriptFilesAsync(Dataset dataset, DatasetPostProcess datasetPostProcess, CloudBlobContainer blobContainer)
        {
            if ((datasetPostProcess.RscriptModelId != null) && (datasetPostProcess.RscriptModelId > 0))
            {
                var continuationToken = new BlobContinuationToken();
                do
                {
                    string folderPath = $"{GetDatasetFileService.BlobResultsFolderName}/{dataset.DatasetId}/{datasetPostProcess.DatasetPostProcessId}".ToLower();
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
            }
        }
    }
}
