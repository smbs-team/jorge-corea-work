// <copyright file="DynamicsSEAppOtherProp.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application Detail info returned by dynamics.
    /// </summary>
    public class DynamicsSEAppOtherProp : ISEAppOtherProp
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seappotherpropid")]
        public string SEAppOtherPropId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seapplicationid_value")]
        public string SEApplicationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_address")]
        public string Address { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_purpose")]
        public int? Purpose { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdon")]
        public DateTime? CreatedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_countyname", NullValueHandling = NullValueHandling.Ignore)]
        public string CountyName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_statename", NullValueHandling = NullValueHandling.Ignore)]
        public string StateName { get; set; }
    }
}
