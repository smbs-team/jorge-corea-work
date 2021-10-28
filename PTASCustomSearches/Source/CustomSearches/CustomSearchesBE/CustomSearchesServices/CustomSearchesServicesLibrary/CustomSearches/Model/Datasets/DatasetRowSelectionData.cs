namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the dataset row selection data.
    /// </summary>
    public class DatasetRowSelectionData : DatasetRowIdData
    {
        /// <summary>
        /// Gets or sets a value indicating whether the row is selected.
        /// </summary>
        public bool Selection { get; set; }
    }
}
