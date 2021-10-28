namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets chart templates.
    /// </summary>
    public class GetChartTemplatesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetChartTemplatesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetChartTemplatesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the charts templates.
        /// </summary>
        /// <param name="customSearchId">Custom search definition id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The post processes for a dataset.
        /// </returns>
        public async Task<GetChartTemplatesResponse> GetChartTemplatesAsync(
            int customSearchId,
            CustomSearchesDbContext dbContext)
        {
            GetChartTemplatesResponse response = new GetChartTemplatesResponse();
            var chartTemplates = await (from c in dbContext.ChartTemplate
                                        join dc in dbContext.CustomSearchChartTemplate
                                        on c.ChartTemplateId equals dc.ChartTemplateId
                                        where dc.CustomSearchDefinitionId == customSearchId
                                        select c).ToArrayAsync();

            if (chartTemplates.Length > 0)
            {
                response.ChartTemplates = new ChartTemplateData[chartTemplates.Length];
                for (int i = 0; i < chartTemplates.Length; i++)
                {
                    response.ChartTemplates[i] = new ChartTemplateData(chartTemplates[i], ModelInitializationType.Summary, userDetails: null);
                }
            }

            return response;
        }
    }
}
