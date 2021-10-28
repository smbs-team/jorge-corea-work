namespace BaseWorkerLibrary
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that processes a specific type of worker job.
    /// </summary>
    public abstract class WorkerJobProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerJobProcessor"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public WorkerJobProcessor(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        /// <value>
        /// The type of the worker.
        /// </value>
        public abstract string ProcessorType { get; }

        /// <summary>
        /// Gets or sets the security principal.
        /// </summary>
        public IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// Checks if the user id can be used to process the job.
        /// Throws an exception if the check does not pass.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public virtual void CheckUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new AuthorizationException(
                    "The user id should not be empty in order to process this job.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public abstract object GetSinalRNotificationPayload(WorkerJobQueue workerJob);

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>A <see cref="Task"/> representing the boolean asynchronous operation.</returns>
        public abstract Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob);

        /// <summary>
        /// Creates a user notification.
        /// </summary>
        /// <param name="workerJob">The worker job.</param>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="text">The text.</param>
        /// <param name="errorMessage">The error message.</param>
        public virtual async Task CreateUserNotificationAsync(
            WorkerJobQueue workerJob,
            JobNotificationType notificationType,
            string text,
            string errorMessage = null)
        {
            try
            {
                var userInfo = this.ServiceContext.AuthProvider.UserInfoData;

                // If there is no user attached to the context, we can't notify.
                if (userInfo == null)
                {
                    return;
                }

                using (var dbContext = this.ServiceContext.DbContextFactory.Create())
                {
                    var notification = new UserJobNotification()
                    {
                        UserId = userInfo.Id,
                        JobId = workerJob.JobId,
                        JobType = workerJob.JobType,
                        JobNotificationType = notificationType.ToString(),
                        JobNotificationPayload = JsonHelper.SerializeObject(this.GetUserNotificationPayload()),
                        JobNotificationText = text,
                        ErrorMessage = errorMessage,
                        CreatedTimestamp = DateTime.UtcNow,
                        Dismissed = false
                    };

                    dbContext.UserJobNotification.Add(notification);
                    await dbContext.SaveChangesWithRetriesAsync();
                }
            }
            catch
            {
                // Exception is forgotten.  We don't care if a user notification could not be created.
            }
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="workerJob">The worker job.</param>
        /// <param name="message">The message.</param>
        protected void LogMessage(WorkerJobQueue workerJob, string message)
        {
            this.Logger.LogInformation($"Processor: {this.ProcessorType}, Job: {workerJob.JobId}, Message: {message}");
        }

        /// <summary>
        /// Gets the user notification payload.  Can be overwritten by inheriting classes to customize
        /// user notifications.
        /// </summary>
        protected virtual object GetUserNotificationPayload()
        {
            return null;
        }
    }
}