namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model indicating a collection of dataset rows to revert.
    /// </summary>
    public class RevertDatasetUpdatesData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevertDatasetUpdatesData"/> class.
        /// </summary>
        public RevertDatasetUpdatesData()
        {
        }

        /// <summary>
        /// Gets or sets the array of rows to revert.
        /// </summary>
        public DatasetRowIdData[] RowIds { get; set; }
    }
}
