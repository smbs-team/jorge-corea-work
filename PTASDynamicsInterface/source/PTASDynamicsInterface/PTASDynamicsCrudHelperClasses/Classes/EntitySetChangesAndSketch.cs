// <copyright file="EntitysetChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Model for entity changes and sketch manipulation.
    /// </summary>
    public class EntitySetChangesAndSketch : EntitysetChanges
    {
        /// <summary>
        /// Gets or sets id of the sketch to save.
        /// </summary>
        public string SketchId { get; set; }

        /// <summary>
        /// Gets or sets sketch object itself.
        /// </summary>
        public dynamic Sketch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to deactivate migrated.
        /// </summary>
        public bool? DeactivateMigrated { get; set; } = false;
    }
}