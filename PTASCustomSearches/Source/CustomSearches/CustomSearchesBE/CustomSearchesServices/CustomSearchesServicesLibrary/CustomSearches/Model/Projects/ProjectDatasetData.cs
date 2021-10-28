namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;

    /// <summary>
    /// Model for the project dataset data.
    /// </summary>
    public class ProjectDatasetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDatasetData"/> class.
        /// </summary>
        public ProjectDatasetData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDatasetData"/> class.
        /// </summary>
        /// <param name="projectDataset">The project dataset.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public ProjectDatasetData(UserProjectDataset projectDataset, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.UserProjectId = projectDataset.UserProjectId;
            this.DatasetId = projectDataset.DatasetId;
            this.OwnsDataset = projectDataset.OwnsDataset;
            this.DatasetRole = projectDataset.DatasetRole;

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (projectDataset.Dataset != null)
                {
                    this.Dataset = new DatasetData(projectDataset.Dataset, initializationType, userDetails);
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the project.
        /// </summary>
        public int UserProjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the project dataset.
        /// </summary>
        public DatasetData Dataset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this project owns this dataset.
        /// </summary>
        public bool OwnsDataset { get; set; }

        /// <summary>
        /// Gets or sets the role of the dataset in the project.
        /// </summary>
        public string DatasetRole { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public UserProjectDataset ToEfModel()
        {
            var toReturn = new UserProjectDataset()
            {
                UserProjectId = this.UserProjectId,
                OwnsDataset = this.OwnsDataset,
                DatasetId = this.DatasetId,
                DatasetRole = this.DatasetRole
            };

            return toReturn;
        }
    }
}
