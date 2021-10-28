// <copyright file="DynamicsMedicarePlan.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// JSON mapping of a DynamicsMedicarePlan.
    /// </summary>
    public class DynamicsMedicarePlan : IMedicarePlan
  {
        /// <inheritdoc/>
        [JsonProperty("ptas_medicareplanid")]
        public string Id { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_organizationname")]
        public string OrganizationName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_approved")]
        public bool Approved { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_yearid_value")]
        public string YearId { get; set; }
    }
}
