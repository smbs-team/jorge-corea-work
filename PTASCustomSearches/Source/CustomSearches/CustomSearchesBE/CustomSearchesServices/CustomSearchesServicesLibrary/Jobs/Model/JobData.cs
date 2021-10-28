namespace CustomSearchesServicesLibrary.Jobs.Model
{
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using CustomSearchesServicesLibrary.Jobs.Services;
    using DocumentFormat.OpenXml.Drawing;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Job Data for running jobs.
    /// </summary>
    public class JobData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobData" /> class.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="payloadSizeLimit">The payload size limit.  If the payload is bigger than this amount, it will not be seralized.</param>
        public JobData(WorkerJobQueue job, int payloadSizeLimit = 24 * 1024)
        {
            this.JobId = job.JobId;
            this.JobStatus = GetJobStatusService.GetStatusFromJob(job);
            this.JobResult = JsonHelper.DeserializeObject(job.JobResult);

            // We just serialize payloads if they are not huge.
            if (job.JobPayload != null && job.JobPayload.Length < payloadSizeLimit)
            {
                this.JobPayload = JsonHelper.DeserializeObject(job.JobPayload);
            }

            this.JobType = job.JobType;
        }

        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the job type.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the job payload.
        /// </summary>
        public object JobPayload { get; set; }

        /// <summary>
        /// Gets or sets the job status.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkerJobStatus JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the job result.
        /// </summary>
        public object JobResult { get; set; }
    }
}
