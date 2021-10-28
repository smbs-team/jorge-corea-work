namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the dataset client state.
    /// </summary>
    public class GetDatasetUserStateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetUserStateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetUserStateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset client state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The dataset client state.
        /// </returns>
        public async Task<GetDatasetUserStateResponse> GetDatasetUserStateAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            GetDatasetUserStateResponse response = new GetDatasetUserStateResponse();

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            DatasetUserClientState datasetUserClientState =
                await dbContext.DatasetUserClientState.FirstOrDefaultAsync(d => ((d.DatasetId == datasetId) && (d.UserId == userId)));

            if (datasetUserClientState != null)
            {
                response.DatasetUserState = datasetUserClientState.DatasetClientState;
            }

            return response;
        }
    }
}
