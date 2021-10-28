namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that sets the dataset lock level.
    /// </summary>
    public class SetDatasetLockLevelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetDatasetLockLevelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public SetDatasetLockLevelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Sets the dataset lock level.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="isLocked">A value indicating if the dataset should be locked.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="NotSupportedException">Updating the dataset lock is not supported for reference datasets.</exception>
        public async Task SetDatasetLockLevelAsync(Guid datasetId, bool isLocked, CustomSearchesDbContext dbContext, ILogger logger)
        {
            Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(dataset.UserId, "SetDatasetLockLevel");

            if (DatasetHelper.IsReferenceDataset(dataset))
            {
                throw new NotSupportedException(
                    $"Updating the dataset lock is not supported for reference datasets.",
                    null);
            }

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            dataset.IsLocked = isLocked;
            dataset.LastModifiedTimestamp = DateTime.UtcNow;
            dataset.LastModifiedBy = userId;
            dbContext.Dataset.Update(dataset);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
