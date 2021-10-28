namespace CustomSearchesServicesLibrary.CustomSearches
{
    /// <summary>
    /// Class containing the timeout for the different worket jobs.
    /// </summary>
    public static class WorkerJobTimeouts
    {
        /// <summary>
        /// The dataset post process execution timeout in seconds.
        /// </summary>
        public const int DatasetPostProcessExecutionTimeout = 3600;

        /// <summary>
        /// The apply land regression to schedule timeout in seconds.
        /// </summary>
        public const int ApplyLandRegressionToScheduleTimeout = 3600;

        /// <summary>
        /// The dataset backend update timeout in seconds.
        /// </summary>
        public const int DatasetBackendUpdateTimeout = 3600;

        /// <summary>
        /// The update dataset data from file timeout in seconds.
        /// </summary>
        public const int UpdateDatasetDataFromFileTimeout = 3600;

        /// <summary>
        /// The RScript timeout in seconds.
        /// </summary>
        public const int RScriptTimeout = 3600;

        /// <summary>
        /// The start job timeout in seconds.
        /// </summary>
        public const int StartJobTimeout = 3600;

        /// <summary>
        /// The dataset generation timeout in seconds.
        /// </summary>
        public const int DatasetGenerationTimeout = 3600;

        /// <summary>
        /// The refresh dataset timeout in seconds.
        /// </summary>
        public const int RefreshDatasetTimeout = 3600;

        /// <summary>
        /// The update dataset timeout in seconds.
        /// </summary>
        public const int UpdateDatasetTimeout = 3600;

        /// <summary>
        /// The duplicate dataset timeout in seconds.
        /// </summary>
        public const int DuplicateDatasetTimeout = 3600;
    }
}
