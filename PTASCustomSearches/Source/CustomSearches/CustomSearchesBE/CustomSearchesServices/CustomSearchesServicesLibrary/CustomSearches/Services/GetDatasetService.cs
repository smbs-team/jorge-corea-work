namespace CustomSearchesServicesLibrary.CustomSearches.Services
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
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets a dataset.
    /// </summary>
    public class GetDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the datset.
        /// </summary>
        /// <param name="datasetId">The datatset id.</param>
        /// <param name="includeDependencies">if set to <c>true</c> [include dependencies].</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The dataset.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        public async Task<GetDatasetResponse> GetDatasetAsync(
            Guid datasetId,
            bool includeDependencies,
            CustomSearchesDbContext dbContext)
        {
            GetDatasetResponse response = new GetDatasetResponse();

            Dataset dataset = null;

            if (includeDependencies)
            {
                var datasetQuery = (from d in dbContext.Dataset where d.DatasetId == datasetId select d).
                    Include(d => d.DatasetPostProcess).
                    Include(d => d.InteractiveChart).
                    Include(d => d.UserProjectDataset).
                    Include(d => d.ParentFolder);

                dataset = await datasetQuery.FirstOrDefaultAsync();

                await DatasetHelper.LoadDatasetUsersAsync(dbContext, dataset);
            }
            else
            {
                var datasetQuery = (from d in dbContext.Dataset where d.DatasetId == datasetId select d).
                    Include(d => d.CreatedByNavigation).
                    Include(d => d.LastModifiedByNavigation).
                    Include(d => d.LastExecutedByNavigation).
                    Include(d => d.User).
                    Include(d => d.UserProjectDataset).
                    Include(d => d.ParentFolder).
                        ThenInclude(f => f.User);
                dataset = await datasetQuery.FirstOrDefaultAsync();
            }

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            ModelInitializationType initializationType = includeDependencies ?
                ModelInitializationType.FullObjectWithDepedendencies :
                ModelInitializationType.FullObject;

            Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
            DatasetData datasetData = new DatasetData(dataset, initializationType, userDetails);
            datasetData.IsOutdated = await CustomSearchDefinitionHelper.GetIsCustomSearchDefinitionOutdated(datasetData.CustomSearchDefinitionId, dbContext);

            if (dataset.ParentFolder != null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                CustomSearchFolderType folderType = Enum.Parse<CustomSearchFolderType>(dataset.ParentFolder.FolderType, ignoreCase: true);
                DatasetFolderManager folderManager = new DatasetFolderManager(folderType, dataset.UserId, dbContext);
                string folderPath = await folderManager.GetFolderPathAsync(dataset);
                datasetData.FolderPath = folderPath;
            }

            response.Dataset = datasetData;
            response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);
            response.DatasetProjectRole = dataset.UserProjectDataset.FirstOrDefault()?.DatasetRole;

            return response;
        }
    }
}
