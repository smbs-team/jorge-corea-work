// <copyright file="SQLGenerator.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Helper class to generate sql.
    /// </summary>
    public static class SQLGenerator
    {
        /// <summary>
        /// Creation script.
        /// </summary>
        /// <param name="table">Table to create.</param>
        /// <returns>SQL Script.</returns>
        public static string CreateScript(this DBTable table)
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
            var resultStr = @$"
                CREATE TABLE [dynamics].[{table.Name}](
                {fieldsStr},
                 CONSTRAINT [PK_{table.Name}] PRIMARY KEY CLUSTERED ([{keyField.Name}] ASC)
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
            => tables.Aggregate(string.Empty, (a, b) => a + "\n" + CreateScript(b));

        /// <summary>
        /// Generate script to add table.
        /// </summary>
        /// <param name="tableName">Table to create.</param>
        /// <param name="newFields">Fields to add.</param>
        /// <returns>Script.</returns>
        public static string AddFieldsScript(string tableName, IEnumerable<DBField> newFields)
        {
            var yy = string.Join(",\n    ", newFields.Select(f => $"{f.Name} {f.DataType} NULL"));
            return $"ALTER TABLE dynamics.{tableName} ADD \n{yy}";
        }
    }
}