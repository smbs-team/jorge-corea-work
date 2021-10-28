namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using BaseWorkerLibrary.SqlServer.Model;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using PTASServicesCommon.DependencyInjection;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using PTASCRMHelpers;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using System.Collections.Generic;

    /// <summary>
    /// User project generation worker class
    /// </summary>
    public class UserProjectGenerationProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The dynamics OData helper factory.
        /// </summary>
        private readonly IFactory<GenericDynamicsHelper> dynamicsODataHelperFactory;

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType 
        { 
            get
            {
                return nameof(CustomSearchesJobTypes.UserProjectGenerationJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProjectGenerationProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/logger parameter is null.</exception>
        public UserProjectGenerationProcessor(IFactory<CustomSearchesDbContext> dbContextFactory, IFactory<GenericDynamicsHelper> dynamicsODataHelper, ILogger logger) : base(logger)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
            this.dynamicsODataHelperFactory = dynamicsODataHelper;
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob"></param>
        /// <returns>True if the job completed successfully</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            var payload = JsonConvert.DeserializeObject<UserProjectGenerationPayloadData>(workerJob.JobPayload);
            List<string> warnings = new List<string>();

            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                UserProject userProject = await dbContext.UserProject
                    .Where(p => p.UserProjectId == payload.UserProjectId)
                    .Include(p => p.ProjectType)
                    .Include(p => p.UserProjectDataset)
                    .ThenInclude(ups => ups.Dataset)
                    .FirstOrDefaultAsync();

                ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
                var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
                var pivotDataset = await projectBusinessLogic.LoadPivotDatasetAsync(datasetId: null);
                ExecuteCustomSearchService executeCustomSearchService = new ExecuteCustomSearchService(this.ServiceContext);

                var sourceDatasetMap = new Dictionary<Guid, Guid>();

                if (payload.NewDatasets?.Length > 0)
                {
                    for (int i = 0; i < payload.NewDatasets.Length; i++)
                    {
                        sourceDatasetMap.Add(payload.NewDatasets[i], payload.SourceDatasets[i]);
                    }
                }

                // Executes pivot dataset and post processes at the end
                foreach (var userProjectDataset in userProject.UserProjectDataset.OrderBy(upd => upd.DatasetId == pivotDataset.DatasetId))
                {

                    DatasetGenerationPayloadData datasetGenerationPayload = new DatasetGenerationPayloadData
                    {
                        CustomSearchDefinitionId = userProjectDataset.Dataset.CustomSearchDefinitionId,
                        Parameters = null,
                        DatasetId = userProjectDataset.DatasetId,
                        SourceDatasetId = sourceDatasetMap[userProjectDataset.DatasetId],
                        ExecutionMode = DatasetExecutionMode.Generate,
                        ApplyRowFilterFromSourceDataset = false,
                        ApplyUserSelectionFromSourceDataset = false,
                        NeedsPostProcessExecution = userProjectDataset.DatasetId == pivotDataset.DatasetId
                    };

                    var validationWarnings = await executeCustomSearchService.ExecuteCustomSearchAsync(
                        datasetGenerationPayload,
                        dbContext,
                        this.dynamicsODataHelperFactory.Create(),
                        (string message) => { this.LogMessage(workerJob, message); });

                    if (validationWarnings?.Count > 0)
                    {
                        warnings.AddRange(validationWarnings);
                    }
                }
            }


            DatasetGenerationJobResultData results = new DatasetGenerationJobResultData()
            {
                Status = "Success",
                Warnings = warnings
            };

            workerJob.JobResult = JsonConvert.SerializeObject(results);

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            DatasetGenerationPayloadData payload = JsonConvert.DeserializeObject<DatasetGenerationPayloadData>(workerJob.JobPayload);
            return payload;
        }
    }
}
