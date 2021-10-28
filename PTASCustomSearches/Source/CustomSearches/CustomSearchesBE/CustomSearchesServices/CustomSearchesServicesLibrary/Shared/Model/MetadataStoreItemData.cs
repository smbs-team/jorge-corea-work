namespace CustomSearchesServicesLibrary.Shared.Model
{
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the metadata store item.
    /// </summary>
    public class MetadataStoreItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStoreItemData"/> class.
        /// </summary>
        public MetadataStoreItemData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStoreItemData"/> class.
        /// </summary>
        /// <param name="metadataStoreItem">The metadata store item.</param>
        public MetadataStoreItemData(MetadataStoreItem metadataStoreItem)
        {
            this.MetadataStoreItemId = metadataStoreItem.MetadataStoreItemId;
            this.StoreType = metadataStoreItem.StoreType;
            this.Version = metadataStoreItem.Version;
            this.ItemName = metadataStoreItem.ItemName;

            this.Value = JsonHelper.DeserializeObject(metadataStoreItem.Value);
        }

        /// <summary>
        /// Gets or sets the identifier of the metadata store item.
        /// </summary>
        public int MetadataStoreItemId { get; set; }

        /// <summary>
        /// Gets or sets the version of the metadata store item.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the store type.
        /// </summary>
        public string StoreType { get; set; }

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
        public MetadataStoreItem ToEfModel()
        {
            var toReturn = new MetadataStoreItem()
            {
                MetadataStoreItemId = this.MetadataStoreItemId,
                StoreType = this.StoreType,
                ItemName = this.ItemName,
                Version = this.Version
            };

            toReturn.Value = JsonHelper.SerializeObject(this.Value);

            return toReturn;
        }
    }
}
