// <copyright file="DynamicsCountry.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Country to read from dynamics.
    /// </summary>
    public class DynamicsCountry : ICountry
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_countryid")]
        public Guid CountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }
    }
}
