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
    /// Service that gets the user map categories.
    /// </summary>
    public class GetUserMapCategoriesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserMapCategoriesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserMapCategoriesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user map categories.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The user map categories.
        /// </returns>
        public async Task<GetUserMapCategoriesResponse> GetUserMapCategories(GisDbContext dbContext)
        {
            GetUserMapCategoriesResponse response = new GetUserMapCategoriesResponse();

            var categories = await dbContext.UserMapCategory.ToArrayAsync();

            response.UserMapCategories = new UserMapCategoryData[categories.Length];

            for (int i = 0; i < categories.Length; i++)
            {
                UserMapCategory userMapCategory = categories[i];

                UserMapCategoryData userMapCategoryData = new UserMapCategoryData(userMapCategory, ModelInitializationType.FullObjectWithDepedendencies);

                response.UserMapCategories[i] = userMapCategoryData;
            }

            return response;
        }
    }
}
