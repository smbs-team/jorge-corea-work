// <copyright file="DynamicsSystemUser.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;

    /// <summary>
    /// Relevant user info from dynamics.
    /// </summary>
    public class DynamicsSystemUser
    {
        /// <summary>
        /// Gets or sets user Id.
        /// </summary>
        [JsonProperty("systemuserid")]
        public string Systemuserid { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [JsonProperty("lastname")] public string Lastname { get; set; }

        /// <summary>
        ///  Gets or sets first name.
        /// </summary>
        [JsonProperty("firstname")] public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [JsonProperty("internalemailaddress")] public string Internalemailaddress { get; set; }

        /// <summary>
        /// Gets full user name.
        /// </summary>
        [JsonProperty("fullname")] public string FullName => $"{this.Firstname} {this.Lastname}";
    }
}