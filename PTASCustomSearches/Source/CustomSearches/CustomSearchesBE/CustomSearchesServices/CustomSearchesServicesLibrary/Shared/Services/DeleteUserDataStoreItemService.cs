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
    /// Service that deletes an user data store item.
    /// </summary>
    public class DeleteUserDataStoreItemService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserDataStoreItemService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteUserDataStoreItemService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes an user data store item.
        /// </summary>
        /// <param name="userDataStoreItemId">The user data store item id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User data store item definition was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteUserDataStoreItemAsync(int userDataStoreItemId, CustomSearchesDbContext dbContext)
        {
            UserDataStoreItem userDataStoreItem =
                await dbContext.UserDataStoreItem.FirstOrDefaultAsync(i => (i.UserDataStoreItemId == userDataStoreItemId));

            InputValidationHelper.AssertEntityExists(userDataStoreItem, nameof(userDataStoreItem), userDataStoreItemId);
            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userDataStoreItem.UserId, "DeleteUserDataStoreItem");

            dbContext.UserDataStoreItem.Remove(userDataStoreItem);

            await dbContext.SaveChangesAsync();
        }
    }
}
