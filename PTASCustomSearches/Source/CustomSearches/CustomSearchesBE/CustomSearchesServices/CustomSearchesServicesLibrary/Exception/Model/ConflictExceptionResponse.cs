namespace CustomSearchesServicesLibrary.Exception.Model
{
    /// <summary>
    /// Model for the response when a conflict exception occurs.
    /// </summary>
    public class ConflictExceptionResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the dataset state.
        /// </summary>
        public string DatasetState { get; set; }

        /// <summary>
        /// Gets or sets the post process state.
        /// </summary>
        public string PostProcessState { get; set; }

        /// <summary>
        /// Gets or sets the database lock type.
        /// </summary>
        public string DbLockType { get; set; }

        /// <summary>
        /// Gets or sets the job id.
        /// </summary>
        public int JobId { get; set; }
    }
}
