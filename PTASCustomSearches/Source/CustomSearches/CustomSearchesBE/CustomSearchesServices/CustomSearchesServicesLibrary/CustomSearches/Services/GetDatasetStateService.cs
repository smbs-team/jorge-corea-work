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
    /// Service that gets the dataset state.
    /// </summary>
    public class GetDatasetStateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetStateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetStateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the dataset state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The dataset state.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        public async Task<GetDatasetStateResponse> GetDatasetStateAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            GetDatasetUserStateResponse response = new GetDatasetUserStateResponse();

            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => (d.DatasetId == datasetId));
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            return new GetDatasetStateResponse
            {
                DatasetState = dataset.DataSetState,
                DatasetPostProcessState = dataset.DataSetPostProcessState
            };
        }
    }
}
