using System;

namespace PTASDynamicsTranfer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CommandLine;

    using D2SSyncHelpers.Models;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;

    using Newtonsoft.Json;

    public class MyConnectionStringSection : IConfigurationSection
    {
        private readonly IConfigurationSection configurationSection;
        private readonly string connectionString;
        public MyConnectionStringSection(IConfigurationSection configurationSection, string connectionString)
        {
            this.connectionString = connectionString;
            this.configurationSection = configurationSection;
        }

        public string Key => this.configurationSection.Key;

        public string Path => this.configurationSection.Path;

        public string Value { get => this.configurationSection.Value; set => this.configurationSection.Value = value; }

        public string this[string key]
        {
            get
            {
                return key == "default" ? this.connectionString : this.configurationSection[key];
            }

            set => this.configurationSection[key] = value;
        }
        /// <inheritdoc/>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return this.configurationSection.GetChildren();
        }

        public IChangeToken GetReloadToken() => this.configurationSection.GetReloadToken();

        public IConfigurationSection GetSection(string key)
        {
            return this.configurationSection.GetSection(key);
        }
    }
}
