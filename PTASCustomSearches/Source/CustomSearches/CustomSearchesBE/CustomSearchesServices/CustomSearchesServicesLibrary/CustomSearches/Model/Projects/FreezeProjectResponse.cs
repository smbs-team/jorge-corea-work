namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System;

    /// <summary>
    /// Model for the response of the FreezeProjectResponse service.
    /// </summary>
    public class FreezeProjectResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeProjectResponse"/> class.
        /// </summary>
        /// <param name="queuedDatasets">The queued dataset ids.</param>
        /// <param name="jobIds">The job ids.</param>
        public FreezeProjectResponse(Guid[] queuedDatasets, int[] jobIds)
        {
            if (queuedDatasets?.Length > 0)
            {
                this.QueuedDatasets = queuedDatasets;
            }

            if (jobIds?.Length > 0)
            {
                this.JobIds = jobIds;
            }
        }

        /// <summary>
        /// Gets or sets the queued dataset ids.
        /// </summary>
        public Guid[] QueuedDatasets { get; set; }

        /// <summary>
        /// Gets or sets the job ids.
        /// </summary>
        public int[] JobIds { get; set; }
    }
}
