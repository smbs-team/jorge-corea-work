namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the map renderer categories.
    /// </summary>
    public class GetMapRendererCategoriesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMapRendererCategoriesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetMapRendererCategoriesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the map renderer categories.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The map renderer categories.
        /// </returns>
        public async Task<GetMapRendererCategoriesResponse> GetMapRendererCategories(GisDbContext dbContext)
        {
            GetMapRendererCategoriesResponse response = new GetMapRendererCategoriesResponse();

            var categories =
                await dbContext.MapRendererCategory
                    .Include(m => m.MapRendererCategoryMapRenderer)
                        .ThenInclude(c => c.MapRenderer)
                    .ToArrayAsync();

            response.MapRendererCategories = new MapRendererCategoryData[categories.Length];

            for (int i = 0; i < categories.Length; i++)
            {
                MapRendererCategory mapRendererCategory = categories[i];
                var results = mapRendererCategory.MapRendererCategoryMapRenderer.ToArray();
                MapRendererCategoryData mapRendererCategoryData = new MapRendererCategoryData(mapRendererCategory, ModelInitializationType.FullObjectWithDepedendencies);

                response.MapRendererCategories[i] = mapRendererCategoryData;
            }

            return response;
        }
    }
}
