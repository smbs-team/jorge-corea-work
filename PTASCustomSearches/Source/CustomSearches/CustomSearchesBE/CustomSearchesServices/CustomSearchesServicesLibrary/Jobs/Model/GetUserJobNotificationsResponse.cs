namespace CustomSearchesServicesLibrary.Jobs.Model
{
    /// <summary>
    /// Model for the response of the GetUserJobNotifications service.
    /// </summary>
    public class GetUserJobNotificationsResponse
    {
        /// <summary>
        /// Gets or sets a value with the latest job notifications.
        /// </summary>
        public JobNotificationData[] JobNotifications { get; set; }

        /// <summary>
        /// Gets or sets a value with the currently running jobs for the user.
        /// </summary>
        public JobData[] PendingJobs { get; set; }
    }
}
