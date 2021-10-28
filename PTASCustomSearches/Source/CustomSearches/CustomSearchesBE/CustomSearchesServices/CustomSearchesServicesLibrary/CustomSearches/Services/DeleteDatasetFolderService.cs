namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes the folder.
    /// </summary>
    public class DeleteDatasetFolderService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDatasetFolderService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteDatasetFolderService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folderData">Folder data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <returns>
        /// The delete dataset folder response.
        /// </returns>
        /// <exception cref="FolderManagerException">Invalid folder path format or folder not found.</exception>
        public async Task<DeleteDatasetFolderResponse> DeleteDatasetFolderAsync(CreateFolderData folderData, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            DatasetFolderManager.ValidateFolderPath(folderData.FolderPath);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(folderData.FolderPath);
            DatasetFolderManager datasetFolderManager = new DatasetFolderManager(folderType, userId, dbContext);
            Folder folder = await datasetFolderManager.ValidateEditableFolderExistenceAsync(folderData.FolderPath);

            // This will retrieve all datasets for the root folder. Might be a good target for optimization.
            var datasets = await (from d in dbContext.Dataset
                                  join f in dbContext.Folder
                                  on d.ParentFolderId equals f.FolderId
                                  where f.FolderType.ToLower() == folderType.ToString().ToLower() &&
                                        ((f.FolderType.ToLower() == CustomSearchFolderType.User.ToString().ToLower() && (f.UserId == userId)) ||
                                            (f.FolderType.ToLower() == CustomSearchFolderType.Shared.ToString().ToLower()))
                                  select d).
                                  Include(d => d.ParentFolder).
                                  Include(d => d.InverseSourceDataset).
                                  ToArrayAsync();

            List<DeleteEntityErrorData> deleteEntityErrors = new List<DeleteEntityErrorData>();

            if ((datasets != null) && (datasets.Length > 0))
            {
                DeleteDatasetService service = new DeleteDatasetService(this.ServiceContext);

                foreach (var dataset in datasets)
                {
                    string folderPath = await datasetFolderManager.GetFolderPathAsync(dataset);

                    if (folderPath.ToLower().StartsWith(folderData.FolderPath.ToLower()))
                    {
                        try
                        {
                            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, "DeleteDatasetFolder");

                            if ((dataset.InverseSourceDataset != null) && (dataset.InverseSourceDataset.Count > 0))
                            {
                                DeleteEntityErrorData entityError = new DeleteEntityErrorData
                                {
                                    Id = dataset.DatasetId.ToString(),
                                    Path = $"{folderPath}/{dataset.DatasetName}",
                                    Message = $"Dataset with id '{dataset.DatasetId}' is being used by a project."
                                };

                                deleteEntityErrors.Add(entityError);

                                continue;
                            }

                            Dataset datasetToDelete = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                                dbContext,
                                dataset.DatasetId,
                                includeRelatedExpressions: true,
                                includeParentFolder: true,
                                includeInverseSourceDatasets: false,
                                includeUserProject: false,
                                includeDatasetUserClientState: true);

                            await service.DeleteDataset(datasetToDelete, deleteDatasetRow: true, dbContext, blobContainer);
                        }
                        catch (Exception ex)
                        {
                            if ((ex is CustomSearchesConflictException) || (ex is AuthorizationException))
                            {
                                DeleteEntityErrorData entityError = new DeleteEntityErrorData
                                {
                                    Id = dataset.DatasetId.ToString(),
                                    Path = $"{folderPath}/{dataset.DatasetName}",
                                    Message = ex.GetBaseException().Message
                                };

                                deleteEntityErrors.Add(entityError);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            await datasetFolderManager.RemoveEmptyFoldersAsync(folder);
            await dbContext.SaveChangesAsync();

            DeleteDatasetFolderResponse response = new DeleteDatasetFolderResponse();
            if (deleteEntityErrors.Count > 0)
            {
                response.DeleteEntityErrors = deleteEntityErrors.ToArray();
                response.Message = "The folder was not deleted because there were datasets that could not be deleted.";
            }

            return response;
        }
    }
}
