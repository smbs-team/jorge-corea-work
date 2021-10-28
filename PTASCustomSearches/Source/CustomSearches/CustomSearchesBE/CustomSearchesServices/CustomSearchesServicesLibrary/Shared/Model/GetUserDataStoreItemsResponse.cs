namespace CustomSearchesServicesLibrary.Shared.Model
{
    /// <summary>
    /// Model for the response of the GetUserDataStoreItems service.
    /// </summary>
    public class GetUserDataStoreItemsResponse
    {
        /// <summary>
        /// Gets or sets the user data store items.
        /// </summary>
        public UserDataStoreItemData[] UserDataStoreItems { get; set; }
    }
}
