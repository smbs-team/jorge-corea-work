namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes a user project and all its child objects.
    /// </summary>
    public class DeleteUserProjectService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserProjectService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteUserProjectService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a user project and all its child objects.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteUserProjectAsync(int userProjectId, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            UserProject userProject = await dbContext.UserProject
                .Where(p => p.UserProjectId == userProjectId)
                .Include(p => p.UserProjectDataset)
                .ThenInclude(pd => pd.Dataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, "UserProject", userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "DeleteUserProject");

            UserProject rootProject;
            if (userProject.RootVersionUserProjectId == null)
            {
                rootProject = userProject;
            }
            else
            {
                rootProject = await dbContext.UserProject
                                    .Where(p => p.UserProjectId == userProject.RootVersionUserProjectId)
                                    .Include(p => p.UserProjectDataset)
                                    .ThenInclude(pd => pd.Dataset)
                                    .FirstOrDefaultAsync();
            }

            List<UserProject> projectVersions = await dbContext.UserProject
                    .Where(p => p.RootVersionUserProjectId == rootProject.UserProjectId)
                    .OrderByDescending(p => p.VersionNumber)
                    .Include(p => p.UserProjectDataset)
                    .ToListAsync();

            DeleteDatasetService service = new DeleteDatasetService(this.ServiceContext);

            List<Dataset> datasetsToDelete = new List<Dataset>();

            var rootUserProjectDatasets = rootProject.UserProjectDataset.ToList();
            foreach (var projectDataset in rootUserProjectDatasets)
            {
                Dataset datasetToDelete = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                        dbContext,
                        projectDataset.Dataset.DatasetId,
                        includeRelatedExpressions: true,
                        includeParentFolder: true,
                        includeInverseSourceDatasets: false,
                        includeUserProject: false,
                        includeDatasetUserClientState: true);

                await service.DeleteDataset(datasetToDelete, deleteDatasetRow: false, dbContext, blobContainer);
                datasetsToDelete.Add(datasetToDelete);
            }

            foreach (var project in projectVersions)
            {
                var userProjectDatasets = project.UserProjectDataset.ToList();
                foreach (var userProjectDataset in userProjectDatasets)
                {
                    Dataset datasetToDelete = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                        dbContext,
                        userProjectDataset.DatasetId,
                        includeRelatedExpressions: true,
                        includeParentFolder: true,
                        includeInverseSourceDatasets: false,
                        includeUserProject: false,
                        includeDatasetUserClientState: true);

                    await service.DeleteDataset(datasetToDelete, deleteDatasetRow: false, dbContext, blobContainer);
                    datasetsToDelete.Add(datasetToDelete);
                }

                dbContext.UserProjectDataset.RemoveRange(project.UserProjectDataset);
            }

            dbContext.UserProject.RemoveRange(projectVersions);
            dbContext.UserProjectDataset.RemoveRange(rootProject.UserProjectDataset);
            dbContext.Dataset.RemoveRange(datasetsToDelete);
            dbContext.UserProject.Remove(rootProject);
            await dbContext.SaveChangesAsync();
        }
    }
}
