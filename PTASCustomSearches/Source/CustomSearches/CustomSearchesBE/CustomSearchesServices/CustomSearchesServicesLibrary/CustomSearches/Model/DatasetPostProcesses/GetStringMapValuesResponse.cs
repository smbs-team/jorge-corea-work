namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the response of the GetStringMapValues service.
    /// </summary>
    public class GetStringMapValuesResponse
    {
        /// <summary>
        /// Gets or sets the string map values.
        /// </summary>
        public StringMapData[] StringMapValues { get; set; }
    }
}
