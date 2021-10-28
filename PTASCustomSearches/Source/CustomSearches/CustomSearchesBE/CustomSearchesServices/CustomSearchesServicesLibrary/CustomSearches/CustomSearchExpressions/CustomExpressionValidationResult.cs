namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Validation results for a custom search expression.
    /// </summary>
    public class CustomExpressionValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExpressionValidationResult" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public CustomExpressionValidationResult(CustomSearchExpression expression)
        {
            this.Success = true;
            if (expression != null)
            {
                this.ValidatedIndex = expression.ExecutionOrder;
                this.ValidatedExpression = expression.Script;
                this.ValidatedExpressionType = expression.ExpressionType;
                this.ValidatedExpressionRole = expression.ExpressionRole;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CustomExpressionValidationResult"/> was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the index of the validated expression.
        /// </summary>
        public int ValidatedIndex { get; set; }

        /// <summary>
        /// Gets or sets the validated expression.
        /// </summary>
        public string ValidatedExpression { get; set; }

        /// <summary>
        /// Gets or sets the validated expression type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public string ValidatedExpressionType { get; set; }

        /// <summary>
        /// Gets or sets the validated expression role.
        /// </summary>
        public string ValidatedExpressionRole { get; set; }

        /// <summary>
        /// Gets or sets the validation error.
        /// </summary>
        public string ValidationError { get; set; }

        /// <summary>
        /// Gets or sets the validation error.
        /// </summary>
        public string ValidationErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the executed statement, in case the expression executed as part of a bigger context.
        /// </summary>
        public string ExecutedStatement { get; set; }
    }
}
