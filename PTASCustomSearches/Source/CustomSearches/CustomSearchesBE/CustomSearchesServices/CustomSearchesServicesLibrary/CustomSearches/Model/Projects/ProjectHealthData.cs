namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Projects;

    /// <summary>
    /// Model for the project health data.
    /// </summary>
    public class ProjectHealthData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectHealthData" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="userId">The current user identifier.</param>
        public ProjectHealthData(UserProject project, Guid userId)
        {
            this.UserProjectId = project.UserProjectId;
            this.DatasetHealthData = new List<DatasetHealthData>();

            foreach (var userProjectDataset in project.UserProjectDataset)
            {
                var datasetHealth = new DatasetHealthData(userProjectDataset);
                if (this.ShouldDisplayHealthForUser(userProjectDataset, userId) &&
                    (datasetHealth.IsProcessing ||
                    !string.IsNullOrWhiteSpace(datasetHealth.DatasetHealthIssue) ||
                    datasetHealth.PostProcessHealthData.Count > 0))
                {
                    this.DatasetHealthData.Add(datasetHealth);
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the project.
        /// </summary>
        public int UserProjectId { get; set; }

        /// <summary>
        /// Gets or sets a list of health issues associated with a dataset.
        /// </summary>
        public List<DatasetHealthData> DatasetHealthData { get; set; }

        /// <summary>
        /// Gets a value indicating whether the health warning for a dataset should be displayed for a given user.
        /// </summary>
        /// <param name="userProjectDataset">The user project dataset.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A value indicating whether the health warning for a dataset should be displayed for a given user.</returns>
        private bool ShouldDisplayHealthForUser(UserProjectDataset userProjectDataset, Guid userId)
        {
            if (userProjectDataset.DatasetRole == ApplyModelService.ApplyModelDatasetRole)
            {
                return userProjectDataset.Dataset.UserId == userId;
            }

            return true;
        }
    }
}
