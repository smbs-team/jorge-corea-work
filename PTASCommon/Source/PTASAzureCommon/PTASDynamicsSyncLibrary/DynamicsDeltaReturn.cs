// <copyright file="DynamicsDeltaReturn.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsSyncLibrary
{
    using D2SSyncHelpers.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// Result from calls to delta.
    /// </summary>
    public class DynamicsDeltaReturn : DynamicsWrapper<dynamic>
    {
        /// <summary>
        /// Gets or sets returned delta link.
        /// </summary>
        [JsonProperty("@odata.deltaLink")]
        public string DeltaLink { get; set; }

        /// <summary>
        /// Gets or sets returned delta link.
        /// </summary>
        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }
    }
}