// <copyright file="EntityManyToManyReference.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Models
{
    /// <summary>
    /// N:M reference in dynamics.
    /// </summary>
    public class EntityManyToManyReference
    {
        /// <summary>
        /// Gets or sets entity1LogicalName.
        /// </summary>
        public string Entity1LogicalName { get; set; }

        /// <summary>
        /// Gets or sets entity2LogicalName.
        /// </summary>
        public string Entity2LogicalName { get; set; }

        /// <summary>
        /// Gets or sets intersectEntityName.
        /// </summary>
        public string IntersectEntityName { get; set; }

        /// <summary>
        /// Gets or sets entity1IntersectAttribute.
        /// </summary>
        public string Entity1IntersectAttribute { get; set; }

        /// <summary>
        /// Gets or sets entity2IntersectAttribute.
        /// </summary>
        public string Entity2IntersectAttribute { get; set; }

        /// <summary>
        /// Gets or sets entity1NavigationPropertyName.
        /// </summary>
        public string Entity1NavigationPropertyName { get; set; }

        /// <summary>
        /// Gets or sets entity2NavigationPropertyName.
        /// </summary>
        public string Entity2NavigationPropertyName { get; set; }

        /// <inheritdoc/>
        public override string ToString() => this.IntersectEntityName;
    }
}