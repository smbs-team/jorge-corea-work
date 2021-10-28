namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the UserMapCategories service.
    /// </summary>
    public class UserMapCategoriesResponse
    {
        /// <summary>
        /// Gets or sets the user map category data collection.
        /// </summary>
        public MapRendererCategoryData[] MapRendererCategories { get; set; }
    }
}
