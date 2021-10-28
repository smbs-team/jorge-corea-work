//-----------------------------------------------------------------------
// <copyright file="OidcModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The OIDC Model.
    /// </summary>
    public class OidcModel
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the JWKS URI.
        /// </summary>
        [JsonProperty("jwks_uri")]
        public string JwksUri { get; set; }

        /// <summary>
        /// Gets or sets the supported values collection.
        /// </summary>
        [JsonProperty("id_token_signing_alg_values_supported")]
        public ICollection<string> IdTokenSigningAlgValuesSupported { get; set; }
    }
}
