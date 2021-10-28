// <copyright file="EntitysetRequestsAndSketch.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Request entities and related sketch.
    /// </summary>
    public class EntitysetRequestsAndSketch : EntitysetRequests
    {
        /// <summary>
        /// Gets or sets id of the sketch to fetch.
        /// </summary>
        public string SketchId { get; set; }
    }
}