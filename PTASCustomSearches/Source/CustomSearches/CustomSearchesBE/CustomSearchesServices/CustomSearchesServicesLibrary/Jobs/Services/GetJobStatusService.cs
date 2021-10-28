namespace CustomSearchesServicesLibrary.Jobs.Services
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using CustomSearchesServicesLibrary.Jobs.Model;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that gets the status of a job.
    /// </summary>
    public class GetJobStatusService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetJobStatusService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetJobStatusService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the job status.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>The job status.</returns>
        public static WorkerJobStatus GetStatusFromJob(WorkerJobQueue job)
        {
            string jobResult = job.JobResult as string;

            if (jobResult != null)
            {
                var objectJobResult = JsonHelper.DeserializeObject<FailedJobResult>(jobResult);
                if (objectJobResult == null || objectJobResult?.Status.ToLower() != "success")
                {
                    return WorkerJobStatus.Failed;
                }

                return WorkerJobStatus.Finished;
            }
            else
            {
                return job.StartedTimestamp == null ? WorkerJobStatus.NotStarted : WorkerJobStatus.InProgress;
            }
        }

        /// <summary>
        /// Gets the job status.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <returns>
        /// The job status.
        /// </returns>
        public async Task<GetJobStatusResponse> GetJobStatus(int jobId)
        {
            GetJobStatusResponse response = new GetJobStatusResponse();
            WorkerJobQueue workerJob = await DbTransientRetryPolicy.GetWorkerJobAsync(this.ServiceContext, jobId, includePayload: false);

            response.JobStatus = GetJobStatusService.GetStatusFromJob(workerJob).ToString();

            if (!string.IsNullOrWhiteSpace(workerJob.JobResult))
            {
                response.JobResult = JsonHelper.DeserializeObject(workerJob.JobResult);
            }

            return response;
        }
    }
}
