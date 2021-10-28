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

    public class MyConfig : IConfigurationRoot
    {
        private readonly IConfigurationRoot baseObject;
        private readonly Dictionary<string, string> options;
        public MyConfig(IConfigurationRoot baseObject, Options options)
        {
            this.options = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(options));
            this.baseObject = baseObject;
        }

        public IEnumerable<IConfigurationProvider> Providers => this.baseObject.Providers;

        public string this[string key]
        {
            get
            {
                if (this.options.TryGetValue(key, out string result))
                {
                    return result;
                }

                return this.baseObject[key];
            }
            set
            {
                this.baseObject[key] = value;
            }
        }
        public IEnumerable<IConfigurationSection> GetChildren() => this.baseObject.GetChildren();

        public IChangeToken GetReloadToken() => this.baseObject.GetReloadToken();

        public IConfigurationSection GetSection(string key)
        {
            return new MyConnectionStringSection(this.baseObject.GetSection(key), this.options["ConnectionString"]);
        }

        public void Reload() => this.baseObject.Reload();
    }
}
