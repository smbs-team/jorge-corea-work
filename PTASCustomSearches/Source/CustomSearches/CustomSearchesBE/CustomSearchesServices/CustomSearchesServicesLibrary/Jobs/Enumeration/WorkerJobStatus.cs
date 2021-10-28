namespace CustomSearchesServicesLibrary.Jobs.Enumeration
{
    /// <summary>
    /// The worker job status.
    /// </summary>
    public enum WorkerJobStatus
    {
        /// <summary>
        /// Not started worker job status.
        /// </summary>
        NotStarted,

        /// <summary>
        /// In progress worker job status.
        /// </summary>
        InProgress,

        /// <summary>
        /// Finished worker job status.
        /// </summary>
        Finished,

        /// <summary>
        /// Failed worker job status.
        /// </summary>
        Failed
    }
}