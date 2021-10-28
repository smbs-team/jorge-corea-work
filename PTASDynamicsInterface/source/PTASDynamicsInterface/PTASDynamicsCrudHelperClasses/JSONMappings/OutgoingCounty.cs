// <copyright file="OutgoingCounty.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// County for output.
    /// </summary>
    public class OutgoingCounty : ICounty
  {
        /// <inheritdoc/>
        [JsonProperty("countyid")]
        public Guid CountyId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("name")]
        public string Name { get; set; }
  }
}
