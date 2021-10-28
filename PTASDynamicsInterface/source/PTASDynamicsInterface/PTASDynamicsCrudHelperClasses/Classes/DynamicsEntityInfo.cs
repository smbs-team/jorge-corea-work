// <copyright file="DynamicsEntityInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
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
        /// Gets or sets list of entity references.
        /// </summary>
        public ManyToOneRelationship[] ManyToOneRelationships { get; set; }

        /// <summary>
        /// Gets or sets list of n:m entity references.
        /// </summary>
        public ManyToManyRelationship[] ManyToManyRelationships { get; set; }
    }
}