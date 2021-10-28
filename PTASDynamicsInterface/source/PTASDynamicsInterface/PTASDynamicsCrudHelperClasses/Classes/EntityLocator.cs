// <copyright file="EntityChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Locator for any entity, has entity name and object id.
    /// </summary>
    public class EntityLocator
    {
        /// <summary>
        /// Gets or sets entity Name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets entity Id.
        /// </summary>
        public string EntityId { get; set; }
    }
}