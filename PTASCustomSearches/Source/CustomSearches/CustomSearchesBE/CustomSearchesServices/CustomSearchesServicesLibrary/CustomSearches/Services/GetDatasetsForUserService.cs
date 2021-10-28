namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the datasets for user.
    /// </summary>
    public class GetDatasetsForUserService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetsForUserService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetsForUserService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the datasets for user.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The datasets for user.
        /// </returns>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<GetDatasetsForUserResponse> GetDatasetsForUserAsync(Guid userId, CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userId, "GetDatasetsForUser");

            GetDatasetsForUserResponse response = new GetDatasetsForUserResponse();

            var datasets = await (from d in dbContext.Dataset
                                  join f in dbContext.Folder
                                  on d.ParentFolderId equals f.FolderId
                                  where (f.FolderType.ToLower() == CustomSearchFolderType.Shared.ToString().ToLower())
                                  || ((f.FolderType.ToLower() == CustomSearchFolderType.User.ToString().ToLower()) && (f.UserId == userId))
                                  select d).
                                  Include(d => d.CreatedByNavigation).
                                  Include(d => d.LastModifiedByNavigation).
                                  Include(d => d.LastExecutedByNavigation).
                                  Include(d => d.User).
                                  Include(d => d.ParentFolder).
                                      ThenInclude(f => f.User).
                                  ToArrayAsync();

            if (datasets != null)
            {
                DatasetFolderManager userDatasetFolderManager = new DatasetFolderManager(CustomSearchFolderType.User, userId, dbContext);
                DatasetFolderManager sharedDatasetFolderManager = new DatasetFolderManager(CustomSearchFolderType.Shared, userId, dbContext);

                Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
                response.Datasets = new DatasetData[datasets.Length];
                for (int i = 0; i < datasets.Length; i++)
                {
                    var dataset = datasets[i];
                    DatasetData datasetData = new DatasetData(dataset, ModelInitializationType.Summary, userDetails);
                    datasetData.IsOutdated = await CustomSearchDefinitionHelper.GetIsCustomSearchDefinitionOutdated(datasetData.CustomSearchDefinitionId, dbContext);

                    if (dataset.ParentFolder.FolderType.ToLower() == CustomSearchFolderType.User.ToString().ToLower())
                    {
                        datasetData.FolderPath = await userDatasetFolderManager.GetFolderPathAsync(dataset);
                    }
                    else
                    {
                        datasetData.FolderPath = await sharedDatasetFolderManager.GetFolderPathAsync(dataset);
                    }

                    response.Datasets[i] = datasetData;
                }

                response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);
            }

            return response;
        }
    }
}
