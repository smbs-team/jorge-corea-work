namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that updates the map renderer.
    /// </summary>
    public class UpdateMapRendererService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMapRendererService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateMapRendererService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the map renderer.
        /// </summary>
        /// <param name="customSearchesDbContext">Custom searches database context.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapRendererData">Map renderer data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task UpdateMapRenderer(CustomSearchesDbContext customSearchesDbContext, GisDbContext dbContext, MapRendererData mapRendererData)
        {
            if (mapRendererData.DatasetId != Guid.Empty)
            {
                CustomSearchesEFLibrary.CustomSearches.Model.Dataset dataset = await customSearchesDbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == mapRendererData.DatasetId);
                InputValidationHelper.AssertEntityExists(dataset, nameof(dataset), mapRendererData.DatasetId);
            }

            MapRenderer mapRenderer = await dbContext.MapRenderer
                .Where(m => m.MapRendererId == mapRendererData.MapRendererId)
                .Include(m => m.UserMap)
                    .ThenInclude(m => m.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(mapRenderer, nameof(mapRenderer), mapRendererData.MapRendererId);
            UserMap userMap = mapRenderer.UserMap;

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, userMap.ParentFolder, userMap.IsLocked, "UpdateMapRenderer");

            InputValidationHelper.AssertNotEmpty(mapRendererData.MapRendererName, nameof(mapRendererData.MapRendererName));

            if (mapRendererData.MapRendererName.Trim().ToLower() != mapRenderer.MapRendererName.Trim().ToLower())
            {
                int repeatedNameCount = await dbContext.MapRenderer
                    .CountAsync(m => m.UserMapId == mapRenderer.UserMapId && m.MapRendererName.Trim().ToLower() == mapRendererData.MapRendererName.Trim().ToLower());
                if (repeatedNameCount > 0)
                {
                    throw new CustomSearchesConflictException(
                        $"The user map already has a map renderer with the same name.",
                        null);
                }

                mapRenderer.MapRendererName = mapRendererData.MapRendererName.Trim();
            }

            mapRenderer.DatasetId = mapRendererData.DatasetId;
            mapRenderer.LastModifiedBy = userId;
            mapRenderer.LastModifiedTimestamp = DateTime.UtcNow;
            mapRenderer.MapRendererType = mapRendererData.MapRendererType;
            mapRenderer.RendererRules = JsonHelper.SerializeObject(mapRendererData.RendererRules);
            mapRenderer.HasLabelRenderer = mapRendererData.HasLabelRenderer;

            dbContext.MapRenderer.Update(mapRenderer);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
