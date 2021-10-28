namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScript;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Executor for RScript dataset post process.
    /// </summary>
    public class RScriptDatasetPostProcessExecutor : DatasetPostProcessExecutor
    {
        /// <summary>
        /// The predicted variable name.
        /// </summary>
        public const string PredictedVariableName = "Predicted";

        /// <summary>
        /// Initializes a new instance of the <see cref="RScriptDatasetPostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public RScriptDatasetPostProcessExecutor(
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
        /// Checks that the result is not NA.  Throws an exception if it is.
        /// </summary>
        /// <param name="resultObject">The result object.</param>
        /// <param name="resultName">Name of the result.</param>
        /// <returns>
        /// The result value.
        /// </returns>
        public static float CheckNotNA(JObject resultObject, string resultName)
        {
            if (resultObject[resultName].ToString().ToLower() == "na")
            {
                throw new InvalidOperationException($"The regression calculation was not successful.  Value of '{resultName}' is NA.", null);
            }

            return (float)resultObject[resultName];
        }

        /// <summary>
        /// Gets the regression results properties.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <returns>
        /// The result value.
        /// </returns>
        public static async Task<List<JProperty>> GetRegressionResultsPropertiesAsync(
            DatasetPostProcess datasetPostProcess,
            CustomSearchesDbContext dbContext,
            Dictionary<string, string> replacementDictionary)
        {
            RScriptResultsData rScriptResultsData = JsonHelper.DeserializeObject<RScriptResultsData>(datasetPostProcess.ResultPayload);
            if (rScriptResultsData.Status?.ToLower() != "success")
            {
                FailedJobResult failedJobResult = JsonHelper.DeserializeObject<FailedJobResult>(datasetPostProcess.ResultPayload);
                string reason = "RScript post process execution failed (couldn't retrieve RScript results).";
                if (!string.IsNullOrWhiteSpace(failedJobResult.Reason))
                {
                    reason += $" Reason: '{failedJobResult.Reason}'.";
                }

                throw new CustomSearchesConflictException(reason, innerException: null);
            }

            if (datasetPostProcess.RscriptModel == null)
            {
                await dbContext.Entry(datasetPostProcess).Reference(dpp => dpp.RscriptModel).LoadAsync();
            }

            var results = rScriptResultsData.Results;
            foreach (JObject rscriptResult in results)
            {
                var resultName = (string)rscriptResult["name"];
                List<JProperty> regressionResultsProperties = null;
                if (!string.IsNullOrWhiteSpace(resultName) && resultName.ToLower() == "regression_results")
                {
                    regressionResultsProperties = rscriptResult.Properties().Where(p => p.Name.ToLower() != "name").ToList();
                    foreach (var property in regressionResultsProperties)
                    {
                        var propertyValue = RScriptDatasetPostProcessExecutor.CheckNotNA(rscriptResult, property.Name);
                        replacementDictionary.Add(property.Name, propertyValue.ToString());
                    }

                    return regressionResultsProperties;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the linear predicted expression.  This is calculated based on the results and expressions.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="regressionResultsProperties">The regression results properties.</param>
        /// <param name="predictedPrecision">The precision of the predicted values.</param>
        /// <returns>
        /// The default linear predicted expression.
        /// </returns>
        public static string GetLinearPredictedExpression(
            DatasetPostProcess datasetPostProcess,
            List<JProperty> regressionResultsProperties,
            int predictedPrecision = -1)
        {
            string toReturn = string.Empty;

            List<JProperty> coefficientProperties = new List<JProperty>();
            coefficientProperties.AddRange(regressionResultsProperties);

            List<string> independentVariableNames =
                (from exp in datasetPostProcess.CustomSearchExpression
                 where exp.ExpressionRole == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent.ToString()
                 select exp.Script).ToList();

            JProperty interceptProperty =
                (from p in coefficientProperties
                 where p.Name.ToLower() == "intercept"
                 select p).FirstOrDefault();

            if (interceptProperty != null)
            {
                coefficientProperties.Remove(interceptProperty);
            }

            if (independentVariableNames.Count == 0)
            {
                return toReturn;
            }

            if (coefficientProperties.Count != independentVariableNames.Count)
            {
                throw new CustomSearchesConflictException(
                   "Can't generate a {Predicted} expression for the RScript Regression because the number of regression coefficient results" +
                   $"({coefficientProperties.Count}) does not match the number of independent variables ({independentVariableNames.Count}).",
                   innerException: null);
            }

            string precision = null;
            if (predictedPrecision >= 0)
            {
                precision = $"F{predictedPrecision}";
            }

            if (interceptProperty != null)
            {
                string inteceptValue = predictedPrecision >= 0 ? ((float)interceptProperty.Value).ToString(precision) : interceptProperty.Value.ToString();
                toReturn += $"{inteceptValue} + ";
            }

            int i = 0;
            foreach (var coefficient in coefficientProperties)
            {
                string coefficientValue = predictedPrecision >= 0 ? ((float)coefficient.Value).ToString(precision) : coefficient.Value.ToString();
                string variableName = independentVariableNames[i];
                toReturn += $"{coefficientValue}*[{variableName}]";
                if (i < (coefficientProperties.Count - 1))
                {
                    toReturn += " + ";
                }

                i++;
            }

            return toReturn;
        }

        /// <summary>
        /// Adds the r script results as replacements.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="predictedPrecision">The precision (number of decimals) of the predicted values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static async Task AddRScriptResultsAsReplacementsAsync(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext,
            Dictionary<string, string> replacementDictionary,
            int predictedPrecision = -1)
        {
            var regressionResultsProperties =
                await RScriptDatasetPostProcessExecutor.GetRegressionResultsPropertiesAsync(datasetPostProcess, dbContext, replacementDictionary);

            string predictedExpression = datasetPostProcess.RscriptModel.PredictedTsqlExpression;

            // Tries to calculate the predicted expression if one was not provided.
            if (string.IsNullOrWhiteSpace(predictedExpression))
            {
                predictedExpression = RScriptDatasetPostProcessExecutor.GetLinearPredictedExpression(datasetPostProcess, regressionResultsProperties, predictedPrecision);
            }

            if (!string.IsNullOrWhiteSpace(predictedExpression))
            {
                await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                    predictedExpression,
                    replacementDictionary,
                    RScriptDatasetPostProcessExecutor.PredictedVariableName,
                    serviceContext,
                    dataset);
            }
        }

        /// <summary>
        /// Creates the RScript post process table.
        /// </summary>
        /// <param name="postProcess">The post process.</param>
        /// <param name="csvHeaders">The csv headers.</param>
        /// <param name="csvIndexKeys">The csv index key.</param>
        /// <param name="csvRows">The csv rows.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public static async Task CreateRScriptPostProcessTableAsync(
            DatasetPostProcess postProcess,
            string[] csvHeaders,
            int[] csvIndexKeys,
            List<string[]> csvRows,
            IServiceContext serviceContext,
            CustomSearchesDbContext dbContext)
        {
            var expressions = dbContext.CustomSearchExpression.Where(
                e =>
                    e.DatasetPostProcessId == postProcess.DatasetPostProcessId &&
                    e.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() &&
                    e.ExpressionType.Trim().ToLower() == CustomSearchExpressionType.RScript.ToString().ToLower());

            if (expressions.Count() == 0)
            {
                return;
            }

            string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(postProcess.Dataset, usePostProcess: true);

            string tableFullName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(postProcess.Dataset, postProcess.DatasetPostProcessId);
            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableName(postProcess.Dataset, postProcess.DatasetPostProcessId);

            string createTableScript = $"CREATE TABLE {tableFullName}(\n" +
                "[CustomSearchResultId] int NOT NULL PRIMARY KEY,\n";

            string[] firstCsvRow = null;
            if (csvRows.Count() > 0)
            {
                firstCsvRow = csvRows[0];
            }

            Dictionary<int, string> columnNameByIndex = new Dictionary<int, string>();
            Dictionary<int, bool> isNumericColumnHash = new Dictionary<int, bool>();

            foreach (var expression in expressions)
            {
                var columnName = expression.ColumnName;
                var headerName = csvHeaders.FirstOrDefault(h => h.Trim().ToLower() == expression.Script.Trim().ToLower());

                if (string.IsNullOrWhiteSpace(headerName))
                {
                    throw new CustomSearchesConflictException(
                        $"The header '{expression.Script}' was not found in the csv output.",
                        innerException: null);
                }

                var headerIndex = Array.IndexOf(csvHeaders, headerName);

                if (!columnNameByIndex.ContainsKey(headerIndex))
                {
                    columnNameByIndex.Add(headerIndex, columnName);
                }

                string databaseType = "float";
                isNumericColumnHash[headerIndex] = true;
                if (firstCsvRow != null && !IsNumericColumn(csvRows, headerIndex))
                {
                    databaseType = "nvarchar(MAX)";
                    isNumericColumnHash[headerIndex] = false;
                }

                createTableScript += $"[{columnName}] {databaseType} NULL,\n";

                if (databaseType == "float")
                {
                    createTableScript += $"INDEX [IX_{tableName}_{columnName}] NONCLUSTERED ({columnName}),\n";
                }
            }

            createTableScript = createTableScript.TrimEnd(new char[] { ',', '\n' });
            createTableScript += ")";

            await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                createTableScript,
                parameters: null);

            var insertIntoColumnsScript = string.Join(", ", columnNameByIndex.Keys.Select(k => $"[{columnNameByIndex[k]}]"));
            insertIntoColumnsScript = $"([CustomSearchResultId], {insertIntoColumnsScript})";
            string batchScript = string.Empty;

            while (csvRows.Count > 0)
            {
                int batchSize = Math.Min(csvRows.Count, UpdateDatasetDataService.UpdateBatchSize);
                var rows = csvRows.GetRange(0, batchSize);
                foreach (var row in rows)
                {
                    string rowValuesScript = string.Join(", ", columnNameByIndex.Keys.Select(k => WrapRowValue(row[k], isNumericColumnHash[k])));
                    string whereConditionsScript = string.Join(" AND ", csvIndexKeys.Select(k => $"[{csvHeaders[k]}] = {row[k]}"));

                    batchScript +=
                        $"INSERT INTO {tableFullName} {insertIntoColumnsScript} " +
                        $"SELECT TOP (1) [CustomSearchResultId], {rowValuesScript} " +
                        $"FROM {viewName} " +
                        $"WHERE {whereConditionsScript}";

                    batchScript += "\n";
                }

                await DbTransientRetryPolicy.ExecuteNonQueryAsync(
                    serviceContext,
                    serviceContext.DataDbContextFactory,
                    batchScript,
                    parameters: null);

                batchScript = string.Empty;
                csvRows.RemoveRange(0, batchSize);
            }
        }

        /// <summary>
        /// Deletes the previous state.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task DeletePreviousStateAsync()
        {
            if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                return;
            }

            string tableName = CustomSearchesDataDbContext.GetDatasetPostProcessTableFullName(this.Dataset, this.DatasetPostProcess.DatasetPostProcessId);
            try
            {
                await DbTransientRetryPolicy.DropTableAsync(
                    this.ServiceContext,
                    this.ServiceContext.DataDbContextFactory,
                    tableName);
            }
            catch (SqlException ex)
            {
                throw new CustomSearchesDatabaseException(string.Format("Cannot drop dataset table: '{0}'.", tableName), ex);
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
            if (this.SingleRowExecutionData.IsSingleRowExecutionMode)
            {
                return;
            }

            // The primary post process executes the rscript job.
            if (this.DatasetPostProcess.PrimaryDatasetPostProcessId == null)
            {
                int jobId;
                try
                {
                    RScriptJobPayloadData payload = new RScriptJobPayloadData();
                    payload.DatasetPostProcessId = this.DatasetPostProcess.DatasetPostProcessId;
                    Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                    jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                        "RScript",
                        "RScriptJobType",
                        userId,
                        payload,
                        WorkerJobTimeouts.RScriptTimeout);
                }
                catch (SqlException ex)
                {
                    throw new CustomSearchesDatabaseException("Cannot queue RScriptJob.", ex);
                }

                this.DatasetPostProcess.ResultPayload = await this.ServiceContext.WaitForJobResultAsync(jobId);
            }
            else
            {
                // The secondary post process takes the result of the primary post process.
                this.DatasetPostProcess.ResultPayload = await this.DbContext.DatasetPostProcess.
                    Where(dpp => dpp.DatasetPostProcessId == this.DatasetPostProcess.PrimaryDatasetPostProcessId).
                    Select(dpp => dpp.ResultPayload).
                    FirstOrDefaultAsync();
            }

            this.DatasetPostProcess.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            this.DatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
            this.DbContext.DatasetPostProcess.Update(this.DatasetPostProcess);
            await this.DbContext.ValidateAndSaveChangesAsync();
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
            if (postProcessViewPhase == PostProcessViewPhase.PostCommit)
            {
                if (this.DatasetPostProcess.ResultPayload == null)
                {
                    return;
                }
            }

            int datasetPostProcessId = this.DatasetPostProcess.DatasetPostProcessId;
            string expressionRole = postProcessViewPhase == PostProcessViewPhase.PostCommit ?
                "CalculatedColumnPostCommit".ToLower() : "CalculatedColumnPreCommit".ToLower();

            var customSearchExpressions = await this.DbContext.CustomSearchExpression.Where(e => e.DatasetPostProcessId == datasetPostProcessId).ToListAsync();

            var calculatedColumnCommitExpressions = customSearchExpressions
                .Where(c => c.ExpressionRole.ToLower().StartsWith(expressionRole) && c.ExpressionType == CustomSearchExpressionType.TSQL.ToString())
                .OrderBy(c => c.ExecutionOrder).ToArray();

            if (calculatedColumnCommitExpressions.Length > 0)
            {
                // Init replacement dictionary with RScript parameters.
                Dictionary<string, string> replacementDictionary = customSearchExpressions
                    .Where(c => c.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.RScriptParameter.ToString().ToLower() &&
                        c.ExpressionType == CustomSearchExpressionType.RScript.ToString())
                    .ToDictionary(e => e.ColumnName, e => $"({e.Script})");

                if (postProcessViewPhase == PostProcessViewPhase.PostCommit)
                {
                    await RScriptDatasetPostProcessExecutor.AddRScriptResultsAsReplacementsAsync(
                        this.Dataset,
                        this.DatasetPostProcess,
                        this.DbContext,
                        this.ServiceContext,
                        replacementDictionary);

                    await this.CalculateViewWithPostProcessTableAsync(CustomSearchExpressionType.RScript);
                }

                bool usePostProcess = !string.IsNullOrWhiteSpace(this.ViewScript);
                string datasetView = DatasetHelper.GetDatasetView(
                    this.Dataset,
                    usePostProcess,
                    datasetPostProcess: postProcessViewPhase == PostProcessViewPhase.PostCommit ? this.DatasetPostProcess : null);
                datasetView = usePostProcess ? this.ViewScript : $"Select * FROM {datasetView}";

                var dbColumns = await DbTransientRetryPolicy.GetDatasetViewColumnSchemaAsync(this.ServiceContext, datasetView);

                string columnScript = string.Empty;
                Dictionary<string, string> expressionsToAdd = new Dictionary<string, string>();
                foreach (var customSearchExpression in calculatedColumnCommitExpressions)
                {
                    string customSearchExpressionScript = customSearchExpression.Script;
                    customSearchExpressionScript =
                        await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                            customSearchExpressionScript,
                            replacementDictionary,
                            customSearchExpression.ColumnName,
                            this.ServiceContext,
                            this.Dataset);

                    expressionsToAdd.Add(customSearchExpression.ColumnName, customSearchExpressionScript);
                }

                foreach (DbColumn column in dbColumns)
                {
                    string colName = column.ColumnName;
                    if (expressionsToAdd.ContainsKey(colName))
                    {
                        columnScript += $"({expressionsToAdd[colName]}) AS [{colName}]";
                        expressionsToAdd.Remove(colName);
                    }
                    else
                    {
                        columnScript += $"[{colName}]";
                    }

                    columnScript += ", ";
                }

                foreach (var expression in expressionsToAdd)
                {
                    columnScript += "(" + expression.Value + ") AS [" + expression.Key + "], ";
                }

                columnScript = columnScript.TrimEnd(new char[] { ',', ' ' });

                string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, !string.IsNullOrWhiteSpace(this.ViewScript) /*usePostProcess*/);
                string previousViewScript = string.IsNullOrWhiteSpace(this.ViewScript) ? ("SELECT * FROM " + viewName) : this.ViewScript;

                string script = "SELECT " + columnScript + "\n";
                script += "FROM (" + previousViewScript + ") a\n";
                this.ViewScript = script;
            }
        }

        /// <summary>
        /// Wraps the row value in quotes if this is not a numeric value.
        /// </summary>
        /// <param name="rowValue">The row value.</param>
        /// <param name="isNumericColumn">If set to <c>true</c> this is a numeric column.</param>
        /// <returns>The conditionally wrapped value.</returns>
        private static string WrapRowValue(string rowValue, bool isNumericColumn)
        {
            if (isNumericColumn)
            {
                return rowValue;
            }

            return "'" + rowValue + "'";
        }

        /// <summary>
        /// Determines whether a csv column is numeric.
        /// </summary>
        /// <param name="csvRows">The CSV rows.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        ///   <c>true</c> if the column is numeric.
        /// </returns>
        private static bool IsNumericColumn(List<string[]> csvRows, int columnIndex)
        {
            foreach (var row in csvRows)
            {
                if (!Information.IsNumeric(row[columnIndex]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
