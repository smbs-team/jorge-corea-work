namespace CustomSearchesServicesLibrary.CustomSearches.Services.Datasets
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Models;
    using PTASServicesCommon.Telemetry;

    /// <summary>
    /// Base service to execute a backend update.
    /// </summary>
    public abstract class ExecuteBaseBackendUpdateService : BaseService
    {
        /// <summary>
        /// State indicating the export was successful.
        /// </summary>
        public const string ExportSuccessfulState = "ExportSuccessful";

        /// <summary>
        /// State indicating the export was failed.
        /// </summary>
        public const string ExportFailedState = "ExportFailed";

        /// <summary>
        /// The update batch size (entities sent per batch request).
        /// </summary>
        private const int UpdateBatchSize = 1;

        /// <summary>
        /// Number of retries before failing an update.
        /// </summary>
        private const int DynamicsUpdateRetries = 3;

        /// <summary>
        /// The dataset backend update job type.
        /// </summary>
        private const string DatasetBackendUpdateJobType = "DatasetBackendUpdateJobType";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteBaseBackendUpdateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExecuteBaseBackendUpdateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Executes the post process backend update.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task ExecuteBackendUpdateAsync(
            DatasetBackendUpdatePayloadData payload,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            await TelemetryHelper.TrackPerformanceAsync(
                "ExecuteBackendUpdate",
                async () =>
                {
                    await this.ExecuteBackendUpdateInternalAsync(payload, dynamicsODataHelper, logAction);

                    return null;
                },
                this.ServiceContext.TelemetryClient,
                new Dictionary<string, string>()
                {
                    { "datasetId", payload.DatasetId.ToString() },
                    { "datasetPostProcessId", payload.DatasetPostProcessId.ToString() },
                    { "major", payload.SingleRowExecutionData.Major },
                    { "minor", payload.SingleRowExecutionData.Minor },
                });
        }

        /// <summary>
        /// Executes the backend update.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public abstract Task ExecuteBackendUpdateInternalAsync(
            DatasetBackendUpdatePayloadData payload,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction);

        /// <summary>
        /// Queues the backend update execution.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="workerJobDbContext">The worker job database context.</param>
        /// <returns>
        /// The id of the update job.  -1 If no update job was started.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        protected async Task<int> QueueExecuteBackendUpdateAsync(
            DatasetBackendUpdatePayloadData payload,
            CustomSearchesDbContext dbContext,
            WorkerJobDbContext workerJobDbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == payload.DatasetId)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            if (!await this.PrevalidateExecuteBackendUpdateAsync(dataset, payload, dbContext, workerJobDbContext))
            {
                return -1;
            }

            return await this.ServiceContext.AddWorkerJobQueueAsync(
                payload.SingleRowExecutionData.IsSingleRowExecutionMode ? "FastQueue" : "DatasetBackendUpdate",
                "DatasetBackendUpdateJobType",
                this.ServiceContext.AuthProvider.UserInfoData.Id,
                payload,
                WorkerJobTimeouts.DatasetBackendUpdateTimeout);
        }

        /// <summary>
        /// Prevalidates the backend update execution.
        /// </summary>
        /// <param name="dataset">The Dataset.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="workerJobDbContext">The worker job database context.</param>
        /// <returns>
        /// True if the validation was successful.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        protected virtual async Task<bool> PrevalidateExecuteBackendUpdateAsync(
            Dataset dataset,
            DatasetBackendUpdatePayloadData payload,
            CustomSearchesDbContext dbContext,
            WorkerJobDbContext workerJobDbContext)
        {
            InputValidationHelper.AssertEntityExists(dataset, nameof(Dataset), dataset.DatasetId);

            // Avoid these validations if it is running a single row execution.
            if (!payload.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

                this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ExecuteDatasetBackendUpdate");

                // Avoid running this job if there is another back-end update for this dataset.
                var possibleCollisions =
                    (from wj in workerJobDbContext.WorkerJobQueue
                     where wj.JobType == DatasetBackendUpdateJobType &&
                       string.IsNullOrEmpty(wj.JobResult) &&
                       !string.IsNullOrWhiteSpace(wj.JobPayload)
                     select wj).ToList();

                foreach (var possibleCollision in possibleCollisions)
                {
                    try
                    {
                        var payloadData = JsonConvert.DeserializeObject<DatasetBackendUpdatePayloadData>(possibleCollision.JobPayload);
                        if (payloadData.Equals(payload))
                        {
                            this.Logger.LogWarning($"DatasetBackendUpdate not queued for datasetId {dataset.DatasetId}.  Backend update already queued.");

                            return false;
                        }
                    }
                    catch (Newtonsoft.Json.JsonException)
                    {
                        // We don't care about serialization exceptions here.  If a job is corrupt we shouldn't stop the new job to launch.
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gathers the entity changes from database.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>A list of entity changes.</returns>
        protected async Task<List<EntityChanges>> GatherEntityChangesAsync(DatasetBackendUpdatePayloadData payload, CustomSearchesDbContext dbContext, Action<string> logAction)
        {
            var properties = new Dictionary<string, string>()
                {
                    { "datasetId", payload.DatasetId.ToString() },
                    { "datasetPostProcessId", payload.DatasetPostProcessId.ToString() },
                    { "major", payload.SingleRowExecutionData.Major },
                    { "minor", payload.SingleRowExecutionData.Minor },
                };

            List<EntityChanges> entityChanges = null;
            await TelemetryHelper.TrackPerformanceAsync(
                "GatherEntityChanges",
                async () =>
                {
                    entityChanges = await this.GatherEntityChangesInternalAsync(payload, dbContext, logAction);

                    properties.Add("entityChangesCount", entityChanges?.Count.ToString());
                    return null;
                },
                this.ServiceContext.TelemetryClient,
                properties);

            return entityChanges;
        }

        /// <summary>
        /// Gathers the entity changes from database.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>A list of entity changes.</returns>
        protected abstract Task<List<EntityChanges>> GatherEntityChangesInternalAsync(DatasetBackendUpdatePayloadData payload, CustomSearchesDbContext dbContext, Action<string> logAction);

        /// <summary>
        /// Updates the custom search export status in the BackendUpdate entities.
        /// </summary>
        /// <param name="committedChanges">The committed changes.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="hasError">Indicates whether there has been an error when updating dynamics.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The Task.</returns>
        protected abstract Task UpdateExportStatusAsync(
            List<EntityChanges> committedChanges,
            CustomSearchesDbContext dbContext,
            bool hasError,
            string errorMessage);

        /// <summary>
        /// Applies the changes to dynamics.
        /// </summary>
        /// <param name="changesList">The change list.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>The task.</returns>
        protected async Task ApplyToDynamicsAsync(
            List<EntityChanges> changesList,
            CustomSearchesDbContext dbContext,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            await TelemetryHelper.TrackPerformanceAsync(
                "ApplyToDynamics",
                async () =>
                {
                    await this.ApplyToDynamicsInternalAsync(changesList, dbContext, dynamicsODataHelper, logAction);
                    return null;
                },
                this.ServiceContext.TelemetryClient,
                new Dictionary<string, string>()
                {
                    { "changesListCount", changesList?.Count.ToString() },
                });
        }

        /// <summary>
        /// Applies the changes to dynamics.
        /// </summary>
        /// <param name="changesList">The change list.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>The task.</returns>
        private async Task ApplyToDynamicsInternalAsync(
            List<EntityChanges> changesList,
            CustomSearchesDbContext dbContext,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            while (changesList != null && changesList.Count > 0)
            {
                List<EntityChanges> changesToSend =
                    (from ec in changesList select ec).
                    Take(ExecuteDatasetBackendUpdateService.UpdateBatchSize).ToList();

                changesList.RemoveRange(0, Math.Min(ExecuteDatasetBackendUpdateService.UpdateBatchSize, changesList.Count));

                logAction.Invoke($"Sending batch update. Batch count: {changesToSend.Count}, Remaining Count: {changesList.Count}.");

                int retries = 0;
                bool success = false;
                while (!success)
                {
                    try
                    {
                        await this.ApplyToDynamicsOperationAsync(changesToSend, dynamicsODataHelper);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        logAction.Invoke($"Error updating dynamics entity batch. Error: {ex}.");
                        retries++;
                        if (retries >= ExecuteDatasetBackendUpdateService.DynamicsUpdateRetries)
                        {
                            await this.UpdateExportStatusAsync(
                                changesToSend,
                                dbContext,
                                hasError: true,
                                errorMessage: ex.ToString());

                            throw;
                        }
                        else
                        {
                            logAction.Invoke($"Retrying entity update...");
                            await Task.Delay(1000 * retries);
                        }
                    }
                }

                await this.UpdateExportStatusAsync(
                    changesToSend,
                    dbContext,
                    hasError: false,
                    errorMessage: null);

                logAction.Invoke($"Committed batch update. Batch count: {changesToSend.Count}, Remaining Count: {changesList.Count}.");
            }
        }

        /// <summary>
        /// Applies the changes to dynamics operation.
        /// </summary>
        /// <param name="changesToSend">The changes to send.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <returns>The task.</returns>
        private async Task ApplyToDynamicsOperationAsync(
            List<EntityChanges> changesToSend,
            GenericDynamicsHelper dynamicsODataHelper)
        {
            await TelemetryHelper.TrackPerformanceAsync(
                "ApplyToDynamicsOperation",
                async () =>
                {
                    await dynamicsODataHelper.ApplyToDynamics(changesToSend, skipUserFields: true, this.ServiceContext.AuthProvider.AzureActiveDirectoryObjectId);
                    return null;
                },
                this.ServiceContext.TelemetryClient,
                new Dictionary<string, string>()
                {
                    { "changesToSendCount", changesToSend?.Count.ToString() },
                },
                telemetryChannel: "Details");
        }
    }
}