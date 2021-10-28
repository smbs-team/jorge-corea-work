namespace CustomSearchesServicesLibrary.Exception
{
    using System;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Represents errors that occur when a conflict is found.
    /// </summary>
    /// <seealso cref="Exception" />
    public class CustomSearchesConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesConflictException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchesConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesConflictException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="dataset">The dataset.</param>
        public CustomSearchesConflictException(string message, Exception innerException, Dataset dataset)
            : base(message, innerException)
        {
            this.PostProcessState = dataset.DataSetPostProcessState;
            this.DatasetState = dataset.DataSetState;
            this.DbLockType = dataset.DbLockType;
            this.JobId = dataset.LockingJobId != null ? (int)dataset.LockingJobId : 0;
        }

        /// <summary>
        /// Gets or sets the post process state.
        /// </summary>
        public string PostProcessState { get; set; }

        /// <summary>
        /// Gets or sets the dataset state.
        /// </summary>
        public string DatasetState { get; set; }

        /// <summary>
        /// Gets or sets the database lock type.
        /// </summary>
        public string DbLockType { get; set; }

        /// <summary>
        /// Gets or sets the job id.
        /// </summary>
        public int JobId { get; set; }
    }
}