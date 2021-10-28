namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the GetLayerSources service.
    /// </summary>
    public class GetLayerSourcesResponse
    {
        /// <summary>
        /// Gets or sets the layer source data collection.
        /// </summary>
        public LayerSourceData[] LayerSources { get; set; }
    }
}
