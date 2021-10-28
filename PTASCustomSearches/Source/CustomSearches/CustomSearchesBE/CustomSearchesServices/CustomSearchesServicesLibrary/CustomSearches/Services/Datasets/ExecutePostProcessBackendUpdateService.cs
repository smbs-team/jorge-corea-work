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
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Models;

    /// <summary>
    /// Service that performs a post process backend update.
    /// </summary>
    public class ExecutePostProcessBackendUpdateService : ExecuteBaseBackendUpdateService
    {
        /// <summary>
        /// The backend batch size.
        /// </summary>
        private const int BackendBatchSize = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutePostProcessBackendUpdateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExecutePostProcessBackendUpdateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets or sets the dictionary mapping the entity changes with the current batch of backend updates.
        /// </summary>
        private Dictionary<EntityChanges, BackendUpdate> EntityChangeBackendUpdateMap { get; set; }

        /// <summary>
        /// Gets or sets the current batch of the backend updates.
        /// </summary>
        private List<BackendUpdate> BackendUpdates { get; set; }

        /// <summary>
        /// Queues the post process backend update execution.
        /// </summary>
        /// <param name="datasetId">Dataset id.</param>
        /// <param name="postProcessId">Dataset post process id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="workerJobDbContext">The worker job database context.</param>
        /// <returns>
        /// The id of the update job.  -1 If no update job was started.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<int> QueueExecutePostProcessBackendUpdateAsync(
            Guid datasetId,
            int postProcessId,
            string major,
            string minor,
            CustomSearchesDbContext dbContext,
            WorkerJobDbContext workerJobDbContext)
        {
            DatasetBackendUpdatePayloadData payload = new DatasetBackendUpdatePayloadData();
            payload.DatasetId = datasetId;
            payload.DatasetPostProcessId = postProcessId;
            payload.SingleRowExecutionData.Major = major;
            payload.SingleRowExecutionData.Minor = minor;
            return await this.QueueExecuteBackendUpdateAsync(payload, dbContext, workerJobDbContext);
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
        public override async Task ExecuteBackendUpdateInternalAsync(
            DatasetBackendUpdatePayloadData payload,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            logAction.Invoke("Starting dataset backend update execution...");

            while (true)
            {
                using (CustomSearchesDbContext dbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    List<EntityChanges> changesList = await this.GatherEntityChangesAsync(payload, dbContext, logAction);
                    if (changesList.Count == 0)
                    {
                        break;
                    }

                    try
                    {
                        await this.ApplyToDynamicsAsync(changesList, dbContext, dynamicsODataHelper, logAction);
                    }
                    catch (Exception)
                    {
                        // If the apply to dynamics fails we cleanup the backend updates without error because they are no longer needed.
                        try
                        {
                            string script = DatasetHelper.GetDeleteBackendUpdateScript(
                                payload.DatasetPostProcessId,
                                payload.SingleRowExecutionData.Major,
                                payload.SingleRowExecutionData.Minor,
                                excludeFailed: true);

                            await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                                this.ServiceContext,
                                this.ServiceContext.DataDbContextFactory,
                                script,
                                parameters: null);
                        }
                        catch (Exception)
                        {
                            // We don't care if we fail the cleanup. If the cleanup fails we don't want to hide the original exception.
                        }

                        throw;
                    }

                    dbContext.BackendUpdate.RemoveRange(this.BackendUpdates.Where(bu => bu.ExportState == null));
                    await dbContext.SaveChangesAsync();
                }
            }

            logAction.Invoke("Finished dataset backend update execution...");
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
        protected override async Task<bool> PrevalidateExecuteBackendUpdateAsync(
            Dataset dataset,
            DatasetBackendUpdatePayloadData payload,
            CustomSearchesDbContext dbContext,
            WorkerJobDbContext workerJobDbContext)
        {
            if (!await base.PrevalidateExecuteBackendUpdateAsync(dataset, payload, dbContext, workerJobDbContext))
            {
                return false;
            }

            DatasetPostProcess datasetPostProcess = await dbContext.DatasetPostProcess
                .Where(d =>
                            d.DatasetPostProcessId == payload.DatasetPostProcessId &&
                            d.DatasetId == payload.DatasetId &&
                            d.PostProcessType.ToLower() == DatasetPostProcessType.StoredProcedureUpdatePostProcess.ToString().ToLower())
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(datasetPostProcess, nameof(DatasetPostProcess), payload.DatasetPostProcessId);
            return true;
        }

        /// <summary>
        /// Updates the custom search export status in the BackendUpdate entities.
        /// </summary>
        /// <param name="committedChanges">The committed changes.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="hasError">Indicates whether there has been an error when updating dynamics.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The Task.</returns>
        protected override async Task UpdateExportStatusAsync(
            List<EntityChanges> committedChanges,
            CustomSearchesDbContext dbContext,
            bool hasError,
            string errorMessage)
        {
            foreach (var committedChange in committedChanges)
            {
                var backendUpdate = this.EntityChangeBackendUpdateMap[committedChange];
                if (hasError)
                {
                    backendUpdate.ExportState = ExecuteBaseBackendUpdateService.ExportFailedState;
                    backendUpdate.ExportError = $"Failed exporting the entity with id '{committedChange.EntityId}': {errorMessage}";
                }
            }

            if (hasError)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gathers the entity changes from the dataset post process backend updates.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>A list of entity changes.</returns>
        protected override async Task<List<EntityChanges>> GatherEntityChangesInternalAsync(DatasetBackendUpdatePayloadData payload, CustomSearchesDbContext dbContext, Action<string> logAction)
        {
            this.EntityChangeBackendUpdateMap = new Dictionary<EntityChanges, BackendUpdate>();
            this.BackendUpdates =
                await (from dpp in dbContext.DatasetPostProcess
                       join bu in dbContext.BackendUpdate
                            on dpp.DatasetPostProcessId equals bu.DatasetPostProcessId
                       where dpp.DatasetId == payload.DatasetId &&
                        dpp.DatasetPostProcessId == payload.DatasetPostProcessId &&
                        bu.SingleRowMajor == payload.SingleRowExecutionData.Major &&
                        bu.SingleRowMinor == payload.SingleRowExecutionData.Minor &&
                        bu.ExportState == null
                       select bu).Take(ExecutePostProcessBackendUpdateService.BackendBatchSize).ToListAsync();

            logAction.Invoke($"Gathered BackendUpdates batch from Database. Batch size: {this.BackendUpdates.Count}.");

            List<EntityChanges> entityChangesList = new List<EntityChanges>();
            foreach (var backendUpdate in this.BackendUpdates)
            {
                if (string.IsNullOrWhiteSpace(backendUpdate.UpdatesJson))
                {
                    continue;
                }

                var updatesJsonArray = JsonHelper.DeserializeObject<JArray>(backendUpdate.UpdatesJson);

                foreach (var updatesJsonToken in updatesJsonArray)
                {
                    JObject updatesJsonObject = updatesJsonToken as JObject;
                    if (updatesJsonObject != null)
                    {
                        string entityName = (string)updatesJsonObject["entityName"];
                        string entityKeyName = (string)updatesJsonObject["entityKeyName"];

                        if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(entityKeyName) || !updatesJsonObject.ContainsKey("entities"))
                        {
                            continue;
                        }

                        JArray entities = (JArray)updatesJsonObject["entities"];

                        Dictionary<string, string> backendKeyNameDictionary = new Dictionary<string, string>();

                        foreach (var entityToken in entities)
                        {
                            var entityChanges = new EntityChanges
                            {
                                EntityName = entityName,
                                Changes = new Dictionary<string, object>(),
                            };

                            JObject entityObject = entityToken as JObject;
                            foreach (var property in entityObject.Properties())
                            {
                                var propertyName = property.Name;
                                var propertyValue = property.Value;

                                // If there is no value... Skip.
                                if (propertyValue == null || propertyValue.GetType() == typeof(System.DBNull) || string.IsNullOrWhiteSpace(propertyName))
                                {
                                    continue;
                                }

                                if (propertyName.Trim().ToLower() == entityKeyName.ToLower())
                                {
                                    entityChanges.EntityId = property.Value.ToString();
                                }
                                else
                                {
                                    entityChanges.Changes[propertyName] = propertyValue.ToObject<object>();
                                }
                            }

                            if (entityChanges.Changes.Count == 0)
                            {
                                continue;
                            }

                            this.EntityChangeBackendUpdateMap[entityChanges] = backendUpdate;
                            entityChangesList.Add(entityChanges);
                        }
                    }
                }
            }

            return entityChangesList;
        }
    }
}