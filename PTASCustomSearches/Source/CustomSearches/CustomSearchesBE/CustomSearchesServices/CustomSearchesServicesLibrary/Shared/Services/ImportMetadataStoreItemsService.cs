namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Model;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports an metadata store item.
    /// </summary>
    public class ImportMetadataStoreItemsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMetadataStoreItemsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportMetadataStoreItemsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports an metadata store item.
        /// </summary>
        /// <param name="metadataStoreItemsData">The metadata store items to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The task.</returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task ImportMetadataStoreItemsAsync(
           ImportMetadataStoreItemsData metadataStoreItemsData,
           CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportMetadataStoreItems");

            InputValidationHelper.AssertNotEmpty(metadataStoreItemsData.MetadataStoreItems, nameof(metadataStoreItemsData.MetadataStoreItems));
            foreach (var metadataStoreItemData in metadataStoreItemsData.MetadataStoreItems)
            {
                if (metadataStoreItemData.Value == null)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"'{nameof(metadataStoreItemData.Value)}' should not be null.",
                        innerException: null);
                }

                InputValidationHelper.AssertZero(metadataStoreItemData.MetadataStoreItemId, nameof(MetadataStoreItem), nameof(metadataStoreItemData.MetadataStoreItemId));
                InputValidationHelper.AssertNotEmpty(metadataStoreItemData.StoreType, nameof(metadataStoreItemData.StoreType));
                InputValidationHelper.AssertNotEmpty(metadataStoreItemData.ItemName, nameof(metadataStoreItemData.ItemName));

                var existingItem = await dbContext.MetadataStoreItem
                    .Where(i =>
                                i.StoreType.Trim().ToLower() == metadataStoreItemData.StoreType.Trim().ToLower() &&
                                i.Version == metadataStoreItemData.Version &&
                                i.ItemName.Trim().ToLower() == metadataStoreItemData.ItemName.Trim().ToLower())
                    .FirstOrDefaultAsync();

                MetadataStoreItem newItem = metadataStoreItemData.ToEfModel();

                MetadataStoreItem itemToSave = newItem;
                if (existingItem == null)
                {
                    // Add new metadata store item if it was not found.
                    dbContext.MetadataStoreItem.Add(newItem);
                }
                else
                {
                    // Update metadata store item if it was found.
                    existingItem.Value = newItem.Value;
                    itemToSave = existingItem;
                    dbContext.MetadataStoreItem.Update(existingItem);
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
