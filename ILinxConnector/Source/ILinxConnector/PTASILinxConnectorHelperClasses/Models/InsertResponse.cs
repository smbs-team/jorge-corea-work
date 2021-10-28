// <copyright file="InsertResponse.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
    /// <summary>
    /// Response sent on positive insertion.
    /// </summary>
    public class InsertResponse
    {
        /// <summary>
        /// Gets or sets id that was assigned by system.
        /// </summary>
        public string AssignedId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether error state of the result.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets error message, empty if no error.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}