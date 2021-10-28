// <copyright file="EntitysetChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Collections.Generic;

    /// <summary>
    /// Set of entitites to be changed in dynamics.
    /// </summary>
    public class EntitysetChanges
    {
        /// <summary>
        /// Gets or sets set of changes to apply.
        /// </summary>
        public IEnumerable<EntityChanges> Items { get; set; }
    }
}