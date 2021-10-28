namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the project type data.
    /// </summary>
    public class ProjectTypeData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTypeData"/> class.
        /// </summary>
        public ProjectTypeData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTypeData"/> class.
        /// </summary>
        /// <param name="projectType">The project type.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public ProjectTypeData(ProjectType projectType, ModelInitializationType initializationType)
        {
            this.ProjectTypeId = projectType.ProjectTypeId;
            this.ProjectTypeName = projectType.ProjectTypeName;
            this.BulkUpdateProcedureName = projectType.BulkUpdateProcedureName;
            this.ApplyModelUserFilterColumnName = projectType.ApplyModelUserFilterColumnName;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (projectType.ProjectTypeCustomSearchDefinition != null && projectType.ProjectTypeCustomSearchDefinition.Count > 0)
                {
                    var projectTypeCustomSearchDefinitionData = new List<ProjectTypeCustomSearchDefinitionData>();

                    foreach (var projectTypeCustomSearchDefinition in projectType.ProjectTypeCustomSearchDefinition)
                    {
                        projectTypeCustomSearchDefinitionData.Add(new ProjectTypeCustomSearchDefinitionData(projectTypeCustomSearchDefinition, initializationType));
                    }

                    this.ProjectTypeCustomSearchDefinitions = projectTypeCustomSearchDefinitionData.ToArray();
                }
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (projectType.CustomSearchExpression != null && projectType.CustomSearchExpression.Count > 0)
                {
                    var expressionsData = new List<CustomSearchExpressionData>();

                    foreach (var expression in projectType.CustomSearchExpression)
                    {
                        expressionsData.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.CustomSearchExpressions = expressionsData.ToArray();
                }
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
        /// Gets or sets the name of the project type.
        /// </summary>
        /// <value>
        /// The name of the project type.
        /// </value>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the bulk update stored procedure.
        /// </summary>
        /// <value>
        /// The name of the bulk update stored procedure.
        /// </value>
        public string BulkUpdateProcedureName { get; set; }

        /// <summary>
        /// Gets or sets the name of the apply model user filter column.
        /// </summary>
        /// <value>
        /// The name of the apply model user filter column.
        /// </value>
        public string ApplyModelUserFilterColumnName { get; set; }

        /// <summary>
        /// Gets or sets the project type custom search definitions.
        /// </summary>
        /// <value>
        /// The project type custom search definitions.
        public ProjectTypeCustomSearchDefinitionData[] ProjectTypeCustomSearchDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the custom search expressions.
        /// </summary>
        /// <value>
        /// The custom search expressions.
        /// </value>
        public CustomSearchExpressionData[] CustomSearchExpressions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public ProjectType ToEfModel()
        {
            var toReturn = new ProjectType()
            {
                ProjectTypeId = this.ProjectTypeId,
                ProjectTypeName = this.ProjectTypeName,
                BulkUpdateProcedureName = this.BulkUpdateProcedureName,
                ApplyModelUserFilterColumnName = this.ApplyModelUserFilterColumnName
            };

            if ((this.ProjectTypeCustomSearchDefinitions != null) && (this.ProjectTypeCustomSearchDefinitions.Length > 0))
            {
                foreach (var projectTypeDefinitionData in this.ProjectTypeCustomSearchDefinitions)
                {
                    ProjectTypeCustomSearchDefinition projectTypeDefinition = new ProjectTypeCustomSearchDefinition
                    {
                        CustomSearchDefinitionId = projectTypeDefinitionData.CustomSearchDefinitionId,
                        DatasetRole = projectTypeDefinitionData.DatasetRole,
                        ProjectType = toReturn,
                        ProjectTypeId = this.ProjectTypeId
                    };

                    toReturn.ProjectTypeCustomSearchDefinition.Add(projectTypeDefinition);
                }
            }

            if ((this.CustomSearchExpressions != null) && (this.CustomSearchExpressions.Length > 0))
            {
                foreach (var item in this.CustomSearchExpressions)
                {
                    CustomSearchExpression customSearchExpression = item.ToEfModel();
                    customSearchExpression.ProjectTypeId = this.ProjectTypeId;
                    customSearchExpression.ProjectType = toReturn;
                    toReturn.CustomSearchExpression.Add(customSearchExpression);
                }
            }

            return toReturn;
        }
    }
}
