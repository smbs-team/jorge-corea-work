namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the response of the GetDatasetColumns service.
    /// </summary>
    public class GetDatasetColumnsResponse
    {
        /// <summary>
        /// Gets or sets the dataset column data collection.
        /// </summary>
        public CustomSearchColumnDefinitionData[] DatasetColumns { get; set; }
    }
}
