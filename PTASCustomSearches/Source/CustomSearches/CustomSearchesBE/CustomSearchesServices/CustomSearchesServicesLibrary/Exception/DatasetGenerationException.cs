namespace CustomSearchesServicesLibrary.Exception
{
    using System;

    /// <summary>
    /// Represents errors that occur during dataset generation.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DatasetGenerationException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetGenerationException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="failedDuringPostProcess">if set to <c>true</c> [failed during post process].</param>
        public DatasetGenerationException(string message, System.Exception innerException, bool failedDuringPostProcess)
            : base(message, innerException)
        {
            this.FailedDuringPostProcess = failedDuringPostProcess;
        }

        /// <summary>
        /// Gets a value indicating whether the dataset generation failed during post process phase.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [failed during post process]; otherwise, <c>false</c>.
        /// </value>
        public bool FailedDuringPostProcess { get; private set; }
    }
}