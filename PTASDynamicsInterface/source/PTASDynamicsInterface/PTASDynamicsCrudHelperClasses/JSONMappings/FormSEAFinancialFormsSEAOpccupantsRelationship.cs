// <copyright file="FormSEAFinancialFormsSEAOpccupantsRelationship.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;

    /// <summary>
    /// SEAFinancialFormsSEAOpccupantsRelationship to be read/updated from API.
    /// </summary>
    public class FormSEAFinancialFormsSEAOpccupantsRelationship
    {
        /// <summary>
        /// Gets or sets the seappoccupantId to be related with a SEAFinancialFormsid.
        /// </summary>
        [JsonProperty("seappoccupantid")]
        public string SEAppOccupantId { get; set; }
    }
}
