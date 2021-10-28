namespace CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the post processes for a dataset.
    /// </summary>
    public class GetDatasetPostProcessesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetPostProcessesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetPostProcessesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the post processes for a dataset.
        /// </summary>
        /// <param name="datasetId">Dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The post processes for a dataset.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        public async Task<GetDatasetPostProcessesResponse> GetDatasetPostProcessesAsync(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            GetDatasetPostProcessesResponse response = new GetDatasetPostProcessesResponse();

            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.DatasetPostProcess)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            var datasetPostProcesses = dataset.DatasetPostProcess.ToArray();
            if (dataset.DatasetPostProcess != null)
            {
                response.PostProcesses = new DatasetPostProcessData[datasetPostProcesses.Length];
                for (int i = 0; i < datasetPostProcesses.Length; i++)
                {
                    var item = datasetPostProcesses[i];
                    response.PostProcesses[i] = new DatasetPostProcessData(item, ModelInitializationType.Summary, userDetails: null);
                }
            }

            return response;
        }
    }
}
