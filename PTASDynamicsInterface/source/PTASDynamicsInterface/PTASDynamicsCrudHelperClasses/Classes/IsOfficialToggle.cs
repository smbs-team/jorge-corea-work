// <copyright file="EntitysetChanges.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Input for setting the official on a sketch.
    /// </summary>
    public class IsOfficialToggle
    {
        /// <summary>
        /// Gets or sets id of the sketch to save.
        /// </summary>
        public string SketchId { get; set; }

        /// <summary>
        /// Gets or sets BuildingId.
        /// </summary>
        public string BuildingId { get; set; }

        /// <summary>
        /// Gets or sets UnitId.
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets AccesoryId.
        /// </summary>
        public string AccesoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to deactivate migrated.
        /// </summary>
        public bool? DeactivateMigrated { get; set; } = false;
    }
}