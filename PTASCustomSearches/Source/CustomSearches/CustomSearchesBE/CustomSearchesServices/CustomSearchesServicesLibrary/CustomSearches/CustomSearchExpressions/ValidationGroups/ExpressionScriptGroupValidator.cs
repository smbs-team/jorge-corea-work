namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions.ValidationGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;

    /// <summary>
    /// Class used to analyze groups of expressions and split them into groups that need to be analyzed together.
    /// </summary>
    public static class ExpressionScriptGroupValidator
    {
        /// <summary>
        /// Creates groups of expressions according to whether they need to be validated together and validates the groups.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results.</returns>
        /// <exception cref="CustomExpressionValidationException">Exception when One or more expressions failed to validate.</exception>
        public static async Task<List<CustomExpressionValidationResult>> GroupAndValidateScriptsAsync(
            List<CustomSearchExpression> expressions,
            ExpressionValidationContext validationContext)
        {
            bool failure = false;
            var toReturn = new List<CustomExpressionValidationResult>();

            try
            {
                // Some expressions might need to be evaluated together
                List<ExpressionValidationGroup> validationGroups =
                    ExpressionScriptGroupValidator.CreateGroups(expressions, validationContext);

                foreach (var group in validationGroups)
                {
                    var result = await ValidateGroupAsync(group, validationContext);
                    if (result != null)
                    {
                        toReturn.Add(result);
                        failure = failure || !result.Success;

                        // Exit at the first failure
                        if (failure)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = new CustomExpressionValidationResult(null)
                {
                    Success = false,
                    ValidationError = $"Unknown error validating expression: {ex.GetBaseException().Message}.",
                    ValidationErrorDetails = ex.GetBaseException().Message,
                    ExecutedStatement = null
                };

                toReturn.Add(result);
                failure = true;
            }

            if (validationContext.ThrowOnFail && failure)
            {
                throw new CustomExpressionValidationException(
                    "One or more expressions failed to validate.",
                    toReturn.ToArray(),
                    CustomExpressionValidationExceptionType.Execution,
                    innerException: null);
            }

            return toReturn;
        }

        /// <summary>
        /// Validates the specified expression group.
        /// </summary>
        /// <param name="expressionGroup">The expression group.</param>
        /// <param name="validationContext">The expression validation context.</param>
        /// <returns>The validation results or null if no validation was performed.</returns>
        private static async Task<CustomExpressionValidationResult> ValidateGroupAsync(
                ExpressionValidationGroup expressionGroup,
                ExpressionValidationContext validationContext)
        {
            switch (expressionGroup.GroupType)
            {
                case ExpressionValidationGroupType.UnitValidationGroup:
                    foreach (var expression in expressionGroup)
                    {
                        var result = await ExpressionScriptGroupValidator.ValidateExpressionAsync(expression, validationContext);

                        if (result != null && !result.Success)
                        {
                            return result;
                        }
                    }

                    break;

                case ExpressionValidationGroupType.TSqlChartExpressionGroup:
                case ExpressionValidationGroupType.TSqlChartExpressionGroupNoGroupBy:
                case ExpressionValidationGroupType.TSqlPrePostCommitGroup:
                case ExpressionValidationGroupType.TSqlSelectColumnChainGroup:
                    return await TSqlExpressionScriptValidator.ValidateTSqlExpressionGroupAsync(expressionGroup, validationContext);
            }

            return null;
        }

        /// <summary>
        /// Validates a single expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation result for the single expression.</returns>
        private static async Task<CustomExpressionValidationResult> ValidateExpressionAsync(
           CustomSearchExpression expression,
           ExpressionValidationContext validationContext)
        {
            var result = new CustomExpressionValidationResult(expression);

            if (expression.ExpressionType.ToLower() == CustomSearchExpressionType.JSonPayload.ToString().ToLower())
            {
                result = JsonExpressionScriptValidator.ValidateJSonPayload(expression);
            }
            else if (expression.ExpressionType.ToLower() == CustomSearchExpressionType.TSQL.ToString().ToLower())
            {
                result = await TSqlExpressionScriptValidator.ValidateTSqlExpressionAsync(expression, validationContext);
            }
            else if (expression.ExpressionType.ToLower() == CustomSearchExpressionType.RScript.ToString().ToLower() &&
                expression.ExpressionRole.ToLower().StartsWith("calculatedcolumnprecommit"))
            {
                result = await TSqlExpressionScriptValidator.ValidateTSqlSelectColumnFragment(
                    expression,
                    validationContext,
                    expression.Script,
                    false);
            }

            return result;
        }

        /// <summary>
        /// Analyzes the expressions and creates groups that can be validated independently of each other.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// The validation groups.
        /// </returns>
        private static List<ExpressionValidationGroup> CreateGroups(
            List<CustomSearchExpression> expressions,
            ExpressionValidationContext validationContext)
        {
            var groups = new Dictionary<ExpressionValidationGroupType, ExpressionValidationGroup>();

            foreach (var expression in expressions)
            {
                if (expression.IsAutoGenerated)
                {
                    continue;
                }

                if ((expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumnPostCommit.ToString().ToLower()) ||
                    (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent.ToString().ToLower()) ||
                    (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent.ToString().ToLower()) ||
                    (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumnPreCommit.ToString().ToLower()) ||
                    (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.RScriptParameter.ToString().ToLower()) ||
                    ((expression.ExpressionType.ToLower() == CustomSearchExpressionType.Imported.ToString().ToLower()) &&
                        (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())))
                {
                    ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                        groups,
                        expression,
                        ExpressionValidationGroupType.TSqlPrePostCommitGroup);
                }
                else if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.IndependentVariable.ToString().ToLower() ||
                         expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.GroupByVariable.ToString().ToLower())
                {
                    // Scatter plot and scatter plot matrix support non-grouping expressions
                    if (validationContext.ChartTypeContext.ToLower() == InteractiveChartType.ScatterPlot.ToString().ToLower() ||
                        validationContext.ChartTypeContext.ToLower() == InteractiveChartType.ScatterPlotMatrix.ToString().ToLower() ||
                        validationContext.ChartTypeContext.ToLower() == InteractiveChartType.BoxPlot.ToString().ToLower())
                    {
                        ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                           groups,
                           expression,
                           ExpressionValidationGroupType.TSqlChartExpressionGroupNoGroupBy);
                    }

                    // Scatter plot matrix and box plot don't support group expression
                    if (validationContext.ChartTypeContext.ToLower() != InteractiveChartType.ScatterPlotMatrix.ToString().ToLower() &&
                        validationContext.ChartTypeContext.ToLower() != InteractiveChartType.BoxPlot.ToString().ToLower())
                    {
                        ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                           groups,
                           expression,
                           ExpressionValidationGroupType.TSqlChartExpressionGroup);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(validationContext.ChartTypeContext))
                {
                    var groupType = ExpressionValidationGroupType.TSqlChartExpressionGroup;

                    // Scatter variables don't allow group by
                    if ((validationContext.ChartTypeContext.ToLower() == InteractiveChartType.ScatterPlot.ToString().ToLower() &&
                         CustomSearchExpressionValidator.IsPlottedStyleExpression(expression)) ||
                        (validationContext.ChartTypeContext.ToLower() == InteractiveChartType.ScatterPlotMatrix.ToString().ToLower()))
                    {
                        groupType = ExpressionValidationGroupType.TSqlChartExpressionGroupNoGroupBy;
                    }

                    ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                        groups,
                        expression,
                        groupType);
                }
                else if (expression.OwnerType.ToLower() == CustomSearchExpressionOwnerType.ExceptionPostProcessRule.ToString().ToLower())
                {
                    if ((expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower()) &&
                            (expression.Script?.Trim().ToLower() == "default"))
                    {
                        // Skip validation for default/filter expression in post-process rule.
                        continue;
                    }

                    if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                    {
                        ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                            groups,
                            expression,
                            ExpressionValidationGroupType.TSqlSelectColumnChainGroup);
                    }
                    else
                    {
                        ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                            groups,
                            expression,
                            ExpressionValidationGroupType.UnitValidationGroup);
                    }
                }
                else
                {
                    ExpressionScriptGroupValidator.AddExpressionToValidationGroup(
                        groups,
                        expression,
                        ExpressionValidationGroupType.UnitValidationGroup);
                }
            }

            return groups.Values.ToList();
        }

        /// <summary>
        /// Adds the expression to a validation group in the dictionary.
        /// </summary>
        /// <param name="validationGroups">The validation groups dictionary.</param>
        /// <param name="expression">The expression.</param>
        private static void AddExpressionToValidationGroup(
            Dictionary<ExpressionValidationGroupType, ExpressionValidationGroup> validationGroups,
            CustomSearchExpression expression,
            ExpressionValidationGroupType groupType)
        {
            ExpressionValidationGroup group = null;
            validationGroups.TryGetValue(groupType, out group);
            if (group == null)
            {
                group = new ExpressionValidationGroup(groupType);
                validationGroups[groupType] = group;
            }

            group.Add(expression);
        }
    }
}
