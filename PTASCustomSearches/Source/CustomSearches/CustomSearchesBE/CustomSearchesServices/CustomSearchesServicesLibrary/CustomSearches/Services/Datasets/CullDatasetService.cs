namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes the filtered out rows in the dataset data and clears all the filtering state.
    /// </summary>
    public class CullDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CullDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public CullDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the filtered out rows in the dataset data and clears all the filtering state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The result objects.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Cull dataset failed in the database.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset or dataset data is locked.</exception>
        public async Task CullDatasetAsync(
            Guid datasetId,
            CustomSearchesDbContext dbContext)
        {
            Dataset dataset = await dbContext.Dataset.Include(d => d.DatasetPostProcess).FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);
            InputValidationHelper.AssertDatasetDataNotLocked(dataset);

            string tableName = CustomSearchesDataDbContext.GetDatasetTableFullName(dataset);

            string script =
                    $"DELETE FROM {tableName}\n" +
                    $"WHERE [{DatasetColumnHelper.FilterStateColumnName}] = 'FilteredOut'\n" +
                    $"UPDATE {tableName}\n" +
                    $"SET [{DatasetColumnHelper.FilterStateColumnName}] = 'NotFilteredOut'\n" +
                    $"WHERE [{DatasetColumnHelper.FilterStateColumnName}] = 'WantsToFilterOut'\n";

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            await DatasetHelper.GetAlterDatasetLockAsyncV2(
                async (datasetState, datasetPostProcessState) =>
                {
                    await DynamicSqlStatementHelper.ExecuteNonQueryWithRetriesAsync(
                        this.ServiceContext,
                        this.ServiceContext.DataDbContextFactory,
                        script,
                        "Update statement failed in the database while culling the dataset.");

                    if (dataset.DatasetPostProcess.Count > 0)
                    {
                        datasetPostProcessState = DatasetPostProcessStateType.NeedsPostProcessUpdate.ToString();
                        foreach (var currentDatasetPostProcess in dataset.DatasetPostProcess)
                        {
                            if (currentDatasetPostProcess.IsDirty == false)
                            {
                                currentDatasetPostProcess.LastModifiedBy = userId;
                                currentDatasetPostProcess.LastModifiedTimestamp = DateTime.UtcNow;
                                currentDatasetPostProcess.IsDirty = true;
                                dbContext.DatasetPostProcess.Update(currentDatasetPostProcess);
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();

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