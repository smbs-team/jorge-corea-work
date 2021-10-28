namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets an chart template.
    /// </summary>
    public class GetChartTemplateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetChartTemplateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetChartTemplateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the chart template.
        /// </summary>
        /// <param name="chartTemplateId">Chart template id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The chart template.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        public async Task<GetChartTemplateResponse> GetChartTemplateAsync(
            int chartTemplateId,
            CustomSearchesDbContext dbContext)
        {
            GetChartTemplateResponse response = new GetChartTemplateResponse();

            var chartTemplate =
                await (from c in dbContext.ChartTemplate where c.ChartTemplateId == chartTemplateId select c).
                Include(c => c.CustomSearchExpression).
                Include(c => c.CustomSearchChartTemplate).
                ThenInclude(cc => cc.CustomSearchDefinition).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(chartTemplate, nameof(chartTemplate), chartTemplateId);

            response.ChartTemplate = new ChartTemplateData(chartTemplate, ModelInitializationType.FullObject, userDetails: null);
            return response;
        }
    }
}
