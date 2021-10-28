namespace CustomSearchesServicesLibrary.Exception
{
    using System;

    /// <summary>
    /// Represents errors that occur during the validation of a custom search.
    /// </summary>
    /// <seealso cref="Exception" />
    public class CustomSearchValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchValidationException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}