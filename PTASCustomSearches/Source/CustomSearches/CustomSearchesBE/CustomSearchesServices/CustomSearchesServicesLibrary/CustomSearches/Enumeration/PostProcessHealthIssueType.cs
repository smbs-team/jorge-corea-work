namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// PostProcess Health Type.
    /// </summary>
    public enum PostProcessHealthIssueType
    {
        /// <summary>
        /// The post-process is dirty.
        /// </summary>
        Dirty = 0,

        /// <summary>
        /// The post-process failed executing.
        /// </summary>
        ExecutionFailed = 1,

        /// <summary>
        /// The post-process has not been executed.
        /// </summary>
        NotExecuted = 2,

        /// <summary>
        /// The post-process calculated view is missing.
        /// </summary>
        MissingCalculatedView = 3,

        /// <summary>
        /// Bulk Update validation error. Should not send back to client
        /// </summary>
        BulkUpdateValidationError = 4
    }
}
