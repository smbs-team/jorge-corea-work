namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the custom search categories.
    /// </summary>
    public class GetCustomSearchCategoriesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomSearchCategoriesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetCustomSearchCategoriesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the custom search categories.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The custom search categories.
        /// </returns>
        public async Task<GetCustomSearchCategoriesResponse> GetCustomSearchCategoriesAsync(CustomSearchesDbContext dbContext)
        {
            GetCustomSearchCategoriesResponse response = new GetCustomSearchCategoriesResponse();

            var query = dbContext.CustomSearchCategory as IQueryable<CustomSearchCategory>;
            var results = await query.ToArrayAsync();

            response.CustomSearchCategories = new CustomSearchCategoryData[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var item = results[i];

                response.CustomSearchCategories[i] = new CustomSearchCategoryData
                {
                    Id = item.CustomSearchCategoryId,
                    Name = item.CategoryName,
                    Description = item.CategoryDescription
                };
            }

            return response;
        }
    }
}
