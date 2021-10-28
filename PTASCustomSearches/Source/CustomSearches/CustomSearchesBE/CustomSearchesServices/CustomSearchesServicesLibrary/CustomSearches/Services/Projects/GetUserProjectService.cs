namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets a user project.
    /// </summary>
    public class GetUserProjectService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserProjectService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserProjectService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user project.
        /// </summary>
        /// <param name="userProjectId">project id.</param>
        /// <param name="includeDependencies">if set to <c>true</c> [include dependencies].</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The user project.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        public async Task<GetUserProjectResponse> GetUserProjectAsync(
            int userProjectId,
            bool includeDependencies,
            CustomSearchesDbContext dbContext)
        {
            GetUserProjectResponse response = new GetUserProjectResponse();

            UserProject project = null;

            if (includeDependencies)
            {
                project = await ProjectHelper.GetProjectWithDependencies(userProjectId, dbContext);
                InputValidationHelper.AssertEntityExists(project, nameof(UserProject), userProjectId);
            }
            else
            {
                var projectQuery = (from p in dbContext.UserProject where p.UserProjectId == userProjectId select p).
                    Include(p => p.ProjectType).
                        ThenInclude(pt => pt.ProjectTypeCustomSearchDefinition).
                    Include(p => p.CreatedByNavigation).
                    Include(p => p.LastModifiedByNavigation).
                    Include(p => p.ProjectType).
                    Include(p => p.UserProjectDataset);

                project = await projectQuery.FirstOrDefaultAsync();
                InputValidationHelper.AssertEntityExists(project, nameof(UserProject), userProjectId);
            }

            ModelInitializationType initializationType = includeDependencies ?
                ModelInitializationType.FullObjectWithDepedendencies :
                ModelInitializationType.FullObject;

            Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
            response.Project = new ProjectData(project, initializationType, userDetails);
            response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);

            if (includeDependencies)
            {
                foreach (var projectDataset in response.Project.ProjectDatasets)
                {
                    projectDataset.Dataset.IsOutdated =
                        await CustomSearchDefinitionHelper.GetIsCustomSearchDefinitionOutdated(projectDataset.Dataset.CustomSearchDefinitionId, dbContext);
                }
            }

            return response;
        }
    }
}
