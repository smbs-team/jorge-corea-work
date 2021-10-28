namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes an metadata store item.
    /// </summary>
    public class DeleteMetadataStoreItemService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMetadataStoreItemService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteMetadataStoreItemService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes an metadata store item.
        /// </summary>
        /// <param name="metadataStoreItemId">The metadata store item id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Metadata store item was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteMetadataStoreItemAsync(int metadataStoreItemId, CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("DeleteMetadataStoreItem");
            MetadataStoreItem metadataStoreItem =
                await dbContext.MetadataStoreItem.FirstOrDefaultAsync(i => (i.MetadataStoreItemId == metadataStoreItemId));

            InputValidationHelper.AssertEntityExists(metadataStoreItem, nameof(metadataStoreItem), metadataStoreItemId);

            dbContext.MetadataStoreItem.Remove(metadataStoreItem);

            await dbContext.SaveChangesAsync();
        }
    }
}
