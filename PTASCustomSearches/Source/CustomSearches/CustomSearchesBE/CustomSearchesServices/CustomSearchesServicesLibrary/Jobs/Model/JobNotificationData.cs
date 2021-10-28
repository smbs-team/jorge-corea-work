namespace CustomSearchesServicesLibrary.Jobs.Model
{
    using System;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches;

    /// <summary>
    /// Job Notification Data.
    /// </summary>
    public class JobNotificationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobNotificationData" /> class.
        /// </summary>
        /// <param name="jobNotification">The job notification.</param>
        public JobNotificationData(UserJobNotification jobNotification)
        {
            this.JobId = jobNotification.JobId;
            this.JobType = jobNotification.JobType;
            this.JobNotificationId = jobNotification.JobNotificationId;
            this.JobNotificationText = jobNotification.JobNotificationText;
            this.JobNotificationType = jobNotification.JobNotificationType;
            this.JobNotificationPayload = JsonHelper.SerializeObject(jobNotification.JobNotificationPayload);
            this.CreatedTimestamp = jobNotification.CreatedTimestamp;
        }

        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        public int? JobId { get; set; }

        /// <summary>
        /// Gets or sets the job type.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the job notification identifier.
        /// </summary>
        public int JobNotificationId { get; set; }

        /// <summary>
        /// Gets or sets the job notification text.
        /// </summary>
        public string JobNotificationText { get; set; }

        /// <summary>
        /// Gets or sets the type of the job notification.
        /// </summary>
        public string JobNotificationType { get; set; }

        /// <summary>
        /// Gets or sets the job notification payload.
        /// </summary>
        public object JobNotificationPayload { get; set; }

        /// <summary>
        /// Gets or sets the created time stamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }
    }
}
