namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the data of the row updated columns.
    /// </summary>
    public class RowUpdatedColumnsData
    {
        /// <summary>
        /// Gets or sets the row key.
        /// </summary>
        public int CustomSearchResultId { get; set; }

        /// <summary>
        /// Gets or sets the updated columns for this row.
        /// </summary>
        public string[] UpdatedColumns { get; set; }
    }
}
