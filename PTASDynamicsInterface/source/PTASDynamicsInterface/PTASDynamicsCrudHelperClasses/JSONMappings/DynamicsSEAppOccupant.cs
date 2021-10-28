// <copyright file="DynamicsSEAppOccupant.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application Occupant info returned by dynamics.
    /// </summary>
    public class DynamicsSEAppOccupant : ISEAppOccupant
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seappoccupantid")]
        public string SEAppOccupantId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seapplicationid_value")]
        public string SEApplicationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupanttype")]
        public int? OccupantType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupantlastname")]
        public string OccupantLastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupantfirstname")]
        public string OccupantFirstName { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupantmiddlename")]
        public string OccupantMiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupantsuffix")]
        public string OccupantSuffix { get; set; }

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
    }
}
