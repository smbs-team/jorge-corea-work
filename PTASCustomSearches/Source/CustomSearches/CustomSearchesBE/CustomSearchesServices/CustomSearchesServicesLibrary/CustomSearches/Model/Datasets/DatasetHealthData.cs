namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Projects;

    /// <summary>
    /// Model for the dataset health data.
    /// </summary>
    public class DatasetHealthData
    {
        /// <summary>
        /// The user messages for dataset issues.
        /// </summary>
        private static string[] healthMessages =
            {
                "The dataset {datasetName} ({datasetId}) failed to generate.  Please perform a 'Refresh' operation or contact your administrator.",
                "The dataset {datasetName} ({datasetId}) needs a post-process update.  Please re-execute the dataset post-processes or contact your administrator."
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetHealthData" /> class.
        /// </summary>
        /// <param name="userProjectDataset">The user project dataset.</param>
        public DatasetHealthData(UserProjectDataset userProjectDataset)
        {
            Dataset dataset = userProjectDataset.Dataset;
            this.PostProcessHealthData = new List<DatasetPostProcessHealthData>();
            this.DatasetId = dataset.DatasetId;
            this.DatasetUserId = dataset.UserId;

            if (dataset.DataSetState.ToLower() == DatasetStateType.NotProcessed.ToString().ToLower() ||
                dataset.DataSetState.ToLower() == DatasetStateType.GeneratingDataset.ToString().ToLower() ||
                dataset.DataSetState.ToLower() == DatasetStateType.ExecutingPostProcess.ToString().ToLower() ||
                dataset.DataSetState.ToLower() == DatasetStateType.GeneratingIndexes.ToString().ToLower() ||
                dataset.DataSetPostProcessState.ToLower() == DatasetPostProcessStateType.Processing.ToString().ToLower())
            {
                this.IsProcessing = true;
                return;
            }

            if (dataset.DataSetState.ToLower() == DatasetStateType.Failed.ToString().ToLower())
            {
                this.DatasetHealthIssue = DatasetHealthIssueType.FailedGeneration.ToString();
                this.HealthMessage = this.GetUserMessage(DatasetHealthIssueType.FailedGeneration, dataset);
                return;
            }

            bool isApplyModelDataset = userProjectDataset.DatasetRole == ApplyModelService.ApplyModelDatasetRole;

            foreach (var postProcess in dataset.DatasetPostProcess)
            {
                // TODO: Due to a bug when updating secondary dataset post-process states, right now they will not be taken into account for post-process health.
                if (postProcess.PrimaryDatasetPostProcessId == null)
                {
                    var postprocessHealth = new DatasetPostProcessHealthData(postProcess, isApplyModelDataset);
                    if (!string.IsNullOrWhiteSpace(postprocessHealth.PostProcessHealthIssue))
                    {
                        // Some issues for Apply Model dataset post-processes are ignored because bulk update post-process is created, but
                        // not fully executed until the appraiser finishes the value selection process for all parcels.
                        if (!isApplyModelDataset || (isApplyModelDataset &&
                            !this.IsIgnoredApplyModelIssue(postprocessHealth)))
                        {
                            this.PostProcessHealthData.Add(postprocessHealth);
                        }
                    }
                }
            }

            if (dataset.DataSetPostProcessState.ToLower() == DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString().ToLower())
            {
                // Apply Model Dataset with NeedsPostProcessUpdate is considered normal, because bulk update post-process is created, but
                // not fully executed until the appraiser finishes the value selection process for all parcels.
                if (!isApplyModelDataset || this.PostProcessHealthData.Count > 0)
                {
                    this.DatasetHealthIssue = DatasetHealthIssueType.NeedsPostProcessUpdate.ToString();
                    this.HealthMessage = this.GetUserMessage(DatasetHealthIssueType.NeedsPostProcessUpdate, dataset);
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the dataset's user.
        /// </summary>
        public Guid DatasetUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is some processing ongoing for the dataset.
        /// </summary>
        public bool IsProcessing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a health issue with this dataset.
        /// </summary>
        public string DatasetHealthIssue { get; set; }

        /// <summary>
        /// Gets or sets a value with a health message.
        /// </summary>
        public string HealthMessage { get; set; }

        /// <summary>
        /// Gets or sets a list of health issues associated with the dataset post-processes.
        /// </summary>
        public List<DatasetPostProcessHealthData> PostProcessHealthData { get; set; }

        /// <summary>
        /// Gets the user message.
        /// </summary>
        /// <param name="issueType">Type of the issue.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The user message.</returns>
        private string GetUserMessage(DatasetHealthIssueType issueType, Dataset dataset)
        {
            string message = DatasetHealthData.healthMessages[(int)issueType];
            message = message.Replace("{datasetId}", dataset.DatasetId.ToString());
            message = message.Replace("{datasetName}", dataset.DatasetName);
            return message;
        }

        /// <summary>
        /// Determines whether the health data should be ignored for apply model datasets.
        /// </summary>
        /// <param name="postProcessHealth">The post process health.</param>
        /// <returns>
        ///   <c>true</c> if the health issue should be ignored for apply model dataset.</c>.
        /// </returns>
        private bool IsIgnoredApplyModelIssue(DatasetPostProcessHealthData postProcessHealth)
        {
            return postProcessHealth.PostProcessHealthIssue == PostProcessHealthIssueType.Dirty.ToString() ||
                postProcessHealth.PostProcessHealthIssue == PostProcessHealthIssueType.NotExecuted.ToString() ||
                postProcessHealth.PostProcessHealthIssue == PostProcessHealthIssueType.BulkUpdateValidationError.ToString();
        }
    }
}
