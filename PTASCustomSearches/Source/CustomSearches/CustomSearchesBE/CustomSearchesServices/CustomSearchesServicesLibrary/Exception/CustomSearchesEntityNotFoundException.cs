namespace CustomSearchesServicesLibrary.Exception
{
    using System;

    /// <summary>
    /// Represents errors that occur when a expected entity is not found in the database.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CustomSearchesEntityNotFoundException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchesEntityNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomSearchesEntityNotFoundException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}