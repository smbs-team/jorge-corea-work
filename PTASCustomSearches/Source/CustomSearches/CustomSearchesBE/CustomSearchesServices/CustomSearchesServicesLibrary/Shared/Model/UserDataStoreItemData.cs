namespace CustomSearchesServicesLibrary.Shared.Model
{
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the user data store item.
    /// </summary>
    public class UserDataStoreItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataStoreItemData"/> class.
        /// </summary>
        public UserDataStoreItemData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataStoreItemData"/> class.
        /// </summary>
        /// <param name="userDataStoreItem">The user data store item.</param>
        public UserDataStoreItemData(UserDataStoreItem userDataStoreItem)
        {
            this.UserDataStoreItemId = userDataStoreItem.UserDataStoreItemId;
            this.StoreType = userDataStoreItem.StoreType;
            this.OwnerType = userDataStoreItem.OwnerType;
            this.OwnerObjectId = userDataStoreItem.OwnerObjectId;
            this.ItemName = userDataStoreItem.ItemName;
            this.Value = JsonHelper.DeserializeObject(userDataStoreItem.Value);
        }

        /// <summary>
        /// Gets or sets the identifier of the user data store item.
        /// </summary>
        public int UserDataStoreItemId { get; set; }

        /// <summary>
        /// Gets or sets the store type.
        /// </summary>
        public string StoreType { get; set; }

        /// <summary>
        /// Gets or sets the owner type.
        /// </summary>
        public string OwnerType { get; set; }

        /// <summary>
        /// Gets or sets the owner object id.
        /// </summary>
        public string OwnerObjectId { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>The entity framework model.</returns>
        public UserDataStoreItem ToEfModel()
        {
            var toReturn = new UserDataStoreItem()
            {
                UserDataStoreItemId = this.UserDataStoreItemId,
                StoreType = this.StoreType,
                OwnerType = this.OwnerType,
                OwnerObjectId = this.OwnerObjectId,
                ItemName = this.ItemName
            };

            toReturn.Value = JsonHelper.SerializeObject(this.Value);

            return toReturn;
        }
    }
}
