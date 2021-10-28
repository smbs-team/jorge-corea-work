namespace CustomSearchesServicesLibrary.Jobs.Model
{
    /// <summary>
    /// Model for the response of the GetJobStatus service.
    /// </summary>
    public class GetJobStatusResponse
    {
        /// <summary>
        /// Gets or sets the job status.
        /// </summary>
        public string JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the job result.
        /// </summary>
        public object JobResult { get; set; }
    }
}
