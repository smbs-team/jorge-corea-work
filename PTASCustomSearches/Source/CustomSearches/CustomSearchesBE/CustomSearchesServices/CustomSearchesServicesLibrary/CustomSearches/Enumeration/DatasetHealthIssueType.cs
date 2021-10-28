namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// Dataset health issue type.
    /// </summary>
    public enum DatasetHealthIssueType
    {
        /// <summary>
        /// The dataset failed generating.
        /// </summary>
        FailedGeneration = 0,

        /// <summary>
        /// A post-process execution is pending on the dataset.
        /// </summary>
        NeedsPostProcessUpdate = 1
    }
}
