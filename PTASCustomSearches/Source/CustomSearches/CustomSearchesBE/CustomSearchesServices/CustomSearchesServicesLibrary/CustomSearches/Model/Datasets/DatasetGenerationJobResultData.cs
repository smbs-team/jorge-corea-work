namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for the dataset generation job result.
    /// </summary>
    public class DatasetGenerationJobResultData
    {
        /// <summary>
        /// Gets or sets the status of the dataset generation.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the warnings found in the dataset generation.
        /// </summary>
        public List<string> Warnings { get; set; }
    }
}
