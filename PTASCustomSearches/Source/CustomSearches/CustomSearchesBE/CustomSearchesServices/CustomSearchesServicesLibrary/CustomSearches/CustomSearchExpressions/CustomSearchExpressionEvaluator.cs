namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Evaluator for custom search expressions.
    /// </summary>
    public class CustomSearchExpressionEvaluator
    {
        /// <summary>
        /// Evaluates the custom search expression.
        /// </summary>
        /// <typeparam name="T">The type of each stack element.</typeparam>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="expressionType">The expression type.</param>
        /// <param name="script">The script.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        public static async Task<T> EvaluateAsync<T, TDbContext>(
            IServiceContext serviceContext,
            IFactory<TDbContext> dbContextFactory,
            string expressionType,
            string script,
            Dictionary<string, string> parameters)
            where TDbContext : DbContext
        {
            string replacedScript = CustomSearchExpressionEvaluator.ReplaceParameters(script, parameters);

            if (expressionType.ToLower() == "jsonpayload")
            {
                try
                {
                    return JsonHelper.DeserializeObject<T>(replacedScript);
                }
                catch (JsonReaderException ex)
                {
                    throw new CustomSearchesJsonException(
                        $"Can't evaluate json expression '{replacedScript}'.",
                        innerException: ex);
                }
            }

            if (expressionType.ToLower() == "tsql")
            {
                var results = await DynamicSqlStatementHelper.GetDynamicResultWithRetriesAsync<TDbContext>(
                    serviceContext,
                    dbContextFactory,
                    replacedScript,
                    $"Can't evaluate expression: '{replacedScript}'.");

                string stringResults = JsonHelper.SerializeObject(results);
                return JsonHelper.DeserializeObject<T>(stringResults);
            }

            return default;
        }

        /// <summary>
        /// Evaluates the formula expression.
        /// </summary>
        /// <typeparam name="T">The type of each stack element.</typeparam>
        /// <param name="formula">The formula to evaluate.</param>
        /// <param name="parameters">The parameters to evaluate.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesJsonException">Exception while evaluating json expression.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Exception while evaluating sql expression.</exception>
        public static async Task<T> EvaluateFormulaAsync<T>(string formula, Dictionary<string, string> parameters, IServiceContext serviceContext, Dataset dataset)
        {
            string replacedFormula = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(formula, parameters, keyName: null, serviceContext, dataset);
            string script = $"Select {replacedFormula}";

            var result = await DynamicSqlStatementHelper.ExecuteScalarWithRetriesAsync(
                serviceContext,
                serviceContext.DataDbContextFactory,
                script,
                "Update statement failed in the database while applying the query row filter.");

            string stringResult = JsonHelper.SerializeObject(result);
            return JsonHelper.DeserializeObject<T>(stringResult);
        }

        /// <summary>
        /// Checks if the column is used in expressions.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="customSearchExpressions">The custom search expressions.</param>
        /// <returns>
        /// True if the column is used in expressions, otherwise false.
        /// </returns>
        public static bool IsColumnUsedInExpressions(string columnName, ICollection<CustomSearchExpression> customSearchExpressions)
        {
            foreach (var customSearchExpression in customSearchExpressions)
            {
                if (IsColumnUsedInExpression(columnName, customSearchExpression))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the column is used in the expression.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="customSearchExpression">The custom search expression.</param>
        /// <returns>
        /// True if the column is used in the expression, otherwise false.
        /// </returns>
        public static bool IsColumnUsedInExpression(string columnName, CustomSearchExpression customSearchExpression)
        {
            string script = customSearchExpression.Script;
            if (!string.IsNullOrWhiteSpace(columnName) &&
                !string.IsNullOrWhiteSpace(script) &&
                customSearchExpression.ExpressionType.Trim().ToLower() != CustomSearchExpressionType.RScript.ToString().ToLower())
            {
                // Removes all variables in brackets because they don't represent columns.
                var matches = RegExHelper.FindVariableReplacements(script);

                var toRemoveArray = matches.Select(m => m.Value).ToArray();

                foreach (var toRemove in toRemoveArray)
                {
                    script.Replace(toRemove, string.Empty);
                }

                if (script.ToLower().Contains(columnName.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the max value from the between expression.
        /// </summary>
        /// <param name="customSearchExpression">The custom search expression.</param>
        /// <returns>
        /// True if the column is used in the expression, otherwise false.
        /// </returns>
        public static float GetMaxValueFromBetweenExpression(CustomSearchExpression customSearchExpression)
        {
            string[] scriptWords = customSearchExpression.Script?.ToLower().Split(" ");
            float result;
            if (scriptWords == null ||
                scriptWords.Length < 5 ||
                !scriptWords.Contains("between") ||
                !scriptWords.Contains("and") ||
                !float.TryParse(scriptWords.LastOrDefault(), out result))
            {
                throw new CustomExpressionValidationException(
                    $"Invalid 'between' expression script: '{customSearchExpression.Script}'." +
                    $" The script should have the following format 'column_name BETWEEN value1 AND value2'",
                    validationResults: null,
                    CustomExpressionValidationExceptionType.Execution,
                    innerException: null);
            }

            return result;
        }

        /// <summary>
        /// Gets the ordered custom search expressions from the dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns>
        /// The custom search expressions in order.
        /// </returns>
        public static List<CustomSearchExpression> GetOrderedCustomSearchExpressions(Dataset dataset)
        {
            List<CustomSearchExpression> result = new List<CustomSearchExpression>();

            // Adds the expressions of the dataset ordered by ExecutionOrder.
            if ((dataset.CustomSearchExpression != null) && (dataset.CustomSearchExpression.Count > 0))
            {
                result.AddRange(dataset.CustomSearchExpression.OrderBy(e => e.ExecutionOrder));
            }

            // Adds the expressions of the dataset post processes ordered by Priority and ExecutionOrder.
            if ((dataset.DatasetPostProcess != null) && (dataset.DatasetPostProcess.Count > 0))
            {
                var datasetPostProcesses = dataset.DatasetPostProcess.OrderBy(p => p.Priority).ThenBy(p => p.ExecutionOrder).ThenBy(p => p.CreatedBy);
                foreach (var datasetPostProcess in datasetPostProcesses)
                {
                    result.AddRange(GetOrderedCustomSearchExpressions(datasetPostProcess));
                }
            }

            // Adds the expressions of the interactive charts ordered by ExecutionOrder.
            if ((dataset.InteractiveChart != null) && (dataset.InteractiveChart.Count > 0))
            {
                foreach (var interactiveChart in dataset.InteractiveChart)
                {
                    if ((interactiveChart.CustomSearchExpression != null) && (interactiveChart.CustomSearchExpression.Count > 0))
                    {
                        result.AddRange(interactiveChart.CustomSearchExpression.OrderBy(e => e.ExecutionOrder));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the orderer custom search expressions from the dataset post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>
        /// The custom search expressions in order.
        /// </returns>
        public static List<CustomSearchExpression> GetOrderedCustomSearchExpressions(DatasetPostProcess datasetPostProcess)
        {
            List<CustomSearchExpression> result = new List<CustomSearchExpression>();

            if ((datasetPostProcess.CustomSearchExpression != null) && (datasetPostProcess.CustomSearchExpression.Count > 0))
            {
                // If the expressions belong to a rscript post process
                // then they must be added in this order: pre-commit, rscript an post-commit.
                if ((datasetPostProcess.RscriptModelId != null) && (datasetPostProcess.RscriptModelId > 0))
                {
                    // Adds the pre-commit expressions ordered by ExecutionOrder.
                    var preCommitExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumnPreCommit.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(preCommitExpressions);

                    // Adds the rscript expressions ordered by ExecutionOrder.
                    var rscriptExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.RScriptParameter.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.RScript.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(rscriptExpressions);

                    // Adds the rscript calculated column expressions ordered by ExecutionOrder.
                    var rscriptCalculatedColumnExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.RScript.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(rscriptCalculatedColumnExpressions);

                    // Adds the post-commit expressions ordered by ExecutionOrder.
                    var postCommitExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumnPostCommit.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(preCommitExpressions);
                }
                else if (datasetPostProcess.PostProcessType.Trim().ToLower() == DatasetPostProcessType.CustomModelingStepPostProcess.ToString().ToLower())
                {
                    // Adds the pre-commit expressions ordered by ExecutionOrder.
                    var preCommitExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumnPreCommit.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(preCommitExpressions);

                    // Adds the imported calculated column expressions ordered by ExecutionOrder.
                    var importedCalculatedColumnExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.Imported.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(importedCalculatedColumnExpressions);

                    // Adds the post-commit expressions ordered by ExecutionOrder.
                    var postCommitExpressions = datasetPostProcess.CustomSearchExpression
                        .Where(
                        e =>
                            e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumnPostCommit.ToString().ToLower()) &&
                            (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()))
                        .OrderBy(e => e.ExecutionOrder);
                    result.AddRange(preCommitExpressions);
                }
                else
                {
                    // Adds the expressions of the dataset post process ordered by ExecutionOrder.
                    result.AddRange(
                        datasetPostProcess.CustomSearchExpression
                        .Where(e => e.ExpressionType == CustomSearchExpressionType.TSQL.ToString())
                        .OrderBy(e => e.ExecutionOrder));
                }
            }

            // If this is an exception post process also adds the expressions of the exception post process rules.
            if ((datasetPostProcess.ExceptionPostProcessRule != null) && (datasetPostProcess.ExceptionPostProcessRule.Count > 0))
            {
                foreach (var exceptionPostProcessRule in datasetPostProcess.ExceptionPostProcessRule)
                {
                    if ((exceptionPostProcessRule.CustomSearchExpression != null) && (exceptionPostProcessRule.CustomSearchExpression.Count > 0))
                    {
                        // Adds the filter expression of the exception post process rule.
                        CustomSearchExpression filterExpression =
                            exceptionPostProcessRule.CustomSearchExpression
                            .FirstOrDefault(
                                e =>
                                    (e.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower()) &&
                                    (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()));
                        result.Add(filterExpression);

                        // Adds the calculated column expression of the exception post process rule.
                        CustomSearchExpression valueExpression =
                            exceptionPostProcessRule.CustomSearchExpression
                            .FirstOrDefault(
                                e => (e.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()) &&
                                (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()));

                        result.Add(valueExpression);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Validates if the column is declared before being used in expressions.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="customSearchExpressions">The custom search expressions.</param>
        public static void AssertExpressionReferencesToColumnAreValid(string columnName, ICollection<CustomSearchExpression> customSearchExpressions)
        {
            foreach (var customSearchExpression in customSearchExpressions)
            {
                // If the column is declared before being used then it is valid.
                if (customSearchExpression.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()) &&
                    (customSearchExpression.ColumnName?.ToLower() == columnName.ToLower()))
                {
                    return;
                }

                // If the column is used before being declared throw an exception.
                if (IsColumnUsedInExpression(columnName, customSearchExpression))
                {
                    if (customSearchExpression.DatasetId != null)
                    {
                        throw new CustomExpressionValidationException(
                            $"Column '{columnName}' is used in the dataset.",
                            validationResults: null,
                            CustomExpressionValidationExceptionType.Reference,
                            innerException: null);
                    }
                    else if ((customSearchExpression.DatasetPostProcessId != null) && (customSearchExpression.DatasetPostProcessId > 0))
                    {
                        throw new CustomExpressionValidationException(
                            $"Column '{columnName}' is used in the dataset post process '{customSearchExpression.DatasetPostProcessId}'.",
                            validationResults: null,
                            CustomExpressionValidationExceptionType.Reference,
                            innerException: null);
                    }
                    else if ((customSearchExpression.ExceptionPostProcessRuleId != null) && (customSearchExpression.ExceptionPostProcessRuleId > 0))
                    {
                        throw new CustomExpressionValidationException(
                            $"Column '{columnName}' is used in the exception post process rule '{customSearchExpression.ExceptionPostProcessRuleId}'.",
                            validationResults: null,
                            CustomExpressionValidationExceptionType.Reference,
                            innerException: null);
                    }
                    else if ((customSearchExpression.DatasetChartId != null) && (customSearchExpression.DatasetChartId > 0))
                    {
                        throw new CustomExpressionValidationException(
                            $"Column '{columnName}' is used in the chart '{customSearchExpression.DatasetChartId}'.",
                            validationResults: null,
                            CustomExpressionValidationExceptionType.Reference,
                            innerException: null);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the use of the columns in the dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbColumns">The database column schema.</param>
        /// <param name="columnsToValidate">The columns to validate.</param>
        public static void AssertExpressionReferencesToColumnsAreValid(Dataset dataset, ReadOnlyCollection<DbColumn> dbColumns, IEnumerable<string> columnsToValidate)
        {
            // We can remove of the analysis the columns found in the schema.
            if ((dbColumns != null) && (columnsToValidate != null))
            {
                columnsToValidate = columnsToValidate.Except(dbColumns.Select(c => c.ColumnName));
            }

            if ((columnsToValidate == null) || columnsToValidate.Count() == 0)
            {
                return;
            }

            // Gets the ordered custom search expressions from the dataset.
            var orderedExpressions = GetOrderedCustomSearchExpressions(dataset).ToList();

            foreach (var columnToValidate in columnsToValidate)
            {
                // Validates if the column is declared before being used in expressions.
                AssertExpressionReferencesToColumnAreValid(columnToValidate, orderedExpressions);
            }
        }

        /// <summary>
        /// Gets the calculated column names used in expressions.
        /// </summary>
        /// <param name="customSearchExpressions">The custom search expressions.</param>
        /// <returns>
        /// The column names.
        /// </returns>
        public static IEnumerable<string> GetCalculatedColumnNames(ICollection<CustomSearchExpression> customSearchExpressions)
        {
            return customSearchExpressions
                .Where(
                    e =>
                        (e.ExpressionType == CustomSearchExpressionType.TSQL.ToString()) &&
                        e.ExpressionRole.ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()))
                .Select(e => e.ColumnName)
                .Distinct();
        }

        /// <summary>
        /// Gets the calculated column names used in the dataset post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <returns>
        /// The column names.
        /// </returns>
        public static IEnumerable<string> GetCalculatedColumnNames(DatasetPostProcess datasetPostProcess)
        {
            List<CustomSearchExpression> datasetPostProcessExpressions = new List<CustomSearchExpression>();
            if ((datasetPostProcess.CustomSearchExpression != null) && (datasetPostProcess.CustomSearchExpression.Count > 0))
            {
                datasetPostProcessExpressions.AddRange(datasetPostProcess.CustomSearchExpression);
            }

            if ((datasetPostProcess.ExceptionPostProcessRule != null) && (datasetPostProcess.ExceptionPostProcessRule.Count > 0))
            {
                foreach (var exceptionPostProcessRule in datasetPostProcess.ExceptionPostProcessRule)
                {
                    datasetPostProcessExpressions.AddRange(exceptionPostProcessRule.CustomSearchExpression);
                }
            }

            return GetCalculatedColumnNames(datasetPostProcessExpressions);
        }

        /// <summary>
        /// Replaces variables in expressions.
        /// </summary>
        /// <param name="customSearchExpressionScript">The custom search expression script.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="keyName">The key to add in the replacement dictionary. Null if not should be added to dictionary.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="throwOnFail">Value indicating whether should throw a exception when a variable can't be replaced.</param>
        /// <returns>
        /// The modified custom searh expression script.
        /// </returns>
        public static async Task<string> ReplaceVariablesAsync(
            string customSearchExpressionScript,
            Dictionary<string, string> replacementDictionary,
            string keyName,
            IServiceContext serviceContext,
            Dataset dataset,
            bool throwOnFail = false)
        {
            if (replacementDictionary == null)
            {
                replacementDictionary = new Dictionary<string, string>();
            }

            var project = await DatasetHelper.GetOwnerProjectAsync(dataset, serviceContext);
            ProjectVariableHelper.AddProjectVariables(project, replacementDictionary);

            string toReturn = customSearchExpressionScript;
            var matches = RegExHelper.FindVariableReplacements(customSearchExpressionScript);

            var toReplaceArray = matches.Select(m => m.Value).ToHashSet();
            foreach (var toReplace in toReplaceArray)
            {
                string key = toReplace.Replace("{", string.Empty).Replace("}", string.Empty).Trim();

                // If the key is not found look in the metadata store
                if (!replacementDictionary.ContainsKey(key))
                {
                    using (var dbContext = serviceContext.DbContextFactory.Create())
                    {
                        var metadataStoreItem = dbContext.MetadataStoreItem
                            .Where(m => (m.StoreType.Trim().ToLower() == "globalconstant" && m.ItemName.Trim().ToLower() == key.Trim().ToLower()))
                            .OrderByDescending(m => m.Version)
                            .FirstOrDefault();

                        if (metadataStoreItem != null)
                        {
                            var metadataStoreItemValue = JsonHelper.DeserializeObject(metadataStoreItem.Value);
                            replacementDictionary.Add(key, $"({metadataStoreItemValue})");
                        }
                    }
                }

                if (replacementDictionary.ContainsKey(key))
                {
                    // TODO: should be replaced by a regex.
                    toReturn = toReturn.Replace(toReplace, replacementDictionary[key]);
                }
                else if (throwOnFail)
                {
                    throw new CustomExpressionValidationException(
                        $"Can't replace '{key}' in the script '{customSearchExpressionScript}'.",
                        validationResults: null,
                        CustomExpressionValidationExceptionType.Reference,
                        innerException: null);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyName) && !replacementDictionary.ContainsKey(keyName))
            {
                replacementDictionary.Add(keyName, $"({toReturn})");
            }

            return toReturn;
        }

        /// <summary>
        /// Replaces parameter values in an expression.
        /// </summary>
        /// <param name="customSearchExpressionScript">The custom search expression script.</param>
        /// <param name="parametersDictionary">The parameters dictionary.</param>
        /// <returns>
        /// The modified custom searh expression script.
        /// </returns>
        public static string ReplaceParameters(
            string customSearchExpressionScript,
            Dictionary<string, string> parametersDictionary)
        {
            if (parametersDictionary == null || parametersDictionary.Count == 0)
            {
                return customSearchExpressionScript;
            }

            string toReturn = customSearchExpressionScript;
            foreach (var parameterName in parametersDictionary.Keys)
            {
                toReturn = toReturn.Replace(
                    $"{{{parameterName}}}",
                    $"'{parametersDictionary[parameterName]}'");
            }

            return toReturn;
        }
    }
}
