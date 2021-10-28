//-----------------------------------------------------------------------
// <copyright file="OidcController.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Controllers
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using PTAS.MagicLinkService.Models;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// The OIDC Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class OidcController : Controller
    {
        /// <summary>
        /// The signing credentials.
        /// </summary>
        private static Lazy<X509SigningCredentials> signingCredentials;

        /// <summary>
        /// The application settings.
        /// </summary>
        private readonly AppSettingsModel appSettings;

        /// <summary>
        /// The hosting environment.
        /// </summary>
        private readonly IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OidcController"/> class.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public OidcController(IOptions<AppSettingsModel> appSettings, IHostingEnvironment hostingEnvironment)
        {
            this.appSettings = appSettings.Value;
            this.hostingEnvironment = hostingEnvironment;

            // Sample: Load the certificate with a private key (must be pfx file)
            signingCredentials = new Lazy<X509SigningCredentials>(() =>
            {
                X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                                            X509FindType.FindByThumbprint,
                                            this.appSettings.SigningCertThumbprint,
                                            false);

                // Get the first cert with the thumbprint
                if (certCollection.Count > 0)
                {
                    return new X509SigningCredentials(certCollection[0]);
                }

                throw new Exception("Certificate not found");
            });
        }

        /// <summary>
        /// Metadata endpoint for the service.
        /// </summary>
        /// <returns>The metadata of the open id endpoint.</returns>
        [Route(".well-known/openid-configuration", Name = "OIDCMetadata")]
        [HttpGet]
        public ActionResult Metadata()
        {
            return this.Content(
                JsonConvert.SerializeObject(new OidcModel
                {
                    // Sample: The issuer name is the application root path
                    Issuer = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase.Value}/",

                    // Sample: Include the absolute URL to JWKs endpoint
                    JwksUri = this.Url.Link("JWKS", null),

                    // Sample: Include the supported signing algorithms
                    IdTokenSigningAlgValuesSupported = new[] { OidcController.signingCredentials.Value.Algorithm },
                }), "application/json");
        }

        /// <summary>
        /// Creates a serialized JWKSs document.
        /// </summary>
        /// <returns>The serialized document.</returns>
        [Route(".well-known/keys", Name = "JWKS")]
        [HttpGet]
        public ActionResult JwksDocument()
        {
            return this.Content(
                JsonConvert.SerializeObject(new JwksModel
                {
                    Keys = new[] { JwksKeyModel.FromSigningCredentials(OidcController.signingCredentials.Value) }
                }), "application/json");
        }
    }
}