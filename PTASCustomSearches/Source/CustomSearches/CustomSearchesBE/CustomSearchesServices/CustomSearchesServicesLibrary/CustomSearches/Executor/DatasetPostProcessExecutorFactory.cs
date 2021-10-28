namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Factory that creates dataset post process executors.
    /// </summary>
    /// <seealso cref="PTASTileStorageWorkerLibrary.SystemProcess.IProcessFactory" />
    public class DatasetPostProcessExecutorFactory : IDatasetPostProcessExecutorFactory
    {
        /// <summary>
        /// Creates the dataset post process executor.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previous view script.</param>
        /// <param name="payload">The dataset generation payload data.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>
        /// A new dataset post process executor.
        /// </returns>
        public DatasetPostProcessExecutor CreatePostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            DatasetPostProcessExecutionPayloadData payload,
            SingleRowExecutionData singleRowExecutionData,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext)
        {
            DatasetPostProcessExecutor datasetPostProcessExecutor = null;
            switch (datasetPostProcess.PostProcessType)
            {
                case nameof(DatasetPostProcessType.ExceptionPostProcess):
                    datasetPostProcessExecutor = new ExceptionDatasetPostProcessExecutor(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext);
                    break;

                case nameof(DatasetPostProcessType.FilterPostProcess):
                    datasetPostProcessExecutor = new FilterDatasetPostProcessExecutor(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext);
                    break;

                case nameof(DatasetPostProcessType.RScriptPostProcess):
                    datasetPostProcessExecutor = new RScriptDatasetPostProcessExecutor(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext);
                    break;

                case nameof(DatasetPostProcessType.StoredProcedureUpdatePostProcess):
                    datasetPostProcessExecutor = new StoredProcedureUpdatePostProcessExecutor(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext);
                    break;

                case nameof(DatasetPostProcessType.CustomModelingStepPostProcess):
                    datasetPostProcessExecutor = new CustomModelingStepPostProcessExecutor(dataset, datasetPostProcess, previousViewScript, payload, dbContext, serviceContext);
                    break;

                default:
                    break;
            }

            return datasetPostProcessExecutor;
        }
    }
}
