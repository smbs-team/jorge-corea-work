namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Executor;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that executes a custom search.
    /// </summary>
    public class ExecuteDatasetPostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteDatasetPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExecuteDatasetPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Queues the dataset post process generation.
        /// </summary>
        /// <param name="datasetId">Dataset id.</param>
        /// <param name="datasetPostProcessId">Dataset post process id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="parameters">Custom search parameters.</param>
        /// <param name="dataStream">The data to upload.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset is used by a worker job.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<IdResult> QueueExecuteDatasetPostProcessAsync(
            Guid datasetId,
            int datasetPostProcessId,
            string major,
            string minor,
            CustomSearchParameterValueData[] parameters,
            Stream dataStream,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetPostProcessExecutionPayloadData payload = new DatasetPostProcessExecutionPayloadData();
            payload.DatasetPostProcessId = datasetPostProcessId;
            payload.DatasetId = datasetId;
            payload.Parameters = parameters;
            payload.SingleRowExecutionData.Major = major;
            payload.SingleRowExecutionData.Minor = minor;

            if (dataStream != null)
            {
                byte[] bytes = new byte[dataStream.Length];
                await dataStream.ReadAsync(bytes);
                payload.AdditionalData = System.Convert.ToBase64String(bytes);
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

                this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ExecuteDatasetPostProcess");

                await DatasetHelper.TestAlterDatasetLockAsync(
                    dataset.DatasetId,
                    dataset.DataSetState,
                    dataset.DataSetPostProcessState,
                    isRootLock: false,
                    userId,
                    lockingJobId: null,
                    dbContext);

                if (datasetPostProcessId >= 0)
                {
                    var datasetPostProcess = await dbContext.DatasetPostProcess
                        .Where(d => (d.DatasetPostProcessId == datasetPostProcessId) && (d.DatasetId == datasetId))
                        .FirstOrDefaultAsync();

                    InputValidationHelper.AssertEntityExists(datasetPostProcess, "Dataset post process", datasetPostProcessId);

                    ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
                    var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
                    projectBusinessLogic.ValidateExecutePostProcess(datasetPostProcess);

                    datasetPostProcess.ParameterValues = JsonHelper.SerializeObject(payload.Parameters);
                    datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                    datasetPostProcess.LastModifiedBy = userId;
                    dbContext.DatasetPostProcess.Update(datasetPostProcess);
                    await dbContext.ValidateAndSaveChangesAsync();
                }
            }

            return new IdResult(await this.ServiceContext.AddWorkerJobQueueAsync(
                payload.SingleRowExecutionData.IsSingleRowExecutionMode ? "FastQueue" : "DatasetPostProcessExecution",
                "DatasetPostProcessExecutionJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetPostProcessExecutionTimeout));
        }

        /// <summary>
        /// Executes the dataset post process.
        /// </summary>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// A unique Id that can be used to get the database data.
        /// </returns>
        public async Task ExecuteDatasetPostProcessAsync(DatasetPostProcessExecutionPayloadData payload, CustomSearchesDbContext dbContext, Action<string> logAction)
        {
            logAction.Invoke("Starting post process execution...");

            var userProject = await DatasetHelper.GetOwnerProjectFromDbContextAsync(payload.DatasetId, dbContext);
            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();

            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);

            // Replicated post process runs as a multi dataset pipeline executing all the primary and secondary post processes in the user project.
            // Non replicated post process only executes the post processes in its own dataset.
            bool useMultiDatasetPipeline = true;
            if (payload.DatasetPostProcessId > 0)
            {
                var datasetPostProcess = await dbContext.DatasetPostProcess
                    .Where(d => (d.DatasetPostProcessId == payload.DatasetPostProcessId) && (d.DatasetId == payload.DatasetId))
                    .FirstOrDefaultAsync();

                useMultiDatasetPipeline = projectBusinessLogic.UseMultiDatasetPipeline(datasetPostProcess.PostProcessRole);
            }

            var dataset = useMultiDatasetPipeline ?
                await projectBusinessLogic.LoadPivotDatasetAsync(payload.DatasetId) :
                await dbContext.Dataset
                .Where(d => d.DatasetId == payload.DatasetId)
                .Include(d => d.DatasetPostProcess)
                .Include(d => d.UserProjectDataset)
                .FirstOrDefaultAsync();

            if (dataset != null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

                var datasetPostProcesses = dataset.DatasetPostProcess.OrderBy(d => d.Priority);

                Dictionary<Dataset, string> datasetViewScripts = new Dictionary<Dataset, string>();

                DatasetPostProcess firstPostProcessToCommit = null;

                int order = 0;
                foreach (var referencePostProcess in datasetPostProcesses)
                {
                    List<DatasetPostProcess> primaryAndSecondariesPostProcesses = useMultiDatasetPipeline ?
                        await DatasetHelper.LoadPrimaryAndSecondariesPostProcessesAsync(referencePostProcess.DatasetPostProcessId, dbContext) :
                        new List<DatasetPostProcess> { referencePostProcess };

                    foreach (var currentPostProcess in primaryAndSecondariesPostProcesses)
                    {
                        if (order == 0)
                        {
                            if (currentPostProcess.Dataset == null)
                            {
                                await dbContext.Entry(currentPostProcess).Reference(dpp => dpp.Dataset).LoadAsync();
                            }

                            datasetViewScripts[currentPostProcess.Dataset] = string.Empty;

                            // TODO: Remove this condition when the lock of multiple datasets is implemented.
                            if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode && (currentPostProcess.Dataset.DatasetId == payload.DatasetId))
                            {
                                currentPostProcess.Dataset.DataSetState = DatasetStateType.ExecutingPostProcess.ToString();
                                currentPostProcess.Dataset.LastModifiedTimestamp = DateTime.UtcNow;
                                currentPostProcess.Dataset.LastModifiedBy = userId;
                                dbContext.Dataset.Update(currentPostProcess.Dataset);
                                await dbContext.ValidateAndSaveChangesAsync();
                            }
                        }

                        string viewScript = datasetViewScripts[currentPostProcess.Dataset];

                        // If all postprocesses are not dirty, any postprocess before payload.DatasetPostProcessId will be not fully execute (only view will be updated).
                        // All postprocesses after and including payload.DatasetPostProcessId will be fully executed.
                        // Also any dirty postprocesses will get fully executed (and any other postprocess after it).
                        // This behavior can be overriden by the payload.OnlyView flag. If this flag is true views will be update but postprocesses will not be fully executed.
                        // If payload.DatasetPostProcessId is -1 the algorithm will use the first post process instead.
                        bool needCommitNewState = false;

                        if (!payload.OnlyView)
                        {
                            bool containsPayloadDatasetPostProcessId =
                                primaryAndSecondariesPostProcesses.FirstOrDefault(dpp => dpp.DatasetPostProcessId == payload.DatasetPostProcessId) != null;

                            if ((firstPostProcessToCommit == null)
                                && (currentPostProcess.IsDirty || (payload.DatasetPostProcessId == -1) || containsPayloadDatasetPostProcessId))
                            {
                                firstPostProcessToCommit = referencePostProcess;
                            }

                            if (currentPostProcess.IsDirty
                                || (payload.DatasetPostProcessId == -1)
                                || ((firstPostProcessToCommit != null)
                                    && ((firstPostProcessToCommit == referencePostProcess) || (referencePostProcess.Priority > firstPostProcessToCommit.Priority))))
                            {
                                needCommitNewState = true;
                            }

                            currentPostProcess.IsDirty = false;
                        }

                        datasetViewScripts[currentPostProcess.Dataset] =
                            await this.ExecuteDatasetPostProcessAsync(currentPostProcess, viewScript, needCommitNewState, order, payload, dbContext, logAction);
                    }

                    order++;
                }

                if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    foreach (var currentDataset in datasetViewScripts.Keys)
                    {
                        currentDataset.DataSetState = DatasetStateType.Processed.ToString();
                        currentDataset.DataSetPostProcessState = DatasetHelper.CalculateDatasetPostProcessState(currentDataset.DatasetPostProcess);
                        currentDataset.LastModifiedTimestamp = DateTime.UtcNow;
                        currentDataset.LastModifiedBy = userId;
                        dbContext.Dataset.Update(currentDataset);
                    }

                    await dbContext.ValidateAndSaveChangesAsync();
                }

                logAction.Invoke("Finished post process execution...");
            }
            else
            {
                throw new CustomSearchesDatabaseException(string.Format("Dataset not found : '{0}'.", payload.DatasetId), null);
            }
        }

        /// <summary>
        /// Executes the dataset post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="viewScript">The post process executor view script.</param>
        /// <param name="needCommitNewState">Value indicating whether the post process needs to commit the new state.</param>
        /// <param name="executionOrder">The post process execution order.</param>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// The view script of the dataset post process executor.
        /// </returns>
        public async Task<string> ExecuteDatasetPostProcessAsync(
            DatasetPostProcess datasetPostProcess,
            string viewScript,
            bool needCommitNewState,
            int executionOrder,
            DatasetPostProcessExecutionPayloadData payload,
            CustomSearchesDbContext dbContext,
            Action<string> logAction)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            try
            {
                logAction.Invoke(string.Format("Starting {0} with Id: {1}...", datasetPostProcess.PostProcessType, datasetPostProcess.DatasetPostProcessId));

                DatasetPostProcessExecutorFactory datasetPostProcessExecutorFactory = new DatasetPostProcessExecutorFactory();
                DatasetPostProcessExecutor datasetPostProcessExecutor =
                    datasetPostProcessExecutorFactory.CreatePostProcessExecutor(datasetPostProcess.Dataset, datasetPostProcess, viewScript, payload, payload.SingleRowExecutionData, dbContext, this.ServiceContext);

                logAction.Invoke("Calculating pre-commit view...");
                await datasetPostProcessExecutor.CalculateViewAsync(PostProcessViewPhase.PreCommit);
                logAction.Invoke("Commiting pre-commit view...");
                await datasetPostProcessExecutor.CommitViewAsync(PostProcessViewPhase.PreCommit);

                if (needCommitNewState)
                {
                    string previousResultPayload = datasetPostProcess.ResultPayload;

                    if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
                    {
                        // We reset the result payload because commit new state async may modify the payload and we need to detect if it did.
                        datasetPostProcess.ResultPayload = null;
                    }

                    logAction.Invoke("Deleting previous state...");
                    await datasetPostProcessExecutor.DeletePreviousStateAsync();
                    logAction.Invoke("Commiting new state...");
                    await datasetPostProcessExecutor.CommitNewStateAsync();

                    // If payload was not modified by commit new state we assign a success state.
                    if (string.IsNullOrWhiteSpace(datasetPostProcess.ResultPayload))
                    {
                        SucceededJobResult succeededJobResult = new SucceededJobResult
                        {
                            Status = "Success"
                        };

                        datasetPostProcess.ResultPayload = JsonHelper.SerializeObject(succeededJobResult);
                    }
                    else
                    {
                        var jobResult = JsonHelper.DeserializeObject<SucceededJobResult>(datasetPostProcess.ResultPayload);
                        if (jobResult == null || jobResult?.Status.ToLower() != "success")
                        {
                            datasetPostProcess.IsDirty = true;
                        }
                    }
                }

                logAction.Invoke("Calculating post-commit view...");
                await datasetPostProcessExecutor.CalculateViewAsync(PostProcessViewPhase.PostCommit);
                logAction.Invoke("Commiting post-commit view...");
                await datasetPostProcessExecutor.CommitViewAsync(PostProcessViewPhase.PostCommit);

                if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    datasetPostProcess.LastModifiedBy = userId;
                    datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                    datasetPostProcess.LastExecutionTimestamp = DateTime.UtcNow;
                    datasetPostProcess.ExecutionOrder = executionOrder;
                    dbContext.DatasetPostProcess.Update(datasetPostProcess);
                    await dbContext.ValidateAndSaveChangesAsync();
                }

                return datasetPostProcessExecutor.ViewScript;
            }
            catch (Exception ex)
            {
                if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    try
                    {
                        // If we failed updating the post process payload we don't want to mask the original exception.
                        FailedJobResult failedJobResult = new FailedJobResult
                        {
                            Reason = ex.GetBaseException().Message,
                            Status = "Failed",
                            Exception = ex.ToString()
                        };

                        datasetPostProcess.ResultPayload = JsonHelper.SerializeObject(failedJobResult);

                        datasetPostProcess.IsDirty = true;
                        datasetPostProcess.LastModifiedBy = userId;
                        datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                        dbContext.DatasetPostProcess.Update(datasetPostProcess);
                        await dbContext.ValidateAndSaveChangesAsync();
                    }
                    catch
                    {
                    }
                }

                throw;
            }
        }
    }
}