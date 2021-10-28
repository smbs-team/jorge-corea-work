namespace CustomSearchesServicesLibrary.Exception
{
    using System;

    /// <summary>
    /// Represents errors that occur during application execution related to the database.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CustomSearchesDatabaseException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesDatabaseException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchesDatabaseException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}