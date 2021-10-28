// <copyright file="DynamicsSEAppPredefNote.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// JSON mapping of a DynamicsSEAppPredefNote.
    /// </summary>
    public class DynamicsSEAppPredefNote : ISEAppPredefNote
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seapppredefnotesid")]
        public string SEAppPredefNotesId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statecode")]
        public int? StateCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statuscode")]
        public int? StatusCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_description")]
        public string Description { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_showonportal")]
        public bool? ShowOnPortal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_portalattachmentlocation")]
        public int? PortalAttachmentLocation { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdon")]
        public DateTime? CreatedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
    }
}