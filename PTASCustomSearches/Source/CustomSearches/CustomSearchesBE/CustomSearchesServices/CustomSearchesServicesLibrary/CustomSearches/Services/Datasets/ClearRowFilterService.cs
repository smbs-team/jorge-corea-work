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
    /// Service that clears the row filter in the dataset data.
    /// </summary>
    public class ClearRowFilterService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearRowFilterService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ClearRowFilterService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Clears the row filter in the dataset data.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="preserveSelection">Value indicating whether the selection should be preserved.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The result objects.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Clear row filter failed in the database.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset.</exception>
        public async Task ClearRowFilterAsync(
            Guid datasetId,
            bool preserveSelection,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            string tableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);
            string script = string.Empty;
            if (preserveSelection)
            {
                script =
                        $"UPDATE {tableName}\n" +
                        $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'NotFilteredOut'\n" +
                        $"WHERE [{DatasetColumnHelper.FilterStateColumnName}] = 'WantsToFilterOut'\n" +
                        $"UPDATE {tableName}\n" +
                        $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'WantsToFilterOut'\n" +
                        $"WHERE [{DatasetColumnHelper.FilterStateColumnName}] = 'FilteredOut'\n";
            }
            else
            {
                script =
                        $"UPDATE {tableName}\n" +
                        $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'NotFilteredOut'\n" +
                        $"WHERE [{DatasetColumnHelper.FilterStateColumnName}] != 'NotFilteredOut'\n";
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        "Update statement failed in the database while clearing the row filter.");

                    return (datasetState, datasetPostProcessState);
                },
                dataset,
                isRootLock: false,
                dataset.DataSetState,
                dataset.DataSetPostProcessState,
                userId,
                lockingJobId: null,
                dbContext,
                this.ServiceContext);
        }
    }
}