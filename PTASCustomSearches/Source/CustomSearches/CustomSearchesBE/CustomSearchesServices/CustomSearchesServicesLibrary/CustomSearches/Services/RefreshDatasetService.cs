namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that refreshes a dataset.
    /// </summary>
    public class RefreshDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RefreshDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Refreshes a dataset.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset is used by a worker job.</exception>
        public async Task<IdResult> RefreshDatasetAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.InverseSourceDataset)
                .Include(d => d.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            // Should be applied over source dataset.
            if (dataset.SourceDatasetId != null)
            {
                dataset = await dbContext.Dataset
                 .Where(d => d.DatasetId == dataset.SourceDatasetId)
                 .Include(d => d.InverseSourceDataset)
                 .Include(d => d.ParentFolder)
                 .FirstOrDefaultAsync();
            }

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "RefreshDataset");

            return await this.AddRefreshDatasetToJobQueueAsync(dataset, major: null, minor: null, dbContext, needsPostProcessExecution: false);
        }

        /// <summary>
        /// Adds the refresh dataset to the job queue.
        /// </summary>
        /// <param name="dataset">The dataset id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="needsPostProcessExecution">A value indicating whether the related post processes should be executed.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<IdResult> AddRefreshDatasetToJobQueueAsync(
            Dataset dataset,
            string major,
            string minor,
            CustomSearchesDbContext dbContext,
            bool needsPostProcessExecution,
            string queueName = "DatasetGeneration")
        {
            InputValidationHelper.AssertDatasetDataNotLocked(dataset);
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            await DatasetHelper.TestAlterDatasetLockAsync(dataset.DatasetId, dataset.DataSetState, dataset.DataSetPostProcessState, true, userId, lockingJobId: null, dbContext);

            DatasetData datasetData = new DatasetData(dataset, ModelInitializationType.Summary, userDetails: null);

            var refreshParameters = datasetData.ParameterValues.ToList();
            if (!string.IsNullOrWhiteSpace(major) && !string.IsNullOrWhiteSpace(minor))
            {
                refreshParameters.Add(new CustomSearchParameterValueData() { Name = "Major", Value = major });
                refreshParameters.Add(new CustomSearchParameterValueData() { Name = "Minor", Value = minor });
            }

            DatasetGenerationPayloadData payload = new DatasetGenerationPayloadData
            {
                CustomSearchDefinitionId = datasetData.CustomSearchDefinitionId,
                Parameters = refreshParameters.ToArray(),
                DatasetId = datasetData.DatasetId,
                ExecutionMode = DatasetExecutionMode.Refresh,
                NeedsPostProcessExecution = needsPostProcessExecution
            };

            int jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                queueName,
                "DatasetGenerationJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetGenerationTimeout);

            return new IdResult(jobId);
        }
    }
}
