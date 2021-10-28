namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that imports the waterfront schedule post process.
    /// </summary>
    public class ImportWaterfrontSchedulePostProcessService : ImportSchedulePostProcessService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportWaterfrontSchedulePostProcessService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportWaterfrontSchedulePostProcessService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports the waterfront schedule post process.
        /// </summary>
        /// <param name="datasetPostProcessData">The dataset post process data to update.</param>
        /// <param name="columnName">The name of column to update.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception in validation of expressions.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task<IdResult> ImportWaterfrontSchedulePostProcessAsync(
            DatasetPostProcessData datasetPostProcessData,
            string columnName,
            CustomSearchesDbContext dbContext)
        {
            var projectType = await DatasetHelper.GetProjectTypeAsync(datasetPostProcessData.DatasetId, dbContext);
            var baseColumnName = $"[{projectType.WaterFrontLotSizeColumnName}]";
            return await this.ImportSchedulePostProcessAsync(datasetPostProcessData, baseColumnName, columnName, dbContext);
        }
    }
}
