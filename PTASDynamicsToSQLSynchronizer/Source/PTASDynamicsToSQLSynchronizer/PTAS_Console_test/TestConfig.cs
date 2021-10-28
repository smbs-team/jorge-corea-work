using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace PTAS_Console_test
{
    internal class TestConfig : IConfiguration
    {
        private readonly Dictionary<string, string> values = new Dictionary<string, string>() {
            {
                "connectionString",
                "Initial Catalog=deltatests;Data Source=(local);Integrated Security=True"
                //"Server=tcp:ptas-sbox-dbserver.database.windows.net,1433;User ID=dbadmin;Password=KUTcFrGRYJwS01;Initial Catalog=ptas-sbox-developmentdb;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            },
            { "organizationId", "8dcf1c09-56fb-4cdd-af75-54b14b04349b"},
            {"organizationName", "org7ff68ed4" },
            {"DynamicsURL", "https://ptas-dev.crm9.dynamics.com" },
            {"AuthUri","https://login.windows.net/KC1.onmicrosoft.com" },
            { "ClientId", "404b9b8b-5315-4ef3-859d-37352a12b2da" },
            { "ClientSecret", "LgMtPZBof8RUhZO/Qa0T1r1gR2pKd/sjnOx3wni5Y4I=" },
        };

        public string this[string key]
        {
            get => values.TryGetValue(key, out string value) ? value : key;
            set => values[key] = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren() => new IConfigurationSection[] { };

        public IChangeToken GetReloadToken() => null;

        public IConfigurationSection GetSection(string key) => null;
    }
}