namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that updates the dataset.
    /// </summary>
    public class UpdateDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the dataset.
        /// </summary>
        /// <param name="datasetData">Dataset data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="FolderManagerException">Invalid folder path format or folder not found.</exception>
        /// <exception cref="CustomSearchesConflictException">Dataset is used by a worker job or dataset name repeated.</exception>
        public async Task<IdResult> UpdateDatasetAsync(DatasetData datasetData, CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetData.DatasetId)
                .Include(d => d.InverseSourceDataset)
                .Include(d => d.ParentFolder)
                .ThenInclude(f => f.Dataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetData.DatasetId);

            // Should be applied over source dataset.
            if (dataset.SourceDatasetId != null)
            {
                dataset = await dbContext.Dataset
                    .Where(d => d.DatasetId == dataset.SourceDatasetId)
                    .Include(d => d.InverseSourceDataset)
                    .Include(d => d.ParentFolder)
                    .ThenInclude(f => f.Dataset)
                    .FirstOrDefaultAsync();
            }

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "UpdateDataset");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            bool parametersChanged = false;

            if (datasetData.ParameterValues?.Length > 0)
            {
                string newParameters = JsonHelper.SerializeObject(datasetData.ParameterValues);

                if (newParameters != dataset.ParameterValues)
                {
                    await DatasetHelper.TestAlterDatasetLockAsync(dataset.DatasetId, dataset.DataSetState, dataset.DataSetPostProcessState, true, userId, lockingJobId: null, dbContext);
                    dataset.ParameterValues = newParameters;
                    parametersChanged = true;
                }
            }

            InputValidationHelper.AssertNotEmpty(datasetData.DatasetName, nameof(datasetData.DatasetName));
            dataset.DatasetName = datasetData.DatasetName.Trim();

            Folder previousFolder = dataset.ParentFolder;
            if (DatasetHelper.IsUserDataset(dataset) && !string.IsNullOrWhiteSpace(datasetData.FolderPath))
            {
                DatasetFolderManager.ValidateFolderPath(datasetData.FolderPath);
                CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(datasetData.FolderPath);
                CustomSearchFolderType prevFolderType = Enum.Parse<CustomSearchFolderType>(dataset.ParentFolder.FolderType, ignoreCase: true);
                this.ServiceContext.AuthProvider.AuthorizeChangeItemFolderOperation(dataset.UserId, prevFolderType, folderType, "UpdateDataset");

                DatasetFolderManager folderManager = new DatasetFolderManager(folderType, dataset.UserId, dbContext);

                // If dataset change of folder and the dataset name is repeated throw an exception.
                await folderManager.AssignFolderToItemAsync(datasetData.FolderPath, dataset, userId);
            }

            // If dataset does not change of folder and the dataset name is repeated throw an exception.
            if (previousFolder == dataset.ParentFolder)
            {
                if (dataset.ParentFolder != null)
                {
                    bool repeatedName = dataset.ParentFolder.Dataset.Count(d => d.DatasetName.Trim().ToLower() == dataset.DatasetName.ToLower()) > 1;
                    if (repeatedName)
                    {
                        throw new CustomSearchesConflictException(
                            $"The folder already has a dataset with the same name.",
                            null);
                    }
                }
                else
                {
                    await DatasetHelper.AssertUniqueUserProjectDatasetName(userProject.UserProjectId, dataset.DatasetName, dbContext);
                }
            }

            dataset.LastModifiedTimestamp = DateTime.UtcNow;
            dataset.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            dbContext.Dataset.Update(dataset);
            await dbContext.ValidateAndSaveChangesAsync();

            int jobId = -1;
            if (parametersChanged)
            {
                DatasetGenerationPayloadData payload = new DatasetGenerationPayloadData
                {
                    CustomSearchDefinitionId = dataset.CustomSearchDefinitionId,
                    Parameters = datasetData.ParameterValues,
                    DatasetId = dataset.DatasetId,
                    ExecutionMode = DatasetExecutionMode.Update
                };

                jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                    "DatasetGeneration",
                    "DatasetGenerationJobType",
                    userId,
                    payload,
                    WorkerJobTimeouts.DatasetGenerationTimeout);
            }

            return new IdResult(jobId);
        }
    }
}
