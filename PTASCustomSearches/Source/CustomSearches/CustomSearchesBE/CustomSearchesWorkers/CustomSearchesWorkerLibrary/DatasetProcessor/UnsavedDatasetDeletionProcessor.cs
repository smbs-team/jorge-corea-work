namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using Microsoft.EntityFrameworkCore;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using System.Linq;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage.Blob;
    using CustomSearchesWorkerLibrary.DatasetProcessor.Model;

    /// <summary>
    /// Worker job processor that deletes old unsaved datasets.
    /// </summary>
    public class UnsavedDatasetDeletionProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// RScript blob container name.
        /// </summary>
        private const string RScriptBlobContainerName = "rscript";

        /// <summary>
        /// User folder type.
        /// </summary>
        private const string UserFolderType = "user";

        /// <summary>
        /// Unsaved folder name.
        /// </summary>
        private const string UnsavedFolderName = "unsaved";

        /// <summary>
        /// Expiration time in hours.
        /// </summary>
        private const int ExpirationTimeInHours = -48;

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType 
        { 
            get
            {
                return nameof(CustomSearchesJobTypes.UnsavedDatasetDeletionJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsavedDatasetDeletionProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the logger parameter is null.</exception>
        public UnsavedDatasetDeletionProcessor(ILogger logger) : base(logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob"></param>
        /// <returns>True if the job completed successfully</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            List<DeleteEntityErrorData> deleteEntityErrors = new List<DeleteEntityErrorData>();

            using (CustomSearchesDbContext dbContext = this.ServiceContext.DbContextFactory.Create())
            {
                var expirationTime = DateTime.UtcNow.AddHours(UnsavedDatasetDeletionProcessor.ExpirationTimeInHours);

                var datasets = await (from pf in dbContext.Folder
                                      join uf in dbContext.Folder
                                      on pf.FolderId equals uf.ParentFolderId
                                      join d in dbContext.Dataset
                                      on uf.FolderId equals d.ParentFolderId
                                      where pf.ParentFolderId == null &&
                                              pf.FolderType.ToLower() == UnsavedDatasetDeletionProcessor.UserFolderType &&
                                              uf.FolderName.ToLower() == UnsavedDatasetDeletionProcessor.UnsavedFolderName &&
                                              d.LastModifiedTimestamp < expirationTime
                                      select d).
                                      Include(d => d.ParentFolder).
                                      Include(d => d.InverseSourceDataset).
                                      ToArrayAsync();

                if (datasets.Length > 0)
                {
                    DeleteDatasetService service = new DeleteDatasetService(this.ServiceContext);

                    CloudBlobContainer blobContainer =
                        await this.ServiceContext.CloudStorageProvider.GetCloudBlobContainer(UnsavedDatasetDeletionProcessor.RScriptBlobContainerName, this.ServiceContext.AppCredential);

                    foreach (var dataset in datasets)
                    {
                        try
                        {
                            if (dataset.InverseSourceDataset.Count > 0)
                            {
                                DeleteEntityErrorData entityError = new DeleteEntityErrorData
                                {
                                    Id = dataset.DatasetId.ToString(),
                                    Path = $"{UnsavedDatasetDeletionProcessor.UserFolderType}/{UnsavedDatasetDeletionProcessor.UnsavedFolderName}/{dataset.DatasetName}",
                                    Message = $"Dataset with id '{dataset.DatasetId}' is being used by a project."
                                };

                                deleteEntityErrors.Add(entityError);
                                this.Logger.LogWarning(entityError.Message);
                                continue;
                            }

                            Dataset datasetToDelete = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                                dbContext,
                                dataset.DatasetId,
                                includeRelatedExpressions: true,
                                includeParentFolder: false,
                                includeInverseSourceDatasets: false,
                                includeUserProject: false,
                                includeDatasetUserClientState: true);
                            this.LogMessage(workerJob, $"Deleting dataset {dataset.DatasetId}.");

                            await service.DeleteDataset(datasetToDelete, deleteDatasetRow: true, dbContext, blobContainer);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            DeleteEntityErrorData entityError = new DeleteEntityErrorData
                            {
                                Id = dataset.DatasetId.ToString(),
                                Path = $"{UnsavedDatasetDeletionProcessor.UserFolderType}/{UnsavedDatasetDeletionProcessor.UnsavedFolderName}/{dataset.DatasetName}",
                                Message = ex.GetBaseException().Message
                            };

                            deleteEntityErrors.Add(entityError);
                            this.Logger.LogError(ex, entityError.Message);
                        }
                    }
                }
            }

            UnsavedDatasetDeletionJobResultData results = new UnsavedDatasetDeletionJobResultData()
            {
                Status = "Success",
                DeleteEntityErrors = deleteEntityErrors
            };

            workerJob.JobResult = JsonConvert.SerializeObject(results);

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            return null;
        }

        /// <summary>
        /// Checks if the user id can be used to process the job.
        /// Throws an exception if the check does not pass.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public override void CheckUser(Guid userId)
        {
            // This processor does not require a user id.
        }
    }
}
