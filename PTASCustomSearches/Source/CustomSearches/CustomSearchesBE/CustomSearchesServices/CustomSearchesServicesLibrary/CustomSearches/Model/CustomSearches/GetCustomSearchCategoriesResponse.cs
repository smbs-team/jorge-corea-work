namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the response of the GetCustomSearchCategories service.
    /// </summary>
    public class GetCustomSearchCategoriesResponse
    {
        /// <summary>
        /// Gets or sets the custom search category data collection.
        /// </summary>
        public CustomSearchCategoryData[] CustomSearchCategories { get; set; }
    }
}
