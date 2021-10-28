namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes the map renderer.
    /// </summary>
    public class DeleteMapRendererService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMapRendererService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteMapRendererService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the map renderer.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapRendererId">Map renderer id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteMapRenderer(GisDbContext dbContext, int mapRendererId)
        {
            MapRenderer mapRenderer = await dbContext.MapRenderer
                .Where(m => m.MapRendererId == mapRendererId)
                .Include(m => m.UserMap)
                    .ThenInclude(m => m.ParentFolder)
                .Include(m => m.MapRendererUserSelection)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(mapRenderer, nameof(mapRenderer), mapRendererId);
            UserMap userMap = mapRenderer.UserMap;

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userMap.CreatedBy, userMap.ParentFolder, userMap.IsLocked, "DeleteMapRenderer");

            dbContext.MapRendererUserSelection.RemoveRange(mapRenderer.MapRendererUserSelection);

            List<MapRendererCategoryMapRenderer> rendererCategories =
                (from rc in dbContext.MapRendererCategoryMapRenderer
                where rc.MapRendererId == mapRendererId
                select rc).ToList();

            if (rendererCategories.Count > 0)
            {
                dbContext.MapRendererCategoryMapRenderer.RemoveRange(rendererCategories.ToArray());
            }

            dbContext.MapRenderer.Remove(mapRenderer);

            await dbContext.SaveChangesAsync();
        }
    }
}
