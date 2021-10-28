namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that imports the schedule adjustments post process.
    /// </summary>
    public class ImportScheduleAdjustmentsPostProcessService : ImportBaseSchedulePostProcessService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportScheduleAdjustmentsPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportScheduleAdjustmentsPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the the dataset post process data with the information of the extensions in the custom search expressions.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="columnName">The database column to update.</param>
        public override void UpdateDatasetPostProcess(DatasetPostProcessData datasetPostProcessData, string columnName)
        {
            if (datasetPostProcessData.ExceptionPostProcessRules?.Length > 0)
            {
                foreach (var exceptionPostProcessRuleData in datasetPostProcessData.ExceptionPostProcessRules)
                {
                    InputValidationHelper.AssertNotEmpty(
                        exceptionPostProcessRuleData.CustomSearchExpressions,
                        nameof(exceptionPostProcessRuleData.CustomSearchExpressions),
                        ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage);

                    var filterExpression = exceptionPostProcessRuleData.CustomSearchExpressions
                        .Where(e => e.ExpressionRole?.Trim().ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower())
                        .FirstOrDefault();

                    var calculatedColumnExpression = exceptionPostProcessRuleData.CustomSearchExpressions
                        .Where(e => e.ExpressionRole?.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                        .FirstOrDefault();

                    if (filterExpression == null || calculatedColumnExpression == null)
                    {
                        throw new CustomSearchesRequestBodyException(ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage, innerException: null);
                    }

                    if (filterExpression.ExpressionExtensions == null)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"{nameof(filterExpression.ExpressionExtensions)} should not be empty in the filter expression.",
                            innerException: null);
                    }

                    ScheduleAdjustmentExtensionsData extensionsData = null;
                    try
                    {
                        extensionsData = JsonHelper.DeserializeObject<ScheduleAdjustmentExtensionsData>(JsonHelper.SerializeObject(filterExpression.ExpressionExtensions));
                    }
                    catch (JsonSerializationException ex)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"Error deserializing the {nameof(filterExpression.ExpressionExtensions)} in the filter expression.",
                            ex);
                    }

                    if (extensionsData.IsViewType())
                    {
                        InputValidationHelper.AssertNotEmpty(extensionsData.ViewType, nameof(extensionsData.ViewType));
                        InputValidationHelper.AssertShouldBeGreaterThan(extensionsData.Ptas_ViewType, expected: 0, nameof(extensionsData.Ptas_ViewType));
                        InputValidationHelper.AssertNotEmpty(extensionsData.Quality, nameof(extensionsData.Quality));
                        InputValidationHelper.AssertShouldBeGreaterThan(extensionsData.Ptas_Quality, expected: 0, nameof(extensionsData.Ptas_Quality));
                    }
                    else if (extensionsData.IsNuisanceType())
                    {
                        InputValidationHelper.AssertNotEmpty(extensionsData.NuisanceType, nameof(extensionsData.NuisanceType));
                        InputValidationHelper.AssertShouldBeGreaterThan(extensionsData.Ptas_NuisanceType, expected: 0, nameof(extensionsData.Ptas_NuisanceType));

                        if (extensionsData.IsAirportNoiseType())
                        {
                            InputValidationHelper.AssertNotEmpty(extensionsData.NoiseLevel, nameof(extensionsData.NoiseLevel));
                            InputValidationHelper.AssertShouldBeGreaterThan(extensionsData.Ptas_NoiseLevel, expected: 0, nameof(extensionsData.Ptas_NoiseLevel));
                        }
                    }
                    else
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"{nameof(extensionsData.CharacteristicType)} should be 'View' or 'Nuisance'.",
                            innerException: null);
                    }

                    filterExpression.Script = this.GetAdjustmentFilterScript(extensionsData);

                    filterExpression.ColumnName = columnName;
                    filterExpression.ExpressionType = CustomSearchExpressionType.TSQL.ToString();

                    exceptionPostProcessRuleData.Description =
                        $"{datasetPostProcessData.PostProcessDefinition}: {extensionsData.CharacteristicType} - {extensionsData.Description}";

                    JObject expressionExtensions = filterExpression.ExpressionExtensions as JObject;
                    expressionExtensions["traceMessage"] = $"'{exceptionPostProcessRuleData.Description} => '" + "{ColumnValue}";

                    calculatedColumnExpression.Script = this.GetAdjustmentValueScript(extensionsData);

                    calculatedColumnExpression.ColumnName = columnName;
                    calculatedColumnExpression.ExpressionType = CustomSearchExpressionType.TSQL.ToString();
                }
            }
        }

        /// <summary>
        /// Imports the schedule adjustments post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportScheduleAdjustmentsPostProcessAsync(
            DatasetPostProcessData datasetPostProcessData,
            CustomSearchesDbContext dbContext)
        {
            return await this.ImportBaseSchedulePostProcessAsync(datasetPostProcessData, "NewLandValue", dbContext);
        }

        /// <summary>
        /// Gets the adjustment parameters script.
        /// </summary>
        /// <param name="extensionsData">The schedule adjustment extension data.</param>
        /// <returns>The adjustment parameters script.</returns>
        private string GetAdjustmentParametersScript(ScheduleAdjustmentExtensionsData extensionsData)
        {
            var attributeName = extensionsData.GetAdjustmentAttributeName();
            var attributeId = extensionsData.GetAdjustmentAttributeId();
            var attribute = extensionsData.GetAdjustmentAttribute();

            var attributeNameText = attributeName == null ? "null" : $"'{attributeName}'";
            var attributeIdText = attributeId == null ? "null" : $"{attributeId}";
            var attributeText = attribute == null ? "null" : $"'{attribute}'";

            string functionParameters =
                $"[Major], " +
                $"[Minor], " +
                $"{extensionsData.Ptas_CharacteristicType}, " +
                $"'{extensionsData.CharacteristicType}', " +
                $"{extensionsData.GetCharacteristicSubtypeId()}, " +
                $"'{extensionsData.GetCharacteristicSubtype()}', " +
                $"{attributeNameText}, " +
                $"{attributeIdText}, " +
                $"{attributeText}";

            return functionParameters;
        }

        /// <summary>
        /// Gets the adjustment filter script.
        /// </summary>
        /// <param name="extensionsData">The schedule adjustment extension data.</param>
        /// <returns>The adjustment filter script.</returns>
        private string GetAdjustmentFilterScript(ScheduleAdjustmentExtensionsData extensionsData)
        {
            return $"[cus].[FN_ShouldApplyModelAdj]({this.GetAdjustmentParametersScript(extensionsData)}) = 1";
        }

        /// <summary>
        /// Gets the adjustment value script.
        /// </summary>
        /// <param name="extensionsData">The schedule adjustment extension data.</param>
        /// <returns>The adjustment value script.</returns>
        private string GetAdjustmentValueScript(ScheduleAdjustmentExtensionsData extensionsData)
        {
            if (extensionsData.Value.ToLower() == "% adjustment")
            {
                string percentaje = string.IsNullOrWhiteSpace(extensionsData.MaxAdjPercentaje) ?
                    $"{extensionsData.MinAdjPercentaje}" :
                    $"0.0"; // Ranged Adjustments are just for reference.

                return $"CAST([NewLandValue] as FLOAT) * CAST({percentaje} as FLOAT) / 100.0";
            }
            else
            {
                string money = string.IsNullOrWhiteSpace(extensionsData.MaxAdjMoney) ?
                    $"{extensionsData.MinAdjMoney}" :
                    $"0.0"; // Ranged Adjustments are just for reference.

                return $"CAST([NewLandValue] as FLOAT) + CAST({money} as FLOAT)";
            }
        }
    }
}
