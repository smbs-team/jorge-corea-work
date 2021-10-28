namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the GetMapRendererCategories service.
    /// </summary>
    public class GetMapRendererCategoriesResponse
    {
        /// <summary>
        /// Gets or sets the map renderer category data collection.
        /// </summary>
        public MapRendererCategoryData[] MapRendererCategories { get; set; }
    }
}
