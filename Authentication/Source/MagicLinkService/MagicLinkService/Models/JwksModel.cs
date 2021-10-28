//-----------------------------------------------------------------------
// <copyright file="JwksModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    /// <summary>
    /// JWKS Model.
    /// </summary>
    public class JwksModel
    {
        /// <summary>
        /// Gets or sets the keys collection.
        /// </summary>
        [JsonProperty("keys")]
        public ICollection<JwksKeyModel> Keys { get; set; }
    }
}
