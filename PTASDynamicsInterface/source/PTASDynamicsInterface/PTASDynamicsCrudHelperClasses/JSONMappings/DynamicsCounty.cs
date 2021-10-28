// <copyright file="DynamicsCounty.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// JSON mapping of a county.
    /// </summary>
    public class DynamicsCounty : ICounty
  {
        /// <inheritdoc/>
        [JsonProperty("ptas_countyid")]
        public Guid CountyId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }
  }
}
