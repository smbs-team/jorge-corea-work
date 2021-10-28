namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Model;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the metadata store items.
    /// </summary>
    /// <seealso cref="CustomSearchesServicesLibrary.ServiceFramework.BaseService" />
    public class GetMetadataStoreItemsService : BaseService
    {
        /// <summary>
        /// The global constant store type.
        /// </summary>
        public const string GlobalConstantStoreType = "GlobalConstant";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMetadataStoreItemsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetMetadataStoreItemsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the metadata store items by store type and item name.
        /// </summary>
        /// <param name="storeType">The store data type.</param>
        /// <param name="itemName">The item name.</param>
        /// <param name="latestVersion">Value indicating whether the results should include only the latest version.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The metadata store items.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<GetMetadataStoreItemsResponse> GetMetadataStoreItemAsync(
            string storeType,
            string itemName,
            bool latestVersion,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(storeType, nameof(storeType));
            InputValidationHelper.AssertNotEmpty(itemName, nameof(itemName));

            GetMetadataStoreItemsResponse response = new GetMetadataStoreItemsResponse();

            IQueryable<MetadataStoreItem> query = dbContext.MetadataStoreItem
                    .Where(i => i.StoreType.Trim().ToLower() == storeType.Trim().ToLower() &&
                                i.ItemName.Trim().ToLower() == itemName.Trim().ToLower())
                    .OrderByDescending(i => i.Version);

            if (latestVersion)
            {
                query = query.Take(1);
            }

            var results = await query.ToArrayAsync();

            response.MetadataStoreItems = new MetadataStoreItemData[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var item = results[i];

                response.MetadataStoreItems[i] = new MetadataStoreItemData(item);
            }

            return response;
        }

        /// <summary>
        /// Gets the metadata store items by store type.
        /// </summary>
        /// <param name="storeType">The store data type.</param>
        /// <param name="latestVersion">Value indicating whether the results should include only the latest version.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The metadata store items.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<GetMetadataStoreItemsResponse> GetMetadataStoreItemsAsync(
            string storeType,
            bool latestVersion,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(storeType, nameof(storeType));

            GetMetadataStoreItemsResponse response = new GetMetadataStoreItemsResponse();
            MetadataStoreItem[] results = null;

            if (latestVersion)
            {
                results = (await dbContext.MetadataStoreItem
                    .Where(p => p.StoreType.Trim().ToLower() == storeType.Trim().ToLower()).ToArrayAsync())
                    .GroupBy(p => p.ItemName)
                    .Select(g => g.OrderByDescending(p => p.Version)
                                .FirstOrDefault()).ToArray();
            }
            else
            {
                results = await dbContext.MetadataStoreItem
                    .Where(p => p.StoreType.Trim().ToLower() == storeType.Trim().ToLower())
                    .OrderByDescending(p => p.ItemName)
                    .ThenByDescending(p => p.Version).ToArrayAsync();
            }

            response.MetadataStoreItems = new MetadataStoreItemData[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var item = results[i];

                response.MetadataStoreItems[i] = new MetadataStoreItemData(item);
            }

            return response;
        }
    }
}
