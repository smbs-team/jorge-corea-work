// <copyright file="EntityChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Collections.Generic;

    /// <summary>
    /// Changes on an entity.
    /// </summary>
    public class EntityChanges : EntityLocator
    {
        /// <summary>
        /// Gets or sets changes.
        /// </summary>
        public Dictionary<string, object> Changes { get; set; }
    }
}