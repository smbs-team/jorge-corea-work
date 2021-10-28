// <copyright file="DynamicsContact.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Contact to be coded from Dynamics.
    /// </summary>
    public class DynamicsContact : IContact
    {
        /// <inheritdoc/>
        [JsonProperty("contactid")]
        public string ContactId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("lastname")]
        public string LastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("emailaddress1")]
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("middlename")]
        public string MiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("mobilephone")]
        [Phone]
        public string Phone { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_textsmscapable")]
        public bool? SMSCapable { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_sealternatekey")]
        public int? AlternateKey { get; set; } = 0;

        /// <inheritdoc/>
        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("birthdate")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public System.DateTime? BirthDate { get; set; }
    }
}
