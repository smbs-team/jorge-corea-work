// <copyright file="DynamicsEntityInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    /// <summary>
    /// Dynamics wrapper, used to decode json.
    /// </summary>
    public class DynamicsEntityInfo
    {
        /// <summary>
        /// Gets or sets name of the entity set.
        /// </summary>
        public string EntitySetName { get; set; }

        /// <summary>
        /// Gets or sets list of entity attributes.
        /// </summary>
        public EntityAttribute[] Attributes { get; set; }
    }
}