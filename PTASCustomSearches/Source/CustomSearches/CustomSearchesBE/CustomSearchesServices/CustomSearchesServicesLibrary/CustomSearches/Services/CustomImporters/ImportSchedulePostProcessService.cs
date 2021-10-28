namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Service that imports the schedule post process.
    /// </summary>
    public class ImportSchedulePostProcessService : ImportBaseSchedulePostProcessService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportSchedulePostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportSchedulePostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets or sets the name of the column used to calculate the value of the column to update.
        /// </summary>
        private string BaseColumnName { get; set; }

        /// <summary>
        /// Updates the the dataset post process data with the information of the extensions in the custom search expressions.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="columnName">The database column to update.</param>
        public override void UpdateDatasetPostProcess(DatasetPostProcessData datasetPostProcessData, string columnName)
        {
            var exceptionPostProcessRuleList = new List<ExceptionPostProcessRuleData>();

            if (datasetPostProcessData.ExceptionPostProcessRules?.Length > 0)
            {
                int columnIndex = 0;
                var stepFilters = new List<string>();
                var scheduleSteps = new List<float>();

                var extensions = new List<ScheduleLandExtensionsData>();

                for (int i = 0; i < datasetPostProcessData.ExceptionPostProcessRules.Length; i++)
                {
                    ExceptionPostProcessRuleData exceptionRuleData = datasetPostProcessData.ExceptionPostProcessRules[i];
                    var filterExpression = exceptionRuleData.CustomSearchExpressions
                        .Where(e => e.ExpressionRole?.Trim().ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower())
                        .FirstOrDefault();

                    if (filterExpression == null)
                    {
                        throw new CustomSearchesRequestBodyException(ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage, innerException: null);
                    }

                    if (filterExpression.ExpressionExtensions == null)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"{nameof(filterExpression.ExpressionExtensions)} should not be empty in the filter expression.",
                            innerException: null);
                    }

                    ScheduleLandExtensionsData extension;
                    try
                    {
                        extension = JsonHelper.DeserializeObject<ScheduleLandExtensionsData>(JsonHelper.SerializeObject(filterExpression.ExpressionExtensions));
                    }
                    catch (System.Exception ex)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"Error deserializing the {nameof(filterExpression.ExpressionExtensions)} in the filter expression.",
                            ex);
                    }

                    extensions.Add(extension);

                    if (!scheduleSteps.Contains(extension.ScheduleStep))
                    {
                        if ((scheduleSteps.Count > 0) && (extension.ScheduleStep <= scheduleSteps.Last()))
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"{nameof(extension.ScheduleStep)} should be incremented in each step.",
                                innerException: null);
                        }

                        scheduleSteps.Add(extension.ScheduleStep);
                        columnIndex = 0;
                    }

                    if (scheduleSteps.Count == 1)
                    {
                        if (stepFilters.Contains(extension.StepFilter))
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"Found more than one column with the {nameof(extension.StepFilter)} '{extension.StepFilter}'.",
                                innerException: null);
                        }

                        stepFilters.Add(extension.StepFilter);
                    }
                    else
                    {
                        int stepFilterColumnIndex = stepFilters.IndexOf(extension.StepFilter);
                        if (stepFilterColumnIndex != columnIndex)
                        {
                            throw new CustomSearchesRequestBodyException(
                                "An error was found in the import, please verify the StepFilter in the exception post process rules.",
                                innerException: null);
                        }
                    }

                    columnIndex++;
                }

                if (scheduleSteps.Count == 1)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"{nameof(DatasetPostProcess)} should have 0 or more than 1 {nameof(datasetPostProcessData.ExceptionPostProcessRules)}.",
                        innerException: null);
                }

                if (!string.IsNullOrWhiteSpace(stepFilters.Last()))
                {
                    throw new CustomSearchesRequestBodyException(
                        $"The last {nameof(ExceptionPostProcessRule)} inserted in each row " +
                        $"should have an empty {nameof(ScheduleLandExtensionsData.StepFilter)} (representing the baseline value). " +
                        $"The {nameof(datasetPostProcessData.ExceptionPostProcessRules)} should be inserted from right to left in each row.",
                        innerException: null);
                }

                int columnsCount = stepFilters.Count;
                int rowsCount = extensions.Select(e => e.ScheduleStep).ToHashSet().Count();
                int requiredExceptionPostprocessRules = columnsCount * rowsCount;
                if (extensions.Count != columnsCount * rowsCount)
                {
                    throw new CustomSearchesRequestBodyException(
                        "An error was found in the import, please verify the ScheduleStep and StepFilter in the exception post process rules.",
                        innerException: null);
                }

                ScheduleLandExtensionsData extensionsData = null;
                ScheduleLandExtensionsData nextExtensionsData = null;

                for (int i = 0; i < datasetPostProcessData.ExceptionPostProcessRules.Length; i++)
                {
                    bool isLastRowRules = i >= datasetPostProcessData.ExceptionPostProcessRules.Length - columnsCount;

                    ExceptionPostProcessRuleData exceptionRuleData = datasetPostProcessData.ExceptionPostProcessRules[i];

                    InputValidationHelper.AssertNotEmpty(
                        exceptionRuleData.CustomSearchExpressions,
                        nameof(exceptionRuleData.CustomSearchExpressions),
                        ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage);

                    var filterExpression = exceptionRuleData.CustomSearchExpressions
                        .Where(e => e.ExpressionRole?.Trim().ToLower() == CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower())
                        .FirstOrDefault();

                    var calculatedColumnExpression = exceptionRuleData.CustomSearchExpressions
                        .Where(e => e.ExpressionRole?.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())
                        .FirstOrDefault();

                    if (calculatedColumnExpression == null)
                    {
                        throw new CustomSearchesRequestBodyException(ImportExceptionPostProcessService.ExceptionPostProcessRolesErrorMessage, innerException: null);
                    }

                    // If this is the last row use the previous row data for calculate the script.
                    int extensionIndex = isLastRowRules ? (i - columnsCount) : i;
                    extensionsData = extensions[extensionIndex];
                    nextExtensionsData = extensions[extensionIndex + columnsCount];

                    if (i < columnsCount)
                    {
                        filterExpression.Script = $" {this.BaseColumnName} < {nextExtensionsData.ScheduleStep}";
                    }
                    else if (isLastRowRules)
                    {
                        filterExpression.Script = $" {this.BaseColumnName} > {nextExtensionsData.ScheduleStep}";
                    }
                    else
                    {
                        filterExpression.Script = $" {this.BaseColumnName} BETWEEN {extensionsData.ScheduleStep} AND {nextExtensionsData.ScheduleStep}";
                    }

                    if (!string.IsNullOrWhiteSpace(extensionsData.StepFilter))
                    {
                        filterExpression.Script += $" AND ({extensionsData.StepFilter})";
                    }

                    filterExpression.ColumnName = columnName;
                    filterExpression.ExpressionType = CustomSearchExpressionType.TSQL.ToString();

                    JObject expressionExtensions = filterExpression.ExpressionExtensions as JObject;
                    expressionExtensions["traceMessage"] = $"'{datasetPostProcessData.PostProcessDefinition}: {filterExpression.Script} => '" + "{ColumnValue}";

                    calculatedColumnExpression.Script =
                        $"({nextExtensionsData.StepValue} - {extensionsData.StepValue}) * " +
                        $"((CAST({this.BaseColumnName} as FLOAT) - {extensionsData.ScheduleStep}) / ({nextExtensionsData.ScheduleStep} - {extensionsData.ScheduleStep})) + " +
                        $"{extensionsData.StepValue}";

                    calculatedColumnExpression.Script = $"(SELECT Max(v) FROM (VALUES ({calculatedColumnExpression.Script}), (0.0)) AS value(v))";

                    calculatedColumnExpression.ColumnName = columnName;
                    calculatedColumnExpression.ExpressionType = CustomSearchExpressionType.TSQL.ToString();

                    exceptionRuleData.Description = $"{datasetPostProcessData.PostProcessDefinition} step: ";
                    exceptionRuleData.Description += isLastRowRules ? nextExtensionsData.ScheduleStep.ToString() : extensionsData.ScheduleStep.ToString();
                }

                exceptionPostProcessRuleList.AddRange(datasetPostProcessData.ExceptionPostProcessRules);
            }
        }

        /// <summary>
        /// Creates and adds the default land model exception rule to the dataset post process data.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="datasetPostProcessData">The dataset post process data.</param>
        public override void AddDefaultLandModelExceptionRule(
            int datasetPostProcessId,
            string columnName,
            DatasetPostProcessData datasetPostProcessData)
        {
            if (datasetPostProcessData.ExceptionPostProcessRules?.Length == 0)
            {
                base.AddDefaultLandModelExceptionRule(datasetPostProcessId, columnName, datasetPostProcessData);
            }
        }

        /// <summary>
        /// Imports the schedule post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="baseColumnName">The name of the column used to calculate the value of the column to update.</param>
        /// <param name="columnName">The name of column to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        protected async Task<IdResult> ImportSchedulePostProcessAsync(
            DatasetPostProcessData datasetPostProcessData,
            string baseColumnName,
            string columnName,
            CustomSearchesDbContext dbContext)
        {
            this.BaseColumnName = baseColumnName;
            return await this.ImportBaseSchedulePostProcessAsync(datasetPostProcessData, columnName, dbContext);
        }
    }
}
