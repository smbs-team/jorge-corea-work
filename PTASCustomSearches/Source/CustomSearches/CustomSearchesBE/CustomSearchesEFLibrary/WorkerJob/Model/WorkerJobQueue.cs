namespace CustomSearchesEFLibrary.WorkerJob.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entity for RScriptJobQueue table.
    /// </summary>
    [Table("WorkerJobQueue", Schema = "dbo")]
    public class WorkerJobQueue
    {
        /// <summary>
        /// Gets or sets the JobId field.
        /// </summary>
        [Key]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the user id field.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the job type field.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the queue name field.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the job payload field.
        /// </summary>
        public string JobPayload { get; set; }

        /// <summary>
        /// Gets or sets the JobResult field.
        /// </summary>
        public string JobResult { get; set; }

        /// <summary>
        /// Gets or sets the ExecutionTime field.
        /// </summary>
        public decimal? ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the keep alive timestamp.
        /// </summary>
        public DateTime KeepAliveTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds.
        /// </summary>
        public int TimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the started timestamp.
        /// </summary>
        public DateTime? StartedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        public DateTime? CreatedTimestamp { get; set; }
    }
}
