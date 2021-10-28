// <copyright file="DBTablesService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Get tables from the database.
    /// </summary>
    public class DBTablesService : IDBTablesService
    {
        private const string GetFields = @"
            SELECT        COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE,
                                     DATETIME_PRECISION
            FROM            INFORMATION_SCHEMA.COLUMNS
            WHERE        (TABLE_SCHEMA = 'dynamics') AND (TABLE_NAME = '{0}')
            ";

        private const string GetSchema = @"
            SELECT        schema_name(schema_id) AS schema_name, name AS table_name, create_date, modify_date
            FROM            sys.tables AS t
            WHERE        (schema_name(schema_id) = 'dynamics')
            ORDER BY table_name
            ";

        private readonly IDataAccessLibrary dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBTablesService"/> class.
        /// </summary>
        /// <param name="dataAccess">Data access object.</param>
        public DBTablesService(IDataAccessLibrary dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DBTable>> GetTables()
        {
            List<DBTable> tables = await this.dataAccess.LoadData<DBTable, object>(GetSchema, null);
            Task<IEnumerable<DBField>>[] fieldLoaders = tables.Select(t => this.GetFieldsForTable(t.Name)).ToArray();
            var r = (await Task.WhenAll(fieldLoaders)).ToArray();
            for (int i = 0; i < r.Length; i++)
            {
                tables[i].Fields = r[i];
            }

            return tables;
        }

        /// <summary>
        /// Create a table.
        /// </summary>
        /// <param name="table">Table to create.</param>
        /// <returns>Empty task.</returns>
        public async Task CreateTable(DBTable table)
        {
            await this.dataAccess.SaveData<object>(SQLGenerator.CreateScript(table), null);
        }

        private async Task<IEnumerable<DBField>> GetFieldsForTable(string tableName)
        {
            string query = string.Format(GetFields, tableName);
            return await this.dataAccess.LoadData<DBField, object>(query, null);
        }
    }
}