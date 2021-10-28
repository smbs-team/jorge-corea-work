namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the dataset row id data.
    /// </summary>
    public class SingleRowExecutionData : DatasetRowIdData
    {
        /// <summary>
        /// Gets a value indicating whether the payload is used in a single row execution mode.
        /// </summary>
        public bool IsSingleRowExecutionMode
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Major) && !string.IsNullOrWhiteSpace(this.Minor);
            }
        }
    }
}
