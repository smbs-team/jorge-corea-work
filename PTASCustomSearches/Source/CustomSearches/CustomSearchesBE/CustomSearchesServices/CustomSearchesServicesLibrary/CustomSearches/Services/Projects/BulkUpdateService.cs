namespace CustomSearchesServicesLibrary.CustomSearches.Services.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that applies the bulk update to the user project model.
    /// </summary>
    public class BulkUpdateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkUpdateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public BulkUpdateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Queues the bulk update to the user project model.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="major">The major value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="minor">The minor value of the row when the payload is used in a single row execution mode.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The job id.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">UserProjectId should be the id of the root project.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">User project was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<IdResult> QueueBulkUpdateAsync(Guid datasetId, string major, string minor, CustomSearchesDbContext dbContext)
        {
            var bulkUpdatePostProcess = await
                (from upd in dbContext.UserProjectDataset
                join dpp in dbContext.DatasetPostProcess
                on upd.DatasetId equals dpp.DatasetId
                where (upd.DatasetId == datasetId) && (dpp.PostProcessRole == ApplyModelService.BulkUpdatePostProcessRole)
                select dpp).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(
                bulkUpdatePostProcess,
                nameof(bulkUpdatePostProcess),
                entityId: null,
                message: $"The dataset should have a post process with the role '{ApplyModelService.BulkUpdatePostProcessRole}'");

            ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
            return await service.QueueExecuteDatasetPostProcessAsync(
                datasetId,
                bulkUpdatePostProcess.DatasetPostProcessId,
                major,
                minor,
                parameters: null,
                dataStream: null,
                dbContext);
        }
    }
}