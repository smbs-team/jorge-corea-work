namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Factory that creates dataset post process executors.
    /// </summary>
    public interface IDatasetPostProcessExecutorFactory
    {
        /// <summary>
        /// Creates the dataset post process executor.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>A new dataset post process executor.</returns>
        DatasetPostProcessExecutor CreatePostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            DatasetPostProcessExecutionPayloadData payload,
            SingleRowExecutionData singleRowExecutionData,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext);
    }
}
