namespace CustomSearchesServicesLibrary.Shared.Model
{
    /// <summary>
    /// Model indicating a collection of metadata store items to import.
    /// </summary>
    public class ImportMetadataStoreItemsData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMetadataStoreItemsData"/> class.
        /// </summary>
        public ImportMetadataStoreItemsData()
        {
        }

        /// <summary>
        /// Gets or sets the array of metadata store items.
        /// </summary>
        public MetadataStoreItemData[] MetadataStoreItems { get; set; }
    }
}
