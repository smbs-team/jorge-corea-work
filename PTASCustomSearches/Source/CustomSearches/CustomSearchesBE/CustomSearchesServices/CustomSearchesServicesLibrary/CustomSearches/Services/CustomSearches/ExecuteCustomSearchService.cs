namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Models;

    /// <summary>
    /// Service that executes a custom search.
    /// </summary>
    public class ExecuteCustomSearchService : BaseService
    {
        /// <summary>
        /// Gets the string format for custom search template table name where 0 is CustomSearchDefinitionId and 1 is Version.
        /// </summary>
        private const string CustomSearchTemplateTableFormat = "[cus].[DatasetTemplate_{0}_{1}]";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteCustomSearchService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ExecuteCustomSearchService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Queues the dataset generation.
        /// </summary>
        /// <param name="customSearchDefinitionId">Custom search definition id.</param>
        /// <param name="customSearchData">Custom search data.</param>
        /// <param name="validate">Value indicating whether the custom search should be validated.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="assignFolder">Value indicating whether the dataset should be assigned to a folder.</param>
        /// <param name="checkExecutionRoles">Value indicating whether the user should be authorized for execution.</param>
        /// <returns>
        /// A unique Id that can be used to get the database data.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="FolderManagerException">Invalid folder path format or folder not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<ExecuteCustomSearchResponse> QueueExecuteCustomSearchAsync(
            int customSearchDefinitionId,
            ExecuteCustomSearchData customSearchData,
            bool validate,
            CustomSearchesDbContext dbContext,
            bool assignFolder = true,
            bool checkExecutionRoles = true)
        {
            var query = dbContext.CustomSearchDefinition as IQueryable<CustomSearchDefinition>;
            var customSearchDefinition = await query
                .Where(c => (c.CustomSearchDefinitionId == customSearchDefinitionId) && (!c.IsDeleted))
                .Include(c => c.CustomSearchParameter)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(customSearchDefinition, "Custom search definition", customSearchDefinitionId);

            if (checkExecutionRoles)
            {
                this.ServiceContext.AuthProvider.AuthorizeAnyRole(
                    customSearchDefinition.ExecutionRoles,
                    $"Current user doesn't have permissions to execute a Custom Search with id '{customSearchDefinitionId}'. Required roles: '{customSearchDefinition.ExecutionRoles}'.");
            }

            CustomSearchesValidationHelper.AssertParameterValuesAreValid(customSearchDefinition, customSearchData.Parameters);
            DatasetGenerationPayloadData payload = new DatasetGenerationPayloadData();
            payload.CustomSearchDefinitionId = customSearchDefinitionId;
            payload.Parameters = customSearchData.Parameters;
            payload.DatasetId = Guid.NewGuid();
            payload.ExecutionMode = DatasetExecutionMode.Generate;
            payload.Validate = validate;

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            string datasetName = customSearchData.DatasetName;
            if (string.IsNullOrWhiteSpace(datasetName))
            {
                datasetName = customSearchDefinition.CustomSearchName + " (" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + ")";
            }

            Dataset entity = new Dataset()
            {
                DatasetId = payload.DatasetId,
                UserId = userId,
                CustomSearchDefinitionId = payload.CustomSearchDefinitionId,
                ParameterValues = JsonHelper.SerializeObject(payload.Parameters),
                CreatedTimestamp = DateTime.UtcNow,
                DatasetName = datasetName,
                Comments = customSearchData.Comments,
                IsLocked = false,
                GenerateIndexesElapsedMs = 0,
                ExecuteStoreProcedureElapsedMs = 0,
                GenerateSchemaElapsedMs = 0,
                DataSetState = DatasetStateType.NotProcessed.ToString(),
                DataSetPostProcessState = DatasetPostProcessStateType.NotProcessed.ToString(),
                CreatedBy = userId,
                LastModifiedBy = userId,
                LastModifiedTimestamp = DateTime.UtcNow,
                LastExecutionTimestamp = DateTime.UtcNow
            };

            entity.GeneratedTableName = CustomSearchesDataDbContext.GetDatasetTableName(entity);

            if (assignFolder)
            {
                if (string.IsNullOrWhiteSpace(customSearchData.FolderPath))
                {
                    CustomSearchFolderType folderType = CustomSearchFolderType.User;
                    DatasetFolderManager folderManager = new DatasetFolderManager(folderType, userId, dbContext);
                    Folder folder = await folderManager.EnsureDefaultPathExistsAsync();
                    entity.ParentFolder = folder;
                }
                else
                {
                    DatasetFolderManager.ValidateFolderPath(customSearchData.FolderPath);
                    CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(customSearchData.FolderPath);
                    DatasetFolderManager folderManager = new DatasetFolderManager(folderType, userId, dbContext);
                    await folderManager.AssignFolderToItemAsync(customSearchData.FolderPath, entity, userId);
                }
            }

            dbContext.Dataset.Add(entity);
            await dbContext.ValidateAndSaveChangesAsync();

            int jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                "DatasetGeneration",
                "DatasetGenerationJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetGenerationTimeout);

            // Returns the dataset id
            return new ExecuteCustomSearchResponse
            {
                DatasetId = payload.DatasetId,
                JobId = jobId
            };
        }

        /// <summary>
        /// Executes a custom search.
        /// </summary>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="logAction">The log action.</param>
        /// <returns>
        /// A unique Id that can be used to get the database data.
        /// </returns>
        public async Task<List<string>> ExecuteCustomSearchAsync(
            DatasetGenerationPayloadData payload,
            CustomSearchesDbContext dbContext,
            GenericDynamicsHelper dynamicsODataHelper,
            Action<string> logAction)
        {
            logAction.Invoke("Starting dataset generation...");

            Dataset dataset = await dbContext.Dataset
                .Where(d => (d.DatasetId == payload.DatasetId))
                .Include(d => d.DatasetPostProcess)
                .Include(d => d.CustomSearchExpression)
                .Include(d => d.InverseSourceDataset)
                .FirstOrDefaultAsync();

            if (dataset == null)
            {
                throw new CustomSearchesDatabaseException(string.Format("Dataset not found: '{0}'.", payload.DatasetId), null);
            }

            try
            {
                List<string> validationWarnings = null;
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                await DatasetHelper.GetAlterDatasetLockAsyncV2(
                    async (datasetState, datasetPostProcessState) =>
                    {
                        try
                        {
                            var definitionId = dataset.CustomSearchDefinitionId;
                            CustomSearchDefinition customSearchDefinition = await dbContext.CustomSearchDefinition
                                .Where(d => (d.CustomSearchDefinitionId == definitionId))
                                .Include(d => d.CustomSearchBackendEntity)
                                .Include(d => d.CustomSearchParameter)
                                .FirstOrDefaultAsync();

                            if (payload.IsSingleRowExecutionMode)
                            {
                                await this.GenerateSearchExecutionDataAsync(dataset, payload, customSearchDefinition, dbContext);
                            }
                            else
                            {
                                await (from d in dbContext.CustomSearchColumnDefinition where d.CustomSearchDefinitionId == definitionId select d).LoadAsync();
                                await (from r in dbContext.CustomSearchValidationRule where r.CustomSearchDefinitionId == definitionId select r).LoadAsync();

                                await (from e in dbContext.CustomSearchExpression
                                       join p in dbContext.CustomSearchParameter
                                       on e.CustomSearchParameterId equals p.CustomSearchParameterId
                                       where p.CustomSearchDefinitionId == definitionId
                                       select e)
                                       .Union(from e in dbContext.CustomSearchExpression
                                              join d in dbContext.CustomSearchColumnDefinition
                                              on e.CustomSearchColumnDefinitionId equals d.CustomSearchColumnDefinitionId
                                              where d.CustomSearchDefinitionId == definitionId
                                              select e)
                                             .Union(from e in dbContext.CustomSearchExpression
                                                    join r in dbContext.CustomSearchValidationRule
                                                    on e.CustomSearchValidationRuleId equals r.CustomSearchValidationRuleId
                                                    where r.CustomSearchDefinitionId == definitionId
                                                    select e).LoadAsync();

                                await this.PreValidateSearchDefinitionAsync(dataset, payload, customSearchDefinition);

                                dataset.DataSetState = DatasetStateType.GeneratingDataset.ToString();
                                dataset.LastModifiedTimestamp = DateTime.UtcNow;
                                dataset.LastModifiedBy = userId;
                                dataset.LastExecutedBy = userId;
                                dbContext.Dataset.Update(dataset);
                                await dbContext.ValidateAndSaveChangesAsync();

                                logAction.Invoke("Generating dataset schema...");
                                Stopwatch stopWatch = new Stopwatch();
                                stopWatch.Start();

                                // If this is not a duplicate or single row execution mode then generates schema.
                                if (payload.SourceDatasetId == Guid.Empty && !payload.IsSingleRowExecutionMode)
                                {
                                    await this.GenerateSchemaAsync(customSearchDefinition, payload);
                                }

                                stopWatch.Stop();
                                long generateSchemaElapsedMs = stopWatch.ElapsedMilliseconds;

                                logAction.Invoke("Executing dataset stored procedure...");
                                stopWatch = new Stopwatch();
                                stopWatch.Start();

                                int totalRows = await this.GenerateSearchExecutionDataAsync(dataset, payload, customSearchDefinition, dbContext);

                                stopWatch.Stop();
                                long executeStoreProcedureElapsedMs = stopWatch.ElapsedMilliseconds;

                                if (totalRows >= 0)
                                {
                                    dataset.TotalRows = totalRows;
                                }

                                dataset.DataSetState = DatasetStateType.GeneratingIndexes.ToString();
                                dataset.GenerateSchemaElapsedMs = (int)generateSchemaElapsedMs;
                                dataset.ExecuteStoreProcedureElapsedMs = (int)executeStoreProcedureElapsedMs;
                                dataset.LastModifiedTimestamp = DateTime.UtcNow;
                                dataset.LastModifiedBy = userId;
                                dbContext.Dataset.Update(dataset);
                                await dbContext.ValidateAndSaveChangesAsync();

                                logAction.Invoke("Generating dataset indexes...");
                                stopWatch = new Stopwatch();
                                stopWatch.Start();

                                if ((payload.ExecutionMode != DatasetExecutionMode.Update) && (payload.ExecutionMode != DatasetExecutionMode.Refresh))
                                {
                                    await this.CreateCustomSearchResultTableIndexesAsync(dataset, customSearchDefinition);
                                }

                                stopWatch.Stop();
                                long generateIndexesElapsedMs = stopWatch.ElapsedMilliseconds;

                                validationWarnings = await this.ValidateCustomSearchDefinitionAsync(dataset, payload, customSearchDefinition, dynamicsODataHelper);

                                dataset.DataSetState = DatasetStateType.Processed.ToString();
                                dataset.GenerateIndexesElapsedMs = (int)generateIndexesElapsedMs;
                                dataset.LastModifiedTimestamp = DateTime.UtcNow;
                                dataset.LastModifiedBy = userId;
                                dataset.LastExecutionTimestamp = DateTime.UtcNow;
                                dbContext.Dataset.Update(dataset);

                                foreach (var referenceDataset in dataset.InverseSourceDataset)
                                {
                                    referenceDataset.LastExecutionTimestamp = dataset.LastExecutionTimestamp;
                                    referenceDataset.TotalRows = dataset.TotalRows;
                                    referenceDataset.ParameterValues = dataset.ParameterValues;
                                    dbContext.Dataset.Update(referenceDataset);
                                }

                                await dbContext.ValidateAndSaveChangesAsync();

                                await this.RevertDatasetDataAsync(dataset, payload, dbContext);

                                if ((dataset.CustomSearchExpression.Count > 0) && (payload.ExecutionMode != DatasetExecutionMode.Update) && (payload.ExecutionMode != DatasetExecutionMode.Refresh))
                                {
                                    var calculatedColumns = dataset.CustomSearchExpression.Where(e => e.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());
                                    if (calculatedColumns.Count() > 0)
                                    {
                                        AddCalculatedColumnService service = new AddCalculatedColumnService(this.ServiceContext);

                                        foreach (var calculatedColumn in calculatedColumns)
                                        {
                                            CalculatedColumnData calculatedColumnData = new CalculatedColumnData
                                            {
                                                ColumnName = calculatedColumn.ColumnName,
                                                Script = calculatedColumn.Script
                                            };

                                            datasetPostProcessState = await service.AddCalculatedColumnAsync(dataset, calculatedColumnData, dbContext, datasetPostProcessState, executePostProcess: false, logger: null);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new DatasetGenerationException($"Dataset generation failed (generating dataset) for dataset {dataset.DatasetId}", ex, false);
                        }

                        try
                        {
                            if (dataset.DatasetPostProcess.Count > 0)
                            {
                                if (payload.NeedsPostProcessExecution)
                                {
                                    DatasetPostProcessExecutionPayloadData postProcessPayload = new DatasetPostProcessExecutionPayloadData();
                                    postProcessPayload.DatasetPostProcessId = -1;
                                    postProcessPayload.DatasetId = dataset.DatasetId;
                                    postProcessPayload.SingleRowExecutionData.Major = payload.RowMajor;
                                    postProcessPayload.SingleRowExecutionData.Minor = payload.RowMinor;

                                    ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
                                    await service.ExecuteDatasetPostProcessAsync(postProcessPayload, dbContext, logAction);

                                    foreach (var referenceDataset in dataset.InverseSourceDataset)
                                    {
                                        postProcessPayload.DatasetId = referenceDataset.DatasetId;
                                        await service.ExecuteDatasetPostProcessAsync(postProcessPayload, dbContext, logAction);
                                    }

                                    datasetPostProcessState = dataset.DataSetPostProcessState;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new DatasetGenerationException($"Dataset generation failed (executing post-processes) for dataset {dataset.DatasetId}", ex, true);
                        }

                        return (dataset.DataSetState, datasetPostProcessState);
                    },
                    dataset,
                    isRootLock: true,
                    dataset.DataSetState,
                    dataset.DataSetPostProcessState,
                    userId,
                    lockingJobId: null,
                    dbContext,
                    this.ServiceContext,
                    skipAlterDatasetLock: payload.IsSingleRowExecutionMode);

                logAction.Invoke("Finished dataset generation...");

                return validationWarnings;
            }
            catch (DatasetGenerationException ex)
            {
                if (!payload.IsSingleRowExecutionMode)
                {
                    try
                    {
                        using (var newDbContext = this.ServiceContext.DbContextFactory.Create())
                        {
                            Dataset toUpdate = await (from d in newDbContext.Dataset where dataset.DatasetId == d.DatasetId select d).FirstOrDefaultAsync();
                            if (toUpdate != null)
                            {
                                if (ex.FailedDuringPostProcess)
                                {
                                    toUpdate.DataSetState = DatasetStateType.Processed.ToString();
                                    toUpdate.DataSetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                                }
                                else
                                {
                                    toUpdate.DataSetState = DatasetStateType.Failed.ToString();
                                }

                                // We try to save the failure state. We purposefully avoid locking here since it would be
                                // an edge case that someone uses the dataset at this point.
                                await newDbContext.SaveChangesWithRetriesAsync();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // If we can't save the dataset we should continue anyways to throw the original exception.
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Creates a temporary table in database with store procedure results.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="sourceDatasetId">The source dataset id.</param>
        /// <returns>The task.</returns>
        /// <exception cref="CustomSearchesDatabaseException">Cannot get the column schema from dataset template.</exception>
        public async Task CreateDatasetUpdateTableAsync(Dataset dataset, CustomSearchDefinition customSearchDefinition, Guid sourceDatasetId)
        {
            string targetTableFullName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(dataset);
            string targetTableName = CustomSearchesDataDbContext.GetDatasetUpdateTableName(dataset);

            var dbColumns =
                (await this.GetColumnSchemaFromTemplateAsync(customSearchDefinition))
                .ToDictionary(c => c.ColumnName.Trim().ToLower(), c => c);

            this.ValidateSearchDefinitionColumns(customSearchDefinition, dbColumns);

            CustomSearchColumnDefinition[] columns = customSearchDefinition.CustomSearchColumnDefinition.Where(d => d.IsEditable == true).ToArray();

            string script = "CREATE TABLE " + targetTableFullName + " (\n";
            string indexScript = string.Empty;
            foreach (var column in columns)
            {
                DbColumn dbColumn = dbColumns[column.ColumnName.Trim().ToLower()];
                script += "[" + column.ColumnName + "] " + DatabaseColumnHelper.GetDatabaseType(dbColumn) + " NULL,\n";

                if (DatabaseColumnHelper.IsDatabaseColumnIndexable(dbColumn))
                {
                    indexScript += $"INDEX [IX_{targetTableName}_{column.ColumnName}] NONCLUSTERED ({column.ColumnName}),\n";
                }
            }

            indexScript += $"INDEX [IX_{targetTableName}_IsValid] NONCLUSTERED (IsValid),\n";
            indexScript += $"INDEX [IX_{targetTableName}_Validated] NONCLUSTERED (Validated),\n";
            indexScript += $"INDEX [IX_{targetTableName}_BackendExportState] NONCLUSTERED (BackendExportState)\n";

            script += "[CustomSearchResultId] INT NOT NULL,\n";
            script += "[RowVersion] INT NOT NULL,\n";
            script += "[IsValid] bit NOT NULL DEFAULT 0,\n";
            script += "[Validated] bit NOT NULL DEFAULT 0,\n";
            script += "[BackendExportState] NVARCHAR(256) NOT NULL DEFAULT 'NotExported',\n";
            script += "[ErrorMessage] NVARCHAR(MAX) NULL,\n";
            script += "[ExportedToBackEndErrorMessage] NVARCHAR(MAX) NULL,\n";
            script += $"CONSTRAINT PK_{targetTableName} PRIMARY KEY NONCLUSTERED([CustomSearchResultId], [RowVersion]),\n";
            script += indexScript;
            script += ")\n";

            if (sourceDatasetId != Guid.Empty)
            {
                string sourceTableFullName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(sourceDatasetId);
                var columnNames = new List<string> { "CustomSearchResultId", "RowVersion", "IsValid", "Validated", "BackendExportState", "ErrorMessage", "ExportedToBackEndErrorMessage" };
                columnNames.AddRange(columns.Select(e => e.ColumnName));

                var columnsScript = string.Join(", ", columnNames.Select(cn => $"[{cn}]"));

                script += $"INSERT INTO {targetTableFullName} ({columnsScript}) SELECT {columnsScript} FROM {sourceTableFullName} \n";
            }

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Error creating the dataset update table: '{targetTableName}'.");
        }

        /// <summary>
        /// Creates the view of the dataset data table.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The task.</returns>
        /// <exception cref="CustomSearchesDatabaseException">Cannot create the dataset data view.</exception>
        public async Task CreateDatasetDataViewAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            string viewFullName = CustomSearchesDataDbContext.GetDatasetViewFullName(dataset, usePostProcess: false);
            string updateView = await DbTransientRetryPolicy.GetDatasetViewScriptAsync(
                this.ServiceContext,
                dataset,
                expressions: null,
                applyUpdates: true,
                dbContext);

            string script = "CREATE VIEW " + viewFullName + " AS " + updateView;

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Cannot create the dataset data view: '{viewFullName}'.");
        }

        /// <summary>
        /// Validates the stored procedure outputs.
        /// </summary>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        private async Task ValidateStoredProcedureOutputsAsync(
            DatasetGenerationPayloadData payload,
            CustomSearchDefinition customSearchDefinition)
        {
            if (!await this.ExistCustomSearchTemplateAsync(customSearchDefinition))
            {
                return;
            }

            var templateDbColumns =
                (await this.GetColumnSchemaFromTemplateAsync(customSearchDefinition))
                .Select(c => new { Name = c.ColumnName.Trim().ToLower(), Type = c.DataType })
                .ToHashSet();

            var storedProcedureDbColumns =
                (await this.GetColumnSchemaAsync(customSearchDefinition, payload))
                .Select(c => new { Name = c.ColumnName.Trim().ToLower(), Type = c.DataType })
                .ToHashSet();

            if (!templateDbColumns.SetEquals(storedProcedureDbColumns))
            {
                throw new CustomSearchValidationException(
                    $"The stored procedure '{customSearchDefinition.StoredProcedureName}' was modified since the last time the custom search was imported. " +
                    $"Please contact the administrator to solve this problem.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates the custom search definition before the dataset generation.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        private async Task PreValidateSearchDefinitionAsync(
            Dataset dataset,
            DatasetGenerationPayloadData payload,
            CustomSearchDefinition customSearchDefinition)
        {
            bool requireValidation = payload.Validate || (customSearchDefinition.Validated != true);
            if (requireValidation)
            {
                // Only lookup expressions can be validated before the dataset generation.
                List<CustomSearchExpression> expressions =
                    this.GetCustomSearchDefinitionExpressions(customSearchDefinition)
                    .Where(e => e.CustomSearchParameter != null || e.CustomSearchColumnDefinition != null).ToList();

                await ImportCustomSearchService.ValidateCustomSearchDefinitionAsync(
                    customSearchDefinition,
                    expressions,
                    dataset,
                    this.ServiceContext,
                    isFakeSearch: false);

                // Validate stored procedure outputs against template.
                await this.ValidateStoredProcedureOutputsAsync(payload, customSearchDefinition);
            }
        }

        /// <summary>
        /// Validates the custom search definition columns against the column schema.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="dbColumns">The column schema.</param>
        private void ValidateSearchDefinitionColumns(CustomSearchDefinition customSearchDefinition, IDictionary<string, DbColumn> dbColumns)
        {
            foreach (var customSearchColumnDefinition in customSearchDefinition.CustomSearchColumnDefinition)
            {
                if (dbColumns.ContainsKey(customSearchColumnDefinition.ColumnName.Trim().ToLower()) == false)
                {
                    throw new CustomSearchValidationException(
                        $"The dataset schema doesn't have the column '{customSearchColumnDefinition.ColumnName}' specified in the column definitions.",
                        innerException: null);
                }
            }

            foreach (var backendEntity in customSearchDefinition.CustomSearchBackendEntity)
            {
                if (dbColumns.ContainsKey(backendEntity.CustomSearchKeyFieldName.Trim().ToLower()) == false)
                {
                    throw new CustomSearchValidationException(
                        $"The dataset schema doesn't have the column '{backendEntity.CustomSearchKeyFieldName}' specified in the backend entities.",
                        innerException: null);
                }
            }
        }

        /// <summary>
        /// Validates the custom search definition against the generated dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <returns>The validation warning list.</returns>
        private async Task<List<string>> ValidateCustomSearchDefinitionAsync(
            Dataset dataset,
            DatasetGenerationPayloadData payload,
            CustomSearchDefinition customSearchDefinition,
            GenericDynamicsHelper dynamicsODataHelper)
        {
            List<string> validationWarnings = new List<string>();
            bool requireValidation = payload.Validate || (customSearchDefinition.Validated != true);
            if (requireValidation)
            {
                // All the expressions are validated after the dataset generation.
                List<CustomSearchExpression> expressions = this.GetCustomSearchDefinitionExpressions(customSearchDefinition);
                await ImportCustomSearchService.ValidateCustomSearchDefinitionAsync(
                    customSearchDefinition,
                    expressions,
                    dataset,
                    this.ServiceContext,
                    isFakeSearch: false);

                if (customSearchDefinition.CustomSearchBackendEntity.Count > 0)
                {
                    if (dataset.TotalRows > 0)
                    {
                        string tableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset.DatasetId);

                        foreach (var backendEntity in customSearchDefinition.CustomSearchBackendEntity)
                        {
                            var topEntityScript = $"Select TOP(1) * FROM {tableName} where {backendEntity.CustomSearchKeyFieldName} is not NULL";

                            IEnumerable<dynamic> topEntityResults = null;
                            try
                            {
                                topEntityResults = await DbTransientRetryPolicy.GetDynamicResultAsync(
                                    this.ServiceContext,
                                    this.ServiceContext.DataDbContextFactory,
                                    topEntityScript);
                            }
                            catch (SqlException ex)
                            {
                                throw new CustomSearchValidationException(
                                    "Data for validation could not be obtained from the dataset.",
                                    ex);
                            }

                            var datasetRow = (IDictionary<string, object>)topEntityResults.FirstOrDefault();

                            if (datasetRow == null)
                            {
                                validationWarnings.Add($"Custom search definition backend information can't be validated because" +
                                    $" a dataset row with key '{backendEntity.CustomSearchKeyFieldName}' was not found.");

                                break;
                            }

                            await this.ValidateCustomSearchBackendEntityAsync(
                                backendEntity,
                                datasetRow,
                                customSearchDefinition,
                                dynamicsODataHelper,
                                validationWarnings);
                        }
                    }
                    else
                    {
                        validationWarnings.Add("Custom search definition backend information can't be validated because the dataset is empty.");
                    }
                }
            }

            customSearchDefinition.Validated = true;

            return validationWarnings;
        }

        /// <summary>
        /// Validates the custom search backend entity.
        /// </summary>
        /// <param name="backendEntity">The backend entity.</param>
        /// <param name="datasetRow">An entity from the dataset table.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper.</param>
        /// <param name="validationWarnings">The validation warnings.</param>
        private async Task ValidateCustomSearchBackendEntityAsync(
            CustomSearchBackendEntity backendEntity,
            IDictionary<string, object> datasetRow,
            CustomSearchDefinition customSearchDefinition,
            GenericDynamicsHelper dynamicsODataHelper,
            List<string> validationWarnings)
        {
            EntityRequestResult entityRequestResult = null;
            try
            {
                EntityRequest entityRequest = new EntityRequest
                {
                    EntityId = datasetRow[backendEntity.CustomSearchKeyFieldName].ToString(),
                    EntityName = backendEntity.BackendEntityName
                };

                EntityRequest[] entityRequests = new EntityRequest[] { entityRequest };

                IEnumerable<EntityRequestResult> entityRequestResults = await dynamicsODataHelper.GetItems(entityRequests);
                entityRequestResult = entityRequestResults.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new CustomSearchValidationException(
                    $"Failed obtaining the dynamics entity '{backendEntity.BackendEntityName}' with id '{datasetRow[backendEntity.CustomSearchKeyFieldName]}'.",
                    innerException: ex);
            }

            JObject backendResult = null;

            if ((entityRequestResult != null) && (entityRequestResult.Changes != null))
            {
                backendResult = entityRequestResult.Changes as JObject;
                if (backendResult != null)
                {
                    if (!backendResult.ContainsKey(backendEntity.BackendEntityKeyFieldName))
                    {
                        throw new CustomSearchValidationException(
                            $"Dynamics entity '{backendEntity.BackendEntityName}' doesn't have the key field '{backendEntity.BackendEntityKeyFieldName}'" +
                            $" specified in the backend entities.",
                            innerException: null);
                    }

                    var columnDefinitions = customSearchDefinition.CustomSearchColumnDefinition
                        .Where(c => c.BackendEntityName?.Trim().ToLower() == backendEntity.BackendEntityName.Trim().ToLower())
                        .ToList();

                    foreach (var columnDefinition in columnDefinitions)
                    {
                        if (!backendResult.ContainsKey(columnDefinition.BackendEntityFieldName))
                        {
                            throw new CustomSearchValidationException(
                                $"Dynamics entity '{backendEntity.BackendEntityName}' doesn't have the field '{columnDefinition.BackendEntityFieldName}'" +
                                $" specified in the column definitions.",
                                innerException: null);
                        }
                    }
                }
            }

            if (backendResult == null)
            {
                validationWarnings.Add(
                    $"Can't validate the backend entity because a dynamics entity with name '{backendEntity.BackendEntityName}'" +
                    $" and key '{datasetRow[backendEntity.CustomSearchKeyFieldName]}' was not found .");
            }
        }

        /// <summary>
        /// Gets the custom search definition expressions.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <returns>The custom search definition expressions.</returns>
        private List<CustomSearchExpression> GetCustomSearchDefinitionExpressions(CustomSearchDefinition customSearchDefinition)
        {
            CustomSearchDefinitionData definitionData =
                new CustomSearchDefinitionData(customSearchDefinition, ModelInitializationType.FullObjectWithDepedendencies);

            List<CustomSearchExpression> expressions;
            definitionData.ToEfModel(out expressions);

            return expressions;
        }

        /// <summary>
        /// Generates the search execution data of the user.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The number rows.</returns>
        private async Task<int> GenerateSearchExecutionDataAsync(
            Dataset dataset,
            DatasetGenerationPayloadData payload,
            CustomSearchDefinition customSearchDefinition,
            CustomSearchesDbContext dbContext)
        {
            if (payload.ExecutionMode == DatasetExecutionMode.Update)
            {
                await this.TruncateDatasetDataTableAsync(dataset);
            }
            else if (payload.ExecutionMode == DatasetExecutionMode.Generate)
            {
                await this.CreateDatasetDataTableAsync(customSearchDefinition, dataset);
                await this.CreateDatasetUpdateTableAsync(dataset, customSearchDefinition, payload.SourceDatasetId);
            }

            int rowsCount = -1;
            if (payload.ExecutionMode == DatasetExecutionMode.Refresh)
            {
                await this.RefreshDatasetAsync(dataset, customSearchDefinition, payload);
            }
            else
            {
                rowsCount = await this.WriteToCustomSearchResultTableAsync(dataset, customSearchDefinition, payload);
            }

            if ((payload.ExecutionMode != DatasetExecutionMode.Update) && (payload.ExecutionMode != DatasetExecutionMode.Refresh))
            {
                await this.CreateDatasetDataViewAsync(dataset, dbContext);
            }

            return rowsCount;
        }

        /// <summary>
        /// Gets the parameters to use in the store procedure.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="payload">The dataset generation payload.</param>
        /// <returns>The parameters to use in the store procedure.</returns>
        private List<SqlParameter> GetDatabaseParameters(CustomSearchDefinition customSearchDefinition, DatasetGenerationPayloadData payload)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            var pendingPayloadParameterList = payload.Parameters.ToList();
            foreach (var customSearchParameter in customSearchDefinition.CustomSearchParameter)
            {
                CustomSearchParameterValueData customSearchParameterValue = payload.Parameters.FirstOrDefault(c =>
                {
                    if (c.Id != null)
                    {
                        return c.Id == customSearchParameter.CustomSearchParameterId;
                    }
                    else
                    {
                        return c.Name.ToLower() == customSearchParameter.ParameterName.ToLower();
                    }
                });

                if (customSearchParameterValue == null && customSearchParameter.ParameterDefaultValue == null)
                {
                    continue;
                }

                pendingPayloadParameterList.Remove(customSearchParameterValue);
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = $"{customSearchParameter.ParameterName}Value";
                sqlParameter.Value = customSearchParameterValue != null ? customSearchParameterValue.Value : customSearchParameter.ParameterDefaultValue;

                sqlParameters.Add(sqlParameter);
            }

            foreach (var customSearchParameterValue in pendingPayloadParameterList)
            {
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = $"{customSearchParameterValue.Name}Value";
                sqlParameter.Value = customSearchParameterValue.Value;

                sqlParameters.Add(sqlParameter);
            }

            return sqlParameters;
        }

        /// <summary>
        /// Gets the parameters string to use in the stored procedure command.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="dbParameters">The sql parameters.</param>
        /// <param name="payload">The dataset generation payload.</param>
        /// <param name="useDefaultTableTypeInputParameter">If this has a table type input parameter then use the default value.</param>
        /// <returns>The parameters to use in the store procedure.</returns>
        private string GetStoredProcedureCommandParameters(
            CustomSearchDefinition customSearchDefinition,
            List<SqlParameter> dbParameters,
            DatasetGenerationPayloadData payload,
            bool useDefaultTableTypeInputParameter = false)
        {
            string commandParameters = string.Empty;
            foreach (var dbParameter in dbParameters)
            {
                if (!string.IsNullOrWhiteSpace(commandParameters))
                {
                    commandParameters += ",";
                }

                // Removing the last 5 characters 'Value' to get the base parameter name.
                string baseParameterName = dbParameter.ParameterName.Substring(0, dbParameter.ParameterName.Length - 5);

                commandParameters += $" @{baseParameterName} = @{baseParameterName}Value";
            }

            if (!string.IsNullOrWhiteSpace(customSearchDefinition.TableInputParameterName))
            {
                if (!string.IsNullOrWhiteSpace(commandParameters))
                {
                    commandParameters += ",";
                }

                bool useDefaultValue = useDefaultTableTypeInputParameter || string.IsNullOrWhiteSpace(payload.TableTypeInputParameterScript);
                string parameterValue = useDefaultValue ? "default" : "@modelParameter";
                commandParameters += $" @{customSearchDefinition.TableInputParameterName} = {parameterValue}";
            }

            return commandParameters;
        }

        /// <summary>
        /// Gets the script with the declaration of the SQL user-defined table type parameter used in the custom search stored procedure.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="payload">The dataset generation payload.</param>
        /// <returns>The parameters to use in the store procedure.</returns>
        private string GetTableTypeInputParameterScript(CustomSearchDefinition customSearchDefinition, DatasetGenerationPayloadData payload)
        {
            string script = string.Empty;
            if (!string.IsNullOrWhiteSpace(customSearchDefinition.TableInputParameterName))
            {
                if (!string.IsNullOrWhiteSpace(payload.TableTypeInputParameterScript))
                {
                    script += $"DECLARE @modelParameter [cus].[{customSearchDefinition.TableInputParameterDbType}]\n";
                    script += "INSERT INTO @modelParameter\n";
                    script += $"{payload.TableTypeInputParameterScript}\n";
                }
            }

            return script;
        }

        /// <summary>
        /// Determines whether the custom search template exists in database.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        private async Task<bool> ExistCustomSearchTemplateAsync(CustomSearchDefinition customSearchDefinition)
        {
            string tableName = $"DatasetTemplate_{customSearchDefinition.CustomSearchDefinitionId}_{customSearchDefinition.Version}";
            string script = string.Empty;
            script += "SELECT Count (*)\n";
            script += "FROM INFORMATION_SCHEMA.TABLES\n";
            script += "WHERE TABLE_SCHEMA = 'cus'\n";
            script += "AND  TABLE_NAME = '" + tableName + "'\n";

            var foundTemplate = (int)await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Cannot verify if the dataset template exists: '{tableName}'.");

            return foundTemplate > 0;
        }

        /// <summary>
        /// Determines whether the custom search template need to be regenerated in the database. Not implemented.
        /// </summary>
        /// <param name="customSearchDefinitionId">Custom search definition id.</param>
        private bool NeedCustomSearchTemplateRegeneration(int customSearchDefinitionId)
        {
            return false;
        }

        /// <summary>
        /// Generates the custom search schema in the database if required.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="payload">The dataset generation payload.</param>
        private async Task GenerateSchemaAsync(CustomSearchDefinition customSearchDefinition, DatasetGenerationPayloadData payload)
        {
            string templateName = string.Format(CustomSearchTemplateTableFormat, customSearchDefinition.CustomSearchDefinitionId, customSearchDefinition.Version);
            bool foundTemplate = await this.ExistCustomSearchTemplateAsync(customSearchDefinition);
            bool needTemplateRegeneration = true;

            if (foundTemplate)
            {
                needTemplateRegeneration = this.NeedCustomSearchTemplateRegeneration(customSearchDefinition.CustomSearchDefinitionId);
            }

            if (needTemplateRegeneration)
            {
                await this.DropUserTemporaryTableAsync(templateName);

                ReadOnlyCollection<DbColumn> columnSchema = await this.GetColumnSchemaAsync(customSearchDefinition, payload);
                await this.CreateCustomSearchTemplateTableAsync(customSearchDefinition, columnSchema);
            }
        }

        /// <summary>
        /// Gets the results and column schema from the store procedure execution.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="payload">The dataset generation payload.</param>
        /// <returns>>The column schema.</returns>
        private async Task<ReadOnlyCollection<DbColumn>> GetColumnSchemaAsync(CustomSearchDefinition customSearchDefinition, DatasetGenerationPayloadData payload)
        {
            List<SqlParameter> dbParameters = this.GetDatabaseParameters(customSearchDefinition, payload);

            string commandParameters = this.GetStoredProcedureCommandParameters(customSearchDefinition, dbParameters, payload, useDefaultTableTypeInputParameter: true);

            return await StoredProcedureHelper.GetStoredProcedureResultSchemaAsync(
                customSearchDefinition.StoredProcedureName,
                commandParameters,
                dbParameters.ToArray(),
                this.ServiceContext);
        }

        /// <summary>
        /// Gets the column schema from the template.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <returns>>The column schema.</returns>
        /// <exception cref = "CustomSearchesDatabaseException">Cannot get the column schema from dataset template.</exception>
        private async Task<ReadOnlyCollection<DbColumn>> GetColumnSchemaFromTemplateAsync(CustomSearchDefinition customSearchDefinition)
        {
            ReadOnlyCollection<DbColumn> result = null;
            string tableName = string.Format(CustomSearchTemplateTableFormat, customSearchDefinition.CustomSearchDefinitionId, customSearchDefinition.Version);
            string commandText = "SELECT * FROM " + tableName;

            await DynamicSqlStatementHelper.ExecuteReaderWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                commandText,
                async (DbCommand command, DbDataReader dataReader) =>
                {
                    result = dataReader.GetColumnSchema();
                },
                $"Cannot get the column schema from dataset template: '{tableName}'.");

            return result;
        }

        /// <summary>
        /// Drops a temporary table in database with store procedure results.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        private async Task DropUserTemporaryTableAsync(string tableName)
        {
            string script = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + tableName + "') AND type in (N'U'))" + "\n";
            script += "DROP TABLE " + tableName;

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Cannot drop dataset table: '{tableName}'.");
        }

        /// <summary>
        /// Truncates the temporary table in database with store procedure results.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The task.</returns>
        private async Task TruncateDatasetDataTableAsync(Dataset dataset)
        {
            // Creates the dataset data table
            string tableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string script = $"TRUNCATE TABLE {tableFullName}\n";

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Error truncating the dataset data table: '{tableFullName}'.");
        }

        /// <summary>
        /// Creates a temporary table in database with store procedure results.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="dataset">The dataset.</param>
        private async Task CreateDatasetDataTableAsync(CustomSearchDefinition customSearchDefinition, Dataset dataset)
        {
            // Creates the dataset data table
            string tableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string templateName = string.Format(CustomSearchTemplateTableFormat, customSearchDefinition.CustomSearchDefinitionId, customSearchDefinition.Version);
            string script = "SELECT * INTO " + tableFullName + " From " + templateName + "\n";

            // Adding CustomSearchResultId as primary key
            script += "ALTER TABLE " + tableFullName + " ADD [CustomSearchResultId][int] PRIMARY KEY IDENTITY(1, 1) NOT NULL\n";

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Error creating the dataset data table: '{tableFullName}'.");
        }

        /// <summary>
        /// Creates a temporary table in database with store procedure results.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <returns>The id of the new table.</returns>
        private async Task CreateCustomSearchResultTableIndexesAsync(Dataset dataset, CustomSearchDefinition customSearchDefinition)
        {
            string tableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string tableName = CustomSearchesDataDbContext.GetDatasetTableName(dataset);

            ReadOnlyCollection<DbColumn> dbColumns = await this.GetColumnSchemaFromTemplateAsync(customSearchDefinition);

            List<Task> tasks = new List<Task>();

            try
            {
                foreach (DbColumn column in dbColumns)
                {
                    if (DatabaseColumnHelper.IsDatabaseColumnIndexable(column))
                    {
                        string colName = column.ColumnName;
                        string script = "CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + colName + "] ON " + tableFullName + " ([" + colName + "]);\n";
                        tasks.Add(this.ExecuteSqlCommandAsync(script));
                    }

                    if (tasks.Count >= 8)
                    {
                        await Task.WhenAll(tasks.ToArray());
                        tasks.Clear();
                    }
                }

                if (tasks.Count > 0)
                {
                    Task.WaitAll(tasks.ToArray());
                }
            }
            catch (Exception ex)
            {
                if (DynamicSqlStatementHelper.ContainSqlException(ex))
                {
                    throw new CustomSearchesDatabaseException($"Error creating non clustered indexes in table: '{tableName}'.", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Execute the sql command.
        /// </summary>
        /// <param name="commandScript">The command script.</param>
        /// <returns>The command task.</returns>
        private async Task ExecuteSqlCommandAsync(string commandScript)
        {
            await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                commandScript,
                parameters: null);
        }

        /// <summary>
        /// Creates a temporary table in database with store procedure results.
        /// </summary>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="columnSchema">The column schema.</param>
        private async Task CreateCustomSearchTemplateTableAsync(CustomSearchDefinition customSearchDefinition, ReadOnlyCollection<DbColumn> columnSchema)
        {
            string templateName = string.Format(CustomSearchTemplateTableFormat, customSearchDefinition.CustomSearchDefinitionId, customSearchDefinition.Version);
            string script = this.GenerateCustomSearchTemplateScript(templateName, columnSchema);

            await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                $"Error creating the dataset template: '{templateName}'.");
        }

        /// <summary>
        /// Create a datatable from a list of ExpandoObjects.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="table">The connection can be created from a dictionary with Dictionary.Values.ToList().</param>
        private string GenerateCustomSearchTemplateScript(string tableName, ReadOnlyCollection<DbColumn> table)
        {
            string script = "CREATE TABLE " + tableName + " (\n";

            foreach (DbColumn column in table)
            {
                script += "[" + column.ColumnName + "] " + DatabaseColumnHelper.GetDatabaseType(column) + ",\n";
            }

            script = script.TrimEnd(new char[] { ',', '\n' }) + "\n";

            if (!script.EndsWith(")"))
            {
                script += ")";
            }

            return script;
        }

        /// <summary>
        /// Refreshes the dataset from the stored procedure.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <returns>The number rows.</returns>
        private async Task<int> RefreshDatasetAsync(Dataset dataset, CustomSearchDefinition customSearchDefinition, DatasetGenerationPayloadData payload)
        {
            string tableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string tableName = CustomSearchesDataDbContext.GetDatasetTableName(dataset);

            List<SqlParameter> dbParameters;

            string setText = string.Empty;
            string columnsText = string.Empty;

            var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

            foreach (DbColumn column in dbColumns)
            {
                if (column.IsIdentity != true)
                {
                    if (!DatasetColumnHelper.IsSelectionOrFilterColumn(column.ColumnName))
                    {
                        columnsText += $"[{column.ColumnName}] {DatabaseColumnHelper.GetDatabaseType(column)}, ";
                        setText += $"{tableFullName}.[{column.ColumnName}] = sp.[{column.ColumnName}], ";
                    }
                }
            }

            columnsText = columnsText.TrimEnd(new char[] { ',', ' ' });
            columnsText = $"({columnsText})";
            setText = setText.TrimEnd(new char[] { ',', ' ' });

            dbParameters = this.GetDatabaseParameters(customSearchDefinition, payload);
            string commandParameters = this.GetStoredProcedureCommandParameters(customSearchDefinition, dbParameters, payload);

            string storedProcedureText = StoredProcedureHelper.GetExecuteScript(customSearchDefinition.StoredProcedureName, commandParameters);

            string script = this.GetTableTypeInputParameterScript(customSearchDefinition, payload);
            script += $"CREATE TABLE #tab {columnsText}\n";
            script += $"INSERT INTO #tab\n";
            script += $"{storedProcedureText}\n";
            script += $"UPDATE {tableFullName}\n";
            script += $"SET {setText}\n";
            script += $"FROM #tab sp\n";
            script += $"WHERE ({tableFullName}.major = sp.major) AND ({tableFullName}.minor = sp.minor)\n";

            return await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                dbParameters.ToArray(),
                $"Error inserting data into the dataset: '{tableName}'.");
        }

        /// <summary>
        /// Writes the store procedure results to the temporary table.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="customSearchDefinition">Custom search definition.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <returns>The number rows.</returns>
        private async Task<int> WriteToCustomSearchResultTableAsync(
            Dataset dataset,
            CustomSearchDefinition customSearchDefinition,
            DatasetGenerationPayloadData payload)
        {
            string tableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string tableName = CustomSearchesDataDbContext.GetDatasetTableName(dataset);

            List<SqlParameter> dbParameters = null;

            string selectFieldsText = string.Empty;
            string insertIntoFieldsText = string.Empty;
            string fromText;

            if ((payload.SourceDatasetId != Guid.Empty) || (payload.ExecutionMode == DatasetExecutionMode.Update))
            {
                var dbColumns = await DbTransientRetryPolicy.GetDatasetColumnSchemaAsync(this.ServiceContext, dataset);

                foreach (DbColumn column in dbColumns)
                {
                    if (column.IsIdentity != true)
                    {
                        if ((payload.ExecutionMode != DatasetExecutionMode.Update) || (!DatasetColumnHelper.IsSelectionOrFilterColumn(column.ColumnName)))
                        {
                            selectFieldsText += $"[{column.ColumnName}], ";
                        }
                    }
                }

                selectFieldsText = selectFieldsText.TrimEnd(new char[] { ',', ' ' });
                insertIntoFieldsText = $"({selectFieldsText})";
            }

            string whereText = string.Empty;
            string tableTypeInputParameterScript = string.Empty;
            if (payload.SourceDatasetId == Guid.Empty)
            {
                dbParameters = this.GetDatabaseParameters(customSearchDefinition, payload);
                string commandParameters = this.GetStoredProcedureCommandParameters(customSearchDefinition, dbParameters, payload);
                tableTypeInputParameterScript = this.GetTableTypeInputParameterScript(customSearchDefinition, payload);

                fromText = StoredProcedureHelper.GetExecuteScript(customSearchDefinition.StoredProcedureName, commandParameters);
            }
            else
            {
                string sourceDatasetTableFullName = CustomSearchesDataDbContext.GetDatasetTableFullName(payload.SourceDatasetId);
                fromText = $"SELECT {selectFieldsText} FROM {sourceDatasetTableFullName}";

                if (payload.ApplyRowFilterFromSourceDataset)
                {
                    whereText = $"[{DatasetColumnHelper.FilterStateColumnName}] != 'FilteredOut'";
                }

                if (payload.ApplyUserSelectionFromSourceDataset)
                {
                    whereText = string.IsNullOrWhiteSpace(whereText) ? whereText : whereText + " AND";
                    whereText += $" [{DatasetColumnHelper.SelectionColumnName}] = 1";
                }
            }

            string script = tableTypeInputParameterScript;
            script += "INSERT INTO " + tableFullName + " " + insertIntoFieldsText + "\n";
            script += fromText + "\n";
            if (!string.IsNullOrWhiteSpace(whereText))
            {
                script += $"WHERE {whereText}\n";
            }

            if (payload.ExecutionMode != DatasetExecutionMode.Update)
            {
                script += $"ALTER TABLE {tableFullName}\n";
                script += $"ADD [{DatasetColumnHelper.SelectionColumnName}] bit NOT NULL\n";
                script += $"CONSTRAINT D_{tableName}_{DatasetColumnHelper.SelectionColumnName}\n";
                script += "DEFAULT (0)\n";
                script += "WITH VALUES,\n";
                script += $"[{DatasetColumnHelper.FilterStateColumnName}] nvarchar(255) NOT NULL\n";
                script += $"CONSTRAINT D_{tableName}_{DatasetColumnHelper.FilterStateColumnName}\n";
                script += "DEFAULT ('NotFilteredOut')\n";
                script += "WITH VALUES;\n";
            }

            return await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                this.ServiceContext,
                this.ServiceContext.DataDbContextFactory,
                script,
                dbParameters?.ToArray(),
                $"Error inserting data into the dataset: '{tableName}'.");
        }

        /// <summary>
        /// Reverts the dataset data if required.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="payload">The dataset payload.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RevertDatasetDataAsync(
            Dataset dataset,
            DatasetGenerationPayloadData payload,
            CustomSearchesDbContext dbContext)
        {
            if ((payload.ExecutionMode == DatasetExecutionMode.Update) || (payload.ExecutionMode == DatasetExecutionMode.Refresh))
            {
                RevertDatasetUpdatesData rowsToRevert = null;

                var rowParameters = payload.Parameters
                    .Where(p => (p.Name?.ToLower() == "major" || p.Name?.ToLower() == "minor") && !string.IsNullOrWhiteSpace(p.Value));

                if (rowParameters.Count() == 2)
                {
                    DatasetRowIdData rowId = new DatasetRowIdData();
                    rowId.Major = rowParameters.First(p => p.Name.ToLower() == "major").Value;
                    rowId.Minor = rowParameters.First(p => p.Name.ToLower() == "minor").Value;
                    rowsToRevert = new RevertDatasetUpdatesData() { RowIds = new DatasetRowIdData[] { rowId } };
                }

                var revertDatasetUpdatesService = new RevertDatasetUpdatesService(this.ServiceContext);
                await revertDatasetUpdatesService.RevertDatasetDataNoLockAsync(dataset, rowsToRevert, includeRevertedRows: false, dbContext);
            }
        }
    }
}