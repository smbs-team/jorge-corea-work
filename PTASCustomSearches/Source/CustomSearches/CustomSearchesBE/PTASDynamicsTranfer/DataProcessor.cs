namespace PTASDynamicsTranfer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Models;
    using D2SSyncHelpers.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// DataProcessor class.
    /// </summary>
    public class DataProcessor
    {
        private const string NoNewData = "No new data to move.";
        private List<string> snapEntities = new List<string>() { "ptas_accessorydetail", "ptas_buildingdetail", "ptas_buildingdetail_commercialuse", "ptas_buildingsectionfeature", "ptas_condocomplex", "ptas_condounit", "ptas_land", "ptas_landvaluebreakdown", "ptas_landvaluecalculation", "ptas_lowincomehousingprogram", "ptas_parceldetail", "ptas_projectdock", "ptas_sectionusesqft", "ptas_taxaccount", "ptas_unitbreakdown" };
        private Guid g = Guid.Empty;

        /// <summary>
        /// Process the entity to move the info.
        /// </summary>
        /// <param name="entityName">Name of entity to be processed.</param>
        /// <param name="chunkSize">Chunk size to process.</param>
        /// <param name="config">The configuration to use in the process.</param>
        /// <param name="useBulkInsert">Flag for Bulk Copy.</param>
        /// <param name="connectionString">ConnectionString.</param>
        /// <param name="principalCredentials">Credentials.</param>
        /// <returns>The result of the process</returns>
        public async Task ProcessEntityAsync(string entityName, int chunkSize, IConfiguration config, int useBulkInsert, string connectionString, ClientCredential principalCredentials)
        {
            Console.WriteLine($"Start processing table {entityName}. Chunk size: {chunkSize}");
            Console.WriteLine("Press enter to stop process.");
            var access = new DataAccessLibrary(connectionString, principalCredentials);
            var dynamicsLoader = new DynamicsLoader(config);
            var tableService = new DBTablesService(access);
            var destTable = await tableService.GetTable(entityName);
            if (destTable != null)
            {
                var entityDefinition = dynamicsLoader.EntityDefinition(entityName).Result;
                string pluralName = entityDefinition.Value<string>("EntitySetName");
                string pkFieldName = entityDefinition.Value<string>("PrimaryIdAttribute");

                if (this.snapEntities.Contains(entityName.ToLower()))
                {
                    // SnapAnual = 1, No Snap = 2, All Snap = 3
                    await this.SaveData(tableService, dynamicsLoader, entityName, pluralName, pkFieldName, chunkSize, config, destTable, useBulkInsert, 2, connectionString, principalCredentials);

                    destTable.Name = destTable.Name + "_snapshot";
                    await this.SaveData(tableService, dynamicsLoader, entityName, pluralName, pkFieldName, chunkSize, config, destTable, useBulkInsert, 1, connectionString, principalCredentials);
                }
                else
                {
                    await this.SaveData(tableService, dynamicsLoader, entityName, pluralName, pkFieldName, chunkSize, config, destTable, useBulkInsert, 0, connectionString, principalCredentials);
                }

            }
        }

        private async Task SaveData(DBTablesService tableService, DynamicsLoader dynamicsLoader, string entityName, string pluralName, string pkFieldName, int chunkSize, IConfiguration config, DBTable destTable, int useBulkInsert, int snapType, string connectionString, ClientCredential principalCredentials)
        {
            while (true)
            {
                TableChange lastSavedInfo;
                if (snapType == 1)
                {
                    lastSavedInfo = await tableService.GetLastSavedInfo(entityName + "_snapshot");
                }
                else
                {
                    lastSavedInfo = await tableService.GetLastSavedInfo(entityName);
                }

                var loadedTableData = await dynamicsLoader.LoadTableDataAsync(entityName, pluralName, pkFieldName, lastSavedInfo.LastGuidProcessed, chunkSize, snapType);
                dynamic deserialized = JsonConvert.DeserializeObject(loadedTableData);
                JArray actualValues = deserialized.value;
                if (actualValues.Any())
                {
                    var startTime = DateTime.Now;
                    bool hadErrors = false;

                    // Implements the Bulk Copy or Insert and Update method
                    if (useBulkInsert == 1)
                    {
                        SaveError result = await tableService.BulkInsert(new DataAccessLibrary(connectionString, principalCredentials).GetConnection(), actualValues, destTable);
                        if (result.HadError)
                        {
                            hadErrors = true;
                            Console.WriteLine("Failed so will retry this whole block.");
                            List<SaveError> list = new List<SaveError>();
                            list.Add(result);
                            await ReportErrorsAsync(list, tableService, entityName);
                        }
                    }
                    else
                    {
                        var tasks = actualValues.Select(async t =>
                        {
                            return await tableService.SaveRecord(t as JObject, destTable, pkFieldName);
                        });
                        SaveError[] results = await Task.WhenAll(tasks);
                        hadErrors = results.Any(r => r.HadError);
                        if (hadErrors)
                        {
                            Console.WriteLine("Failed so will retry this whole block.");
                            await ReportErrorsAsync(results.Where(r => r.HadError).ToList(), tableService, entityName);
                        }
                    }

                    if (!hadErrors)
                    {
                        // var t = actualValues.LastOrDefault().Value<DateTime>("createdon");
                        g = Guid.Parse(actualValues.LastOrDefault().Value<string>(pkFieldName));
                        if (snapType == 1)
                        {
                            await tableService.SaveLastSavedInfo(entityName + "_snapshot", DateTime.Now, this.g);
                        } else
                        {
                            await tableService.SaveLastSavedInfo(entityName, DateTime.Now, this.g);
                        }
                    }

                    var endTime = DateTime.Now;
                    var time = endTime.Subtract(startTime).TotalMilliseconds;

                    Console.WriteLine($"Saved {actualValues.Count} in {time} MS.");
                }
                else
                {
                    Console.WriteLine(NoNewData);
                    break;
                }
            }
        }

        private static async Task ReportErrorsAsync(List<SaveError> list, DBTablesService tableService, string entityName)
        {
            try
            {
                foreach (var item in list)
                {
                    await tableService.SaveErrorAsync(item, entityName);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("Errors.txt", ex.Message);
                File.AppendAllLines("Errors.txt", list.Select(itm => itm.ToString()));
            }
        }
    }
}
