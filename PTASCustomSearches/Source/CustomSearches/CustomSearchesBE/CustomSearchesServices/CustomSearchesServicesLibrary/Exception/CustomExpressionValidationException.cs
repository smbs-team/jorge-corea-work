namespace CustomSearchesServicesLibrary.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.Enumeration;

    /// <summary>
    /// Represents errors that occur when a custom expression is not valid.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CustomExpressionValidationException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExpressionValidationException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="validationResults">The validation results.</param>
        /// <param name="customExpressionValidationExceptionType">The custom search expression validation exception type.</param>
        /// <param name="innerException">The inner exception.</param>
        public CustomExpressionValidationException(
            string message,
            CustomExpressionValidationResult[] validationResults,
            CustomExpressionValidationExceptionType customExpressionValidationExceptionType,
            Exception innerException)
            : base(message, innerException)
        {
            if (validationResults?.Length > 0)
            {
                var errors = new List<CustomExpressionValidationResult>();
                foreach (var result in validationResults)
                {
                    if (result != null && !result.Success)
                    {
                        errors.Add(result);
                    }
                }

                this.ValidationErrors = errors.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the custom search expression validation exception type.
        /// </summary>
        public CustomExpressionValidationExceptionType CustomExpressionValidationExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        public CustomExpressionValidationResult[] ValidationErrors { get; set; }
    }
}