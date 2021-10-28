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

    /// <summary>
    /// Service that creates the map renderer.
    /// </summary>
    public class CreateMapRendererService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMapRendererService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public CreateMapRendererService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Creates the map renderer.
        /// </summary>
        /// <param name="customSearchesDbContext">Custom searches database context.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapRendererData">Map renderer data.</param>
        /// <returns>
        /// The map render id.
        /// </returns>
        public async Task<int> CreateMapRenderer(CustomSearchesDbContext customSearchesDbContext, GisDbContext dbContext, MapRendererData mapRendererData)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            CustomSearchesEFLibrary.CustomSearches.Model.Dataset dataset =
                await customSearchesDbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == mapRendererData.DatasetId);

            InputValidationHelper.AssertEntityExists(dataset, nameof(dataset), mapRendererData.DatasetId);

            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == mapRendererData.UserMapId)
                .Include(m => m.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userMap, nameof(userMap), mapRendererData.UserMapId);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, userMap.ParentFolder, userMap.IsLocked, "CreateMapRenderer");

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess: false);

            MapRenderer mapRenderer = mapRendererData.ToEfModel();
            mapRenderer.CreatedBy = userId;
            mapRenderer.CreatedTimestamp = DateTime.UtcNow;
            mapRenderer.LastModifiedBy = userId;
            mapRenderer.LastModifiedTimestamp = DateTime.UtcNow;

            int repeatedNameCount = await dbContext.MapRenderer
                .CountAsync(m => m.UserMapId == mapRenderer.UserMapId && m.MapRendererName.Trim().ToLower() == mapRendererData.MapRendererName.Trim().ToLower());
            if (repeatedNameCount > 0)
            {
                throw new CustomSearchesConflictException(
                    $"The user map already has a map renderer with the same name.",
                    null);
            }

            dbContext.MapRenderer.Add(mapRenderer);
            await dbContext.ValidateAndSaveChangesAsync();

            return mapRenderer.MapRendererId;
        }
    }
}
