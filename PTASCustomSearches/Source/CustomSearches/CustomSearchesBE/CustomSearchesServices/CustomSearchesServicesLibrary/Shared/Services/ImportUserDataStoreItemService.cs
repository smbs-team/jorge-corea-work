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
    /// Service that imports an user data store item.
    /// </summary>
    public class ImportUserDataStoreItemService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportUserDataStoreItemService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportUserDataStoreItemService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports an user data store item.
        /// </summary>
        /// <param name="userDataStoreItemData">The user data store item to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The imported object id.</returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<IdResult> ImportUserDataStoreItemAsync(
            UserDataStoreItemData userDataStoreItemData,
            CustomSearchesDbContext dbContext)
        {
            if (userDataStoreItemData.Value == null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"'{nameof(userDataStoreItemData.Value)}' should not be null.",
                    innerException: null);
            }

            InputValidationHelper.AssertZero(userDataStoreItemData.UserDataStoreItemId, nameof(UserDataStoreItem), nameof(userDataStoreItemData.UserDataStoreItemId));
            InputValidationHelper.AssertNotEmpty(userDataStoreItemData.StoreType, nameof(userDataStoreItemData.StoreType));
            if (string.IsNullOrWhiteSpace(userDataStoreItemData.OwnerType))
            {
                userDataStoreItemData.OwnerType = CustomSearchExpressionOwnerType.NoOwnerType.ToString();
            }

            userDataStoreItemData.OwnerType =
                InputValidationHelper.ValidateEnum<CustomSearchExpressionOwnerType>(userDataStoreItemData.OwnerType, nameof(userDataStoreItemData.OwnerType)).ToString();

            if (string.IsNullOrWhiteSpace(userDataStoreItemData.OwnerObjectId))
            {
                userDataStoreItemData.OwnerObjectId = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(userDataStoreItemData.ItemName))
            {
                userDataStoreItemData.ItemName = string.Empty;
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            var existingItem = await dbContext.UserDataStoreItem
                .Where(i =>
                            i.UserId == userId &&
                            i.StoreType.Trim().ToLower() == userDataStoreItemData.StoreType.Trim().ToLower() &&
                            i.OwnerType.Trim().ToLower() == userDataStoreItemData.OwnerType.Trim().ToLower() &&
                            i.OwnerObjectId.Trim().ToLower() == userDataStoreItemData.OwnerObjectId.Trim().ToLower() &&
                            i.ItemName.Trim().ToLower() == userDataStoreItemData.ItemName.Trim().ToLower())
                .FirstOrDefaultAsync();

            UserDataStoreItem newItem = userDataStoreItemData.ToEfModel();

            UserDataStoreItem itemToSave = newItem;
            if (existingItem == null)
            {
                // Add new user data store item if it was not found.
                newItem.UserId = userId;
                dbContext.UserDataStoreItem.Add(newItem);
            }
            else
            {
                // Update user data store item if it was found.
                existingItem.Value = newItem.Value;
                itemToSave = existingItem;
                dbContext.UserDataStoreItem.Update(existingItem);
            }

            await dbContext.ValidateAndSaveChangesAsync();
            return new IdResult(itemToSave.UserDataStoreItemId);
        }
    }
}
