namespace CustomSearchesWorkerLibrary.Enumeration
{
    /// <summary>
    /// Custo Searches Job Types
    /// </summary>
    enum CustomSearchesJobTypes
    {
        /// <summary>
        /// RScript job type
        /// </summary>
        RScriptJobType,

        /// <summary>
        /// Dataset generation job type
        /// </summary>
        DatasetGenerationJobType,

        /// <summary>
        /// Dataset post process execution job type
        /// </summary>
        DatasetPostProcessExecutionJobType,

        /// <summary>
        /// Dataset backend update job type
        /// </summary>
        DatasetBackendUpdateJobType,

        /// <summary>
        /// Update Dataset Data From File job type
        /// </summary>
        UpdateDatasetDataFromFileJobType,

        /// <summary>
        /// Apply land regression to schedule job type
        /// </summary>
        ApplyLandRegressionToScheduleJobType,

        /// <summary>
        /// Unsaved dataset deletion job type
        /// </summary>
        UnsavedDatasetDeletionJobType,

        /// <summary>
        /// Apply model job type
        /// </summary>
        ApplyModelJobType,

        /// <summary>
        /// Dataset generation job type
        /// </summary>
        UserProjectGenerationJobType,
    }
}
