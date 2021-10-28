// <copyright file="OutgoingCountry.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Year to read from dynamics.
    /// </summary>
    public class OutgoingCountry : ICountry
    {
        /// <inheritdoc/>
        [JsonProperty("countryid")]
        public Guid CountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
