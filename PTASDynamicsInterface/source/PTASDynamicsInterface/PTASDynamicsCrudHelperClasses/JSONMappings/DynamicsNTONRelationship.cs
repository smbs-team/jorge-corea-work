// <copyright file="DynamicsNTONRelationship.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;

    /// <summary>
    /// SEAFinancialFormsSEAOpccupantsRelationship to be read/updated from API.
    /// </summary>
    public class DynamicsNTONRelationship
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsNTONRelationship"/> class.
        /// </summary>
        /// <param name="v">v is the odata refid to other entity.</param>
        public DynamicsNTONRelationship(string v)
        {
            this.TheRefId = v;
        }

        /// <summary>
        /// Gets or sets the seappoccupantId to be related with a SEAFinancialFormsid.
        /// </summary>
        [JsonProperty("@odata.id")]
        public string TheRefId { get; set; }
    }
}