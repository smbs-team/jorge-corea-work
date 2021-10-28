namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports a user project.
    /// </summary>
    public class ImportProjectTypeService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportProjectTypeService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportProjectTypeService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the user project.
        /// </summary>
        /// <param name="projectTypeData">The user project to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The id of the imported project.</returns>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<IdResult> ImportProjectTypeAsync(
            ProjectTypeData projectTypeData,
            CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportProjectType");

            if ((projectTypeData.ProjectTypeCustomSearchDefinitions == null) || (projectTypeData.ProjectTypeCustomSearchDefinitions.Length == 0))
            {
                throw new CustomSearchesRequestBodyException("Project type must have at least a CustomSearchDefinition.", null);
            }

            // Resolve custom search definitions ids based on names
            foreach (var customSearchRelation in projectTypeData.ProjectTypeCustomSearchDefinitions)
            {
                CustomSearchDefinition customSearchDefinition = null;
                int id = customSearchRelation.CustomSearchDefinitionId;
                string name = customSearchRelation.CustomSearchDefinitionName;

                if (id != 0 && !string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(customSearchRelation.CustomSearchDefinitionName),
                        $"CustomSearchDefinitionName shouldn't be present when CustomSearchDefinitionId is present.");
                }

                if (id != 0)
                {
                    customSearchDefinition = await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(id, dbContext);
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    customSearchDefinition = await CustomSearchDefinitionHelper.GetCustomSearchDefinitionLatestVersion(name, dbContext);

                    if (customSearchDefinition != null)
                    {
                        customSearchRelation.CustomSearchDefinitionId = customSearchDefinition.CustomSearchDefinitionId;
                    }
                }

                InputValidationHelper.AssertEntityExists(
                    customSearchDefinition,
                    nameof(customSearchDefinition),
                    id != 0 ? id.ToString() : name);

                if (customSearchDefinition.Validated != true)
                {
                    throw new CustomSearchesEntityNotFoundException(
                        $"{nameof(CustomSearchDefinition)} '{id}' should be validated.",
                        innerException: null);
                }
            }

            InputValidationHelper.AssertZero(projectTypeData.ProjectTypeId, nameof(ProjectType), nameof(projectTypeData.ProjectTypeId));
            InputValidationHelper.AssertNotEmpty(projectTypeData.ProjectTypeName, nameof(projectTypeData.ProjectTypeName));

            ProjectType existingProjectType = await dbContext.ProjectType
                .Where(p => p.ProjectTypeName.Trim().ToLower() == projectTypeData.ProjectTypeName.Trim().ToLower())
                .Include(p => p.ProjectTypeCustomSearchDefinition)
                .Include(p => p.CustomSearchExpression)
                .FirstOrDefaultAsync();

            projectTypeData.ProjectTypeId = existingProjectType != null ? existingProjectType.ProjectTypeId : 0;
            ProjectType newProjectType = projectTypeData.ToEfModel();

            var projectTypeToSave = existingProjectType;

            if (projectTypeToSave != null)
            {
                existingProjectType.ProjectTypeName = projectTypeData.ProjectTypeName;

                dbContext.CustomSearchExpression.RemoveRange(existingProjectType.CustomSearchExpression);
                existingProjectType.CustomSearchExpression.Clear();

                // Add new expressions
                if (projectTypeData.CustomSearchExpressions != null)
                {
                    foreach (var expressionData in projectTypeData.CustomSearchExpressions)
                    {
                        var newExpression = expressionData.ToEfModel();
                        existingProjectType.CustomSearchExpression.Add(newExpression);
                    }
                }

                dbContext.ProjectTypeCustomSearchDefinition.RemoveRange(existingProjectType.ProjectTypeCustomSearchDefinition);
                existingProjectType.ProjectTypeCustomSearchDefinition.Clear();

                // Add new ProjectTypeCustomSearchDefinitions
                if (projectTypeData.ProjectTypeCustomSearchDefinitions != null)
                {
                    foreach (var customSearchRelationData in projectTypeData.ProjectTypeCustomSearchDefinitions)
                    {
                        var newCustomSearchRelation = customSearchRelationData.ToEfModel();
                        existingProjectType.ProjectTypeCustomSearchDefinition.Add(newCustomSearchRelation);
                    }
                }

                dbContext.ProjectType.Update(existingProjectType);
            }
            else
            {
                projectTypeToSave = newProjectType;
                dbContext.ProjectType.Add(newProjectType);
            }

            InputValidationHelper.AssertEmpty(projectTypeToSave.CustomSearchExpression.ToArray(), nameof(ProjectTypeData.CustomSearchExpressions));

            await dbContext.ValidateAndSaveChangesAsync();
            return new IdResult(projectTypeToSave.ProjectTypeId);
        }
    }
}
