// <copyright file="Building.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp.Types
{
    using Newtonsoft.Json;

    /// <summary>Building entity.</summary>
    public class Building
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty("ptas_buildingdetailid")]
        public string Id { get; set; }
    }
}
