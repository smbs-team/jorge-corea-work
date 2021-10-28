namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets all the interactive chart types.
    /// </summary>
    public class GetInteractiveChartTypesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInteractiveChartTypesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetInteractiveChartTypesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets all the interactive chart types.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The interactive chart types response.
        /// </returns>
        public async Task<GetInteractiveChartTypesResponse> GetInteractiveChartTypesAsync(CustomSearchesDbContext dbContext)
        {
            GetInteractiveChartTypesResponse response = new GetInteractiveChartTypesResponse();
            response.InteractiveChartTypes = await (from c in dbContext.ChartType select c.ChartType1).ToArrayAsync();
            return response;
        }
    }
}
