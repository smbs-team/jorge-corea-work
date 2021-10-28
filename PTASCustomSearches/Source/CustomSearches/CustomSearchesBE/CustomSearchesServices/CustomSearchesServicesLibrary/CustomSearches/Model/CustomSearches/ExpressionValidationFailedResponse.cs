namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;

    /// <summary>
    /// Model for the response when an expression validation fails.
    /// </summary>
    public class ExpressionValidationFailedResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the validation results..
        /// </summary>
        public CustomExpressionValidationResult[] ValidationErrors { get; set; }
    }
}
