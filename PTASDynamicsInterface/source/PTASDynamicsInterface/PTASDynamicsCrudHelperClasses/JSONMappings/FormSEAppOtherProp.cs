// <copyright file="FormSEAppOtherProp.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// SEAppOccupant to be read from API.
    /// </summary>
    public class FormSEAppOtherProp : FormInput, ISEAppOtherProp
    {
        /// <inheritdoc/>
        [JsonProperty("seappotherpropid")]
        public string SEAppOtherPropId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("seapplicationid")]
        public string SEApplicationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <inheritdoc/>
        [JsonProperty("purpose")]
        public int? Purpose { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdon")]
        public DateTime? CreatedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdby")]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("countyname", NullValueHandling = NullValueHandling.Ignore)]
        public string CountyName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statename", NullValueHandling = NullValueHandling.Ignore)]
        public string StateName { get; set; }

        /// <summary>
        /// Sets the id for a new object.
        /// </summary>
        public override void SetId()
        {
            if (string.IsNullOrEmpty(this.SEAppOtherPropId))
            {
                this.SEAppOtherPropId = Guid.NewGuid().ToString();
            }
        }
    }
}
