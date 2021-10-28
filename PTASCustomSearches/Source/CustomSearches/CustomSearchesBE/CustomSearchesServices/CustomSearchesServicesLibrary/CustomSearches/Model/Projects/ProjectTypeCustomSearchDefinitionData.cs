namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the project type data.
    /// </summary>
    public class ProjectTypeCustomSearchDefinitionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTypeCustomSearchDefinitionData"/> class.
        /// </summary>
        public ProjectTypeCustomSearchDefinitionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTypeCustomSearchDefinitionData"/> class.
        /// </summary>
        /// <param name="projectTypeDefinition">The project type custom search definition.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public ProjectTypeCustomSearchDefinitionData(ProjectTypeCustomSearchDefinition projectTypeDefinition, ModelInitializationType initializationType)
        {
            this.ProjectTypeId = projectTypeDefinition.ProjectTypeId;
            this.DatasetRole = projectTypeDefinition.DatasetRole;
            this.CustomSearchDefinitionId = projectTypeDefinition.CustomSearchDefinitionId;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }
        }

        /// <summary>
        /// Gets or sets the project type identifier.
        /// </summary>
        /// <value>
        /// The project type identifier.
        /// </value>
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// Gets or sets the dataset role.
        /// </summary>
        /// <value>
        /// The dataset role.
        /// </value>
        public string DatasetRole { get; set; }

        /// <summary>
        /// Gets or sets the custom search definition identifier.
        /// </summary>
        /// <value>
        /// The custom search definition identifier.
        /// </value>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the custom search definition name.
        /// </summary>
        /// <value>
        /// The custom search definition name.
        /// </value>
        public string CustomSearchDefinitionName { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public ProjectTypeCustomSearchDefinition ToEfModel()
        {
            var toReturn = new ProjectTypeCustomSearchDefinition()
            {
                ProjectTypeId = this.ProjectTypeId,
                DatasetRole = this.DatasetRole,
                CustomSearchDefinitionId = this.CustomSearchDefinitionId
            };

            return toReturn;
        }
    }
}
