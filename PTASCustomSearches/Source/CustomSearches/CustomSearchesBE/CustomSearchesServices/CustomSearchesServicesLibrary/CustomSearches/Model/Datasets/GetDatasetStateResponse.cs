namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the response of the GetDataseState service.
    /// </summary>
    public class GetDatasetStateResponse
    {
        /// <summary>
        /// Gets or sets the dataset state.
        /// </summary>
        public string DatasetState { get; set; }

        /// <summary>
        /// Gets or sets the dataset state.
        /// </summary>
        public string DatasetPostProcessState { get; set; }
    }
}
