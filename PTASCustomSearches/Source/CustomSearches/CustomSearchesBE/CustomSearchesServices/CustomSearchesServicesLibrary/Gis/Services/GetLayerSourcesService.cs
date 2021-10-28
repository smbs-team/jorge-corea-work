namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that gets the layer sources.
    /// </summary>
    public class GetLayerSourcesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLayerSourcesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetLayerSourcesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the layer sources.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The layer sources.
        /// </returns>
        public async Task<GetLayerSourcesResponse> GetLayerSources(GisDbContext dbContext)
        {
            GetLayerSourcesResponse response = new GetLayerSourcesResponse();

            var query = dbContext.LayerSource as IQueryable<LayerSource>;
            var results = await query.ToArrayAsync();

            response.LayerSources = new LayerSourceData[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var layerSource = results[i];
                var layerSourceData = new LayerSourceData(layerSource);
                response.LayerSources[i] = layerSourceData;
            }

            return response;
        }
    }
}
