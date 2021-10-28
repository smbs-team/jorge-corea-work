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
    /// Service that unlocks a dataset, independently of the lock state.
    /// </summary>
    public class UnlockLockedDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockLockedDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UnlockLockedDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Unlocks a dataset, independently of the lock state.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>The async task. </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset or post process was not found.</exception>
        /// <exception cref="CustomSearchesDatabaseException">Cull dataset failed in the database.</exception>
        /// <exception cref="CustomSearchesConflictException">Can't alter the dataset or dataset data is locked.</exception>
        public async Task UnlockLockedDataset(Guid datasetId, CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("UnlockLockedDataset");

            Dataset dataset = await dbContext.Dataset.Include(d => d.DatasetPostProcess).FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            bool unlocked = false;
            try
            {
                unlocked = await DatasetHelper.TestAlterDatasetLockAsync(
                    datasetId,
                    DatasetStateType.Processed.ToString(),
                    DatasetPostProcessStateType.Processed.ToString(),
                    isRootLock: false,
                    userId,
                    null,
                    dbContext);
            }
            catch (CustomSearchesConflictException)
            {
            }

            if (unlocked)
            {
                throw new CustomSearchesConflictException($"The dataset {dataset.DatasetId} is not db locked.", null, dataset);
            }

            await DatasetHelper.ReleaseAlterDatasetLockAsyncV2(
                dataset,
                DatasetStateType.Processed.ToString(),
                DatasetHelper.CalculateDatasetPostProcessState(dataset.DatasetPostProcess),
                userId,
                dbContext);
        }
    }
}