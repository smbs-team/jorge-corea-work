namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the data for duplicate dataset.
    /// </summary>
    public class DuplicateDatasetData
    {
        /// <summary>
        /// Gets or sets the name of the new dataset.
        /// </summary>
        public string DatasetName { get; set; }

        /// <summary>
        /// Gets or sets the role of the new dataset.
        /// </summary>
        public string DatasetRole { get; set; }

        /// <summary>
        /// Gets or sets the comments of the new dataset.
        /// </summary>
        public string DatasetComments { get; set; }
    }
}
