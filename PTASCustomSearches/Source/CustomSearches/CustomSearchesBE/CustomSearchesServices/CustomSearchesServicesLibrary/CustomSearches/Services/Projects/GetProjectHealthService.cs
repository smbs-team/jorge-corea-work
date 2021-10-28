namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that gets information about project health.
    /// </summary>
    public class GetProjectHealthService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectHealthService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetProjectHealthService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets information about project health.
        /// </summary>
        /// <param name="projectId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>Information about project health.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Project was not found.</exception>
        public async Task<ProjectHealthData> GetProjectHealthAsync(int projectId, CustomSearchesDbContext dbContext)
        {
            UserProject project = await ProjectHelper.GetProjectWithDependencies(projectId, dbContext);
            InputValidationHelper.AssertEntityExists(project, "Project", projectId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            ProjectHealthData response = new ProjectHealthData(project, userId);
            return response;
        }
    }
}
