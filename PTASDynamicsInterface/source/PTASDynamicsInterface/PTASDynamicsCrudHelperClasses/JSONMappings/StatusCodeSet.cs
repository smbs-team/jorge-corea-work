// <copyright file="StatusCodeSet.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    /// <summary>
    /// Class for the representation of Status codes.
    /// </summary>
    public class StatusCodeSet
    {
        /// <summary>
        /// Gets or sets Status of the Status Code Set.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets Value of the Status Code Set.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets State of the Status Code Set.
        /// </summary>
        public int State { get; set; }
    }
}
