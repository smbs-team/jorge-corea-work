namespace CustomSearchesWorkerLibrary.DatasetProcessor.Model
{
    using System;

    /// <summary>
    /// Model for additional data for dataset post process notifications
    /// </summary>
    public class PostProcessUserNotificationPayload
    {
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the post process identifier.
        /// </summary>
        public int? PostProcessId { get; set; }

        /// <summary>
        /// Gets or sets the post process type.
        /// </summary>
        public string PostProcessType { get; set; }

        /// <summary>
        /// Gets or sets the post process role.
        /// </summary>
        public string PostProcessRole { get; set; }

        /// <summary>
        /// Gets or sets the dataset identifier.
        /// </summary>
        public Guid DatasetId  { get; set; }
    }
}
