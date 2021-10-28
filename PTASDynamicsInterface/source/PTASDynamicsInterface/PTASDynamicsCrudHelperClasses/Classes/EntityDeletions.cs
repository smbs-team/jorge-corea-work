// <copyright file="EntitysetChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Collections.Generic;

    /// <summary>
    /// Set of entitites to be deleted in dynamics.
    /// </summary>
    public class EntityDeletions
    {
        /// <summary>
        /// Gets or sets set of changes to apply.
        /// </summary>
        public IEnumerable<EntityLocator> Items { get; set; }
    }
}