// <copyright file="DynamicsUtility.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsSyncLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Models;
    using D2SSyncHelpers.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASDynamicsDigesterLibrary;
    using PTASDynamicsDigesterLibrary.Classes;

    /// <summary>
    /// Utility to unify all dynamics access for delta tracking.
    /// </summary>
    public class DynamicsUtility
    {
        private readonly DataAccessLibrary dbAccess;
        private readonly DynamicsPayloadDigester digester;
        private readonly DynamicsSyncStateManager dynamicsSyncStateManager;
        private readonly List<string> errorList = new List<string>();
        private readonly string sqlConnectionString;
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsUtility"/> class.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="principalCredentials">Credentials.</param>
        public DynamicsUtility(IConfiguration config, ClientCredential principalCredentials)
        {
            this.sqlConnectionString = config.GetConnectionString("default") ?? config["connectionString"];
            this.dbAccess = DynamicsStaticHelper.CreateDBAccess(this.sqlConnectionString, principalCredentials);
            this.digester = new DynamicsPayloadDigester(this.sqlConnectionString, config["organizationId"], config["organizationName"], principalCredentials);
            this.DynamicsRelationshipLoader = new DynamicsRelationsLoader(config);
            this.dynamicsSyncStateManager = new DynamicsSyncStateManager(this.sqlConnectionString, principalCredentials);
            DapperConfig.ConfigureMapper(typeof(DBTable), typeof(DBField), typeof(TableChange));
        }

        private DynamicsRelationsLoader DynamicsRelationshipLoader { get; }

        /// <summary>
        /// Clears the sync state from the db.
        /// </summary>
        /// <param name="entityName">Name of the entity to reset.</param>
        /// <param name="resetSQLTable">Reset SQL table.</param>
        /// <returns>Task.</returns>
        public async Task ClearSyncState(string entityName, bool resetSQLTable)
        {
            if (resetSQLTable)
            {
                await this.dynamicsSyncStateManager.ResetTable(entityName);
            }

            await this.dynamicsSyncStateManager.ClearSyncState(entityName);
        }

        /// <summary>
        /// Returns a list of all tables.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<DBTable>> GetAllTables() =>
            await new DBTablesService(this.dbAccess).GetTables();

        /// <summary>
        /// Get dependent entities for entity.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <returns>Nothing.</returns>
        public async Task<EntityManyToManyReference[]> GetDependentEntities(string entityName)
        {
            return await this.DynamicsRelationshipLoader.GetNMRelatedEntities(entityName);
        }

        /// <summary>
        /// Syncs an entity/table by using delta sync.  Since this may be called from a webhook trigger, it's a good idea that it implements a timeout, so it stops processing records after the allotted time.
        /// As it progresses through data pages it should store OData Next Link or Delta Link.In case it fails it then can pick up from the last "Next" or "Delta"  link.
        /// </summary>
        /// <param name="dbTable">DB Table name.</param>
        /// <param name="entityName">Entity name.</param>
        /// <param name="timeoutMS">Timeout in milliseconds.</param>
        /// <param name="batchCopy">Use batch copy.</param>
        /// <param name="chunkSize">How many records we process in parallel.</param>
        /// <param name="log">Output log.</param>
        /// <param name="waitWhenFailMS">How many MS to wait after a failure.
        /// If zero it will exit on failure.
        /// </param>
        /// <returns>Nothing.</returns>
        public async Task<string> Sync(string dbTable, string entityName, int timeoutMS, bool batchCopy, int chunkSize = 50, ILogger log = null, int waitWhenFailMS = 3000)
        {
            this.log = log;
            log.LogInformation($"Synching {dbTable} ({entityName})");
            try
            {
                var tableInfo = await new DBTablesService(this.dbAccess).GetTable(dbTable);
                if (tableInfo == null)
                {
                    throw new ArgumentException("Can't find table " + dbTable);
                }

                var uniqueTypes = tableInfo.Fields.Select(f => f.DataType).Distinct().ToArray();
                var (idField, qry) = this.CreateupsertScript(tableInfo);
                string dynamicsEntityName = entityName;
                if (!entityName.EndsWith("set"))
                {
                    var metadata = await this.DynamicsRelationshipLoader.GetMetadataAsync(entityName);
                    dynamicsEntityName = metadata?.EntitySetName ?? entityName;
                }

                var syncUrl = await this.dynamicsSyncStateManager.GetSyncUrlForEntityFromDB(entityName)
                    ?? this.DynamicsRelationshipLoader.GetSyncUrlForEntity(dynamicsEntityName);
                var finished = false;
                var limit = DateTime.Now.AddMilliseconds(timeoutMS);
                this.errorList.Clear();
                do
                {
                    var result = await this.DynamicsRelationshipLoader.RunUri(syncUrl);
                    var deserialized = JsonConvert.DeserializeObject<DynamicsDeltaReturn>(result);
                    if (deserialized.Value.Length > 0)
                    {
                        if (batchCopy)
                        {
                            var d2 = JsonConvert.DeserializeObject<dynamic>(result);
                            await new DBTablesService(this.dbAccess).BulkInsert(this.dbAccess.GetConnection(), d2.value as JArray, tableInfo);
                        }
                        else
                        {
                            await this.ChunkUpdate(chunkSize, tableInfo, qry, deserialized);
                        }

                        if (this.errorList.Any())
                        {
                            log.LogError("There was an error. Will retry in 3 secs.");
                            this.errorList.ForEach(err => log.LogError(err));
                            this.errorList.Clear();
                            if (waitWhenFailMS > 0)
                            {
                                await Task.Delay(waitWhenFailMS);
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(deserialized.NextLink))
                            {
                                syncUrl = deserialized.NextLink;
                                await this.dynamicsSyncStateManager.SetSyncUrlForEntity(entityName, syncUrl);
                            }
                            else
                            {
                                await this.dynamicsSyncStateManager.SetSyncUrlForEntity(entityName, deserialized.DeltaLink);
                                finished = true;
                            }
                        }
                    }
                    else
                    {
                        log.LogInformation("No records.");
                        finished = true;
                    }
                }
                while (DateTime.Now < limit && !finished);
                log.LogInformation("Finished");
                return "OK";
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                log.LogError(errorMessage);
                return errorMessage;
            }
        }

        /// <summary>
        /// Syncs n:m.
        /// </summary>
        /// <param name="parentTable">Parent table.</param>
        /// <param name="timeout">Timeout in MS.</param>
        /// <param name="useBatch">Use batch.</param>
        /// <param name="chunkSize">Chunk size.</param>
        /// <param name="log">Output log.</param>
        /// <param name="waitWhenFailMS">How many MS to wait after a failure.
        /// If zero it will exit on failure.
        /// </param>
        /// <returns>Task.</returns>
        public async Task SyncNM(string parentTable, int timeout, bool useBatch, int chunkSize, ILogger log, int waitWhenFailMS = 3000)
        {
            log.LogInformation($"Starting NM sync on {parentTable}");
            var relatedTables = await this.GetDependentEntities(parentTable);
            var syncTasks = relatedTables
                .Select(itm => this.Sync(itm.IntersectEntityName, itm.IntersectEntityName + "set", timeout, useBatch, chunkSize, log, waitWhenFailMS))
                .ToArray();
            await Task.WhenAll(syncTasks);
        }

        /// <summary>
        /// updates a record in report db using the payload coming from dynamics.
        /// </summary>
        /// <param name="payload">Payload to apply.</param>
        /// <returns>Nothing.</returns>
        public async Task<(ReceivedEntityInfo info, string result)> UpdateDbRecord(string payload)
        {
            return await this.digester.DigestItem(payload);
        }

        private void AddError(string queryToRun, Exception ex)
        {
            this.errorList.Add("Error " + ex.Message + " in " + queryToRun);
        }

        private async Task<bool> ApplyChange(DBTable tableInfo, string qry, dynamic item)
        {
            var queryToRun = tableInfo.Fields
                .Aggregate(
                    qry,
                    (string prev, DBField curr) =>
                        prev.Replace("#" + curr.Name + "#", (string)this.GetFieldValue(curr, item)));
            try
            {
                await this.dbAccess.SaveData<object>(queryToRun, null);
                return true;
            }
            catch (Exception ex)
            {
                this.AddError(queryToRun, ex);
                return false;
            }
        }

        private async Task ChunkUpdate(int chunkSize, DBTable tableInfo, string qry, DynamicsDeltaReturn deserialized)
        {
            var chunked = deserialized.Value
                                .Select((v, i) => (value: v, group: i / chunkSize))
                                .GroupBy(itm => itm.group)
                                .Select(gr => gr.Select(itm => itm.value).ToArray())
                                .ToArray();

            for (int i = 0; i < chunked.Length; i += 1)
            {
                dynamic[] chunk = chunked[i];
                this.log.LogInformation("Starting chunk");
                bool[] results = await Task.WhenAll(chunk.Select(async (c, idx) =>
                    await (this.ApplyChange(tableInfo, qry, c) as Task<bool>)));
                this.log.LogInformation($"Finished chunk {i}.");
                if (!results.All(r => r))
                {
                    this.log.LogInformation("Had errors on chunk {i}.");
                    break;
                }
            }
        }

        private (string keyField, string query) CreateupsertScript(DBTable tableInfo)
        {
            IEnumerable<string> allFields = tableInfo.Fields.Select(field => field.Name);
            var insertFieldNames = string.Join(", ", allFields);
            var updateFields = string.Join(", ", allFields.Skip(1).Select(f => $"{f}=#{f}#"));
            var idField = allFields.First();
            var tableName = $"dynamics.{tableInfo.Name}";

            var insertFieldReplace = string.Join(",", tableInfo.Fields.Select(field => field.Name).Select(fn => $"#{fn}#"));
            var initialQuery = @$"
            if (exists
                    (select * from {tableName}
                        where {idField}=#{idField}#))
            BEGIN
	            update {tableName} set {updateFields}
	            where {idField}=#{idField}#
            END ELSE BEGIN
	            insert into {tableName} ({insertFieldNames})
	            values ({insertFieldReplace})
            END
            ";
            return (idField, initialQuery);
        }

        private string GetFieldValue(DBField curr, dynamic item)
        {
            dynamic currValue = item[curr.Name];
            if (curr.Name == "modifiedon" && currValue is null)
            {
                currValue = DateTime.Now;
            }

            string currStrValue = $"{currValue}";
            return currValue == null
                    ? "null"
                    : curr.DataType.Equals("bit")
                        ? bool.Parse(currStrValue.ToLower()) ? "1" : "0"
                        : "nvarchar,uniqueidentifier,varbinary,datetime,datetimeoffset".Contains(curr.DataType)
                            ? $"'{currStrValue.Replace("'", "''")}'"
                            : currStrValue;
        }
    }
}