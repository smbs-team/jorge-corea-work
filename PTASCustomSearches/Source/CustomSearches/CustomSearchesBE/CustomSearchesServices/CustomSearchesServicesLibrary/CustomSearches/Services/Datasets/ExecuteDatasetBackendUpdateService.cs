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
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Models;

    /// <summary>
    /// Service that performs a backend update for a dataset.
    /// </summary>
    public class ExecuteDatasetBackendUpdateService : ExecuteBaseBackendUpdateService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteDatasetBackendUpdateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExecuteDatasetBackendUpdateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets or sets the backend entities dictionary.
        /// </summary>
        private Dictionary<string, CustomSearchBackendEntity> BackendEntitiesDictionary { get; set; }

        /// <summary>
        /// Gets or sets the dataset.
        /// </summary>
        private Dataset Dataset { get; set; }

        /// <summary>
        /// Queues the dataset backend update execution.
        /// </summary>
        /// <param name="datasetId">Dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="workerJobDbContext">The worker job database context.</param>
        /// <returns>
        /// The id of the update job.  -1 If no update job was started.
        /// </returns>
        /// <exception cref="CustomSearchesConflictException">Dataset has validation errors or dataset is used by a worker job.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Update statement failed in the database while getting Dataset validations status.</exception>
        public async Task<int> QueueExecuteDatasetBackendUpdateAsync(
            Guid datasetId,
            CustomSearchesDbContext dbContext,
            WorkerJobDbContext workerJobDbContext)
        {
            DatasetBackendUpdatePayloadData payload = new DatasetBackendUpdatePayloadData();
            payload.DatasetId = datasetId;
            return await this.QueueExecuteBackendUpdateAsync(payload, dbContext, workerJobDbContext);
        }

        /// <summary>
        /// Executes the dataset backend update.
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

            using (CustomSearchesDbContext dbContext = this.ServiceContext.DbContextFactory.Create())
            {
                List<EntityChanges> changesList = await this.GatherEntityChangesAsync(payload, dbContext, logAction);

                await this.ApplyToDynamicsAsync(changesList, dbContext, dynamicsODataHelper, logAction);
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
        /// <exception cref="CustomSearchesConflictException">Dataset has validation errors or dataset is used by a worker job.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Update statement failed in the database while getting Dataset validations status.</exception>
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

            if (await this.HasValidationErrorsAsync(dataset))
            {
                throw new CustomSearchesConflictException(
                  $"Dataset with id '{dataset.DatasetId}' has validation errors.",
                  null);
            }

            var editableColumns = await DatasetColumnHelper.GetEditableColumnsAsync(dataset, dbContext);
            var backendEntities = await this.GetBackendEntitiesAsync(dataset, dbContext);

            // Don't update if there are no editable columns or no back-end entities in the dataset
            if (editableColumns.Count == 0 || backendEntities.Count == 0)
            {
                return false;
            }

            await DatasetHelper.TestAlterDatasetLockAsync(
                dataset.DatasetId,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                isRootLock: false,
                this.ServiceContext.AuthProvider.UserInfoData.Id,
                lockingJobId: null,
                dbContext);

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
            string targetTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(this.Dataset);
            string sourceTableName = CustomSearchesDataDbContext.GetDatasetTableFullName(this.Dataset);

            foreach (var change in committedChanges)
            {
                string fieldKey = this.BackendEntitiesDictionary[change.EntityName].CustomSearchKeyFieldName;

                string exportStatus = hasError ? ExecuteBaseBackendUpdateService.ExportFailedState : ExecuteBaseBackendUpdateService.ExportSuccessfulState;
                string script =
                    $"UPDATE U " +
                    $"SET [BackendExportState] = '{exportStatus}', ExportedToBackEndErrorMessage = @errorMessage " +
                    $"FROM {targetTableName} U " +
                    $"INNER JOIN {sourceTableName} S ON U.[CustomSearchResultId] = S.[CustomSearchResultId] " +
                    $"WHERE S.[{fieldKey}] = '{change.EntityId}'";

                await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    script,
                    new SqlParameter[] { new SqlParameter("@errorMessage", errorMessage ?? string.Empty) },
                    "Update statement failed in the database while updating Dataset export status.");
            }
        }

        /// <summary>
        /// Gathers the dataset changes from the updates array.
        /// </summary>
        /// <param name="payload">The payload with dataset information.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>A list of entity changes.</returns>
        protected override async Task<List<EntityChanges>> GatherEntityChangesInternalAsync(DatasetBackendUpdatePayloadData payload, CustomSearchesDbContext dbContext, Action<string> logAction)
        {
            this.Dataset = await dbContext.Dataset.Where(d => (d.DatasetId == payload.DatasetId)).FirstOrDefaultAsync();

            if (this.Dataset == null)
            {
                throw new CustomSearchesDatabaseException(string.Format("Dataset not found : '{0}'.", payload.DatasetId), null);
            }

            // Get information related to dynamics schema.
            var editableColumns = await DatasetColumnHelper.GetEditableColumnsAsync(this.Dataset, dbContext);
            var backendEntities = await this.GetBackendEntitiesAsync(this.Dataset, dbContext);

            if (editableColumns.Count == 0 || backendEntities.Count == 0)
            {
                logAction.Invoke("No changes. Finished dataset backend update execution...");
                return null;
            }

            string getDatasetUpdatesScript = this.GetDatasetUpdatesScript(backendEntities, editableColumns);

            // Get updates from the database
            dynamic[] updates = (await DbTransientRetryPolicy.GetDynamicResultAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                getDatasetUpdatesScript)).ToArray();

            // Reformat the changes to send to dynamics.
            this.BackendEntitiesDictionary = (from be in backendEntities select be).ToDictionary(be => be.BackendEntityName, be => be);

            var editableColumnsDictionary = (from ec in editableColumns select ec).ToDictionary(ec => ec.ColumnName, ec => ec);

            var entityChangesDictionary = new Dictionary<string, EntityChanges>();

            foreach (var update in updates)
            {
                var updateDictionary = update as IDictionary<string, object>;

                foreach (var columnName in updateDictionary.Keys)
                {
                    var columnDefinition = editableColumnsDictionary.GetValueOrDefault(columnName);

                    // If there is no column definition or no mapping for the update... Skip.
                    if (columnDefinition == null ||
                        string.IsNullOrWhiteSpace(columnDefinition.BackendEntityName) ||
                        string.IsNullOrWhiteSpace(columnDefinition.BackendEntityFieldName))
                    {
                        continue;
                    }

                    var backendEntity = this.BackendEntitiesDictionary.GetValueOrDefault(columnDefinition.BackendEntityName);

                    // If there is backend entity for the update... Skip.
                    if (backendEntity == null)
                    {
                        continue;
                    }

                    object value = updateDictionary[columnName];

                    // If there is no value... Skip.
                    if (value == null || value.GetType() == typeof(System.DBNull))
                    {
                        continue;
                    }

                    var backendEntityKey = updateDictionary[backendEntity.CustomSearchKeyFieldName];

                    // If there is no backend entity for the update... Skip.
                    if (backendEntityKey == null || string.IsNullOrWhiteSpace(backendEntityKey.ToString()))
                    {
                        continue;
                    }

                    string entityAndKey = $"{backendEntity.BackendEntityName}_{backendEntityKey.ToString()}";

                    var entityChanges = entityChangesDictionary.GetValueOrDefault(entityAndKey);
                    if (entityChanges == null)
                    {
                        entityChanges = new EntityChanges
                        {
                            EntityId = backendEntityKey.ToString(),
                            EntityName = backendEntity.BackendEntityName,
                            Changes = new Dictionary<string, object>()
                        };

                        entityChangesDictionary[entityAndKey] = entityChanges;
                    }

                    entityChanges.Changes[columnDefinition.BackendEntityFieldName] = value;
                }
            }

            return entityChangesDictionary.Values.ToList();
        }

        /// <summary>
        /// Gets the backend entities for the dataset.
        /// </summary>
        /// <param name="dataset">The Dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The list of editable columns.</returns>
        private async Task<List<CustomSearchBackendEntity>> GetBackendEntitiesAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            return await (from be in dbContext.CustomSearchBackendEntity
                          where be.CustomSearchDefinitionId == dataset.CustomSearchDefinitionId
                          select be).ToListAsync();
        }

        /// <summary>
        /// Gets the script that returns dataset updates.
        /// </summary>
        /// <param name="backendEntities">The backend entities.</param>
        /// <param name="editableColumns">The editable columns.</param>
        /// <returns>
        /// The script that returns dataset updates.
        /// </returns>
        private string GetDatasetUpdatesScript(
            List<CustomSearchBackendEntity> backendEntities,
            List<CustomSearchColumnDefinition> editableColumns)
        {
            var updateTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableName(this.Dataset);
            var tableName = CustomSearchesDataDbContext.GetDatasetTableName(this.Dataset);
            var schema = CustomSearchesDataDbContext.CustomSearchesSchema;
            string keyFieldList = string.Empty;
            string editableFieldList = string.Empty;
            var keyFields = new HashSet<string>();

            // Generate key field list.
            foreach (var backendEntity in backendEntities)
            {
                var columnDefinition =
                    (from ec in editableColumns
                     where ec.ColumnName.ToLower() == backendEntity.CustomSearchKeyFieldName.ToLower()
                     select ec).FirstOrDefault();

                var colName = backendEntity.CustomSearchKeyFieldName;
                keyFields.Add(colName);
                if (columnDefinition != null)
                {
                    keyFieldList += $" IIF(U.{colName} IS NULL, S.{colName}, U.{colName}) AS {colName},";
                }
                else
                {
                    keyFieldList += $" S.{colName},";
                }
            }

            // Generate update field list.
            foreach (var editableColumn in editableColumns)
            {
                var colName = editableColumn.ColumnName;
                if (!keyFields.Contains(colName))
                {
                    editableFieldList += $" U.{colName},";
                }
            }

            keyFieldList = keyFieldList.TrimEnd(new char[] { ',', ' ' });
            editableFieldList = editableFieldList.TrimEnd(new char[] { ',', ' ' });

            return $"SELECT {editableFieldList}, {keyFieldList} " +
                $"FROM {schema}.{updateTableName} U " +
                $"INNER JOIN {schema}.{tableName} S " +
                $"ON U.CustomSearchResultId = S.CustomSearchResultId " +
                $"WHERE U.IsValid = 1 AND U.Validated = 1 AND U.BackendExportState != '{ExecuteBaseBackendUpdateService.ExportSuccessfulState}'";
        }

        /// <summary>
        /// Determines whether the dataset has validation errors.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>True if the dataset has validation errors.</returns>
        /// <exception cref="CustomSearchesDatabaseException">Update statement failed in the database while getting Dataset validations status.</exception>
        private async Task<bool> HasValidationErrorsAsync(Dataset dataset)
        {
            string script =
                $"SELECT Count(*) " +
                $"FROM {CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset)} " +
                $"WHERE IsValid = 0 OR Validated = 0";

            return (int)await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                "Update statement failed in the database while getting Dataset validations status.") > 0;
        }
    }
}