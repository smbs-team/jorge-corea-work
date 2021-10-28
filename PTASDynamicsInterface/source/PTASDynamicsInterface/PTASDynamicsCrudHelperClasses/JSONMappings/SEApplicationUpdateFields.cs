// <copyright file="SEApplicationUpdateFields.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Class to represent the json fields to be updated.
    /// </summary>
    public class SEApplicationUpdateFields
    {
        /// <summary>
        /// Gets or sets a value indicating whether it is blob?.
        /// </summary>
        [JsonProperty("ptas_isblob")]
        public bool IsBlob { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is islinx?.
        /// </summary>
        [JsonProperty("ptas_isilinx")]
        public bool IsIlinx { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether it is sharepoint?.
        /// </summary>
        [JsonProperty("ptas_issharepoint")]
        public bool IsSharepoint { get; set; } = false;

        /// <summary>
        /// Gets or sets the documentid fetched from the item.
        /// </summary>
        [JsonProperty("ptas_icsdocumentid")]
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets redaction Url.
        /// </summary>
        [JsonProperty("ptas_redactionurl")]
        public object RedactionUrl { get; set; }

        /// <summary>
        /// Gets or sets sharepoint Url.
        /// </summary>
        [JsonProperty("ptas_sharepointurl")]
        public object SharepointUrl { get; set; }
    }
}
