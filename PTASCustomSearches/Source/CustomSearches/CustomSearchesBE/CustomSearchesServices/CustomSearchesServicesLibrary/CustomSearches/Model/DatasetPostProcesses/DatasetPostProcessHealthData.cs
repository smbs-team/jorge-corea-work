namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Model;

    /// <summary>
    /// Model for the dataset health data.
    /// </summary>
    public class DatasetPostProcessHealthData
    {
        /// <summary>
        /// The user messages for post-process issues.
        /// </summary>
        private static string[] healthMessages =
            {
                "The post process {postProcessRole} ({postProcessId}) for the dataset {datasetName} ({datasetId}) is dirty.  Please try to re-execute the post-process.  If the problem persists contact your administrator.",
                "The post process {postProcessRole} ({postProcessId}) for the dataset {datasetName} ({datasetId}) failed to execute.  Please try to check all the expressions in the post-process and re-execute it.  If the problem persists contact your administrator.",
                "The post process {postProcessRole} ({postProcessId}) for the dataset {datasetName} ({datasetId}) has not executed.  Please try to re-execute the post-process.  If the problem persists contact your administrator.",
                "The post process {postProcessRole} ({postProcessId}) for the dataset {datasetName} ({datasetId}) failed to calculate its internal view.  Please try to re-execute the post-process.  If the problem persists contact your administrator.",
                "No user message."
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetPostProcessHealthData" /> class.
        /// </summary>
        /// <param name="postProcess">The post process.</param>
        /// <param name="isApplyModelDatset">if set to <c>true</c> indicates that the owner dataset has the ApplyModel role.</param>
        public DatasetPostProcessHealthData(DatasetPostProcess postProcess, bool isApplyModelDatset)
        {
            this.DatasetPostProcessId = postProcess.DatasetPostProcessId;

            if (!string.IsNullOrWhiteSpace(postProcess.ResultPayload))
            {
                var jobResult = JsonHelper.DeserializeObject<SucceededJobResult>(postProcess.ResultPayload);
                if (jobResult == null || jobResult?.Status.ToLower() != "success")
                {
                    if (isApplyModelDatset && postProcess.ResultPayload.Contains("BulkUpdateValidationError"))
                    {
                        this.PostProcessHealthIssue = PostProcessHealthIssueType.BulkUpdateValidationError.ToString();
                        this.PostProcessHealthMessage = string.Empty;
                    }
                    else
                    {
                        this.PostProcessHealthIssue = PostProcessHealthIssueType.ExecutionFailed.ToString();
                        this.PostProcessHealthMessage = this.GetUserMessage(PostProcessHealthIssueType.ExecutionFailed, postProcess);
                    }

                    return;
                }
            }

            if (postProcess.IsDirty)
            {
                this.PostProcessHealthIssue = PostProcessHealthIssueType.Dirty.ToString();
                this.PostProcessHealthMessage = this.GetUserMessage(PostProcessHealthIssueType.Dirty, postProcess);
                return;
            }

            if (postProcess.LastExecutionTimestamp == null)
            {
                this.PostProcessHealthIssue = PostProcessHealthIssueType.NotExecuted.ToString();
                this.PostProcessHealthMessage = this.GetUserMessage(PostProcessHealthIssueType.NotExecuted, postProcess);
                return;
            }

            if (string.IsNullOrWhiteSpace(postProcess.CalculatedView))
            {
                this.PostProcessHealthIssue = PostProcessHealthIssueType.MissingCalculatedView.ToString();
                this.PostProcessHealthMessage = this.GetUserMessage(PostProcessHealthIssueType.MissingCalculatedView, postProcess);
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the dataset post process.
        /// </summary>
        public int DatasetPostProcessId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a health issue with this post-process.
        /// </summary>
        public string PostProcessHealthIssue { get; set; }

        /// <summary>
        /// Gets or sets a value with a health message.
        /// </summary>
        public string PostProcessHealthMessage { get; set; }

        /// <summary>
        /// Gets the user message.
        /// </summary>
        /// <param name="issueType">Type of the issue.</param>
        /// <param name="postProcess">The dataset.</param>
        /// <returns>The user message.</returns>
        private string GetUserMessage(PostProcessHealthIssueType issueType, DatasetPostProcess postProcess)
        {
            string message = DatasetPostProcessHealthData.healthMessages[(int)issueType];
            message = message.Replace("{datasetId}", postProcess.DatasetId.ToString());
            message = message.Replace("{datasetName}", postProcess.Dataset.DatasetName);
            message = message.Replace("{postProcessRole}", postProcess.PostProcessRole);
            message = message.Replace("{postProcessId}", postProcess.DatasetPostProcessId.ToString());
            return message;
        }
    }
}
