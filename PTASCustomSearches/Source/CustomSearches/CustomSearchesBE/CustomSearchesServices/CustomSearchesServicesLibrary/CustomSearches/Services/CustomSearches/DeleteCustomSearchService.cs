namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that soft deletes a Custom Search.
    /// </summary>
    public class DeleteCustomSearchService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCustomSearchService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteCustomSearchService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Soft deletes a Custom Search.
        /// </summary>
        /// <param name="customSearchId">The custom search id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteCustomSearchAsync(int customSearchId, CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("DeleteCustomSearch");
            CustomSearchDefinition customSearch =
                await dbContext.CustomSearchDefinition.FirstOrDefaultAsync(d => (d.CustomSearchDefinitionId == customSearchId) && (!d.IsDeleted));

            InputValidationHelper.AssertEntityExists(customSearch, "Custom search definition", customSearchId);

            var existingSearches = await (from cd in dbContext.CustomSearchDefinition
                                          where cd.CustomSearchName.Trim().ToLower() == customSearch.CustomSearchName.Trim().ToLower()
                                          select cd).ToArrayAsync();

            foreach (var currentSearch in existingSearches)
            {
                currentSearch.IsDeleted = true;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
