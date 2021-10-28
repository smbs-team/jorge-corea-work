namespace CustomSearchesWorkerLibrary.DatasetProcessor.Model
{
    using CustomSearchesServicesLibrary.Model;
    using System.Collections.Generic;

    /// <summary>
    /// Model for the unsaved dataset deletion job result.
    /// </summary>
    public class UnsavedDatasetDeletionJobResultData
    {
        /// <summary>
        /// Gets or sets the status of the dataset deletion.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the errors found in the dataset deletion.
        /// </summary>
        public List<DeleteEntityErrorData> DeleteEntityErrors { get; set; }
    }
}
