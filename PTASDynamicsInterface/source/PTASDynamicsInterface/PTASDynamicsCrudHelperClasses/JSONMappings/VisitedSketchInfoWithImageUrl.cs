// <copyright file="VisitedSketchInfoWithImageUrl.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Output sketch info.
    /// </summary>
    public class VisitedSketchInfoWithImageUrl
    {
        /// <summary>
        /// Gets or sets a value indicating whether edited.
        /// </summary>
        [JsonProperty("ptas_edited")]
        public bool Ptas_edited { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [JsonProperty("ptas_name")]
        public string Ptas_name { get; set; }

        /// <summary>
        /// Gets or sets sketch Id.
        /// </summary>
        [JsonProperty("_ptas_sketchid_value")]
        public string Ptas_sketchid { get; set; }

        /// <summary>
        /// Gets or sets date visited.
        /// </summary>
        [JsonProperty("ptas_visiteddate")]
        public DateTime Ptas_visiteddate { get; set; }

        /// <summary>
        /// Gets or sets id of visited sketch.
        /// </summary>
        [JsonProperty("ptas_visitedsketchid")]
        public string Ptas_visitedsketchid { get; set; }

        /// <summary>
        /// Gets url of the associated image.
        /// </summary>
        public string ImageURL { get; }
    }
}