namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the data of the execute custom search.
    /// </summary>
    public class ExecuteCustomSearchData
    {
        /// <summary>
        /// Gets or sets the custom search parameters.
        /// </summary>
        public CustomSearchParameterValueData[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the dataset name.
        /// </summary>
        public string DatasetName { get; set; }

        /// <summary>
        /// Gets or sets the dataset comments.
        /// </summary>
        public string Comments { get; set; }
    }
}
