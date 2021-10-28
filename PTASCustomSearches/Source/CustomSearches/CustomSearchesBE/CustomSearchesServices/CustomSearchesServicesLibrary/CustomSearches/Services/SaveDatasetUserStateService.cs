namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that saves the dataset user state.
    /// </summary>
    public class SaveDatasetUserStateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveDatasetUserStateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public SaveDatasetUserStateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Saves the dataset user state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="datasetClientState">Dataset client state.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        public async Task SaveDatasetUserStateAsync(Guid datasetId, string datasetClientState, CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            DatasetUserClientState datasetUserClientState =
                await dbContext.DatasetUserClientState.FirstOrDefaultAsync(d => ((d.DatasetId == datasetId) && (d.UserId == userId)));

            if (datasetUserClientState == null)
            {
                datasetUserClientState = new DatasetUserClientState
                {
                    UserId = userId,
                    DatasetId = datasetId,
                    DatasetClientState = datasetClientState
                };

                dbContext.DatasetUserClientState.Add(datasetUserClientState);
            }
            else
            {
                datasetUserClientState.DatasetClientState = datasetClientState;
                dbContext.DatasetUserClientState.Update(datasetUserClientState);
            }

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
