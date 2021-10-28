namespace CustomSearchesServicesLibrary.Exception
{
    using System.Collections.Generic;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;

    /// <summary>
    /// Represents errors that occur when an expression result is not valid.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidExpressionResultException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionResultException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidExpressionResultException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}