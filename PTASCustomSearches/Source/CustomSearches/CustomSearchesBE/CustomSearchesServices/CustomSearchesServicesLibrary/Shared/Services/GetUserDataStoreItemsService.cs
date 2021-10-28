namespace CustomSearchesServicesLibrary.Shared.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Model;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the user data store items.
    /// </summary>
    public class GetUserDataStoreItemsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserDataStoreItemsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserDataStoreItemsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user data store items.
        /// </summary>
        /// <param name="storeType">The store data type.</param>
        /// <param name="ownerType">The owner type.</param>
        /// <param name="ownerObjectId">The owner object id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The user data store items.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<GetUserDataStoreItemsResponse> GetUserDataStoreItemsAsync(
            string storeType,
            string ownerType,
            string ownerObjectId,
            CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(storeType, nameof(storeType));
            if (string.IsNullOrWhiteSpace(ownerType))
            {
                ownerType = CustomSearchExpressionOwnerType.NoOwnerType.ToString();
            }

            ownerType = InputValidationHelper.ValidateEnum<CustomSearchExpressionOwnerType>(ownerType, nameof(ownerType)).ToString();

            if (string.IsNullOrWhiteSpace(ownerObjectId))
            {
                ownerObjectId = string.Empty;
            }

            GetUserDataStoreItemsResponse response = new GetUserDataStoreItemsResponse();

            var results = await dbContext.UserDataStoreItem
                .Where(i =>
                            i.UserId == this.ServiceContext.AuthProvider.UserInfoData.Id &&
                            i.StoreType.Trim().ToLower() == storeType.Trim().ToLower() &&
                            i.OwnerType.Trim().ToLower() == ownerType.Trim().ToLower() &&
                            i.OwnerObjectId.Trim().ToLower() == ownerObjectId.Trim().ToLower())
                .ToArrayAsync();

            response.UserDataStoreItems = new UserDataStoreItemData[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var item = results[i];

                response.UserDataStoreItems[i] = new UserDataStoreItemData(item);
            }

            return response;
        }
    }
}
