// <copyright file="Accessory.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using Newtonsoft.Json;

    /// <summary>Accessory entity.</summary>
    public class Accessory
    {
        /// <summary>Gets or sets the type of the residential.</summary>
        /// <value>The type of the residential.</value>
        [JsonProperty("ptas_resaccessorytype")]
        public int? ResidentialType { get; set; }

        /// <summary>Gets or sets the type of the commercial.</summary>
        /// <value>The type of the commercial.</value>
        [JsonProperty("ptas_commaccessorytype")]
        public int? CommercialType { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty("ptas_accessorydetailid")]
        public string Id { get; set; }
    }
}
