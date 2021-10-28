// <copyright file="DBTablesService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Interfaces;
    using D2SSyncHelpers.Models;

    using Dapper;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Get tables from the database.
    /// </summary>
    public class DBTablesService : IDBTablesService
    {
        private const string GetEntityLastProcessed = @"
            SELECT [tableId] ,[LastDateProcessed], [LastGuidProcessed]
             FROM [dbo].[tableTransfer]
              where TableId='{0}'
            ";

        private const string GetFields = @"
            SELECT        COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE,
                                     DATETIME_PRECISION
            FROM            INFORMATION_SCHEMA.COLUMNS
            WHERE        (TABLE_SCHEMA = 'dynamics') and (TABLE_NAME='{0}')
            ";

        private const string GetOneSchema = @"
            SELECT        schema_name(schema_id) AS schema_name, name AS table_name, create_date, modify_date
            FROM            sys.tables AS t
            WHERE        (schema_name(schema_id) = 'dynamics') and (name='{0}')
            ORDER BY table_name
            ";

        private const string GetSchema = @"
            SELECT        schema_name(schema_id) AS schema_name, name AS table_name, create_date, modify_date
            FROM            sys.tables AS t
            WHERE        (schema_name(schema_id) = 'dynamics')
            ORDER BY table_name
            ";

        private const string SaveErrorQry = @"
            INSERT INTO [dbo].[TransferErrors]
                        ([EntityName]
                        ,[ExecutedQuery]
                        ,[ErrorMessage]
                        ,[ErrorType])
                    VALUES
                        ('{0}','{1}','{2}','{3}')
";

        private const string SetEntityLastInfo = @"
            EXECUTE [dbo].[UpdateLastProcessedInfo]
               '{0}'
              ,'{1}'
              ,'{2}'
            ";

        private readonly IDataAccessLibrary dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBTablesService"/> class.
        /// </summary>
        /// <param name="dataAccess">Data access object.</param>
        public DBTablesService(IDataAccessLibrary dataAccess)
        {
            this.dataAccess = dataAccess ?? throw new System.ArgumentNullException(nameof(dataAccess));
        }

        /// <summary>
        /// Attempt to do a Bulk Copy to Dynamics DB.
        /// </summary>
        /// <param name="connection">The Connection.</param>
        /// <param name="actualValues">list to save.</param>
        /// <param name="destTable">table destination.</param>
        /// <returns>Task.</returns>
        public async Task<SaveError> BulkInsert(SqlConnection connection, JArray actualValues, DBTable destTable)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = (DataTable)JsonConvert.DeserializeObject(actualValues.ToString(), typeof(DataTable));
            }
            catch (JsonException)
            {
                foreach (var i in destTable.Fields.ToList())
                {
                    if (!dataTable.Columns.Contains(i.Name))
                    {
                        dataTable.Columns.Add(i.Name);
                    }
                }

                foreach (JObject item in actualValues)
                {
                    List<(string field, string val)> list = destTable.Fields.Select(f =>
                    {
                        var v = item.Value<object>(f.Name);
                        if (v == null || string.IsNullOrEmpty(v.ToString()))
                        {
                            return (f.Name, null);
                        }

                        var value = f.EncodeBulk(v);
                        return (field: f.Name, val: value.Replace('"', ' ').Trim());
                    }).ToList();

                    DataRow row = dataTable.NewRow();
                    list.OrderBy(c => destTable.Name);
                    List<DBField> dBFields = destTable.Fields.AsList();
                    int fieldCount = list.Count();
                    list
                        .Select((item, index) =>
                            (item.field, value: GetValueFrom(item.val, dBFields[index].DataType)))
                        .Where(itm => itm.value != null)
                        .ToList()
                        .ForEach((itm) => row[itm.field] = itm.value);

                    dataTable.Rows.Add(row);
                }
            }

            dataTable.TableName = destTable.Name;

            List<string> extraOriginCols = this.DistinctColumnsOrigin(dataTable, destTable);
            foreach (string columnName in extraOriginCols)
            {
                dataTable.Columns.Remove(columnName);
            }

            List<string> extraDestinyCols = this.DistinctColumnsDestiny(dataTable, destTable);
            foreach (string columnName in extraDestinyCols)
            {
                if (columnName.Equals("modifiedon"))
                {
                    DataColumn column = new DataColumn(columnName, typeof(DateTime))
                    {
                        DefaultValue = DateTime.UtcNow,
                    };
                    dataTable.Columns.Add(column);
                }
                else
                {
                    dataTable.Columns.Add(columnName, typeof(string));
                }
            }

            // Order Columns
            for (int i = 0; i < destTable.Fields.Cast<DBField>().ToList().Count(); i++)
            {
                dataTable.Columns[destTable.Fields.Cast<DBField>().ToList()[i].Name].SetOrdinal(i);
            }

            // Insert columns that not stay in Odata but Dynamics
            DataTable dtCloned = dataTable.Clone();

            this.ChangeDataType(dtCloned, destTable);

            foreach (DataRow row in dataTable.Rows)
            {
                dtCloned.ImportRow(row);
            }

            // IEnumerable de datos de la tabla
            connection.Open();
            using (SqlBulkCopy bulkInsert = new SqlBulkCopy(connection))
            {
                bulkInsert.DestinationTableName = string.Format("[{0}].[{1}]", "dynamics", destTable.Name);
                try
                {
                    bulkInsert.BulkCopyTimeout = 0;
                    await bulkInsert.WriteToServerAsync(dtCloned);
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in Bulk Copy: " + ex.Message);
                    connection.Close();
                    return new SaveError
                    {
                        HadError = true,
                        ErrorMessage = ex.Message + " " + ex.InnerException?.Message,
                        ErrorType = ex.GetType().ToString(),
                        ExecutedQuery = "Bulk Copy Command.",
                    };
                }
            }

            return new SaveError { HadError = false };
        }

        /// <summary>
        /// Create a table.
        /// </summary>
        /// <param name="table">Table to create.</param>
        /// <returns>Empty task.</returns>
        public async Task CreateTable(DBTable table)
        {
            await this.dataAccess.SaveData<object>(table.CreateScript(), null);
        }

        /// <summary>
        /// Retrieves the last date saved for entity.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <returns>Date.</returns>
        public async Task<TableChange> GetLastSavedInfo(string entityName)
        {
            var l = await this.dataAccess.LoadData<TableChange, object>(string.Format(GetEntityLastProcessed, entityName), null);
            var r = l.FirstOrDefault();
            DateTime lastDate = new DateTime(1960, 1, 1);
            Guid lastGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
            if (r != null)
            {
                lastDate = r.LastDateProcessed;
                lastGuid = r.LastGuidProcessed;
            }

            TableChange lastSavedInfo = new TableChange
            {
                TableId = entityName,
                LastDateProcessed = lastDate,
                LastGuidProcessed = lastGuid,
            };

            return lastSavedInfo;
        }

        /// <inheritdoc/>
        public async Task<DBTable> GetTable(string tableName)
        {
            var query = string.Format(GetOneSchema, tableName);
            List<DBTable> tables = await this.dataAccess.LoadData<DBTable, object>(query, null);
            Task<IEnumerable<DBField>>[] fieldLoaders = tables.Select(t => this.GetFieldsForTable(t.Name)).ToArray();
            var r = (await Task.WhenAll(fieldLoaders)).ToArray();
            for (int i = 0; i < r.Length; i++)
            {
                tables[i].Fields = r[i];
            }

            return tables.FirstOrDefault();
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
        /// Save an error message.
        /// </summary>
        /// <param name="s">Error to save.</param>
        /// <param name="entityName">Entity affected.</param>
        /// <returns>Task.</returns>
        public async Task SaveErrorAsync(SaveError s, string entityName)
        {
            var qry = string.Format(SaveErrorQry, entityName, s.ExecutedQuery.Replace("'", "''"), s.ErrorMessage.Replace("'", "''"), s.ErrorType);
            await this.dataAccess.SaveData<object>(qry, null);
        }

        /// <summary>
        /// Saves last load date for table.
        /// </summary>
        /// <param name="tableName">Name of the entity.</param>
        /// <param name="lastDate">Date of last save.</param>
        /// <param name="lastGuid">Guid of last save.</param>
        /// <returns>Async Task.</returns>
        public async Task SaveLastSavedInfo(string tableName, DateTime lastDate, Guid lastGuid)
        {
            var x = string.Format(SetEntityLastInfo, tableName, lastDate, lastGuid);
            await this.dataAccess.SaveData<object>(x, null);
        }

        /// <summary>
        /// Attempt to save dynamics record to db.
        /// </summary>
        /// <param name="token">value to save.</param>
        /// <param name="destTable">destination table.</param>
        /// <param name="pkFieldName">name of primary key field.</param>
        /// <returns>task result.</returns>
        public async Task<SaveError> SaveRecord(JObject token, DBTable destTable, string pkFieldName)
        {
            List<(string field, string val)> yyy = destTable.Fields.Select(f =>
            {
                var v = token.Value<object>(f.Name);
                if (v == null || string.IsNullOrEmpty(v.ToString()))
                {
                    return (null, null);
                }

                var value = f.Encode(v);
                return (field: f.Name, val: value);
            }).Where(t => t.field != null).ToList();
            ////string updateQuery = this.CreateUpdateQuery(destTable.Name, yyy);
            string insertQuery = this.CreateInsertQuery(destTable.Name, yyy, pkFieldName);
            try
            {
                await this.dataAccess.SaveData<object>(insertQuery, null);
                return new SaveError { HadError = false };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return new SaveError
                {
                    HadError = true,
                    ErrorMessage = ex.Message + " " + ex.InnerException?.Message,
                    ErrorType = ex.GetType().ToString(),
                    ExecutedQuery = insertQuery,
                };
            }
        }

        private static object GetValueFrom(string val, string dataType)
        {
            if (val == null)
            {
                return null;
            }

            switch (dataType)
            {
                case "int":
                    {
                        return int.Parse(val);
                    }

                case "bigint":
                    {
                        return long.Parse(val);
                    }

                case "bit":
                    {
                        return int.Parse(val) == 0 ? false : (object)true;
                    }

                default:
                    return val;
            }
        }

        private void ChangeDataType(DataTable dtCloned, DBTable destTable)
        {
            for (int i = 0; i < dtCloned.Columns.Count; i++)
            {
                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("char") ||
                    destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("text"))
                {
                    dtCloned.Columns[i].DataType = typeof(string);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("float", StringComparison.OrdinalIgnoreCase))
                {
                    dtCloned.Columns[i].DataType = typeof(float);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("datetime"))
                {
                    dtCloned.Columns[i].DataType = typeof(DateTime);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("int"))
                {
                    dtCloned.Columns[i].DataType = typeof(int);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("bigint"))
                {
                    dtCloned.Columns[i].DataType = typeof(long);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("smallint"))
                {
                    dtCloned.Columns[i].DataType = typeof(int);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("binary") ||
                    destTable.Fields.Cast<DBField>().ToList()[i].DataType.Contains("image"))
                {
                    dtCloned.Columns[i].DataType = typeof(byte[]);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("tinyint"))
                {
                    dtCloned.Columns[i].DataType = typeof(byte);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("bit"))
                {
                    dtCloned.Columns[i].DataType = typeof(bool);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("decimal") ||
                    destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("money") ||
                    destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("smallmoney"))
                {
                    dtCloned.Columns[i].DataType = typeof(decimal);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("uniqueidentifier"))
                {
                    dtCloned.Columns[i].DataType = typeof(Guid);
                }

                if (destTable.Fields.Cast<DBField>().ToList()[i].DataType.Equals("real"))
                {
                    dtCloned.Columns[i].DataType = typeof(SqlSingle);
                }
            }
        }

        private string CreateInsertQuery(string tableName, List<(string field, string val)> queryParams, string pkFieldName)
        {
            var fieldValues = string.Join(", ", queryParams.Select(qp => $"{qp.val} as {qp.field}"));

            // var (field, val) = queryParams.First();
            var fieldSets = string.Join(", ", queryParams.Select(f => $"{f.field}={f.val}"));

            var fieldList = string.Join(", ", queryParams.Select(y => y.field));
            var valuesList = string.Join(", ", queryParams.Select(y => y.val));

            var q = @$"
                SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                merge dynamics.{tableName}  as myTarget
                using (select {fieldValues}) as mySource
                on mySource.{pkFieldName} = myTarget.{pkFieldName}
                when matched then update set {fieldSets}
                when not matched then insert ({fieldList}) values ({valuesList});";
            return q;
        }

        private List<string> DistinctColumnsDestiny(DataTable dataTable, DBTable destTable)
        {
            List<string> originColumns = new List<string>();
            List<string> destinyColumns = new List<string>();

            foreach (DataColumn item in dataTable.Columns)
            {
                originColumns.Add(item.ColumnName);
            }

            for (int i = 0; i < destTable.Fields.Cast<DBField>().ToList().Count(); i++)
            {
                destinyColumns.Add(destTable.Fields.Cast<DBField>().ToList()[i].Name);
            }

            return destinyColumns.Except(originColumns).ToList<string>();
        }

        private List<string> DistinctColumnsOrigin(DataTable dataTable, DBTable destTable)
        {
            List<string> originColumns = new List<string>();
            List<string> destinyColumns = new List<string>();

            foreach (DataColumn item in dataTable.Columns)
            {
                originColumns.Add(item.ColumnName);
            }

            for (int i = 0; i < destTable.Fields.Cast<DBField>().ToList().Count(); i++)
            {
                destinyColumns.Add(destTable.Fields.Cast<DBField>().ToList()[i].Name);
            }

            return originColumns.Except(destinyColumns).ToList<string>();
        }

        ////private string CreateUpdateQuery(string tableName, List<(string field, string val)> queryParams)
        ////{
        ////    var (field, val) = queryParams.First();
        ////    var fieldSets = string.Join(", ", queryParams.Skip(1).Select(f => $"{f.field}={f.val}"));
        ////    var queryString = $"update dynamics.{tableName}  set {fieldSets} where {field}={val}";
        ////    return queryString;
        ////}
        private async Task<IEnumerable<DBField>> GetFieldsForTable(string tableName)
        {
            string query = string.Format(GetFields, tableName);
            return await this.dataAccess.LoadData<DBField, object>(query, null);
        }
    }
}