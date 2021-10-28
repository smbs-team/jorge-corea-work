namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the user map.
    /// </summary>
    public class GetUserMapService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserMapService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserMapService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user map.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userMapId">User map id.</param>
        /// <returns>
        /// The user map response.
        /// </returns>
        public async Task<GetUserMapResponse> GetUserMap(GisDbContext dbContext, int userMapId)
        {
            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == userMapId)
                .Include(m => m.MapRenderer)
                .Include(m => m.ParentFolder)
                .FirstOrDefaultAsync();

            if (userMap == null)
            {
                throw new CustomSearchesEntityNotFoundException(
                    $"UserMap with id '{userMapId}' not found.",
                    null);
            }

            CustomSearchFolderType folderType = Enum.Parse<CustomSearchFolderType>(userMap.ParentFolder.FolderType, ignoreCase: true);
            UserMapFolderManager folderManager = new UserMapFolderManager(folderType, userMap.CreatedBy, dbContext);
            string folderPath = await folderManager.GetFolderPathAsync(userMap);

            UserMapData userMapData = new UserMapData(userMap, ModelInitializationType.FullObject, userDetails: null);
            userMapData.FolderPath = folderPath;

            GetUserMapResponse getUserMapResponse = new GetUserMapResponse
            {
                UserMap = userMapData
            };

            return getUserMapResponse;
        }
    }
}
