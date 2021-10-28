namespace CustomSearchesServicesLibrary.Shared.Model
{
    /// <summary>
    /// Model for the response of the GetMetadataStoreItems service.
    /// </summary>
    public class GetMetadataStoreItemsResponse
    {
        /// <summary>
        /// Gets or sets the metadata store items.
        /// </summary>
        public MetadataStoreItemData[] MetadataStoreItems { get; set; }
    }
}
