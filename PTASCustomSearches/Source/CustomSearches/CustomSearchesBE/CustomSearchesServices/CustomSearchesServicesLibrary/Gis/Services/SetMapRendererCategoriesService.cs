namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that sets the map renderer categories.
    /// </summary>
    public class SetMapRendererCategoriesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetMapRendererCategoriesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public SetMapRendererCategoriesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Sets the map renderer categories.
        /// </summary>
        /// <param name="mapRendererId">Map renderer id.</param>
        /// <param name="setMapRendererCategoriesData">Map renderer categories to set.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task SetMapRendererCategories(int mapRendererId, SetMapRendererCategoriesData setMapRendererCategoriesData, GisDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("SetMapRendererCategories");

            MapRenderer mapRenderer = await dbContext.MapRenderer
                .Where(m => m.MapRendererId == mapRendererId)
                .Include(m => m.MapRendererCategoryMapRenderer)
                    .ThenInclude(c => c.MapRendererCategory)
                .FirstOrDefaultAsync();

            if (mapRenderer == null)
            {
                throw new CustomSearchesEntityNotFoundException(
                    $"Map renderer with id '{mapRendererId}' not found.",
                    null);
            }

            dbContext.MapRendererCategoryMapRenderer.RemoveRange(mapRenderer.MapRendererCategoryMapRenderer);

            mapRenderer.MapRendererCategoryMapRenderer.Clear();

            if ((setMapRendererCategoriesData != null) &&
                (setMapRendererCategoriesData.MapRendererCategories != null) &&
                (setMapRendererCategoriesData.MapRendererCategories.Length > 0))
            {
                var categories = await dbContext.MapRendererCategory.ToDictionaryAsync(m => m.CategoryName.ToLower());

                HashSet<string> assignedCategories = new HashSet<string>();
                foreach (var categoryData in setMapRendererCategoriesData.MapRendererCategories)
                {
                    string categoryKey = categoryData.CategoryName.ToLower();

                    // Checks assigned categories to avoid repetitions.
                    if (!assignedCategories.Contains(categoryKey))
                    {
                        assignedCategories.Add(categoryKey);

                        MapRendererCategory mapRendererCategory;
                        if (categories.ContainsKey(categoryKey))
                        {
                            mapRendererCategory = categories[categoryKey];
                        }
                        else
                        {
                            mapRendererCategory = new MapRendererCategory()
                            {
                                CategoryName = categoryData.CategoryName,
                                CategoryDescription = categoryData.CategoryDescription
                            };
                        }

                        MapRendererCategoryMapRenderer mapRendererCategoryMapRenderer = new MapRendererCategoryMapRenderer();
                        mapRendererCategoryMapRenderer.MapRenderer = mapRenderer;
                        mapRendererCategoryMapRenderer.MapRendererCategory = mapRendererCategory;
                        dbContext.MapRendererCategoryMapRenderer.Add(mapRendererCategoryMapRenderer);
                    }
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
