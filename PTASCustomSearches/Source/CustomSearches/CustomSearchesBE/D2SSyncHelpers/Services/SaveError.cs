// <copyright file="SaveError.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    /// <summary>
    /// Save error detail.
    /// </summary>
    public class SaveError
    {
        /// <summary>
        /// Gets or sets a value indicating whether had error.
        /// </summary>
        public bool HadError { get; set; } = false;

        /// <summary>
        /// Gets or sets executed Query.
        /// </summary>
        public string ExecutedQuery { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets error Message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>Object converted to string.</returns>
        public override string ToString()
        {
            return this.HadError ? $"ErrorMessage: {this.ErrorMessage}\nErrorType: {this.ErrorType}\nQuery:\n{this.ExecutedQuery}" : string.Empty;
        }
    }
}