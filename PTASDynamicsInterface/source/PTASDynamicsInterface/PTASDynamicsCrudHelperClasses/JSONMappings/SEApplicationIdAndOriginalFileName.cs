// <copyright file="SEApplicationIdOnly.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// TO load the ids that have to be update when finalizing.
    /// </summary>
    public class SEApplicationIdAndOriginalFileName
    {
        /// <summary>
        /// Gets or sets id fetched from the item.
        /// </summary>
        [JsonProperty("ptas_fileattachmentmetadataid")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets original File Name.
        /// </summary>
        [JsonProperty("ptas_originalfilename")]
        public string OriginalFileName { get; set; }
    }
}
