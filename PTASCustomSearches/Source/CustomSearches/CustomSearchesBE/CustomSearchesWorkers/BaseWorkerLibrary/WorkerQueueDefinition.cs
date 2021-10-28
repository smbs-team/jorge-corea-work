namespace BaseWorkerLibrary
{
    /// <summary>
    /// Worker queue definition.
    /// </summary>
    public class WorkerQueueDefinition
    {
        /// <summary>
        /// The queue name.
        /// </summary>
        public string QueueName;

        /// <summary>
        /// The maximum parallel jobs.
        /// </summary>
        public int MaxParallelJobs;

        /// <summary>
        /// The queue poll interval in MS.
        /// </summary>
        public int PollIntervalMs;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerQueueDefinition"/> class.
        /// </summary>
        public WorkerQueueDefinition()
        {
            this.QueueName = "Default";
            this.MaxParallelJobs = 1;
            this.PollIntervalMs = 1000;
        }
    }
}