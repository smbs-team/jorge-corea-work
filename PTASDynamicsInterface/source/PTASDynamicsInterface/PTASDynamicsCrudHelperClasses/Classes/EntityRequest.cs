// <copyright file="EntityRequest.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Request for a dynamics entity.
    /// </summary>
    public class EntityRequest
    {
        /// <summary>
        /// Gets or sets entity Name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets entity Id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets query string.
        /// </summary>
        public string Query { get; set; }
    }
}