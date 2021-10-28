namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the data of the custom search grid sorting.
    /// </summary>
    public class CustomSearchGridSortingData
    {
        /// <summary>
        /// Gets or sets the sort field.
        /// </summary>
        public string ColId { get; set; }

        /// <summary>
        /// Gets or sets the sort direction (i.e 'asc', 'desc').
        /// </summary>
        public string Sort { get; set; }
    }
}
