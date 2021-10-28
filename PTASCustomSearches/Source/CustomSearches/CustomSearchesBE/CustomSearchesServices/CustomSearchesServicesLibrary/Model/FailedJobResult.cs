namespace CustomSearchesServicesLibrary.Model
{
    /// <summary>
    /// Model for the data of the failed job result.
    /// </summary>
    public class FailedJobResult
    {
        /// <summary>
        /// Gets or sets the status of the failed job result.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the reason of the failed job result.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the exception of the failed job result.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the exception additional info.
        /// </summary>
        public object AdditionalInfo { get; set; }
    }
}
