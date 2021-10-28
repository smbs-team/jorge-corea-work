namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the response of the GetCustomSearchesByCategory service.
    /// </summary>
    public class GetCustomSearchesByCategoryResponse
    {
        /// <summary>
        /// Gets or sets the custom searches.
        /// </summary>
        public CustomSearchDefinitionData[] CustomSearches { get; set; }
    }
}
