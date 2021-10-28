namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions.ValidationGroups;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Class used to validate custom search expressions.
    /// </summary>
    public static class CustomSearchExpressionValidator
    {
        /// <summary>
        /// Validates a list of expressions depending on their context.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="datasetContext">The dataset context.  Some expressions will need this to be able to run validations against data.</param>
        /// <param name="previousPostProcessContext">The dataset post process that would execute previous to the current one.  Some expressions will need this to be able to run validations against data.</param>
        /// <param name="postProcessContext">The dataset post process context.  Some expressions will need this to be able to run validations against data.</param>
        /// <param name="chartTypeContext">The chart type context.  Some expressions will need to be able to run different validations depending on the chart type.</param>
        /// <param name="throwOnFail">if set to <c>true</c> [throw on fail].</param>
        /// <returns>The validation results.</returns>
        /// <exception cref="CustomExpressionValidationException">Exception when One or more expressions failed to validate.</exception>
        public static async Task<List<CustomExpressionValidationResult>> ValidateExpressionScriptsAsync(
            List<CustomSearchExpression> expressions,
            IServiceContext serviceContext,
            Dataset datasetContext,
            DatasetPostProcess previousPostProcessContext,
            DatasetPostProcess postProcessContext,
            string chartTypeContext,
            bool throwOnFail = false)
        {
            ExpressionValidationContext validationContext = new ExpressionValidationContext()
            {
                ServiceContext = serviceContext,
                DatasetContext = datasetContext,
                PreviousPostProcessContext = previousPostProcessContext,
                PostProcessContext = postProcessContext,
                ChartTypeContext = chartTypeContext,
                ThrowOnFail = throwOnFail
            };

            return await ExpressionScriptGroupValidator.GroupAndValidateScriptsAsync(expressions, validationContext);
        }

        /// <summary>
        /// Validates if the lookup expression results are valid.
        /// </summary>
        /// <param name="results">The custom search expressions.</param>
        /// <exception cref="InvalidExpressionResultException">Expression result does not have a 'value' property.".</exception>
        public static void AssertLookupExpressionResultsAreValid(
            object[] results)
        {
            if (results?.Length > 0)
            {
                JProperty valueProperty = (
                    from p in (results[0] as JObject).Properties()
                    where p.Name.ToLower() == "Value".ToLower()
                    select p).FirstOrDefault();

                // 'Value' is required, 'Key' is optional.
                if (valueProperty == null)
                {
                    throw new InvalidExpressionResultException($"Expression result does not have a 'value' property.", innerException: null);
                }
            }
        }

        /// <summary>
        /// Validates if the custom search expressions have only one expression of the specified role type.
        /// </summary>
        /// <param name="expressions">The custom search expressions.</param>
        /// <param name="validRoleType">The valid role.</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Expressions count is different than expected.</exception>
        public static void AssertHasOneExpression(
            IEnumerable<CustomSearchExpression> expressions,
            CustomSearchExpressionRoleType validRoleType,
            string message = null)
        {
            int matchCount = 0;
            foreach (var expression in expressions)
            {
                CustomSearchExpressionRoleType roleType = InputValidationHelper.ValidateEnum<CustomSearchExpressionRoleType>(expression.ExpressionRole, nameof(expression.ExpressionRole));
                if (validRoleType == roleType)
                {
                    matchCount++;
                }
            }

            if (matchCount != 1)
            {
                throw new CustomSearchesRequestBodyException(
                    message ?? $"{matchCount} expression(s) with the role '{validRoleType}' were found. Expected: 1.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if all the custom search expressions have different ExpressionGroup.
        /// </summary>
        /// <param name="expressions">The custom search expressions.</param>
        /// <exception cref="CustomSearchesRequestBodyException">ExpressionGroup value should not be repeated in different expressions with role: '{expressions.First().ExpressionRole}'</exception>
        public static void AssertNoDuplicateExpressionGroups(IEnumerable<CustomSearchExpression> expressions)
        {
            var duplicateGroupByExpressionGroups = expressions
                .GroupBy(e => e.ExpressionGroup)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            if (duplicateGroupByExpressionGroups.Count() > 0)
            {
                string invalidValues = string.Join(" and ", duplicateGroupByExpressionGroups.Select(v => $"'{v}'"));
                throw new CustomSearchesRequestBodyException(
                    $"ExpressionGroup value should not be repeated in different expressions with role: '{expressions.First().ExpressionRole}'. Repeated ExpressionGroup(s): {invalidValues}.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Checks if the custom search expression has defined the plotted style.
        /// </summary>
        /// <param name="expression">The custom search expression</param>
        /// <returns>True if the expression has the plotted style, otherwise false.</returns>
        public static bool IsPlottedStyleExpression(CustomSearchExpression expression)
        {
            if (string.IsNullOrWhiteSpace(expression.ExpressionExtensions))
            {
                return false;
            }

            var expressionExtensions = JsonHelper.DeserializeObject(expression.ExpressionExtensions);
            JProperty valueProperty = (
                from p in (expressionExtensions as JObject).Properties()
                where (p.Value != null) && (p.Name.ToLower() == "Style".ToLower()) && (p.Value.ToString().ToLower() == "Plotted".ToLower())
                select p).FirstOrDefault();

            return valueProperty != null;
        }

        /// <summary>
        /// Validates the histogram data in the independent expressions.
        /// </summary>
        /// <param name="independentExpressions">The independent expressions.</param>
        public static void ValidateHistogramData(IEnumerable<CustomSearchExpression> independentExpressions)
        {
            foreach (var expression in independentExpressions)
            {
                if (string.IsNullOrWhiteSpace(expression.ExpressionExtensions))
                {
                    throw new CustomSearchesRequestBodyException(
                        $"{nameof(independentExpressions)} should have the histogram data in the {nameof(expression.ExpressionExtensions)}.",
                        innerException: null);
                }

                var histogramChartExtensionData = JsonHelper.DeserializeObject<HistogramChartExtensionData>(expression.ExpressionExtensions);

                if (histogramChartExtensionData.AutoBins == false)
                {
                    if (histogramChartExtensionData.NumBins < 1)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"{nameof(histogramChartExtensionData.NumBins)} should be greater than 0 when AutoBins is false.",
                            innerException: null);
                    }
                }
            }
        }

        /// <summary>
        /// Validates if the custom search expressions have at least 1 plotted style expression.
        /// </summary>
        /// <param name="expressions">The custom search expressions.</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Plotted style expression was not found.</exception>
        public static void AssertHasPlottedStyleExpression(
            IEnumerable<CustomSearchExpression> expressions,
            string message = null)
        {
            foreach (var expression in expressions)
            {
                if (CustomSearchExpressionValidator.IsPlottedStyleExpression(expression))
                {
                    return;
                }
            }

            throw new CustomSearchesRequestBodyException(
                message ?? $"Plotted style dependent variable is required.",
                innerException: null);
        }

        /// <summary>
        /// Validates the custom search expressions types.
        /// </summary>
        /// <param name="expressions">The custom search expressions.</param>
        /// <param name="validRoles">The valid roles. If null all roles are valid.</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Expression with invalid type.</exception>
        public static void ValidateTypes(
            IEnumerable<CustomSearchExpression> expressions,
            IEnumerable<CustomSearchExpressionRoleType> validRoles,
            string message = null)
        {
            if ((expressions == null) || (expressions.Count() == 0))
            {
                return;
            }

            foreach (var expression in expressions)
            {
                CustomSearchExpressionRoleType roleType = InputValidationHelper.ValidateEnum<CustomSearchExpressionRoleType>(expression.ExpressionRole, nameof(expression.ExpressionRole));
                CustomSearchExpressionType expressionType = InputValidationHelper.ValidateEnum<CustomSearchExpressionType>(expression.ExpressionType, nameof(expression.ExpressionType));
                bool isValidRole = (validRoles?.Count() > 0) ? validRoles.Contains(roleType) : true;

                if (!isValidRole)
                {
                    string validValues = string.Join(" or ", validRoles.Select(v => $"'{v}'"));
                    throw new CustomSearchesRequestBodyException(
                        message ?? $"Expression '{expression.ColumnName}' should have ExpressionRole {validValues}.",
                        innerException: null);
                }

                InputValidationHelper.AssertNotEmpty(expression.Script, nameof(expression.Script));

                if (roleType == CustomSearchExpressionRoleType.CalculatedColumn ||
                    roleType == CustomSearchExpressionRoleType.CalculatedColumnPostCommit ||
                    roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit ||
                    roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent ||
                    roleType == CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent)
                {
                    InputValidationHelper.AssertNotEmpty(
                        expression.ColumnName,
                        nameof(expression.ColumnName),
                        $"ColumnName should not be empty for expression with ExpressionRole '{roleType}'.");
                }

                if (roleType == CustomSearchExpressionRoleType.GroupByVariable)
                {
                    InputValidationHelper.AssertNotEmpty(
                        expression.ExpressionGroup,
                        nameof(expression.ExpressionGroup),
                        $"ExpressionGroup should not be empty for expression with ExpressionRole '{roleType}'.");
                }

                IEnumerable<CustomSearchExpressionType> validTypes;
                switch (roleType)
                {
                    case CustomSearchExpressionRoleType.LookupExpression:
                    case CustomSearchExpressionRoleType.EditLookupExpression:
                    case CustomSearchExpressionRoleType.RangedValuesOverrideExpression:
                        validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.TSQL, CustomSearchExpressionType.JSonPayload };
                        break;
                    case CustomSearchExpressionRoleType.RScriptParameter:
                    case CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent:
                    case CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent:
                        validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.RScript };
                        break;
                    case CustomSearchExpressionRoleType.CalculatedColumn:
                        if (expression.DatasetPostProcess != null &&
                            expression.DatasetPostProcess.PostProcessType.ToLower().Trim() == DatasetPostProcessType.CustomModelingStepPostProcess.ToString().ToLower())
                        {
                            validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.Imported };
                        }
                        else if ((expression.RscriptModel != null) ||
                            (expression.RscriptModelId > 0) ||
                            (expression.DatasetPostProcess != null && expression.DatasetPostProcess.RscriptModelId > 0))
                        {
                            validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.RScript };
                        }
                        else
                        {
                            validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.TSQL };
                        }

                        break;
                    default:
                        validTypes = new CustomSearchExpressionType[] { CustomSearchExpressionType.TSQL };
                        break;
                }

                bool isValidType = (validTypes?.Count() > 0) ? validTypes.Contains(expressionType) : true;
                if (!isValidType)
                {
                    string validValues = string.Join(" or ", validTypes.Select(v => $"'{v}'"));
                    string expressionName = string.IsNullOrWhiteSpace(expression.ColumnName) ? roleType.ToString() : expression.ColumnName;
                    throw new CustomSearchesRequestBodyException(
                        message ?? $"Expression '{expressionName}' should have ExpressionType {validValues}.",
                        innerException: null);
                }
            }
        }
    }
}
