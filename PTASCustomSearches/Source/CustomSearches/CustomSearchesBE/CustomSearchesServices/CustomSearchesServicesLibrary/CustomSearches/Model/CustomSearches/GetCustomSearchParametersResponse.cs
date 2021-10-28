namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the response of the GetCustomSearchParameters service.
    /// </summary>
    public class GetCustomSearchParametersResponse
    {
        /// <summary>
        /// Gets or sets the custom search parameter data collection.
        /// </summary>
        public CustomSearchParameterData[] CustomSearchParameters { get; set; }
    }
}
