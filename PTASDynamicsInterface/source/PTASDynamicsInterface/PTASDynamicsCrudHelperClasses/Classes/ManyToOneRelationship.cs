// <copyright file="ManyToOneRelationship.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Reference of a dynamics entity.
    /// </summary>
    public class ManyToOneRelationship
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