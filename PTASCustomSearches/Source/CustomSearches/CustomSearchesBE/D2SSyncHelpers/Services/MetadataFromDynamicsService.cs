// <copyright file="MetadataFromDynamicsService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using D2SSyncHelpers.Exceptions;
    using D2SSyncHelpers.Interfaces;
    using D2SSyncHelpers.Models;

    /// <summary>
    /// Load metadata from dynamics.
    /// </summary>
    public class MetadataFromDynamicsService : IDBTablesService
    {
        /// <summary>
        /// Error marker string.
        /// </summary>
        public const string ErrStr = "ERR";

        /// <summary>
        /// Mapping for datatypes.
        /// </summary>
        public static readonly Dictionary<string, string> DataTypeMapping = new Dictionary<string, string>
        {
            { "Edm.Boolean", DataTypes.Bit },
            { "Edm.Date", DataTypes.Datetime },
            { "Edm.DateTimeOffset", DataTypes.DtOffset },
            { "Edm.Decimal", DataTypes.Money },
            { "Edm.Double", DataTypes.Float },
            { "Edm.Guid", DataTypes.UniqueIdentifier },
            { "Edm.Int32", DataTypes.Int },
            { "Edm.Int64", DataTypes.BigInt },
            { "Edm.String", DataTypes.Nvarchar },
            { "Edm.Binary", DataTypes.VarBinary },
            { "mscrm.BooleanManagedProperty", DataTypes.Bit },
            { ErrStr, "Err" },
        };

        private readonly IAsyncStringReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataFromDynamicsService"/> class.
        /// </summary>
        /// <param name="reader">Metadata reader we're using.</param>
        public MetadataFromDynamicsService(IAsyncStringReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <inheritdoc/>
        public Task<DBTable> GetTable(string tableName)
        {
            // functionality not needed.
            return null;
        }

        /// <summary>
        /// Gets the table list from dynamics metadata.
        /// </summary>
        /// <returns>List of tables.</returns>
        public async Task<IEnumerable<DBTable>> GetTables()
        {
            var content = await this.reader.GetContentAsync();
            var doc = XDocument.Parse(content);
            var enumerable = from c in doc.Root.Descendants()
                             where c.Name.LocalName.Contains("Schema")
                             select c;
            var schemaNode = doc.Root.Descendants().FirstOrDefault(c => c.Name.LocalName.Contains("Schema")) ?? throw new DynamicsXMLException("Schema Expected.", null);
            var entities = schemaNode
                .Descendants($"{{{schemaNode.Name.Namespace}}}EntityType")
                .Where(node => !node.Attribute("Name").Value.EndsWith("Metadata")).ToList();
            var xyx = entities.Select(e =>
            {
                var keyField = e.Descendants($"{{{schemaNode.Name.Namespace}}}Key").FirstOrDefault()?.Elements().FirstOrDefault()?.Attribute("Name")?.Value;
                var itemName = e.Attribute("Name").Value;
                if (keyField == null)
                {
                    return null;
                }

                var descendantFields = e.Descendants($"{{{schemaNode.Name.Namespace}}}NavigationProperty").ToList().Select(nf =>
                {
                    var c = nf.Descendants().FirstOrDefault();
                    return c == null
                        ? null
                        : new TempInfo
                        {
                            Partner = nf.Attribute("Partner").Value,
                            Name = c?.Attribute("Property").Value,
                            Referenced = c?.Attribute("ReferencedProperty").Value,
                        };
                }).Where(itm => itm?.Name != null)
                .Distinct(comparer: new TempInfoComparer())
                .ToDictionary(k => k.Name, k => $"{k.Name}");
                DBField[] dBFields = e
                                        .Descendants($"{{{schemaNode.Name.Namespace}}}Property")
                                        .Select(e => ExtractField(e, descendantFields))
                                        .ToArray();
                if (!dBFields.Any(itm => itm.Name.Equals(keyField)))
                {
                    keyField = itemName + "id";
                }

                DBField[] orderedFields = dBFields.OrderBy(itm => itm.Name.Equals(keyField) ? "_" : itm.Name.StartsWith("_") ? "ZZ" + itm.Name : itm.Name).ToArray();
                return new DBTable
                {
                    Name = itemName,
                    Fields = orderedFields,
                };
            })
                .Where(e => e != null)
                .OrderBy(e => e.Name).ToArray();
            return await Task.FromResult(xyx);
        }

        private static DBField ExtractField(XElement e, Dictionary<string, string> fields)
        {
            string datatype = e.Attribute("Type")?.Value ?? ErrStr;
            if (!DataTypeMapping.TryGetValue(datatype, out string dstDatatype))
            {
                dstDatatype = "ERR";
            }

            string name = e.Attribute("Name")?.Value ?? e.ToString();
            if (fields.TryGetValue(name, out string newName))
            {
                name = newName;
            }

            return new DBField
            {
                Name = name,
                DataType = dstDatatype,
            };
        }

        private class TempInfo
        {
            public string Name { get; set; }

            public string Partner { get; set; }

            public string Referenced { get; set; }
        }

        private class TempInfoComparer : IEqualityComparer<TempInfo>
        {
            public bool Equals([AllowNull] TempInfo x, [AllowNull] TempInfo y)
            {
                return x?.Name == y?.Name;
            }

            public int GetHashCode([DisallowNull] TempInfo obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}