namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the user maps by category.
    /// </summary>
    public class GetUserMapsByCategoryService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserMapsByCategoryService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserMapsByCategoryService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user maps by category.
        /// </summary>
        /// <param name="categoryId">Category id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The user maps.
        /// </returns>
        public async Task<GetUserMapsForUserResponse> GetUserMapsByCategoryAsync(int categoryId, GisDbContext dbContext)
        {
            GetUserMapsForUserResponse response = new GetUserMapsForUserResponse();

            var userMapQuery =
                from mc in dbContext.UserMapCategory
                join mcm in dbContext.UserMapCategoryUserMap
                on mc.UserMapCategoryId equals mcm.UserMapCategoryId
                join m in dbContext.UserMap
                on mcm.UserMapId equals m.UserMapId
                where mc.UserMapCategoryId == categoryId
                select m;

            var results = await userMapQuery.
                Include(m => m.ParentFolder).
                ToArrayAsync();

            if (results.Length > 0)
            {
                Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
                response.UserMaps = new UserMapData[results.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    var item = results[i];
                    response.UserMaps[i] = new UserMapData(item, ModelInitializationType.Summary, userDetails);
                }
            }

            return response;
        }
    }
}
