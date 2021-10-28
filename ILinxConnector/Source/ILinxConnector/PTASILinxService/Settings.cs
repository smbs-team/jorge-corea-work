// <copyright file="Settings.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    using IdentityModel;

    using Serilog;

    /// <summary>
    /// Represents all application settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets allowedOrigins.
        /// </summary>
        public static string AllowedOrigins => Get(Constants.Settings.AllowedOrigins);

        /// <summary>
        /// Get value from appSettings section of web.config and expand environmemt variables.
        /// </summary>
        /// <param name="name">Key name.</param>
        /// <returns>Env value.</returns>
        public static string Get(string name) => Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings[name] ?? string.Empty);

        /// <summary>
        /// Get Connection String.
        /// </summary>
        /// <param name="name">Name of connection.</param>
        /// <returns>Connection string.</returns>
        public static string GetConnectionString(string name) => Environment.ExpandEnvironmentVariables(ConfigurationManager.ConnectionStrings[name]?.ConnectionString ?? string.Empty);

        /// <summary>
        /// Static class for authentication.
        /// </summary>
        public static class Auth
        {
            private static X509Certificate2 signingCertificate;

            /// <summary>
            /// Gets audience.
            /// </summary>
            public static string Audience => Get(Constants.Settings.Auth.Audience);

            /// <summary>
            /// Gets issuer.
            /// </summary>
            public static string Issuer => Get(Constants.Settings.Auth.Issuer);

            /// <summary>
            /// Gets issuer Certificate.
            /// </summary>
            public static X509Certificate2 IssuerCertificate
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(IssuerCertThumbprint))
                    {
                        Log.Error($"Using Settings.Auth.IssuerCertificate before setting up a '{Constants.Settings.Auth.CertThumbprint}' value in the web.config");
                        throw new Exception($"Your have to set up a '{Constants.Settings.Auth.CertThumbprint}' value in the web.config before using Settings.Auth.IssuerCertificate");
                    }

                    if (signingCertificate != null)
                    {
                        return signingCertificate;
                    }

                    signingCertificate = X509.LocalMachine.My.Thumbprint.Find(IssuerCertThumbprint).FirstOrDefault();
                    if (signingCertificate == null)
                    {
                        Log.Error("Can't find certificate with a thumbpring '{cert}'", IssuerCertThumbprint);
                        throw new Exception($"Can't find certificate with a thumbpring '{IssuerCertThumbprint}'");
                    }

                    return signingCertificate;
                }
            }

            /// <summary>
            /// Gets issuer Thumbprint.
            /// </summary>
            public static string IssuerCertThumbprint => Get(Constants.Settings.Auth.CertThumbprint);
        }
    }
}