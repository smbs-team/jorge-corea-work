namespace CustomSearchesServicesLibrary.CustomSearches.Executor
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
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Executor for exception dataset post process.
    /// </summary>
    public class ExceptionDatasetPostProcessExecutor : DatasetPostProcessExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDatasetPostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public ExceptionDatasetPostProcessExecutor(
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
            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            try
            {
                bool usePostProcess = !string.IsNullOrWhiteSpace(this.ViewScript);
                string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, usePostProcess);
                string datasetView = usePostProcess ? this.ViewScript : $"Select * FROM {viewName}";

                DatasetPostProcess datasetPostProcess = await this.DbContext.DatasetPostProcess
                    .Where(d => d.DatasetPostProcessId == this.DatasetPostProcess.DatasetPostProcessId)
                    .Include(d => d.CustomSearchExpression)
                    .Include(d => d.ExceptionPostProcessRule)
                    .ThenInclude(e => e.CustomSearchExpression)
                    .FirstOrDefaultAsync();

                var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

                Dictionary<string, string> traceEnabledFieldsDictionary = new Dictionary<string, string>();
                HashSet<string> traceEnabledFields = null;
                if (!string.IsNullOrWhiteSpace(datasetPostProcess.TraceEnabledFields))
                {
                    traceEnabledFields = JsonHelper.DeserializeObject<string[]>(datasetPostProcess.TraceEnabledFields).Where(f => !string.IsNullOrWhiteSpace(f)).Select(f => f.Trim().ToLower()).ToHashSet();
                }

                // Dictionary where the key is the column name and the value is the column script.
                Dictionary<string, string> columnDictionary = new Dictionary<string, string>();
                Dictionary<string, string> columnDefaultValueDictionary = new Dictionary<string, string>();

                // Generates the default value of the scripts. Default script value is:
                // if default expression is found then use the expression value,
                // else if column is found then use the column value,
                // otherwise the first value found ordered by execution order.
                foreach (var exceptionRule in datasetPostProcess.ExceptionPostProcessRule.OrderBy(e => e.ExecutionOrder))
                {
                    CustomSearchExpression filterExpression = exceptionRule.CustomSearchExpression
                        .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower());
                    CustomSearchExpression valueExpression = exceptionRule.CustomSearchExpression
                        .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

                    string valueScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                                valueExpression.Script,
                                replacementDictionary: null,
                                keyName: null,
                                this.ServiceContext,
                                this.Dataset,
                                throwOnFail: true);

                    string filterScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                                filterExpression.Script,
                                replacementDictionary: null,
                                keyName: null,
                                this.ServiceContext,
                                this.Dataset,
                                throwOnFail: true);

                    string columnNameKey = valueExpression.ColumnName.Trim();
                    if (!columnDictionary.ContainsKey(columnNameKey))
                    {
                        columnDictionary.Add(columnNameKey, string.Empty);

                        if (dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == columnNameKey.ToLower()) != null)
                        {
                            columnDefaultValueDictionary.Add(columnNameKey, columnNameKey);
                        }
                        else
                        {
                            columnDefaultValueDictionary.Add(columnNameKey, valueScript);
                        }

                        if ((traceEnabledFields != null) && traceEnabledFields.Contains(columnNameKey.Trim().ToLower()))
                        {
                            traceEnabledFieldsDictionary.Add(columnNameKey, string.Empty);
                            if (dbColumns.FirstOrDefault(c => c.ColumnName.ToLower() == (columnNameKey + "_RulesTrace").ToLower()) != null)
                            {
                                columnDefaultValueDictionary.Add(columnNameKey + "_RulesTrace", columnNameKey + "_RulesTrace");
                            }
                            else
                            {
                                columnDefaultValueDictionary.Add(columnNameKey + "_RulesTrace", "''");
                            }
                        }
                    }
                    else if (filterScript.Trim().ToLower() == "default")
                    {
                        columnDefaultValueDictionary[columnNameKey] = valueScript;
                    }
                }

                // Generates the full value of the scripts.
                foreach (var exceptionRule in datasetPostProcess.ExceptionPostProcessRule.OrderBy(e => e.ExecutionOrder))
                {
                    CustomSearchExpression filterExpression = exceptionRule.CustomSearchExpression
                        .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower());
                    CustomSearchExpression valueExpression = exceptionRule.CustomSearchExpression
                        .FirstOrDefault(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower());

                    string valueScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                                valueExpression.Script,
                                replacementDictionary: null,
                                keyName: null,
                                this.ServiceContext,
                                this.Dataset,
                                throwOnFail: true);

                    string filterScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                                filterExpression.Script,
                                replacementDictionary: null,
                                keyName: null,
                                this.ServiceContext,
                                this.Dataset,
                                throwOnFail: true);

                    if (filterScript.Trim().ToLower() == "default")
                    {
                        continue;
                    }

                    List<CustomSearchExpression> groupFilterExpressions =
                        datasetPostProcess.CustomSearchExpression
                        .Where(e => (string.IsNullOrWhiteSpace(e.ExpressionGroup) == true) || (e.ExpressionGroup == exceptionRule.GroupName)).ToList();

                    if (groupFilterExpressions != null && groupFilterExpressions.Count > 0)
                    {
                        filterScript = $"({filterScript})";
                        foreach (var groupFilterExpression in groupFilterExpressions)
                        {
                            string groupFilterExpressionScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                                groupFilterExpression.Script,
                                replacementDictionary: null,
                                keyName: null,
                                this.ServiceContext,
                                this.Dataset,
                                throwOnFail: true);

                            filterScript += $" AND ({groupFilterExpressionScript})";
                        }
                    }

                    string columnNameKey = valueExpression.ColumnName.Trim();

                    columnDictionary[columnNameKey] += datasetPostProcess.PostProcessSubType == ExceptionMethodType.MultipleConditionModifier.ToString() ?
                        $"+ IIF({filterScript}, {valueScript}, 0)\n" :
                        $"WHEN {filterScript} THEN {valueScript}\n";

                    if (traceEnabledFieldsDictionary.ContainsKey(columnNameKey))
                    {
                        if (!string.IsNullOrWhiteSpace(filterExpression.ExpressionExtensions))
                        {
                            var expressionExtensions = JsonHelper.DeserializeObject<JObject>(filterExpression.ExpressionExtensions);
                            var traceMessageProperty = expressionExtensions.Properties().FirstOrDefault(p => p.Name.ToLower() == "tracemessage");
                            if (traceMessageProperty != null)
                            {
                                var traceMessageValue = expressionExtensions[traceMessageProperty.Name];
                                if (traceMessageValue != null)
                                {
                                    // Replaces the ColumnValue variable in the trace message .
                                    string traceMessageText = $"{traceMessageValue}".Replace("{ColumnValue}", $" + CAST({valueScript} AS nvarchar) + '. ' + CHAR(13) + CHAR(10)");

                                    traceEnabledFieldsDictionary[columnNameKey] += datasetPostProcess.PostProcessSubType == ExceptionMethodType.MultipleConditionModifier.ToString() ?
                                    $"+ IIF({filterScript}, {traceMessageText}, '')\n" :
                                    $"WHEN {filterScript} THEN {columnDefaultValueDictionary[columnNameKey + "_RulesTrace"]} + {traceMessageText}\n";
                                }
                            }
                        }
                        else
                        {
                            columnNameKey = valueExpression.ColumnName.Trim();
                        }
                    }
                }

                string createIndexesScript = string.Empty;

                string script = this.SingleRowExecutionData.IsSingleRowExecutionMode ? $"INSERT INTO {tableFullName}" : string.Empty;
                script += "SELECT CustomSearchResultId,\n";
                foreach (var column in columnDictionary)
                {
                    string columnValue = string.Empty;
                    if (datasetPostProcess.PostProcessSubType == ExceptionMethodType.MultipleConditionModifier.ToString())
                    {
                        columnValue = $"{column.Value} + {columnDefaultValueDictionary[column.Key]}";
                    }
                    else if (!string.IsNullOrWhiteSpace(column.Value))
                    {
                        columnValue = $"CASE {column.Value} ELSE {columnDefaultValueDictionary[column.Key]} END";
                    }
                    else
                    {
                        columnValue = $"{columnDefaultValueDictionary[column.Key]}";
                    }

                    script += $"{columnValue} AS [{column.Key}],\n";
                    createIndexesScript += $"CREATE NONCLUSTERED INDEX [IX_{tableName}_{column.Key}] ON {tableFullName} ([{column.Key}]);\n";
                }

                foreach (var column in traceEnabledFieldsDictionary)
                {
                    string columnValue = string.Empty;
                    if (datasetPostProcess.PostProcessSubType == ExceptionMethodType.MultipleConditionModifier.ToString())
                    {
                        columnValue = $"{columnDefaultValueDictionary[column.Key + "_RulesTrace"]} + {column.Value}";
                    }
                    else if (!string.IsNullOrWhiteSpace(column.Value))
                    {
                        columnValue = $"CASE {column.Value} ELSE {columnDefaultValueDictionary[column.Key + "_RulesTrace"]} END";
                    }
                    else
                    {
                        columnValue = $"{columnDefaultValueDictionary[column.Key + "_RulesTrace"]}";
                    }

                    script += $"{columnValue} AS [{column.Key}_RulesTrace],\n";
                }

                script = script.TrimEnd(new char[] { ',', '\n' }) + "\n";

                script += this.SingleRowExecutionData.IsSingleRowExecutionMode ? string.Empty : "INTO " + tableFullName + "\n";

                if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
                {
                    string updateView = await DbTransientRetryPolicy.GetDatasetUpdateViewScriptAsync(
                        this.ServiceContext,
                        this.Dataset,
                        datasetView,
                        isPostProcess: true,
                        this.DbContext);

                    script += $"FROM ({updateView}) u\n";
                    script += $"WHERE Major = '{this.SingleRowExecutionData.Major}' AND Minor = '{this.SingleRowExecutionData.Minor}';\n";
                }
                else
                {
                    script += "FROM " + viewName + "\n";
                    script += $"ALTER TABLE {tableFullName} ADD PRIMARY KEY (CustomSearchResultId);\n";
                    script += createIndexesScript;
                }

                await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    script,
                    parameters: null);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(string.Format("Cannot create exception dataset post process table: '{0}'.", tableFullName), ex);
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
                .Include(d => d.ExceptionPostProcessRule)
                    .ThenInclude(eppr => eppr.CustomSearchExpression)
                .FirstOrDefaultAsync();

            var expressionColumnNames = datasetPostProcess
                .ExceptionPostProcessRule
                .SelectMany(r => r.CustomSearchExpression.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.CalculatedColumn.ToString())
                .Select(e => e.ColumnName.Trim())).ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(datasetPostProcess.TraceEnabledFields))
            {
                HashSet<string> traceEnabledColumnNames = JsonHelper.DeserializeObject<string[]>(datasetPostProcess.TraceEnabledFields)
                    .Where(f => !string.IsNullOrWhiteSpace(f))
                    .Select(f => f.Trim())
                    .Intersect(expressionColumnNames, StringComparer.OrdinalIgnoreCase)
                    .Select(f => f + "_RulesTrace")
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                expressionColumnNames = expressionColumnNames.Union(traceEnabledColumnNames, StringComparer.OrdinalIgnoreCase).ToHashSet();
            }

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
    }
}
