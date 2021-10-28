namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
    /// Service that refreshes the project datasets.
    /// </summary>
    public class RefreshProjectDatasetsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshProjectDatasetsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RefreshProjectDatasetsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Refreshes the project datasets.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="allVersions">Value indicating whether should be refreshed all project versions.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The user project response.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">UserProjectId should be the id of the root project when allVersions is true.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<ImportUserProjectResponse> RefreshProjectDatasetsAsync(int userProjectId, bool allVersions, CustomSearchesDbContext dbContext)
        {
            var userProjects = await dbContext.UserProject
                .Where(p => (p.UserProjectId == userProjectId) || (allVersions && p.RootVersionUserProjectId == userProjectId))
                .Include(p => p.UserProjectDataset)
                .ToArrayAsync();

            var userProject = userProjects.FirstOrDefault(p => (p.UserProjectId == userProjectId));
            InputValidationHelper.AssertEntityExists(userProject, nameof(UserProject), userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "RefreshProjectDatasets");

            if (allVersions && userProject.RootVersionUserProjectId != null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"UserProjectId '{userProjectId}' should be the id of the root project when allVersions is true.",
                    innerException: null);
            }

            RefreshDatasetService service = new RefreshDatasetService(this.ServiceContext);
            List<Guid> datasetIds = new List<Guid>();
            List<int> jobIds = new List<int>();

            foreach (var project in userProjects)
            {
                foreach (var userProjecDataset in project.UserProjectDataset)
                {
                    if (userProjecDataset.OwnsDataset)
                    {
                        var idResult = await service.RefreshDatasetAsync(userProjecDataset.DatasetId, dbContext);
                        datasetIds.Add(userProjecDataset.DatasetId);
                        jobIds.Add((int)idResult.Id);
                    }
                }
            }

            return new ImportUserProjectResponse(userProjectId, datasetIds.ToArray(), jobIds.ToArray());
        }
    }
}