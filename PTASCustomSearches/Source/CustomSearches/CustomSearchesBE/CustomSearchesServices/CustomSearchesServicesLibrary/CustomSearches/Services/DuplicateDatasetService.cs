namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that duplicates a dataset and all its child objects (including charts and DB tables).
    /// </summary>
    public class DuplicateDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DuplicateDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Duplicates a dataset and all its child objects (including charts and DB tables).
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="newUserProject">
        /// The new user project to create. The creation of the project and its relationships should be created outside this method.
        /// </param>
        /// <param name="applyRowFilter">Value indicating whether the row filter should be used to define the rows to duplicate.</param>
        /// <param name="applyUserSelection">Value indicating whether the user selection should be used to define the rows to duplicate.</param>
        /// <param name="duplicateDatasetData">The dataset data.</param>
        /// <param name="duplicatePostProcess">Value indicating whether the post processes should be duplicated.</param>
        /// <param name="needsPostProcessExecution">Value indicating whether the post processes should be executed.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The custom search response.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset or the folder already has a dataset with the same name.</exception>
        /// <exception cref="NotSupportedException">The dataset should belongs to a folder or user project.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<ExecuteCustomSearchResponse> DuplicateDatasetAsync(
            Guid datasetId,
            UserProject newUserProject,
            bool applyRowFilter,
            bool applyUserSelection,
            DuplicateDatasetData duplicateDatasetData,
            bool duplicatePostProcess,
            bool needsPostProcessExecution,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                dbContext,
                datasetId,
                includeRelatedExpressions: true,
                includeParentFolder: true,
                includeInverseSourceDatasets: false,
                includeUserProject: true,
                includeDatasetUserClientState: false);

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            DatasetHelper.DetachDatasetWithDependencies(dbContext, dataset);

            if (dataset.UserProjectDataset.Count > 0)
            {
                // If the dataset is duplicated in a new user project, the project and its relationships should be created outside this method.
                if (newUserProject != null)
                {
                    dataset.UserProjectDataset.Clear();
                }
                else
                {
                    // If the dataset is duplicated in an existing user project, the existing project should own the dataset.
                    foreach (var userProjectDataset in dataset.UserProjectDataset)
                    {
                        if (!userProjectDataset.OwnsDataset)
                        {
                            dataset.UserProjectDataset.Remove(userProjectDataset);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(duplicateDatasetData?.DatasetRole))
                            {
                                userProjectDataset.DatasetRole = duplicateDatasetData.DatasetRole;
                            }
                        }
                    }
                }
            }

            var newDatasetId = await this.DuplicateDatasetAsync(
                dataset,
                newUserProject,
                duplicateDatasetData,
                duplicatePostProcess,
                dbContext);

            dbContext.Dataset.Add(dataset);

            DatasetGenerationPayloadData payload = new DatasetGenerationPayloadData
            {
                CustomSearchDefinitionId = dataset.CustomSearchDefinitionId,
                Parameters = null,
                DatasetId = newDatasetId,
                SourceDatasetId = datasetId,
                ExecutionMode = DatasetExecutionMode.Generate,
                ApplyRowFilterFromSourceDataset = applyRowFilter,
                ApplyUserSelectionFromSourceDataset = applyUserSelection,
                NeedsPostProcessExecution = needsPostProcessExecution
            };

            await dbContext.ValidateAndSaveChangesAsync();

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            int jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                "DatasetGeneration",
                "DatasetGenerationJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetGenerationTimeout);

            // Returns the dataset id
            return new ExecuteCustomSearchResponse
            {
                DatasetId = payload.DatasetId,
                JobId = jobId
            };
        }

        /// <summary>
        /// Duplicates a dataset and all its child objects (including charts and DB tables).
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="newUserProject">
        /// The new user project to create. The creation of the project and its relationships should be created outside this method.
        /// </param>
        /// <param name="duplicateDatasetData">The dataset data.</param>
        /// <param name="duplicatePostProcess">Value indicating whether the post processes should be duplicated.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The dataset id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't read from dataset or the folder already has a dataset with the same name.</exception>
        /// <exception cref="NotSupportedException">The dataset should belongs to a folder or user project.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<Guid> DuplicateDatasetAsync(
            Dataset dataset,
            UserProject newUserProject,
            DuplicateDatasetData duplicateDatasetData,
            bool duplicatePostProcess,
            CustomSearchesDbContext dbContext)
        {
            // The user project that is owner of the new dataset. Null if the dataset belongs to a folder.
            UserProject ownerProject = newUserProject ?? dataset.UserProjectDataset.FirstOrDefault(pd => pd.OwnsDataset)?.UserProject;

            if (ownerProject != null)
            {
                if (duplicateDatasetData != null)
                {
                    InputValidationHelper.AssertNotEmpty(duplicateDatasetData.DatasetName, nameof(duplicateDatasetData.DatasetName));
                }

                // Datasets in user projects don't have a parent folder.
                dataset.ParentFolderId = null;
            }
            else
            {
                if (DatasetHelper.IsUserDataset(dataset) == false)
                {
                    throw new NotSupportedException(
                        $"The dataset should belongs to a folder or user project.",
                        null);
                }
            }

            DatasetHelper.AssertCanReadFromDataset(dataset, usePostProcess: true);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, ownerProject, "DuplicateDataset");

            dataset.DatasetName = await this.GetDatasetNameAsync(dataset, ownerProject, duplicateDatasetData, dbContext);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            dataset.DatasetId = Guid.NewGuid();
            dataset.UserId = userId;
            dataset.CreatedBy = userId;
            dataset.CreatedTimestamp = DateTime.UtcNow;
            dataset.LastModifiedTimestamp = DateTime.UtcNow;
            dataset.LastModifiedBy = userId;
            dataset.DataSetPostProcessState = DatasetPostProcessStateType.NotProcessed.ToString();
            dataset.DataSetState = DatasetStateType.NotProcessed.ToString();
            dataset.ExecuteStoreProcedureElapsedMs = 0;
            dataset.GenerateIndexesElapsedMs = 0;
            dataset.GenerateSchemaElapsedMs = 0;
            dataset.IsLocked = false;
            dataset.LockingJobId = null;
            dataset.TotalRows = 0;
            dataset.GeneratedTableName = CustomSearchesDataDbContext.GetDatasetTableName(dataset);
            dataset.Comments = duplicateDatasetData?.DatasetComments;

            // Clean entities identity before add.
            dataset.CustomSearchExpression.ToList().ForEach(customSearchExpression => customSearchExpression.CustomSearchExpressionId = 0);

            if (duplicatePostProcess)
            {
                dataset.DatasetPostProcess.ToList().ForEach(datasetPostProcess =>
                {
                    datasetPostProcess.IsDirty = true;
                    datasetPostProcess.DatasetPostProcessId = 0;
                    datasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                    datasetPostProcess.LastModifiedBy = userId;
                    datasetPostProcess.CustomSearchExpression.ToList().ForEach(customSearchExpression => customSearchExpression.CustomSearchExpressionId = 0);
                    datasetPostProcess.ExceptionPostProcessRule.ToList().ForEach(exceptionPostProcessRule =>
                    {
                        exceptionPostProcessRule.ExceptionPostProcessRuleId = 0;
                        exceptionPostProcessRule.CustomSearchExpression.ToList().ForEach(customSearchExpression => customSearchExpression.CustomSearchExpressionId = 0);
                    });
                });
            }
            else
            {
                dataset.DatasetPostProcess.Clear();
            }

            dataset.InteractiveChart.ToList().ForEach(interactiveChart =>
            {
                interactiveChart.InteractiveChartId = 0;
                interactiveChart.LastModifiedTimestamp = DateTime.UtcNow;
                interactiveChart.LastModifiedBy = userId;
                interactiveChart.CustomSearchExpression.ToList().ForEach(customSearchExpression => customSearchExpression.CustomSearchExpressionId = 0);
            });

            return dataset.DatasetId;
        }

        /// <summary>
        /// Gets the name of the new dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="userProject">The user project.</param>
        /// <param name="duplicateDatasetData">The dataset data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The name of the new dataset.
        /// </returns>
        private async Task<string> GetDatasetNameAsync(Dataset dataset, UserProject userProject, DuplicateDatasetData duplicateDatasetData, CustomSearchesDbContext dbContext)
        {
            string datasetName = null;
            if (userProject != null)
            {
                datasetName = string.IsNullOrWhiteSpace(duplicateDatasetData?.DatasetName) ? dataset.DatasetName : duplicateDatasetData.DatasetName;

                await DatasetHelper.AssertUniqueUserProjectDatasetName(userProject.UserProjectId, datasetName, dbContext);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(duplicateDatasetData?.DatasetName))
                {
                    datasetName = await this.GetDeafaultUserDatasetNameAsync(dataset, dbContext);
                }
                else
                {
                    bool repeatedName =
                        (await dbContext.Dataset.FirstOrDefaultAsync(d =>
                            (d.ParentFolderId == dataset.ParentFolderId) &&
                            (d.DatasetName.Trim().ToLower() == duplicateDatasetData.DatasetName.Trim().ToLower()))) != null;

                    if (repeatedName)
                    {
                        throw new CustomSearchesConflictException(
                            $"The folder already has a dataset with the same name '{datasetName}'.",
                            null);
                    }

                    datasetName = duplicateDatasetData.DatasetName;
                }
            }

            return datasetName;
        }

        /// <summary>
        /// Gets the default name for the user dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The name of the dataset.
        /// </returns>
        private async Task<string> GetDeafaultUserDatasetNameAsync(Dataset dataset, CustomSearchesDbContext dbContext)
        {
            string datasetName = dataset.DatasetName + " - ";
            int newDatasetNameNumber = 1;
            int datasetNameLenght = datasetName.Length;
            List<Dataset> datasetCopies = await dbContext.Dataset
                .Where(d => (d.ParentFolderId == dataset.ParentFolderId) && d.DatasetName.ToLower().StartsWith(datasetName.ToLower()))
                .OrderByDescending(d => d.DatasetName.Length)
                .OrderByDescending(d => d.DatasetName).ToListAsync();

            if (datasetCopies.Count > 0)
            {
                foreach (var datasetCopy in datasetCopies)
                {
                    string datasetNameNumberText = datasetCopy.DatasetName.Remove(0, datasetName.Length);
                    int datasetNameNumber;
                    if (int.TryParse(datasetNameNumberText, out datasetNameNumber))
                    {
                        // To avoid string values like 001
                        if (datasetNameNumberText.Length == datasetNameNumber.ToString().Length)
                        {
                            if (datasetNameNumber >= 0)
                            {
                                newDatasetNameNumber = datasetNameNumber + 1;
                                break;
                            }
                        }
                    }
                }
            }

            datasetName += newDatasetNameNumber.ToString();
            return datasetName;
        }
    }
}
