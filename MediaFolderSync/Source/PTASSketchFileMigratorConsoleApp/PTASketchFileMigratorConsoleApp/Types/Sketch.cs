// <copyright file="Sketch.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using Newtonsoft.Json;
    using PTASketchFileMigratorConsoleApp.Types;

    /// <summary>Sketch entity properties.</summary>
    public class Sketch
    {
        /// <summary>Gets or sets the PTAS sketch id.</summary>
        /// <value>The PTAS sketch id.</value>
        [JsonProperty("ptas_sketchid")]
        public string SketchId { get; set; }

        /// <summary>Gets or sets the version.</summary>
        /// <value>The version.</value>
        [JsonProperty("ptas_version")]
        public string Version { get; set; }

        /// <summary>Gets or sets the condo unit.</summary>
        /// <value>The condo unit.</value>
        [JsonProperty("ptas_unitid")]
        public Unit CondoUnit { get; set; }

        /// <summary>Gets or sets the accessory.</summary>
        /// <value>The accessory.</value>
        [JsonProperty("ptas_accessoryid")]
        public Accessory Accessory { get; set; }

        /// <summary>Gets or sets the building.</summary>
        /// <value>The building.</value>
        [JsonProperty("ptas_buildingid")]
        public Building Building { get; set; }
    }
}
