namespace CustomSearchesServicesLibrary.Exception
{
    using System;

    /// <summary>
    /// Represents errors that occur during application execution related to the json.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CustomSearchesJsonException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesJsonException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchesJsonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}