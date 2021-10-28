namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions.ValidationGroups;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validates TSQl expressions.
    /// </summary>
    public static class TSqlExpressionScriptValidator
    {
        private const string SqlValidationError = "Error executing TSql expression.";
        private const string MockReplacementValue = "1.1";

        /// <summary>
        /// Validates the TSQL expression groups for grouped expressions.
        /// </summary>
        /// <param name="expressionGroup">The expression group.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results. </returns>
        /// <exception cref="ArgumentNullException"> DbContext or DataDbContext. </exception>
        public static async Task<CustomExpressionValidationResult> ValidateTSqlExpressionGroupAsync(
            ExpressionValidationGroup expressionGroup,
            ExpressionValidationContext validationContext)
        {
            switch (expressionGroup.GroupType)
            {
                case ExpressionValidationGroupType.TSqlPrePostCommitGroup:
                    return await TSqlExpressionScriptValidator.ValidatePrePostCommitGroupAsync(expressionGroup, validationContext);
                case ExpressionValidationGroupType.TSqlChartExpressionGroup:
                    return await TSqlExpressionScriptValidator.ValidateChartGroupAsync(expressionGroup, validationContext, true);
                case ExpressionValidationGroupType.TSqlChartExpressionGroupNoGroupBy:
                    return await TSqlExpressionScriptValidator.ValidateChartGroupAsync(expressionGroup, validationContext, false);
                case ExpressionValidationGroupType.TSqlSelectColumnChainGroup:
                    return await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnsAsync(
                        expressionGroup,
                        validationContext,
                        false,
                        new Dictionary<string, string>());
            }

            return null;
        }

        /// <summary>
        /// Validates the TSQL expression (for non grouped expressions).
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results. </returns>
        /// <exception cref="ArgumentNullException"> DbContext or DataDbContext. </exception>
        public static async Task<CustomExpressionValidationResult> ValidateTSqlExpressionAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext)
        {
            if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.EditLookupExpression.ToString().ToLower() ||
                expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.LookupExpression.ToString().ToLower() ||
                expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.RangedValuesOverrideExpression.ToString().ToLower())
            {
                return await TSqlExpressionScriptValidator.ValidateTSqlFullScriptAsync(expression, validationContext);
            }
            else if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower() ||
                expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.ValidationConditionExpression.ToString().ToLower())
            {
                return await TSqlExpressionScriptValidator.ValidateTSqlFilterExpressionAsync(expression, validationContext);
            }
            else if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() ||
                expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.ValidationErrorExpression.ToString().ToLower())
            {
                return await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnAsync(expression, validationContext, false, null);
            }
            else if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.UpdateStoredProcedure.ToString().ToLower())
            {
                return await ValidateTSqlStoredProcedureExpressionAsync(expression, validationContext);
            }

            return null;
        }

        /// <summary>
        /// Validates the TSQL calculated select column fragment.  (this is the piece of SQL that is used to select a single column).
        /// </summary>
        /// <param name="expression">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="selectColumnScript">The select column script.</param>
        /// <param name="removeAscDesc">if set to true removes ASC, DESC at the end of the statement, allowing
        /// to test group by expressions as if they were calculated columns.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="modifiedDatasetScript">If sent, this is used as the dataset script,
        /// instead of the one this function would normally generate.</param>
        /// <returns>
        /// The validation results.
        /// </returns>
        public static async Task<CustomExpressionValidationResult> ValidateTSqlSelectColumnFragment(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext,
            string selectColumnScript,
            bool removeAscDesc,
            Dictionary<string, string> replacementDictionary = null,
            string modifiedDatasetScript = null)
        {
            return await TSqlExpressionScriptValidator.ValidateDbActionAsync(
               async (string datasetScript) =>
               {
                   datasetScript = modifiedDatasetScript ?? datasetScript;

                   string scriptWithReplacements = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                        selectColumnScript,
                        replacementDictionary,
                        expression.ColumnName,
                        validationContext.ServiceContext,
                        validationContext.DatasetContext,
                        true);

                   if (removeAscDesc)
                   {
                       scriptWithReplacements = TSqlExpressionScriptValidator.RemoveAscDesc(scriptWithReplacements);
                   }

                   string columnName = expression.ColumnName;
                   if (string.IsNullOrWhiteSpace(columnName))
                   {
                       if (expression.ExpressionRole.Trim().ToLower() != CustomSearchExpressionRoleType.ValidationErrorExpression.ToString().ToLower())
                       {
                           var validationResult = new CustomExpressionValidationResult(expression)
                           {
                               Success = false,
                               ValidationError = "Column name is required.",
                           };

                           return validationResult;
                       }
                       else
                       {
                           columnName = "TempColumnName";
                       }
                   }

                   var sqlScript = $"SELECT {scriptWithReplacements} AS [{columnName}] FROM ({datasetScript}) ds ";
                   validationContext.LastExecutedStatement = sqlScript;

                   await DbTransientRetryPolicy.GetDynamicResultAsync(
                       validationContext.ServiceContext,
                       validationContext.ServiceContext.DataDbContextFactory,
                       sqlScript);

                   return null;
               },
               expression,
               validationContext,
               true);
        }

        /// <summary>
        /// Validates the TSQL chart expression group.
        /// </summary>
        /// <param name="expressionGroup">The expression group.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="allowGroupBy">if set to <c>false</c>, dependent variables will
        /// be validated as if they were independent.  This is used for plot cases.</param>
        /// <returns>
        /// The validation results.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateChartGroupAsync(
            ExpressionValidationGroup expressionGroup,
            ExpressionValidationContext validationContext,
            bool allowGroupBy)
        {
            validationContext.AssertDatasetContextNotNull();

            IList<CustomSearchExpression> independentVariables =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.IndependentVariable);

            IList<CustomSearchExpression> dependentVariables =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.DependentVariable);

            IList<CustomSearchExpression> groupByExpression =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.GroupByVariable);

            // If group by is not allowed all expressions are tested as calculated columns.
            // This is the case for plot style charts.
            if (!allowGroupBy)
            {
                var expressionsToValidate = independentVariables.Concat(groupByExpression).ToList();
                expressionsToValidate = expressionsToValidate.Concat(dependentVariables).ToList();

                var result = await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnsAsync(
                    expressionsToValidate,
                    validationContext,
                    removeAscDesc: true);

                if (result != null && !result.Success)
                {
                    return result;
                }
            }
            else
            {
                string datasetScript = await validationContext.GetDatasetScript();

                // First we test the independent variable just by themselves
                var result = await TSqlExpressionScriptValidator.ValidateTSqlGroupByExpressionsAsync(
                    independentVariables,
                    validationContext);

                if (result != null && !result.Success)
                {
                    return result;
                }

                // Next we test them with grouping attributes
                foreach (var indepedentVar in independentVariables)
                {
                    var groupByChain = new List<CustomSearchExpression>();
                    groupByChain.Add(indepedentVar);

                    var relatedGroupByExpr =
                        (from gbe in groupByExpression
                         where gbe.ExpressionGroup.ToLower() == indepedentVar.ExpressionGroup?.ToLower()
                         select gbe).FirstOrDefault();

                    // If there is a 'group by' chain we want to validate the group chain before independently.
                    if (relatedGroupByExpr != null)
                    {
                        groupByChain.Add(relatedGroupByExpr);

                        // Validate the expression with the group by result.
                        var chainResult = await TSqlExpressionScriptValidator.ValidateTSqlGroupByExpressionChainAsync(
                            groupByChain,
                            null,
                            validationContext);

                        if (!chainResult.Success)
                        {
                            return chainResult;
                        }
                    }

                    // Finally we want to evaluate the group by chain with any calcualted column
                    // Next we test them with grouping attributes
                    foreach (var depedentVar in dependentVariables)
                    {
                        // Validate the expression with the group by result.
                        var chainResult = await TSqlExpressionScriptValidator.ValidateTSqlGroupByExpressionChainAsync(
                            groupByChain,
                            depedentVar,
                            validationContext);

                        if (!chainResult.Success)
                        {
                            return chainResult;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Validates the list of SQL calculated columns.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="removeAscDesc">if set to <c>true</c> [remove asc decimal].</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="modifiedDatasetScript">The modified dataset script.</param>
        /// <returns>
        /// Null if the validation passed.  Otherwise the validation error.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlCalculatedColumnsAsync(
            IList<CustomSearchExpression> expressions,
            ExpressionValidationContext validationContext,
            bool removeAscDesc,
            Dictionary<string, string> replacementDictionary = null,
            string modifiedDatasetScript = null)
        {
            foreach (var expression in expressions)
            {
                var result = await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnAsync(
                    expression,
                    validationContext,
                    removeAscDesc,
                    replacementDictionary,
                    modifiedDatasetScript);

                if (result != null && !result.Success)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Validates the list of SQL group by expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="modifiedDatasetScript">The modified dataset script.</param>
        /// <returns>
        /// Null if the validation passed.  Otherwise the validation error.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlGroupByExpressionsAsync(
            IList<CustomSearchExpression> expressions,
            ExpressionValidationContext validationContext,
            Dictionary<string, string> replacementDictionary = null,
            string modifiedDatasetScript = null)
        {
            foreach (var expression in expressions)
            {
                var expressionChain = new List<CustomSearchExpression>();
                expressionChain.Add(expression);

                var result = await TSqlExpressionScriptValidator.ValidateTSqlGroupByExpressionChainAsync(
                    expressionChain,
                    null,
                    validationContext);

                if (result != null && !result.Success)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Mocks the r script results in the replacement dictionary.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        private static async Task MockRScriptResults(
            ExpressionValidationContext validationContext,
            Dictionary<string, string> replacementDictionary)
        {
            using (var dbContext = validationContext.ServiceContext.DbContextFactory.Create())
            {
                var postProcess = validationContext.PostProcessContext;
                var rScriptModel = postProcess.RscriptModel;
                if (rScriptModel == null)
                {
                    rScriptModel = await (
                        from rs in dbContext.RscriptModel
                        where rs.RscriptModelId == postProcess.RscriptModelId
                        select rs).FirstOrDefaultAsync();
                }

                if (!string.IsNullOrWhiteSpace(rScriptModel.RscriptResultsDefinition))
                {
                    var rScriptResults = JsonHelper.DeserializeObject<CustomSearchParameterData[]>(rScriptModel.RscriptResultsDefinition);
                    if (rScriptResults != null && rScriptResults.Count() > 0)
                    {
                        foreach (var rScriptResult in rScriptResults)
                        {
                            if (!string.IsNullOrWhiteSpace(rScriptResult.Name))
                            {
                                string defaultValue = rScriptResult.DefaultValue;
                                if (string.IsNullOrWhiteSpace(rScriptResult.DefaultValue))
                                {
                                    // Try to mock a numeric default value if not provided.
                                    defaultValue = TSqlExpressionScriptValidator.MockReplacementValue;
                                }

                                replacementDictionary.Add(rScriptResult.Name, defaultValue);
                            }
                        }
                    }
                }

                replacementDictionary.Add("Predicted", TSqlExpressionScriptValidator.MockReplacementValue);
            }
        }

        /// <summary>
        /// Validates the TSQL pre/post commit expression group (post-process).
        /// </summary>
        /// <param name="expressionGroup">The expression group.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns> The validation results.</returns>
        private static async Task<CustomExpressionValidationResult> ValidatePrePostCommitGroupAsync(
            ExpressionValidationGroup expressionGroup,
            ExpressionValidationContext validationContext)
        {
            validationContext.AssertDatasetContextNotNull();

            IList<CustomSearchExpression> preCommitExpressions =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.CalculatedColumnPreCommit);

            // We treat CalculatedColumn as pre-commit because this will be added from the file import and used in the post-commit.
            preCommitExpressions = preCommitExpressions.Concat(
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.CalculatedColumn)).ToList();

            IList<CustomSearchExpression> postCommitExpressions =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.CalculatedColumnPostCommit);

            // We treat CalculatedColumnPreCommit_Dependent and CalculatedColumnPreCommit_Independent as post-commit
            // because we want to know if the columns they refer to will be available to R-Script.  So we need to test
            // after we've applied the pre-commit expressions.
            postCommitExpressions = postCommitExpressions.Concat(
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent)).ToList();

            postCommitExpressions = postCommitExpressions.Concat(
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent)).ToList();

            string preCommitColumnsSelectScript = string.Empty;
            List<string> precommitColumnNames = new List<string>();

            // Init replacement dictionary with RScript parameters.
            Dictionary<string, string> replacementDictionary =
                expressionGroup.GetExpressionsWithRole(CustomSearchExpressionRoleType.RScriptParameter).ToDictionary(e => e.ColumnName, e => $"({e.Script})");

            foreach (var expression in preCommitExpressions)
            {
                var result = await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnAsync(
                    expression,
                    validationContext,
                    false,
                    replacementDictionary);

                if (result != null && !result.Success)
                {
                    return result;
                }

                // We put together a select statement with all pre-commit columns so we can use it later as a base for the
                // post-commit columns.
                string scriptWithReplacements = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                      expression.Script,
                      replacementDictionary,
                      expression.ColumnName,
                      validationContext.ServiceContext,
                      validationContext.DatasetContext,
                      true);

                precommitColumnNames.Add(expression.ColumnName);
                preCommitColumnsSelectScript += $"{scriptWithReplacements} AS [{expression.ColumnName}], ";
            }

            string postCommitDatasetScript = await validationContext.GetDatasetScript();

            if (validationContext.PostProcessContext.PostProcessType.ToLower() == DatasetPostProcessType.RScriptPostProcess.ToString().ToLower())
            {
                await TSqlExpressionScriptValidator.MockRScriptResults(validationContext, replacementDictionary);
            }

            if (precommitColumnNames.Count > 0)
            {
                // Strip the last " ," from the selection script
                preCommitColumnsSelectScript = preCommitColumnsSelectScript[0..^2];

                // We generate a select statement that selects columns individually.  This is because columns in the dataset
                // can be overwritten by pre-commit columns.
                var selectColumnStatement = await TSqlExpressionScriptValidator.GetDatsetColumnSelectStatement(
                    validationContext,
                    precommitColumnNames);

                postCommitDatasetScript = $"SELECT {selectColumnStatement}, {preCommitColumnsSelectScript} FROM ({postCommitDatasetScript}) ds2";
            }

            // We validate the post-commit columns by using a base script that has all pre-commit columns included.
            return await TSqlExpressionScriptValidator.ValidateTSqlCalculatedColumnsAsync(
                postCommitExpressions,
                validationContext,
                removeAscDesc: false,
                replacementDictionary,
                postCommitDatasetScript);
        }

        /// <summary>
        /// Validates the TSQL stored procedure expression.
        /// </summary>
        /// <param name="expression">The stored procedure expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// The validation results.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlStoredProcedureExpressionAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext)
        {
            try
            {
                await StoredProcedureHelper.ValidateStoredProcedureExistsAsync(expression.Script, validationContext.ServiceContext);
            }
            catch (Exception ex)
            {
                var validationResult = new CustomExpressionValidationResult(expression)
                {
                    Success = false,
                    ValidationError = ex.GetBaseException().Message,
                };

                return validationResult;
            }

            return null;
        }

        /// <summary>
        /// Validates the TSQL calculated column expression.
        /// </summary>
        /// <param name="expression">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="removeAscDesc">if set to true removes ASC, DESC at the end of the statement, allowing
        /// to test group by expressions as if they were calculated columns.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="modifiedDatasetScript">If sent, this is used as the dataset script,
        /// instead of the one this function would normally generate.</param>
        /// <returns>
        /// The validation results.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlCalculatedColumnAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext,
            bool removeAscDesc,
            Dictionary<string, string> replacementDictionary = null,
            string modifiedDatasetScript = null)
        {
            return await TSqlExpressionScriptValidator.ValidateTSqlSelectColumnFragment(
                expression,
                validationContext,
                expression.Script,
                removeAscDesc,
                replacementDictionary,
                modifiedDatasetScript);
        }

        /// <summary>
        /// Validates the TSQL group by expression chain.  The expression chain is composed of consecutive "group by"
        /// expressions and a optional column select statement.
        /// </summary>
        /// <param name="groupByExpressions">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        /// <param name="modifiedDatasetScript">If sent, this is used as the dataset script,
        /// instead of the one this function would normally generate.</param>
        /// <returns>
        /// The validation results.  If calculatedColumnExpression is provided, the validation is output
        /// as if done with this expression, otherwise the last expression in groupByExpressions is used.
        /// </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlGroupByExpressionChainAsync(
            IList<CustomSearchExpression> groupByExpressions,
            CustomSearchExpression calculatedColumnExpression,
            ExpressionValidationContext validationContext,
            Dictionary<string, string> replacementDictionary = null,
            string modifiedDatasetScript = null)
        {
            if (groupByExpressions.Count == 0)
            {
                throw new ArgumentException("Need group by expressions when validating group by chain", nameof(groupByExpressions));
            }

            return await TSqlExpressionScriptValidator.ValidateDbActionAsync(
               async (string datasetScript) =>
               {
                   datasetScript = modifiedDatasetScript ?? datasetScript;

                   string selectScript = "1 AS TestNumber";

                   if (calculatedColumnExpression != null)
                   {
                       string columnScriptWithReplacements = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                            calculatedColumnExpression.Script,
                            replacementDictionary,
                            calculatedColumnExpression.ColumnName,
                            validationContext.ServiceContext,
                            validationContext.DatasetContext,
                            true);

                       selectScript = $"{columnScriptWithReplacements} AS [{calculatedColumnExpression.ColumnName}]";
                   }

                   string groupByScript = string.Empty;
                   string orderByScript = string.Empty;

                   foreach (var groupByExpression in groupByExpressions)
                   {
                       string expressionScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                           groupByExpression.Script,
                           replacementDictionary,
                           groupByExpression.ColumnName,
                           validationContext.ServiceContext,
                           validationContext.DatasetContext,
                           false);

                       var noAscDescScript = TSqlExpressionScriptValidator.RemoveAscDesc(expressionScript);

                       orderByScript += $"{expressionScript}, ";
                       groupByScript += $"{noAscDescScript}, ";
                   }

                   // Remove trailing ' ,'
                   groupByScript = groupByScript.Substring(0, groupByScript.Length - 2);
                   orderByScript = orderByScript.Substring(0, orderByScript.Length - 2);

                   var sqlScript = $"SELECT {selectScript} FROM ({datasetScript}) ds GROUP BY {groupByScript} ORDER BY {orderByScript} ";
                   validationContext.LastExecutedStatement = sqlScript;

                   await DbTransientRetryPolicy.GetDynamicResultAsync(
                       validationContext.ServiceContext,
                       validationContext.ServiceContext.DataDbContextFactory,
                       sqlScript);

                   return null;
               },
               calculatedColumnExpression ?? groupByExpressions.Last(),
               validationContext,
               true);
        }

        /// <summary>
        /// Validates the TSQL full script expression.
        /// </summary>
        /// <param name="expression">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns> The validation results.</returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlFullScriptAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext)
        {
            return await TSqlExpressionScriptValidator.ValidateDbActionAsync(
                async (string datasetScript) =>
                {
                    validationContext.LastExecutedStatement = expression.Script;
                    Dictionary<string, string> parameters = null;

                    // In case this is a custom search parameter, we'll need to retrieve default parameter values.
                    if (expression.CustomSearchParameterId != null)
                    {
                        parameters = ParameterReplacementHelper.GetDefaultDependedParameterValues(expression.CustomSearchParameter.ParameterExtensions);
                    }

                    // In case this is a custom search parameter, we'll need to retrieve default parameter values.
                    if (expression.CustomSearchColumnDefinitionId != null)
                    {
                        parameters = ParameterReplacementHelper.GetDefaultDependedParameterValues(expression.CustomSearchColumnDefinition.ColumDefinitionExtensions);
                    }

                    var scriptResults = await CustomSearchExpressionEvaluator.EvaluateAsync<object[], CustomSearchesDataDbContext>(
                        validationContext.ServiceContext,
                        validationContext.ServiceContext.DataDbContextFactory,
                        expression.ExpressionType,
                        expression.Script,
                        parameters);

                    if (scriptResults.Length == 0)
                    {
                        var validationResult = new CustomExpressionValidationResult(expression)
                        {
                            Success = false,
                            ValidationError = "TSql expression returned no results.",
                        };

                        return validationResult;
                    }

                    if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.EditLookupExpression.ToString().ToLower() ||
                        expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.LookupExpression.ToString().ToLower())
                    {
                        // 'Value' is required, 'Key' is optional.
                        JProperty valueProperty = (
                               from p in (scriptResults[0] as JObject).Properties()
                               where p.Name.ToLower() == "Value".ToLower()
                               select p).FirstOrDefault();

                        if (valueProperty == null)
                        {
                            var validationResult = new CustomExpressionValidationResult(expression)
                            {
                                Success = false,
                                ValidationError = "TSql expression result does not contain a 'Value' field."
                            };

                            return validationResult;
                        }
                    }
                    else if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.RangedValuesOverrideExpression.ToString().ToLower())
                    {
                        // A response with the column name is required
                        string columnName = expression.CustomSearchColumnDefinition.ColumnName.ToLower();
                        JProperty columnProperty = (
                           from p in (scriptResults[0] as JObject).Properties()
                            where p.Name.ToLower() == columnName
                            select p).FirstOrDefault();

                        if (columnProperty == null)
                        {
                            var validationResult = new CustomExpressionValidationResult(expression)
                            {
                                Success = false,
                                ValidationError = $"TSql expression result does not contain a '{columnName}' field."
                            };

                            return validationResult;
                        }
                    }

                    return null;
                },
                expression,
                validationContext,
                generateDatasetScript: false);
        }

        /// <summary>
        /// Validates the TSQL filter expression.
        /// </summary>
        /// <param name="expression">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results. </returns>
        private static async Task<CustomExpressionValidationResult> ValidateTSqlFilterExpressionAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext)
        {
            return await TSqlExpressionScriptValidator.ValidateDbActionAsync(
               async (string datasetScript) =>
               {
                   var sqlScript = $"SELECT * FROM ( {datasetScript} ) expressionValidateTable WHERE {expression.Script}";
                   validationContext.LastExecutedStatement = sqlScript;
                   var results = await DbTransientRetryPolicy.GetDynamicResultAsync(
                       validationContext.ServiceContext,
                       validationContext.ServiceContext.DataDbContextFactory,
                       sqlScript);

                   return null;
               },
               expression,
               validationContext,
               generateDatasetScript: true);
        }

        /// <summary>
        /// Validates the dataset state to make sure validations can be run.
        /// </summary>
        /// <param name="expression">The lookup expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns> The validation results. </returns>
        private static async Task<CustomExpressionValidationResult> ValidateDatasetStateAsync(
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext)
        {
            var stateValidationResult = await validationContext.DatasetContextHasValidState();
            if (!stateValidationResult.result)
            {
                var validationResult = new CustomExpressionValidationResult(expression)
                {
                    Success = false,
                    ValidationError = stateValidationResult.message,
                    ValidationErrorDetails = $"Dataset state: {validationContext.DatasetContext.DataSetState}, " +
                        $"Dataset PostProcess state: {validationContext.DatasetContext.DataSetPostProcessState}"
                };

                return validationResult;
            }

            return null;
        }

        /// <summary>
        /// Maps Exceptions to validation results.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The corresponding validation result for the exception.</returns>
        private static CustomExpressionValidationResult ExceptionToValidationResult(
            System.Exception ex,
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext,
            string message)
        {
            string newMessage = null;

            // TODO: Add aggregation exception management.
            if ((ex.GetType() == typeof(Microsoft.Data.SqlClient.SqlException)) ||
                (ex.GetType() == typeof(CustomSearchesDatabaseException)))
            {
                newMessage = TSqlExpressionScriptValidator.SqlValidationError;
            }
            else if (ex.GetType() == typeof(CustomExpressionValidationException))
            {
                newMessage = "Expression validation error.";
            }
            else
            {
                newMessage = $"Unknown error validating expression.";
            }

            return new CustomExpressionValidationResult(expression)
            {
                Success = false,
                ValidationError = message ?? newMessage,
                ValidationErrorDetails = ex.GetBaseException().Message,
                ExecutedStatement = validationContext.LastExecutedStatement
            };
        }

        /// <summary>
        /// Gets the column select statement for the validation context, taking into account all column names for the dataset
        /// and excluding columns in the columnNameExceptions list.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="columnNameExceptions">The column name exceptions.</param>
        /// <returns>
        /// The column select statement.
        /// </returns>
        private static async Task<string> GetDatsetColumnSelectStatement(
            ExpressionValidationContext validationContext,
            List<string> columnNameExceptions)
        {
            var dbColumns = await validationContext.GetDatasetContextDbColumns();
            string columnScript = string.Empty;

            foreach (DbColumn column in dbColumns)
            {
                bool found = (from c in columnNameExceptions
                              where c.ToLower() == column.ColumnName.ToLower()
                              select c).Count() > 0;

                if (!found)
                {
                    columnScript += $"[{column.ColumnName}], ";
                }
            }

            if (columnScript.Length > 0)
            {
                // Removing trailer ", "
                columnScript = columnScript.Substring(0, columnScript.Length - 2);
            }

            return columnScript;
        }

        /// <summary>
        /// Validates the database action.
        /// </summary>
        /// <param name="dbAction">The database action.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The validation result after executing the action.</returns>
        private static async Task<CustomExpressionValidationResult> ValidateDbActionAsync(
            Func<string, Task<CustomExpressionValidationResult>> dbAction,
            CustomSearchExpression expression,
            ExpressionValidationContext validationContext,
            bool generateDatasetScript,
            string errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(expression.Script))
            {
                return new CustomExpressionValidationResult(expression)
                {
                    Success = false,
                    ValidationError = "Script field can't be empty."
                };
            }

            var validationResult = new CustomExpressionValidationResult(expression);
            string generatedScript = string.Empty;

            if (generateDatasetScript)
            {
                validationContext.AssertDatasetContextNotNull();

                var datasetValidationResult = await TSqlExpressionScriptValidator.ValidateDatasetStateAsync(
                  expression,
                  validationContext);

                if (datasetValidationResult != null && !datasetValidationResult.Success)
                {
                    return datasetValidationResult;
                }

                generatedScript = await validationContext.GetDatasetScript();
            }

            try
            {
                validationContext.LastExecutedStatement = string.Empty;
                var actionResult = await dbAction(generatedScript);
                validationResult = actionResult ?? validationResult;
            }
            catch (Exception ex)
            {
                validationResult = TSqlExpressionScriptValidator.ExceptionToValidationResult(
                    ex,
                    expression,
                    validationContext,
                    errorMessage);
            }

            return validationResult;
        }

        /// <summary>
        /// Removes the asc/desc statement at the end of the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The input string without the asc/desc at the end of it.</returns>
        private static string RemoveAscDesc(string input)
        {
            if (RegExHelper.IsLastWord(input, "asc"))
            {
                input = input.Substring(0, input.Length - "asc".Length);
            }
            else if (RegExHelper.IsLastWord(input, "desc"))
            {
                input = input.Substring(0, input.Length - "desc".Length);
            }

            return input;
        }
    }
}
