namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System;

    /// <summary>
    /// Model for the response of the ImportUserProjectResponse service.
    /// </summary>
    public class ImportUserProjectResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportUserProjectResponse"/> class.
        /// </summary>
        /// <param name="newProjectId">The new project identifier.</param>
        /// <param name="queuedDatasets">The queued dataset ids.</param>
        /// <param name="jobIds">The job ids.</param>
        public ImportUserProjectResponse(object newProjectId, Guid[] queuedDatasets, int[] jobIds)
        {
            this.ProjectId = newProjectId;

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
        /// Gets or sets the project id.
        /// </summary>
        public object ProjectId { get; set; }

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
