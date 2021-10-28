namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports an interactive chart.
    /// </summary>
    public class ImportInteractiveChartService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportInteractiveChartService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportInteractiveChartService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the interactive chart.
        /// </summary>
        /// <param name="chartExpressions">The chart expressions.</param>
        /// <param name="chartType">The chart type.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Invalid input data.</exception>
        public static void ValidateChartExpressions(ICollection<CustomSearchExpression> chartExpressions, InteractiveChartType chartType)
        {
            var columnNames = chartExpressions.Select(e => e.ColumnName?.Trim().ToLower());

            if (columnNames.Any(c => string.IsNullOrWhiteSpace(c)))
            {
                throw new CustomSearchesRequestBodyException(
                    $"{nameof(CustomSearchExpression.ColumnName)} should not be null or empty.",
                    null);
            }

            var repeatedColumnNameCollection = columnNames.GroupBy(c => c).Where(group => group.Count() > 1).Select(group => group.Key);

            if (repeatedColumnNameCollection.Count() > 0)
            {
                string repeatedColumnNames = string.Join(", ", repeatedColumnNameCollection);
                throw new CustomSearchesRequestBodyException(
                    $"{nameof(CustomSearchExpression.ColumnName)} should not be repeated in chart expressions. Repeated column names: {repeatedColumnNames}.",
                    null);
            }

            CustomSearchExpressionRoleType[] validRoles =
            {
                CustomSearchExpressionRoleType.IndependentVariable,
                CustomSearchExpressionRoleType.DependentVariable,
                CustomSearchExpressionRoleType.GroupByVariable,
            };

            CustomSearchExpressionValidator.ValidateTypes(
                chartExpressions,
                validRoles: null);

            CustomSearchExpression[] independentExpressions = chartExpressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.IndependentVariable.ToString()).ToArray();
            CustomSearchExpression[] dependentExpressions = chartExpressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.DependentVariable.ToString()).ToArray();
            CustomSearchExpression[] groupByVariableExpressions = chartExpressions.Where(c => c.ExpressionRole == CustomSearchExpressionRoleType.GroupByVariable.ToString()).ToArray();

            InputValidationHelper.AssertNotEmpty(independentExpressions, nameof(independentExpressions));

            if (chartType != InteractiveChartType.Histogram)
            {
                InputValidationHelper.AssertNotEmpty(dependentExpressions, nameof(dependentExpressions));
            }

            switch (chartType)
            {
                case InteractiveChartType.Histogram:
                    CustomSearchExpressionValidator.ValidateHistogramData(independentExpressions);
                    CustomSearchExpressionValidator.AssertNoDuplicateExpressionGroups(groupByVariableExpressions);
                    break;
                case InteractiveChartType.Pie:
                    InputValidationHelper.AssertEmpty(groupByVariableExpressions, nameof(groupByVariableExpressions));
                    break;
                case InteractiveChartType.ScatterPlot:
                    CustomSearchExpressionValidator.AssertHasPlottedStyleExpression(dependentExpressions);
                    break;
                default:
                    break;
            }

            // ExpressionGroup value should not be repeated in different independent variables.
            var independentExpressionsWithExpressionGroup = independentExpressions.Where(e => !string.IsNullOrWhiteSpace(e.ExpressionGroup));
            CustomSearchExpressionValidator.AssertNoDuplicateExpressionGroups(independentExpressionsWithExpressionGroup);

            // ExpressionGroup value is present either in the independent variable or in the group by variable, but should be defined in both.
            var groupBySet = groupByVariableExpressions.Where(e => !string.IsNullOrWhiteSpace(e.ExpressionGroup)).Select(e => e.ExpressionGroup).ToHashSet();
            var independentSet = independentExpressionsWithExpressionGroup.Select(e => e.ExpressionGroup).ToHashSet();

            groupBySet.SymmetricExceptWith(independentSet);
            if (groupBySet.Count > 0)
            {
                string invalidValues = string.Join(" and ", groupBySet.Select(v => $"'{v}'"));
                throw new CustomSearchesRequestBodyException(
                    $"ExpressionGroup value is present either in the independent variable or in the group by variable, but should be defined in both. ExpressionGroup(s) to check: {invalidValues}.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Imports the interactive chart.
        /// </summary>
        /// <param name="interactiveChartData">The interactive chart data to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The interactive chart id.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Invalid input data.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<IdResult> ImportInteractiveChartAsync(
            InteractiveChartData interactiveChartData,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertZero(interactiveChartData.InteractiveChartId, nameof(InteractiveChart), nameof(interactiveChartData.InteractiveChartId));
            InputValidationHelper.AssertNotEmpty(interactiveChartData.ChartTitle, nameof(interactiveChartData.ChartTitle));

            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == interactiveChartData.DatasetId)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", interactiveChartData.DatasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "ImportInteractiveChart");

            var existingChart = await (from ic in dbContext.InteractiveChart
                                 where ic.ChartTitle == interactiveChartData.ChartTitle &&
                                    ic.DatasetId == interactiveChartData.DatasetId
                                 select ic).
                                 Include(ic => ic.CustomSearchExpression).
                                 FirstOrDefaultAsync();

            InteractiveChart newChart = interactiveChartData.ToEfModel();

            // Assign explicit order to expressions
            int executionOrder = 0;
            foreach (var expression in newChart.CustomSearchExpression)
            {
                expression.ExecutionOrder = executionOrder;
                executionOrder++;
            }

            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                newChart.CustomSearchExpression.ToList(),
                this.ServiceContext,
                datasetContext: dataset,
                previousPostProcessContext: null,
                postProcessContext: null,
                chartTypeContext: interactiveChartData.ChartType,
                true);

            newChart.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            newChart.LastModifiedTimestamp = DateTime.Now;

            InteractiveChart chartToSave = newChart;
            if (existingChart == null)
            {
                await RenameInteractiveChartService.ValidateNewChartTitle(dbContext, dataset.DatasetId, chartId: 0, interactiveChartData.ChartTitle);
                newChart.CreatedBy = newChart.LastModifiedBy;
                newChart.CreatedTimestamp = newChart.LastModifiedTimestamp;

                // Add new custom search definition if it was not found.
                dbContext.InteractiveChart.Add(newChart);
            }
            else
            {
                // Update chart if it was found.
                chartToSave = existingChart;

                existingChart.ChartExtensions = newChart.ChartExtensions;

                // Delete all existing custom search expressions.
                dbContext.CustomSearchExpression.RemoveRange(existingChart.CustomSearchExpression);
                existingChart.CustomSearchExpression.Clear();

                // Add new parameters
                foreach (var expression in newChart.CustomSearchExpression)
                {
                    existingChart.CustomSearchExpression.Add(expression);
                }
            }

            InteractiveChartType interactiveChartType =
                InputValidationHelper.ValidateEnum<InteractiveChartType>(interactiveChartData.ChartType, nameof(interactiveChartData.ChartType));

            chartToSave.ChartType = interactiveChartType.ToString();
            ImportInteractiveChartService.ValidateChartExpressions(chartToSave.CustomSearchExpression, interactiveChartType);
            await dbContext.ValidateAndSaveChangesAsync();
            return new IdResult(chartToSave.InteractiveChartId);
        }
     }
}
