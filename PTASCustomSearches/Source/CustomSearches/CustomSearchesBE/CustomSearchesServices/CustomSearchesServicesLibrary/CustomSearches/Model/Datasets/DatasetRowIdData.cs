namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the dataset row id data.
    /// </summary>
    public class DatasetRowIdData
    {
        /// <summary>
        /// Gets or sets the custom search result id.
        /// </summary>
        public int? CustomSearchResultId { get; set; }

        /// <summary>
        /// Gets or sets the major value.
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the minor value.
        /// </summary>
        public string Minor { get; set; }
    }
}
