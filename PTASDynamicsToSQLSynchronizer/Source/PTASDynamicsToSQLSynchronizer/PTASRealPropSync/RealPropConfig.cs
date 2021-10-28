// <copyright file="DeltaConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASRealPropSync
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Configuration to be used by delta loader.
    /// Masks environment variables as a configuration to be compatible.
    /// </summary>
    internal class RealPropConfig : IConfiguration
    {
        private string x = "\"";

        private readonly Dictionary<string, string> values = new Dictionary<string, string>()
        {
            {
                "connectionString",

                "sqlconnectionstring"
            },

            { "organizationId", "organizationId" },

            { "organizationName", "organizationName" },
            { "DynamicsURL", "dynamicsurl" },
            { "AuthUri", "authuri" },
            { "ClientId", "clientid" },
            { "ClientSecret", "clientsecret" },
        };

        /// <inheritdoc/>
        public string this[string key]
        {
            get => Environment.GetEnvironmentVariable(this.MapKey(key));

            set => Environment.SetEnvironmentVariable(this.MapKey(key), value);
        }

        /// <inheritdoc/>
        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => new IConfigurationSection[] { };

        /// <inheritdoc/>
        IChangeToken IConfiguration.GetReloadToken() => null;

        /// <inheritdoc/>
        IConfigurationSection IConfiguration.GetSection(string key) => null;

        private string MapKey(string key) =>
                                    this.values.TryGetValue(key, out string value) ? value : key;
    }
}