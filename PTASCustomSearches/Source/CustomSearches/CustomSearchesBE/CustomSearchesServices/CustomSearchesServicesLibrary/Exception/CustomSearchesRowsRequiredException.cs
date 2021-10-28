namespace CustomSearchesServicesLibrary.Exception
{
    using System;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Represents errors that occur when a required rows conflict is found.
    /// </summary>
    /// <seealso cref="Exception" />
    public class CustomSearchesRowsRequiredException : CustomSearchesConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesRowsRequiredException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchesRowsRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesRowsRequiredException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="dataset">The dataset.</param>
        public CustomSearchesRowsRequiredException(string message, Exception innerException, Dataset dataset)
            : base(message, innerException)
        {
            this.PostProcessState = dataset.DataSetPostProcessState;
            this.DatasetState = dataset.DataSetState;
            this.DbLockType = dataset.DbLockType;
            this.JobId = dataset.LockingJobId != null ? (int)dataset.LockingJobId : 0;
        }
    }
}