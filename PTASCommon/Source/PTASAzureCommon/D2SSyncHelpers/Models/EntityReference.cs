// <copyright file="EntityReference.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Models
{
    /// <summary>
    /// Reference of a dynamics entity.
    /// </summary>
    public class EntityReference
    {
        /// <summary>
        /// Gets or sets referenced Attribute.
        /// </summary>
        public string ReferencedAttribute { get; set; }

        /// <summary>
        /// Gets or sets referenced Entity.
        /// </summary>
        public string ReferencedEntity { get; set; }

        /// <summary>
        /// Gets or sets referencing Attribute.
        /// </summary>
        public string ReferencingAttribute { get; set; }

        /// <summary>
        /// Gets or sets referencing Entity.
        /// </summary>
        public string ReferencingEntity { get; set; }

        /// <summary>
        /// Gets or sets metadata Id.
        /// </summary>
        public string MetadataId { get; set; }
    }
}