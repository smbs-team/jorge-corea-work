namespace CustomSearchesServicesLibrary.CustomSearches.Executor
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
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Executor for stored procedure update post process.
    /// </summary>
    public class StoredProcedureUpdatePostProcessExecutor : DatasetPostProcessExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureUpdatePostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previous view script.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public StoredProcedureUpdatePostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            SingleRowExecutionData singleRowExecutionData,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext)
            : base(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext)
        {
        }

        /// <summary>
        /// Gets the script to create stored procedure update table.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process including its custom custom search expressions.</param>
        /// <returns>
        /// The script to create stored procedure update table.
        /// </returns>
        public static string GetCreateTableScript(DatasetPostProcess datasetPostProcess)
        {
            var calculatedColumnExpressions = datasetPostProcess.CustomSearchExpression
                 .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

            string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(datasetPostProcess);
            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(datasetPostProcess);

            string createTableScript = $"CREATE TABLE {tableFullName}(\n" +
                "[CustomSearchResultId] int NOT NULL PRIMARY KEY,\n";

            foreach (var column in calculatedColumnExpressions)
            {
                DatabaseColumnExtensionData databaseColumnExtensionData =
                    JsonHelper.DeserializeObject<DatabaseColumnExtensionData>(column.ExpressionExtensions);

                string columnScript = $"[{column.ColumnName}] {databaseColumnExtensionData.DatabaseType} NULL,\n";
                createTableScript += columnScript;

                if (databaseColumnExtensionData.IsDatabaseColumnIndexable)
                {
                    createTableScript += $"INDEX [IX_{tableName}_{column.ColumnName}] NONCLUSTERED ({column.ColumnName}),\n";
                }
            }

            createTableScript += ");\n";

            return createTableScript;
        }

        /// <summary>
        /// Deletes the previous state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task DeletePreviousStateAsync()
        {
            string postProcessTableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                try
                {
                    string datasetTableName = CustomSearchesDataDbContext.GetDatasetTableFullName(this.Dataset);
                    string script =
                        $"DELETE dpp " +
                        $"FROM {postProcessTableName} dpp " +
                        $"LEFT JOIN {datasetTableName} d ON dpp.CustomSearchResultId = d.CustomSearchResultId " +
                        $"WHERE d.[Major] = '{this.SingleRowExecutionData.Major}' AND d.[Minor] = '{this.SingleRowExecutionData.Minor}';\n";

                    await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        parameters: null);
                }
                catch (SqlException ex)
                {
                    throw new CustomSearchesDatabaseException(
                        $"Cannot delete row where Major is '{this.SingleRowExecutionData.Major}' an Minor is '{this.SingleRowExecutionData.Minor}' from table: '{postProcessTableName}'.",
                        ex);
                }
            }
            else
            {
                try
                {
                    string script = DatasetHelper.GetDeleteBackendUpdateScript(this.DatasetPostProcess.DatasetPostProcessId);
                    await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        parameters: null);

                    await DbTransientRetryPolicy.DropTableAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        postProcessTableName);
                }
                catch (SqlException ex)
                {
                    throw new CustomSearchesDatabaseException(string.Format("Cannot drop dataset table: '{0}'.", postProcessTableName), ex);
                }
            }
        }

        /// <summary>
        /// Commits the new state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CommitNewStateAsync()
        {
            string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            string datasetUpdateTableFullName = CustomSearchesDataDbContext.GetDatasetUpdateTableFullName(this.Dataset);

            try
            {
                bool usePostProcess = !string.IsNullOrWhiteSpace(this.ViewScript);
                string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, usePostProcess);
                string datasetView = usePostProcess ? this.ViewScript : $"Select * FROM {viewName}";

                DatasetPostProcess datasetPostProcess = await this.DbContext.DatasetPostProcess
                    .Where(d => d.DatasetPostProcessId == this.DatasetPostProcess.DatasetPostProcessId)
                    .Include(d => d.CustomSearchExpression)
                    .FirstOrDefaultAsync();

                var datasetViewColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

                var storedProcedureName = datasetPostProcess.CustomSearchExpression
                    .FirstOrDefault(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.UpdateStoredProcedure.ToString().ToLower())
                    .Script;

                var calculatedColumnExpressions = datasetPostProcess.CustomSearchExpression
                    .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

                // The column names are in the auto generated expressions.
                var expressionColumnNames = calculatedColumnExpressions.Select(e => e.ColumnName).ToHashSet();

                HashSet<string> storedProcedureParameters =
                    await StoredProcedureHelper.GetStoredProcedureParametersAsync(storedProcedureName, this.ServiceContext);

                var storedProcedureResultSchema = await ImportStoredProcedureUpdatePostProcessService.GetStoredProcedureResultSchemaAsync(
                    storedProcedureName: storedProcedureName,
                    storedProcedureParameters,
                    this.Dataset,
                    datasetPostProcess,
                    this.ServiceContext,
                    throwIfNotRow: false);

                if (storedProcedureResultSchema != null)
                {
                    // The column names from stored procedure execution.
                    var storedProcedureResultColumnNames = storedProcedureResultSchema.Select(c => c.ColumnName.Trim().ToLower()).ToHashSet();

                    if ((expressionColumnNames.Count != storedProcedureResultColumnNames.Count) ||
                        expressionColumnNames.Except(storedProcedureResultColumnNames, StringComparer.OrdinalIgnoreCase).Count() > 0)
                    {
                        throw new CustomSearchesConflictException(
                            $"The resulting schema of the stored procedure '{storedProcedureName}' changed since last import. The post process needs to be reimported.",
                            innerException: null);
                    }
                }

                storedProcedureParameters.RemoveWhere(p =>
                    p.ToLower() == ImportStoredProcedureUpdatePostProcessService.IsSchemaProbeParameterName.ToLower() ||
                    p.ToLower() == ImportStoredProcedureUpdatePostProcessService.BackendUpdatesParameterName.ToLower());

                // Checks for invalid stored procedure parameters.
                var invalidParameterNames = storedProcedureParameters.Except(datasetViewColumns.Select(c => c.ColumnName).ToHashSet(), StringComparer.OrdinalIgnoreCase);
                if (invalidParameterNames.Count() > 0)
                {
                    string invalidParameters = string.Join(", ", invalidParameterNames);
                    throw new CustomSearchesConflictException(
                        $"The input parameters of the stored procedure '{storedProcedureName}' changed. " +
                        $"The following input parameter does not match with the database view schema: {invalidParameters}.",
                        innerException: null);
                }

                // Script generation
                string createTableScript = StoredProcedureUpdatePostProcessExecutor.GetCreateTableScript(datasetPostProcess);

                string script = this.SingleRowExecutionData.IsSingleRowExecutionMode ? string.Empty : createTableScript;

                string selectedColumnsForCursor = string.Empty;
                script += $"DECLARE @CustomSearchResultId int\n";
                script += $"DECLARE @FoundValidationError bit\n";
                foreach (var parameter in storedProcedureParameters)
                {
                    var column = datasetViewColumns.FirstOrDefault(c => c.ColumnName.Trim().ToLower() == parameter.Trim().ToLower());
                    script += $"DECLARE @{parameter} {DatabaseColumnHelper.GetDatabaseType(column)}\n";

                    selectedColumnsForCursor += $", [{column.ColumnName}]";
                }

                script += $"DECLARE DatasetInfo CURSOR FOR SELECT [CustomSearchResultId]{selectedColumnsForCursor}\n";

                if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    string updateView = await DbTransientRetryPolicy.GetDatasetUpdateViewScriptAsync(
                        this.ServiceContext,
                        this.Dataset,
                        datasetView,
                        isPostProcess: true,
                        this.DbContext);

                    script += $"FROM ({updateView}) uv\n";
                    script += $"WHERE uv.Major = '{this.SingleRowExecutionData.Major}' AND uv.Minor = '{this.SingleRowExecutionData.Minor}';\n";
                }
                else
                {
                    script += $"FROM {viewName}\n";
                }

                script += "OPEN DatasetInfo\n";

                string fetchParameters = string.Join(", ", storedProcedureParameters.Select(p => $"@{p}"));
                string fetchScript = $"FETCH NEXT FROM DatasetInfo INTO @CustomSearchResultId, {fetchParameters}\n";
                script += fetchScript;
                script += "WHILE @@fetch_status = 0\n";
                script += "BEGIN\n";

                var columnNamesAndTypesText = StoredProcedureUpdatePostProcessExecutor.GetColumnNamesAndTypesScript(datasetPostProcess);
                script += $"DECLARE @Temp_Table TABLE ({columnNamesAndTypesText})\n";
                script += $"DECLARE @BackendUpdates NVARCHAR(MAX) = NULL\n";
                script += "BEGIN TRY\n";

                string insertIntoParameters = string.Join(", ", storedProcedureParameters.Select(p => $"@{p} = @{p}"));
                script += $"INSERT INTO @Temp_Table exec [cus].[{storedProcedureName}] {insertIntoParameters}," +
                    " @backendUpdates = @BackendUpdates OUTPUT," +
                    $" @{ImportStoredProcedureUpdatePostProcessService.IsSchemaProbeParameterName} = false\n";

                script += $"INSERT INTO {tableFullName}\n";

                string major = this.SingleRowExecutionData.IsSingleRowExecutionMode ? this.SingleRowExecutionData.Major : "NULL";
                string minor = this.SingleRowExecutionData.IsSingleRowExecutionMode ? this.SingleRowExecutionData.Minor : "NULL";
                string columnNames = string.Join(", ", expressionColumnNames.Select(c => $"[{c}]"));
                script += $"SELECT @CustomSearchResultId, {columnNames} from @Temp_Table\n";

                script += "IF ((@BackendUpdates IS NOT NULL) AND (@BackendUpdates != ''))\n";
                script += "BEGIN\n";
                script += "INSERT INTO [cus].BackendUpdate ([DatasetPostProcessId], [SingleRowMajor], [SingleRowMinor], [UpdatesJson], [ExportState], [ExportError])\n";
                script += $"VALUES ({this.DatasetPostProcess.DatasetPostProcessId},'{major}', '{minor}', @BackendUpdates, NULL, NULL);\n";
                script += "END\n";

                script += "END TRY\n";

                script += "BEGIN CATCH\n";
                script += "DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity int, @ErrorState int;\n";
                script += "SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();\n";
                script += "DECLARE @isValidationError AS BIT\n";
                script += "SET @isValidationError = iif(left(trim(@ErrorMessage), 25) = 'BulkUpdateValidationError', 1, 0);\n";
                script += "IF (@isValidationError = 0)\n";
                script += "BEGIN\n";
                script += "raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);\n";
                script += "RETURN;\n";
                script += "END\n";
                script += "SET @FoundValidationError = 1\n";
                script += $"UPDATE {datasetUpdateTableFullName} set [RowVersion] = 0, [IsValid] = 0, [Validated] = 1, [BackendExportState] = 'NotExported', [ErrorMessage] = @ErrorMessage where [CustomSearchResultId] = @CustomSearchResultId\n";
                script += "IF @@ROWCOUNT=0\n";
                script += $"INSERT INTO {datasetUpdateTableFullName}([CustomSearchResultId], [RowVersion], [Validated], [ErrorMessage] ) values(@CustomSearchResultId, 0, 1, @ErrorMessage);\n";
                script += "END CATCH\n";

                script += "DELETE from @Temp_Table\n";
                script += fetchScript;
                script += "END\n";
                script += "CLOSE DatasetInfo\n";
                script += "DEALLOCATE DatasetInfo\n";

                script += "IF (@FoundValidationError = 1)\n";
                script += "BEGIN\n";
                script += $"DELETE FROM [cus].[BackendUpdate] WHERE ([DatasetPostProcessId] = {this.DatasetPostProcess.DatasetPostProcessId})\n";
                if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    script += $" AND [SingleRowMajor] = '{major}' AND [SingleRowMinor] = '{minor}'\n";
                }

                script += "raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);\n";
                script += "END\n";

                await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    script,
                    parameters: null);

                var service = new ExecutePostProcessBackendUpdateService(this.ServiceContext);

                int jobId;
                using (WorkerJobDbContext workerJobDbContext = this.ServiceContext.WorkerJobDbContextFactory.Create())
                {
                    jobId = await service.QueueExecutePostProcessBackendUpdateAsync(
                        this.Dataset.DatasetId,
                        this.DatasetPostProcess.DatasetPostProcessId,
                        this.SingleRowExecutionData.Major,
                        this.SingleRowExecutionData.Minor,
                        this.DbContext,
                        workerJobDbContext);
                }

                string jobResult = await this.ServiceContext.WaitForJobResultAsync(jobId);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(string.Format("Cannot create stored procedure update post process table: '{0}'.", tableName), ex);
            }
        }

        /// <summary>
        /// Calculates the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CalculateViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (postProcessViewPhase == PostProcessViewPhase.PreCommit)
            {
                return;
            }

            DatasetPostProcess datasetPostProcess =
                await this.DbContext.DatasetPostProcess
                .Where(d => d.DatasetPostProcessId == this.DatasetPostProcess.DatasetPostProcessId)
                .Include(d => d.CustomSearchExpression)
                .FirstOrDefaultAsync();

            var expressionColumnNames = datasetPostProcess.CustomSearchExpression
                .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                .OrderBy(c => c.ExecutionOrder)
                .Select(e => e.ColumnName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            await this.UpdateViewScriptWithPostProcessTableAsync(expressionColumnNames);
        }

        /// <summary>
        /// Commits the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CommitViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (postProcessViewPhase == PostProcessViewPhase.PreCommit)
            {
                return;
            }

            await base.CommitViewAsync(postProcessViewPhase);
        }

        /// <summary>
        /// Gets script with the column names and types used in the temporal table declaration.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process including its custom custom search expressions.</param>
        /// <returns>
        /// The script with the column names and types used in the temporal table declaration.
        /// </returns>
        private static string GetColumnNamesAndTypesScript(DatasetPostProcess datasetPostProcess)
        {
            var calculatedColumnExpressions = datasetPostProcess.CustomSearchExpression
                 .Where(e => e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()).OrderBy(e => e.ExecutionOrder);

            string columnNamesAndTypesText = string.Empty;
            foreach (var column in calculatedColumnExpressions)
            {
                DatabaseColumnExtensionData databaseColumnExtensionData =
                    JsonHelper.DeserializeObject<DatabaseColumnExtensionData>(column.ExpressionExtensions);

                columnNamesAndTypesText += $"[{column.ColumnName}] {databaseColumnExtensionData.DatabaseType} NULL,\n";
            }

            columnNamesAndTypesText = columnNamesAndTypesText.TrimEnd(new char[] { ',', '\n' });

            return columnNamesAndTypesText;
        }
    }
}