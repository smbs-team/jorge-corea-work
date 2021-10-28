//-----------------------------------------------------------------------
// <copyright file="IdentityController.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using PTAS.MagicLinkService.Models;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Base controller that builds id tokens.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class IdentityControllerBase : ControllerBase
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
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The cloud storage configuration provider.
        /// </summary>
        private ICloudStorageConfigurationProvider cloudStorageConfigurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityControllerBase" /> class.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="logger">The logger.</param>
        public IdentityControllerBase(IOptions<AppSettingsModel> appSettings, IHostingEnvironment hostingEnvironment, ILogger logger)
        {
            try
            {
                this.logger = logger;
                this.appSettings = appSettings.Value;
                this.hostingEnvironment = hostingEnvironment;

                // Load the certificate with a private key (must be pfx file)
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

                // Blob provider configuration
                this.cloudStorageConfigurationProvider =
                    new CloudStorageConfigurationProvider(this.AppSettings.StorageConnectionString);
            }
            catch (Exception ex)
            {
                this.Logger.LogError("Error initializing identity controller: " + ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the signing credentials.
        /// </summary>
        protected static Lazy<X509SigningCredentials> SigningCredentials
        {
            get
            {
                return IdentityControllerBase.signingCredentials;
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        protected AppSettingsModel AppSettings
        {
            get
            {
                return this.appSettings;
            }
        }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        protected IHostingEnvironment HostingEnvironment
        {
            get
            {
                return this.hostingEnvironment;
            }
        }

        /// <summary>
        /// Gets the get table storage provider.
        /// </summary>
        /// <returns>The storage provider.</returns>
        protected ITableStorageProvider GetTableStorageProvider()
        {
            IServiceTokenProvider tokenProvider = new PTASServicesCommon.TokenProvider.AzureTokenProvider();
            ICloudStorageProvider couldStorageProvider =
                new CloudStorageProvider(this.cloudStorageConfigurationProvider, tokenProvider);
            return new TableStorageProvider(couldStorageProvider);
        }

        /// <summary>
        /// Builds the identifier token.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The id token hint.</returns>
        protected string BuildIdToken(string email)
        {
            string issuer = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase.Value}/";

            // All parameters send to Azure AD B2C needs to be sent as claims
            IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("email", email, System.Security.Claims.ClaimValueTypes.String, issuer));
            claims.Add(new System.Security.Claims.Claim("accountEnabled", "true", System.Security.Claims.ClaimValueTypes.Boolean, issuer));

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer,
                    this.appSettings.B2CClientId,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(this.appSettings.LinkExpiresAfterMinutes),
                    IdentityControllerBase.signingCredentials.Value);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }
    }
}
