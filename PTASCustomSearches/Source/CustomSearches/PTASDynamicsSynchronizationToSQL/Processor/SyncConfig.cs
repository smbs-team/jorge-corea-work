// <copyright file="SyncConfig.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsSynchronizationToSQL.Processor
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Sync configuration dictionary.
    /// </summary>
    internal class SyncConfig : IConfiguration
    {
        private readonly Dictionary<string, string> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConfig"/> class.
        /// </summary>
        /// <param name="values">Value to initialize.</param>
        public SyncConfig(Dictionary<string, string> values)
        {
            this.values = values;
        }

        /// <inheritdoc/>
        public string this[string key]
        {
            get => this.values.TryGetValue(key, out string value) ? value : key;
            set => this.values[key] = value;
        }

        /// <inheritdoc/>
        public IEnumerable<IConfigurationSection> GetChildren() => new IConfigurationSection[] { };

        /// <inheritdoc/>
        public IChangeToken GetReloadToken() => null;

        /// <inheritdoc/>
        public IConfigurationSection GetSection(string key) => null;
    }
}