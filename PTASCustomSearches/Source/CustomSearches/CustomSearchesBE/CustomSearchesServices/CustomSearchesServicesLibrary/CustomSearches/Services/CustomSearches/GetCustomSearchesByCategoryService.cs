namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the custom searches by category.
    /// </summary>
    public class GetCustomSearchesByCategoryService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomSearchesByCategoryService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetCustomSearchesByCategoryService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the custom searches by category.
        /// </summary>
        /// <param name="categoryId">Category id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The custom searches.
        /// </returns>
        public async Task<GetCustomSearchesByCategoryResponse> GetCustomSearchesByCategoryAsync(int categoryId, CustomSearchesDbContext dbContext)
        {
            GetCustomSearchesByCategoryResponse response = new GetCustomSearchesByCategoryResponse();

            bool isAdmin = this.ServiceContext.AuthProvider.GetIsAdminUser();

            var query = from csc in dbContext.CustomSearchCategory
                        join cscd in dbContext.CustomSearchCategoryDefinition
                        on csc.CustomSearchCategoryId equals cscd.CustomSearchCategoryId
                        join csd in dbContext.CustomSearchDefinition
                        on cscd.CustomSearchDefinitionId equals csd.CustomSearchDefinitionId
                        where csc.CustomSearchCategoryId == categoryId && csd.IsDeleted == false && ((csd.Validated == true) || isAdmin)
                        select csd;

            var results = (await query.ToArrayAsync())
                .GroupBy(csd => csd.CustomSearchName.Trim().ToLower())
                .Select(gcsd => gcsd.OrderByDescending(d => d.Version).FirstOrDefault());

            if (results.Count() > 0)
            {
                var customSearchDefinitionDataList = new List<CustomSearchDefinitionData>();
                foreach (var customSearchDefinition in results)
                {
                    if (this.ServiceContext.AuthProvider.IsAuthorizedToAnyRole(customSearchDefinition.ExecutionRoles))
                    {
                        customSearchDefinitionDataList.Add(new CustomSearchDefinitionData(customSearchDefinition, ModelInitializationType.Summary));
                    }
                }

                response.CustomSearches = customSearchDefinitionDataList.Count() > 0 ? customSearchDefinitionDataList.ToArray() : null;
            }

            return response;
        }
    }
}
