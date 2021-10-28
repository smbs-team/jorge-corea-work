// <copyright file="EntityRequestResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Result of an entity request.
    /// </summary>
    public class EntityRequestResult
    {
        /// <summary>
        /// Gets or sets requested changes.
        /// </summary>
        public dynamic Changes { get; set; }

        /// <summary>
        /// Gets or sets id of the fetched entity.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets name of the searched entity.
        /// </summary>
        public string EntityName { get; set; }
    }
}