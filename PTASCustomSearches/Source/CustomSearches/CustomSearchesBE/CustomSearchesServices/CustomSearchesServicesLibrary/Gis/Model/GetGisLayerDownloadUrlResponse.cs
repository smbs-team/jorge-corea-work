namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the GetGisLayerDownloadUrl service.
    /// </summary>
    public class GetGisLayerDownloadUrlResponse
    {
        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public long FileSize { get; set; }
    }
}
