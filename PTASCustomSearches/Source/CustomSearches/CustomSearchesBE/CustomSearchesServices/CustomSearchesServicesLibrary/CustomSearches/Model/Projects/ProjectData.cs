namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the project data.
    /// </summary>
    public class ProjectData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectData"/> class.
        /// </summary>
        public ProjectData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectData"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public ProjectData(UserProject project, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.UserProjectId = project.UserProjectId;
            this.RootVersionUserProjectId = project.RootVersionUserProjectId;
            this.VersionNumber = project.VersionNumber;
            this.CreatedTimestamp = project.CreatedTimestamp;
            this.LastModifiedTimestamp = project.LastModifiedTimestamp;
            this.AssessmentDateFrom = project.AssessmentDateFrom;
            this.AssessmentDateTo = project.AssessmentDateTo;
            this.AssessmentYear = project.AssessmentYear;
            this.Comments = project.Comments;
            this.CreatedBy = project.CreatedBy;
            this.LastModifiedBy = project.LastModifiedBy;
            this.ProjectName = project.ProjectName;
            this.ProjectTypeName = project.ProjectType.ProjectTypeName;
            this.SelectedAreas = ProjectData.ToSelectedValueArray(project.SelectedAreas);
            this.SplitModelValues = ProjectData.ToSelectedValueArray(project.SplitModelValue);
            this.ModelArea = project.ModelArea;
            this.SplitModelProperty = project.SplitModelProperty;
            this.UserId = project.UserId;
            this.IsLocked = project.IsLocked;
            this.IsFrozen = project.IsFrozen;
            this.VersionType = project.VersionType;

            ProjectTypeData projectTypeData = new ProjectTypeData(project.ProjectType, ModelInitializationType.FullObject);
            this.ProjectTypeCustomSearchDefinitions = projectTypeData.ProjectTypeCustomSearchDefinitions;

            UserDetailsHelper.GatherUserDetails(project.CreatedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(project.LastModifiedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(project.User, userDetails);

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                List<ProjectDatasetData> userProjectDatasetsData = new List<ProjectDatasetData>();
                if (project.UserProjectDataset != null && project.UserProjectDataset.Count > 0)
                {
                    foreach (var userProjectDataset in project.UserProjectDataset)
                    {
                        userProjectDatasetsData.Add(new ProjectDatasetData(userProjectDataset, initializationType, userDetails));
                    }

                    this.ProjectDatasets = userProjectDatasetsData.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the project.
        /// </summary>
        public int UserProjectId { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the project is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the project is frozen.
        /// </summary>
        public bool IsFrozen { get; set; }

        /// <summary>
        /// Gets or sets ther version type.
        /// </summary>
        public string VersionType { get; set; }

        /// <summary>
        /// Gets or sets the Assessment year.
        /// </summary>
        public int AssessmentYear { get; set; }

        /// <summary>
        /// Gets or sets the Assessment date from.
        /// </summary>
        public DateTime AssessmentDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the Assessment date to.
        /// </summary>
        public DateTime AssessmentDateTo { get; set; }

        /// <summary>
        /// Gets or sets the selected areas.
        /// </summary>
        public string[] SelectedAreas { get; set; }

        /// <summary>
        /// Gets or sets the selected areas.
        /// </summary>
        public int? RootVersionUserProjectId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the type of the project.
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// Gets or sets the created by field.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by field.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created time stamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified time stamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the split model values.
        /// </summary>
        public string[] SplitModelValues { get; set; }

        /// <summary>
        /// Gets or sets the split model property.
        /// </summary>
        public string SplitModelProperty { get; set; }

        /// <summary>
        /// Gets or sets the model area.
        /// </summary>
        public int ModelArea { get; set; }

        /// <summary>
        /// Gets or sets the project type custom search definitions.
        /// </summary>
        /// <value>
        /// The project type custom search definitions.
        public ProjectTypeCustomSearchDefinitionData[] ProjectTypeCustomSearchDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the project datasets.
        /// </summary>
        /// <value>
        /// The datasets.
        /// </value>
        public ProjectDatasetData[] ProjectDatasets { get; set; }

        /// <summary>
        /// Converts selected values string to a string array.
        /// </summary>
        /// <param name="selectedValues">The selected values.</param>
        /// <returns>A selected values array.</returns>
        public static string[] ToSelectedValueArray(string selectedValues)
        {
            return selectedValues == null ?
                null :
                selectedValues.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();
        }

        /// <summary>
        /// Converts selected area string array to a comma separated string.
        /// </summary>
        /// <param name="selectedValues">The selected values.</param>
        /// <returns>A comma separated string with the values.</returns>
        public static string ToSelectedValueString(string[] selectedValues)
        {
            string toReturn = string.Empty;
            if (selectedValues != null)
            {
                for (int i = 0; i < selectedValues.Length; i++)
                {
                    toReturn += selectedValues[i];

                    if (i < selectedValues.Length - 1)
                    {
                        toReturn += ", ";
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <param name="projectTypeIds">The project type ids.</param>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public UserProject ToEfModel(Dictionary<string, int> projectTypeIds)
        {
            var toReturn = new UserProject()
            {
                UserProjectId = this.UserProjectId,
                RootVersionUserProjectId = this.RootVersionUserProjectId,
                VersionNumber = this.VersionNumber,
                CreatedTimestamp = this.CreatedTimestamp,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
                AssessmentDateFrom = this.AssessmentDateFrom,
                AssessmentDateTo = this.AssessmentDateTo,
                AssessmentYear = this.AssessmentYear,
                Comments = this.Comments,
                CreatedBy = this.CreatedBy,
                LastModifiedBy = this.LastModifiedBy,
                ProjectName = this.ProjectName,
                ProjectTypeId = projectTypeIds[this.ProjectTypeName],
                SelectedAreas = ProjectData.ToSelectedValueString(this.SelectedAreas),
                SplitModelValue = ProjectData.ToSelectedValueString(this.SplitModelValues),
                SplitModelProperty = this.SplitModelProperty,
                ModelArea = this.ModelArea,
                UserId = this.UserId,
                IsLocked = this.IsLocked,
                IsFrozen = this.IsFrozen,
                VersionType = this.VersionType
            };

            if (this.ProjectDatasets != null)
            {
                toReturn.UserProjectDataset = new List<UserProjectDataset>();
                foreach (var userProjectDataset in this.ProjectDatasets)
                {
                    UserProjectDataset newUserProjectDataset = userProjectDataset.ToEfModel();
                    newUserProjectDataset.UserProjectId = toReturn.UserProjectId;
                    newUserProjectDataset.UserProject = toReturn;
                    toReturn.UserProjectDataset.Add(newUserProjectDataset);
                }
            }

            return toReturn;
        }
    }
}
