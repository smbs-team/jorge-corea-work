namespace CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets a single post processes.
    /// </summary>
    public class GetDatasetPostProcessService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetPostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetPostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the post processes for a dataset.
        /// </summary>
        /// <param name="datasetPostProcessId">DatasetPostProcess id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The post processes for a dataset.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset post process was not found.</exception>
        public async Task<GetDatasetPostProcessResponse> GetDatasetPostProcessAsync(
            int datasetPostProcessId,
            CustomSearchesDbContext dbContext)
        {
            GetDatasetPostProcessResponse response = new GetDatasetPostProcessResponse();

            var datasetPostProcessQuery =
                from dp in dbContext.DatasetPostProcess
                where dp.DatasetPostProcessId == datasetPostProcessId
                select dp;

            var datasetPostProcess = await datasetPostProcessQuery.
                Include(dp => dp.PrimaryDatasetPostProcess).
                Include(dp => dp.InversePrimaryDatasetPostProcess).
                Include(dp => dp.CustomSearchExpression).
                Include(dp => dp.CreatedByNavigation).
                Include(dp => dp.LastModifiedByNavigation).
                Include(dp => dp.ExceptionPostProcessRule).
                    ThenInclude(e => e.CustomSearchExpression).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(datasetPostProcess, "Dataset post process", datasetPostProcessId);

            Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();
            response.PostProcess = new DatasetPostProcessData(datasetPostProcess, ModelInitializationType.FullObject, userDetails);
            response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);

            return response;
        }
    }
}
