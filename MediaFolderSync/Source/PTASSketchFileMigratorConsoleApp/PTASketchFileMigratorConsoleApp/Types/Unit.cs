// <copyright file="Unit.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using Newtonsoft.Json;

    /// <summary>Unit class representing a filtered ptas_condounit object.</summary>
    public class Unit
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty("ptas_condounitid")]
        public string Id { get; set; }

        /// <summary>Gets or sets the type of the account.</summary>
        /// <value>The type of the account.</value>
        [JsonProperty("ptas_accounttype")]
        public int? AccountType { get; set; }
    }
}
