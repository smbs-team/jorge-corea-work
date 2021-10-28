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
    /// Service that gets an interactive chart.
    /// </summary>
    public class GetInteractiveChartService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInteractiveChartService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetInteractiveChartService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the interactive chart.
        /// </summary>
        /// <param name="interactiveChartId">Interactive chart id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The interactive chart.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        public async Task<GetInteractiveChartResponse> GetInteractiveChartAsync(
            int interactiveChartId,
            CustomSearchesDbContext dbContext)
        {
            GetInteractiveChartResponse response = new GetInteractiveChartResponse();

            var interactiveChart =
                await (from ic in dbContext.InteractiveChart where ic.InteractiveChartId == interactiveChartId select ic).
                Include(ic => ic.CustomSearchExpression).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(interactiveChart, "Interactive chart", interactiveChartId);

            response.InteractiveChart = new InteractiveChartData(interactiveChart, ModelInitializationType.FullObject, userDetails: null);
            return response;
        }
    }
}
