namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the data of the custom search category.
    /// </summary>
    public class CustomSearchCategoryData
    {
        /// <summary>
        /// Gets or sets the identifier of the custom search category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the custom search category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the custom search category.
        /// </summary>
        public string Description { get; set; }
    }
}
