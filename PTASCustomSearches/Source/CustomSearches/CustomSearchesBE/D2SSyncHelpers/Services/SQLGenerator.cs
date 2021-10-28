// <copyright file="SQLGenerator.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using D2SSyncHelpers.Models;

    /// <summary>
    /// Helper class to generate sql.
    /// </summary>
    public static class SQLGenerator
    {
        /// <summary>
        /// Creation script.
        /// </summary>
        /// <param name="table">Table to create.</param>
        /// <param name="isSnapTable">Flag for table snapshot.</param>
        /// <returns>SQL Script.</returns>
        public static string CreateScript(this DBTable table, bool isSnapTable)
        {
            var fields = table.Fields.ToList();
            if (!fields.Any())
            {
                return string.Empty;
            }

            var keyField = fields.First();
            var result = fields.Select((a, idx) =>
            {
                var isNullable = idx > 0 ? string.Empty : " NOT ";
                if (idx == 0 && a.DataType.Contains("(max)"))
                {
                    a.DataType = a.DataType.Replace("max", "100");
                }

                return $"[{a.Name}] {a.DataType} {isNullable} NULL";
            });
            var fieldsStr = string.Join(",\n", result);
            bool hasLongTextFields = table.Fields?.Any(f => f.DataType.Contains("(max)")) ?? false;
            string textImageStr = hasLongTextFields ? " TEXTIMAGE_ON [PRIMARY] " : string.Empty;
            string tableNameSnap;
            if (isSnapTable)
            {
                tableNameSnap = table.Name + "_snapshot";
            }
            else
            {
                tableNameSnap = table.Name;
            }

            var resultStr = @$"
                CREATE TABLE [dynamics].[{tableNameSnap}](
                {fieldsStr},
                 CONSTRAINT [PK_{tableNameSnap}] PRIMARY KEY CLUSTERED ([{keyField.Name}] ASC)
                    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] {textImageStr}
            ";
            return resultStr;
        }

        /// <summary>
        /// All creation scripts for tables.
        /// </summary>
        /// <param name="tables">Tables to process.</param>
        /// <returns>Script.</returns>
        public static string CreateScripts(IEnumerable<DBTable> tables)
            => tables.Aggregate(string.Empty, (a, b) => a + "\n" + b.CreateScript(false));

        /// <summary>
        /// Generate script to add table.
        /// </summary>
        /// <param name="tableName">Table to create.</param>
        /// <param name="newFields">Fields to add.</param>
        /// <returns>Script.</returns>
        public static string AddFieldsScript(string tableName, IEnumerable<DBField> newFields)
        {
            List<string> snapEntities = new List<string>() { "ptas_accessorydetail", "ptas_buildingdetail", "ptas_buildingdetail_commercialuse", "ptas_buildingsectionfeature", "ptas_condocomplex", "ptas_condounit", "ptas_land", "ptas_landvaluebreakdown", "ptas_landvaluecalculation", "ptas_lowincomehousingprogram", "ptas_parceldetail", "ptas_projectdock", "ptas_sectionusesqft", "ptas_taxaccount", "ptas_unitbreakdown" };
            string query = string.Empty;

            if (snapEntities.Contains(tableName.ToLower()))
            {
                var yy = string.Join(",\n    ", newFields.Select(f => $"{f.Name} {f.DataType} NULL"));
                query = $"ALTER TABLE dynamics.{tableName} ADD \n{yy};";
                string tableNameSnap = tableName.ToLower() + "_snapshot";
                query += $"\nALTER TABLE dynamics.{tableNameSnap} ADD \n{yy}";
                return query;
            }
            else
            {
                var yy = string.Join(",\n    ", newFields.Select(f => $"{f.Name} {f.DataType} NULL"));
                return $"ALTER TABLE dynamics.{tableName} ADD \n{yy}";
            }
        }
    }
}