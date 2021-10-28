namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System;

    /// <summary>
    /// Model for the response of the ExecuteCustomSearch service.
    /// </summary>
    public class ExecuteCustomSearchResponse
    {
        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the job.
        /// </summary>
        public int JobId { get; set; }
    }
}
