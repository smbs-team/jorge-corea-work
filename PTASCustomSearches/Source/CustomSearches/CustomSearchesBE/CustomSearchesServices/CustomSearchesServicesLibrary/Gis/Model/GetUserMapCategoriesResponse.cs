namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the GetUserMapCategories service.
    /// </summary>
    public class GetUserMapCategoriesResponse
    {
        /// <summary>
        /// Gets or sets the user map category data collection.
        /// </summary>
        public UserMapCategoryData[] UserMapCategories { get; set; }
    }
}
