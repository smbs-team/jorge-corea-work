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
    /// Service that deletes a project version for a project and all child objects.
    /// </summary>
    public class DeleteProjectVersionService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectVersionService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteProjectVersionService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a project version for a project and all child objects.
        /// </summary>
        /// <param name="userProjectId">The project id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesConflictException">UserProject is the root or unique version of the project. It can't be deleted.</exception>
        public async Task DeleteProjectVersionAsync(int userProjectId, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            UserProject userProject = await dbContext.UserProject
                .Where(p => p.UserProjectId == userProjectId)
                .Include(p => p.UserProjectDataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, "UserProject", userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "DeleteProjectVersion");

            int? rootProjectId = userProject.RootVersionUserProjectId == null ? userProject.UserProjectId : userProject.RootVersionUserProjectId;

            List<UserProject> projectVersions = await dbContext.UserProject
                    .Where(p => p.RootVersionUserProjectId == rootProjectId)
                    .OrderBy(p => p.VersionNumber)
                    .Include(p => p.UserProjectDataset)
                    .ToListAsync();

            if (projectVersions.Count == 0)
            {
                throw new CustomSearchesConflictException($"UserProject with id '{userProjectId}' is the unique version of the project. It can't be deleted.", null);
            }

            // If root sets the next one as the new root
            if (userProject.RootVersionUserProjectId == null)
            {
                throw new CustomSearchesConflictException($"UserProject with id '{userProjectId}' is the root version of the project. It can't be deleted.", null);
            }

            List<Dataset> datasetsToDelete = new List<Dataset>();
            DeleteDatasetService service = new DeleteDatasetService(this.ServiceContext);
            foreach (var userProjecDataset in userProject.UserProjectDataset)
            {
                Dataset datasetToDelete = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                    dbContext,
                    userProjecDataset.DatasetId,
                    includeRelatedExpressions: true,
                    includeParentFolder: true,
                    includeInverseSourceDatasets: false,
                    includeUserProject: false,
                    includeDatasetUserClientState: true);

                await service.DeleteDataset(datasetToDelete, deleteDatasetRow: false, dbContext, blobContainer);
                datasetsToDelete.Add(datasetToDelete);
            }

            dbContext.UserProjectDataset.RemoveRange(userProject.UserProjectDataset);
            dbContext.Dataset.RemoveRange(datasetsToDelete);
            dbContext.UserProject.Remove(userProject);
            await dbContext.SaveChangesAsync();
        }
    }
}
