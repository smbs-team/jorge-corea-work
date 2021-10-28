// <copyright file="ReturnedVisitedSketch.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    /// <summary>
    /// Value returned for visited sketch.
    /// </summary>
    public class ReturnedVisitedSketch
    {
        /// <summary>
        /// Gets or sets a value indicating whether edited.
        /// </summary>
        public bool Edited { get; set; }

        /// <summary>
        /// Gets or sets id of visited sketch.
        /// </summary>
        public string SketchId { get; set; }

        /// <summary>
        /// Gets or sets date visited.
        /// </summary>
        public DateTime VisitedDate { get; set; }

        /// <summary>
        /// Gets or sets url of the image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets id of visited sketch.
        /// </summary>
        public string VisitedSketchId { get; set; }

        /// <summary>
        /// Gets or sets related entity Id.
        /// </summary>
        public Guid RelatedEntityId { get; set; }

        /// <summary>
        /// Gets or sets related entity type.
        /// </summary>
        public string RelatedEntityType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is official.
        /// </summary>
        public bool IsOfficial { get; set; }

        /// <summary>
        /// Gets or sets id of the parcel.
        /// </summary>
        public Guid ParcelId { get; set; }

        /// <summary>
        /// Gets or sets address for this sketch info.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets name of the parcel.
        /// </summary>
        public dynamic ParcelName { get; set; }

        /// <summary>
        /// Gets or sets the svg.
        /// </summary>
        public string Svg { get; set; }
    }
}