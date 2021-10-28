namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports an chart template.
    /// </summary>
    public class ImportChartTemplateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportChartTemplateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportChartTemplateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Validates if the new chart title is not used by other dataset charts.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="chartId">The chart id.</param>
        /// <param name="newChartTitle">The new chart title.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public static async Task ValidateNewChartTitle(CustomSearchesDbContext dbContext, int chartId, string newChartTitle)
        {
            var repeatedChartsCount = await dbContext.ChartTemplate
                .CountAsync(c =>
                    c.ChartTemplateId != chartId &&
                    c.ChartTitle.Trim().ToLower() == newChartTitle.Trim().ToLower());

            if (repeatedChartsCount > 0)
            {
                throw new CustomSearchesConflictException("A chart with this title already exists.", innerException: null);
            }
        }

        /// <summary>
        /// Imports the chart template.
        /// </summary>
        /// <param name="chartTemplateData">The chart template data to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The chart template id.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Invalid input data.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<IdResult> ImportChartTemplateAsync(
            ChartTemplateData chartTemplateData,
            CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportChartTemplate");
            InputValidationHelper.AssertZero(chartTemplateData.ChartTemplateId, nameof(ChartTemplate), nameof(chartTemplateData.ChartTemplateId));
            InputValidationHelper.AssertNotEmpty(chartTemplateData.ChartTitle, nameof(chartTemplateData.ChartTitle));
            InputValidationHelper.AssertNotEmpty(chartTemplateData.CustomSearches, nameof(chartTemplateData.CustomSearches));

            var existingChart = await (from c in dbContext.ChartTemplate
                                 where c.ChartTitle == chartTemplateData.ChartTitle
                                 select c).
                                 Include(c => c.CustomSearchExpression).
                                 Include(c => c.CustomSearchChartTemplate).
                                 FirstOrDefaultAsync();

            ChartTemplate newChart = chartTemplateData.ToEfModel();

            // TODO: Add validations
            ////// Validate expresions
            ////using (var dataDbContext = this.ServiceContext.DataDbContextFactory.Create())
            ////{
            ////    await CustomSearchExpressionValidator.ValidateAsync(newChart.CustomSearchExpression.ToList(), dbContext, dataDbContext, datasetContext: null, datasetPostProcessContext: null, true);
            ////}

            newChart.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            newChart.LastModifiedTimestamp = DateTime.Now;

            ChartTemplate chartToSave = newChart;
            if (existingChart == null)
            {
                await ImportChartTemplateService.ValidateNewChartTitle(dbContext, chartId: 0, chartTemplateData.ChartTitle);
                newChart.CreatedBy = newChart.LastModifiedBy;
                newChart.CreatedTimestamp = newChart.LastModifiedTimestamp;

                // Add new custom search definition if it was not found.
                dbContext.ChartTemplate.Add(newChart);
            }
            else
            {
                // Update custom search definition if it was found.
                chartToSave = existingChart;

                // Delete all existing custom search expressions.
                dbContext.CustomSearchExpression.RemoveRange(existingChart.CustomSearchExpression);
                existingChart.CustomSearchExpression.Clear();

                // Add new parameters
                foreach (var expression in newChart.CustomSearchExpression)
                {
                    existingChart.CustomSearchExpression.Add(expression);
                }
            }

            // First remove existing custom search associations
            dbContext.CustomSearchChartTemplate.RemoveRange(chartToSave.CustomSearchChartTemplate);
            chartToSave.CustomSearchChartTemplate.Clear();

            if (chartTemplateData.CustomSearches.Length > 0)
            {
                var query = from csd in dbContext.CustomSearchDefinition
                            where csd.IsDeleted == false
                            select csd;

                var customSearchDefinitions = (await query.ToArrayAsync())
                    .GroupBy(csd => csd.CustomSearchName.Trim().ToLower())
                    .Select(gcsd => gcsd.OrderByDescending(d => d.Version).FirstOrDefault());

                // Then add new ones.
                foreach (var customSearch in chartTemplateData.CustomSearches)
                {
                    InputValidationHelper.AssertNotEmpty(customSearch, nameof(customSearch));

                    CustomSearchDefinition customSearchDefinition =
                        (from c in customSearchDefinitions where c.CustomSearchName.Trim().ToLower() == customSearch.Trim().ToLower() select c).FirstOrDefault();

                    InputValidationHelper.AssertEntityExists(customSearchDefinition, nameof(customSearch), customSearch);

                    var newCustoSearch = new CustomSearchChartTemplate()
                    {
                        CustomSearchDefinition = customSearchDefinition,
                        ChartTemplate = chartToSave
                    };

                    chartToSave.CustomSearchChartTemplate.Add(newCustoSearch);
                }
            }

            InteractiveChartType interactiveChartType =
                InputValidationHelper.ValidateEnum<InteractiveChartType>(chartTemplateData.ChartType, nameof(chartTemplateData.ChartType));

            chartToSave.ChartType = interactiveChartType.ToString();
            ImportInteractiveChartService.ValidateChartExpressions(chartToSave.CustomSearchExpression, interactiveChartType);
            await dbContext.ValidateAndSaveChangesAsync();
            return new IdResult(chartToSave.ChartTemplateId);
        }
     }
}
