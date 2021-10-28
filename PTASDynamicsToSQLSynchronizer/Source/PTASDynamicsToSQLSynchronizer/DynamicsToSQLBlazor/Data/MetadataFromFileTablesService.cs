// <copyright file="MetadataFromFileTablesService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Load metadata from dynamics.
    /// </summary>
    public class MetadataFromFileTablesService : IDBTablesService
    {
        private const string ErrStr = "ERR";

        private static readonly Dictionary<string, string> DataTypeMapping = new Dictionary<string, string>
        {
            { "Edm.Boolean", "bit" },
            { "Edm.Date", "datetime" },
            { "Edm.DateTimeOffset", "datetimeoffset(7)" },
            { "Edm.Decimal", "money" },
            { "Edm.Double", "float" },
            { "Edm.Guid", "uniqueidentifier" },
            { "Edm.Int32", "int" },
            { "Edm.Int64", "bigint" },
            { "Edm.String", "nvarchar(max)" },
            { "Edm.Binary", "varbinary(max)" },
            { "mscrm.BooleanManagedProperty", "bit" },
            { ErrStr, "Err" },
        };

        private readonly DynamicsMetadataReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataFromFileTablesService"/> class.
        /// </summary>
        /// <param name="reader">Metadata reader we're using.</param>
        public MetadataFromFileTablesService(DynamicsMetadataReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Gets the table list from dynamics metadata.
        /// </summary>
        /// <returns>List of tables.</returns>
        public async Task<IEnumerable<DBTable>> GetTables()
        {
            var content = await this.reader.LoadMetadataAsync();
            var doc = XDocument.Parse(content);
            var enumerable = from c in doc.Root.Descendants()
                             where c.Name.LocalName.Contains("Schema")
                             select c;
            var schemaNode = doc.Root.Descendants().FirstOrDefault(c => c.Name.LocalName.Contains("Schema"));
            var entities = schemaNode
                .Descendants($"{{{schemaNode.Name.Namespace}}}EntityType")
                .Where(node => !node.Attribute("Name").Value.EndsWith("Metadata"));
            var xyx = entities.Select(e =>
            {
                var keyField = e.Descendants($"{{{schemaNode.Name.Namespace}}}Key").FirstOrDefault()?.Elements().FirstOrDefault()?.Attribute("Name")?.Value;
                if (keyField == null)
                {
                    return null;
                }

                return new DBTable
                {
                    Name = e.Attribute("Name").Value,
                    Fields = e
                        .Descendants($"{{{schemaNode.Name.Namespace}}}Property")
                        .Select(e => ExtractField(e))
                        .OrderBy(itm => itm.Name.Equals(keyField) ? "_" : itm.Name.StartsWith("_") ? "ZZ" + itm.Name : itm.Name)
                        .ToArray(),
                };
            })
                .Where(e => e != null)
                .OrderBy(e => e.Name).ToArray();
            return await Task.FromResult(xyx);
        }

        private static DBField ExtractField(XElement e)
        {
            string datatype = e.Attribute("Type")?.Value ?? ErrStr;
            if (!DataTypeMapping.TryGetValue(datatype, out string dstDatatype))
            {
                dstDatatype = "ERR";
            }

            return new DBField
            {
                Name = e.Attribute("Name")?.Value ?? e.ToString(),
                DataType = dstDatatype,
            };
        }
    }
}