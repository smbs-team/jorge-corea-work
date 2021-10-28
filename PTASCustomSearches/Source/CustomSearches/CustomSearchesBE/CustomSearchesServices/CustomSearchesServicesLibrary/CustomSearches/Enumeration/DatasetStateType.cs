namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// Dataset state.
    /// </summary>
    public enum DatasetStateType
    {
        /// <summary>
        /// Not processed dataset state.
        /// </summary>
        NotProcessed = 0,

        /// <summary>
        /// Generating dataset state.
        /// </summary>
        GeneratingDataset = 1,

        /// <summary>
        /// Generating indexes dataset state.
        /// </summary>
        GeneratingIndexes = 2,

        /// <summary>
        /// Processed dataset state.
        /// </summary>
        Processed = 3,

        /// <summary>
        /// Executing post process dataset state.
        /// </summary>
        ExecutingPostProcess = 4,

        /// <summary>
        /// Dataset failed to generate or refresh.
        /// </summary>
        Failed = 5
    }
}
