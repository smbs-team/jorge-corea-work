// <copyright file="DynamicsSEAppNote.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application Note info returned by dynamics.
    /// </summary>
    public class DynamicsSEAppNote : ISEAppNote
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seappnoteid")]
        public string SEAppNoteId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seapplicationid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string SEApplicationId { get; set; }

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
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seappdetailid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string SEAppDetailId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_showonportal")]
        public bool? ShowOnPortal { get; set; }

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