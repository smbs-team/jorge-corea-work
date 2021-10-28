namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that creates a new version of the project that is a clone of the existing one.
    /// </summary>
    public class CreateNewProjectVersionService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewProjectVersionService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public CreateNewProjectVersionService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Creates a new version of the project that is a clone of the existing one.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="projectVersionType">The project version type.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The user project response.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't be execute because the datasets of the project are still being generated.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Cannot get the column schema or create the dataset data view.</exception>
        public async Task<ImportUserProjectResponse> CreateNewProjectVersionAsync(int userProjectId, UserProjectVersionType projectVersionType, CustomSearchesDbContext dbContext)
        {
            var payload = await this.PrepareCreateNewProjectVersionAsync(userProjectId, projectVersionType, dbContext);
            return await this.QueueCreateNewProjectVersionAsync(payload);
        }

        /// <summary>
        /// Creates a new version of the project cloning and saving the database entities.
        /// </summary>
        /// <param name="userProjectId">The user project id.</param>
        /// <param name="projectVersionType">The project version type.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The payload to be used in the queue for the user project generation.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't be execute because the datasets of the project are still being generated.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Cannot get the column schema or create the dataset data view.</exception>
        public async Task<UserProjectGenerationPayloadData> PrepareCreateNewProjectVersionAsync(int userProjectId, UserProjectVersionType projectVersionType, CustomSearchesDbContext dbContext)
        {
            UserProject userProject = await dbContext.UserProject
                .Where(p => p.UserProjectId == userProjectId)
                .Include(p => p.UserProjectDataset)
                .ThenInclude(ups => ups.Dataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userProject, "UserProject", userProjectId);

            this.ServiceContext.AuthProvider.AuthorizeProjectItemOperation(userProject, "CreateNewProjectVersion");

            UserProject newVersion = null;

            // In draft version we create the new version based in the latest version.
            if (projectVersionType == UserProjectVersionType.Draft)
            {
                int? rootProjectId = userProject.RootVersionUserProjectId == null ? userProject.UserProjectId : userProject.RootVersionUserProjectId;

                newVersion = await dbContext.UserProject
                        .Where(p => p.RootVersionUserProjectId == rootProjectId)
                        .OrderByDescending(p => p.VersionNumber)
                        .Include(p => p.UserProjectDataset)
                        .ThenInclude(ups => ups.Dataset)
                        .FirstOrDefaultAsync();
            }

            if (newVersion == null)
            {
                newVersion = userProject;
            }

            foreach (var projectDataset in newVersion.UserProjectDataset)
            {
                Dataset dataset = await DatasetHelper.LoadDatasetWithDependenciesAsync(
                    dbContext,
                    projectDataset.DatasetId,
                    includeRelatedExpressions: true,
                    includeParentFolder: true,
                    includeInverseSourceDatasets: false,
                    includeUserProject: true,
                    includeDatasetUserClientState: false);
            }

            foreach (var projectDataset in newVersion.UserProjectDataset)
            {
                DatasetHelper.DetachDatasetWithDependencies(dbContext, projectDataset.Dataset);
            }

            dbContext.Entry(newVersion).State = EntityState.Detached;

            newVersion.VersionType = projectVersionType.ToString();
            newVersion.VersionNumber++;
            newVersion.RootVersionUserProjectId = newVersion.RootVersionUserProjectId == null ? newVersion.UserProjectId : newVersion.RootVersionUserProjectId;
            newVersion.UserProjectId = 0;

            List<Guid> sourceDatasetIds = new List<Guid>();
            List<Guid> newDatasetIds = new List<Guid>();

            List<DatasetGenerationPayloadData> payloads = new List<DatasetGenerationPayloadData>();

            foreach (var projectDataset in newVersion.UserProjectDataset)
            {
                sourceDatasetIds.Add(projectDataset.DatasetId);
                DatasetStateType datasetState = Enum.Parse<DatasetStateType>(projectDataset.Dataset.DataSetState, ignoreCase: true);
                if (datasetState <= DatasetStateType.GeneratingIndexes)
                {
                    throw new CustomSearchesConflictException($"Can't be execute because the datasets of the project are still being generated.", null);
                }

                projectDataset.UserProjectId = 0;

                var duplicateDatasetService = new DuplicateDatasetService(this.ServiceContext);
                var newDatasetId = await duplicateDatasetService.DuplicateDatasetAsync(
                    projectDataset.Dataset,
                    newVersion,
                    duplicateDatasetData: null,
                    duplicatePostProcess: true,
                    dbContext);

                newDatasetIds.Add(newDatasetId);
            }

            dbContext.UserProject.Add(newVersion);
            await dbContext.ValidateAndSaveChangesAsync();

            UserProjectGenerationPayloadData payload = new UserProjectGenerationPayloadData()
            {
                UserProjectId = newVersion.UserProjectId,
                SourceDatasets = sourceDatasetIds.ToArray(),
                NewDatasets = newDatasetIds.ToArray(),
            };

            return payload;
        }

        /// <summary>
        /// Queues the user project generation in order to create the related tables and views in the database.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// The user project response.
        /// </returns>
        public async Task<ImportUserProjectResponse> QueueCreateNewProjectVersionAsync(UserProjectGenerationPayloadData payload)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            int jobId = await this.ServiceContext.AddWorkerJobQueueAsync(
                "DatasetGeneration",
                "UserProjectGenerationJobType",
                userId,
                payload,
                WorkerJobTimeouts.DatasetGenerationTimeout);

            return new ImportUserProjectResponse(payload.UserProjectId, payload.NewDatasets, new int[] { jobId });
        }
    }
}
