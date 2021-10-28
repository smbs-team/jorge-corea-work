//-----------------------------------------------------------------------
// <copyright file="JwksKeyModel.cs" company="King County">
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
    /// The JWKS key model.
    /// </summary>
    public class JwksKeyModel
    {
        /// <summary>
        /// Gets or sets the kid.
        /// </summary>
        [JsonProperty("kid")]
        public string Kid { get; set; }

        /// <summary>
        /// Gets or sets the NBF.
        /// </summary>
        [JsonProperty("nbf")]
        public long Nbf { get; set; }

        /// <summary>
        /// Gets or sets the use property.
        /// </summary>
        [JsonProperty("use")]
        public string Use { get; set; }

        /// <summary>
        /// Gets or sets the kty.
        /// </summary>
        [JsonProperty("kty")]
        public string Kty { get; set; }

        /// <summary>
        /// Gets or sets the alg.
        /// </summary>
        [JsonProperty("alg")]
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the x5 c.
        /// </summary>
        [JsonProperty("x5c")]
        public ICollection<string> X5C { get; set; }

        /// <summary>
        /// Gets or sets the x5 t.
        /// </summary>
        [JsonProperty("x5t")]
        public string X5T { get; set; }

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        [JsonProperty("n")]
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the e.
        /// </summary>
        [JsonProperty("e")]
        public string E { get; set; }

        /// <summary>
        /// Creates a model from the signing credentials.
        /// </summary>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <returns>The newly created model.</returns>
        /// <exception cref="Exception">Certificate is not an RSA certificate.</exception>
        public static JwksKeyModel FromSigningCredentials(X509SigningCredentials signingCredentials)
        {
            X509Certificate2 certificate = signingCredentials.Certificate;

            // JWK cert data must be base64 (not base64url) encoded
            string certData = Convert.ToBase64String(certificate.Export(X509ContentType.Cert));

            // JWK thumbprints must be base64url encoded (no padding or special chars)
            string thumbprint = Base64UrlEncoder.Encode(certificate.GetCertHash());

            // JWK must have the modulus and exponent explicitly defined
            RSACng rsa = certificate.PublicKey.Key as RSACng;

            if (rsa == null)
            {
                throw new Exception("Certificate is not an RSA certificate.");
            }

            RSAParameters keyParams = rsa.ExportParameters(false);
            string keyModulus = Base64UrlEncoder.Encode(keyParams.Modulus);
            string keyExponent = Base64UrlEncoder.Encode(keyParams.Exponent);

            return new JwksKeyModel
            {
                Kid = signingCredentials.Kid,
                Kty = "RSA",
                Nbf = new DateTimeOffset(certificate.NotBefore).ToUnixTimeSeconds(),
                Use = "sig",
                Alg = signingCredentials.Algorithm,
                X5C = new[] { certData },
                X5T = thumbprint,
                N = keyModulus,
                E = keyExponent
            };
        }
    }
}
