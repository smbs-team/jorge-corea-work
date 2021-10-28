// <copyright file="ManyToManyRelationship.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Represents the returned info on a many to many query.
    /// </summary>
    public class ManyToManyRelationship
    {
        /// <summary>
        /// Gets or sets entity1 Logical Name.
        /// </summary>
        public string Entity1LogicalName { get; set; }

        /// <summary>
        /// Gets or sets entity2 Logical Name.
        /// </summary>
        public string Entity2LogicalName { get; set; }

        /// <summary>
        /// Gets or sets entity1 Intersect Attribute.
        /// </summary>
        public string Entity1NavigationPropertyName { get; set; }
    }
}