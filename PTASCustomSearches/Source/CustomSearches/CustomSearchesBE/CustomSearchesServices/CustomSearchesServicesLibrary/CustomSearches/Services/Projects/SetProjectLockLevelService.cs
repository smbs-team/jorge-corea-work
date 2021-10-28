namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that sets the project lock level.
    /// </summary>
    public class SetProjectLockLevelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetProjectLockLevelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public SetProjectLockLevelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Sets the project lock level.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="isLocked">A value indicating if the project should be locked.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task SetProjectLockLevelAsync(int projectId, bool isLocked, CustomSearchesDbContext dbContext, ILogger logger)
        {
            UserProject project = await dbContext.UserProject.FirstOrDefaultAsync(p => p.UserProjectId == projectId);
            InputValidationHelper.AssertEntityExists(project, "UserProject", projectId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            project.IsLocked = isLocked;
            project.LastModifiedTimestamp = DateTime.UtcNow;
            project.LastModifiedBy = userId;
            dbContext.UserProject.Update(project);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
