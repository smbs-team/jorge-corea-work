// <copyright file="DynamicsTransactionHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json;

    using PTASCRMHelpers;
    using PTASCRMHelpers.Exception;

    using PTASDynamicsCrudCore.Classes;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Serilog;

    /// <summary>
    /// Class to execute dynamics transactions.
    /// </summary>
    public class DynamicsTransactionHelper
    {
        /// <summary>
        /// List of related entities.
        /// </summary>
        public static readonly (string entity, string keyfield, string relatingField)[] RelatedEntities = new (string, string, string)[]
        {
            (AccesoryDetailsName, AccesoryDetailsKeyField, ParcelDetailIdFieldName),
            (BuildingDetailssName, BuildingDetailsKeyField, ParcelDetailIdFieldName),
            ("ptas_condocomplexes", "ptas_condocomplexid", ParcelIdFieldName),
            ("ptas_condounits", "ptas_condounitid", ParcelIdFieldName),
            ////("ptas_unitbreakdowns", "ptas_unitbreakdownid"),
            ////("ptas_buildingsectionuses", "ptas_buildingsectionuseid"),
            ////("ptas_buildingsectionfeatures", "ptas_buildingsectionfeatureid"),
        };

        private const string AccesoryDetailsKeyField = "ptas_accessorydetailid";

        private const string AccesoryDetailsName = "ptas_accessorydetails";

        private const string BuildingDetailsKeyField = "ptas_buildingdetailid";

        private const string BuildingDetailssName = "ptas_buildingdetails";

        private const string ParcelDetailIdFieldName = "parceldetailid";

        private const string ParcelIdFieldName = "parcelid";

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsTransactionHelper"/> class.
        /// </summary>
        /// <param name="config">Application configuration.</param>
        /// <param name="memoryCache">Memory cache manager.</param>
        /// <param name="httpContext">Current context.</param>
        /// <param name="tokenManager">Token manager.</param>
        public DynamicsTransactionHelper(IConfigurationParams config, IMemoryCache memoryCache, HttpContext httpContext, ITokenManager tokenManager)
        {
            this.Config = config;
            this.MemoryCache = memoryCache;
            this.HttpContext = httpContext;
            this.CrmHelper = new CrmOdataHelper(config.CRMUri, config.AuthUri, config.ClientId, config.ClientSecret, tokenManager);
        }

        /// <summary>
        /// Gets associated CRM helper.
        /// </summary>
        public CrmOdataHelper CrmHelper { get; }

        /// <summary>
        /// Gets current httpcontext.
        /// </summary>
        public HttpContext HttpContext { get; }

        private IConfigurationParams Config { get; }

        private IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Executes a CRM query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="query">Query to execute.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecuteQuery(string tableName, string query)
        {
            var queryStr = $"{this.Config.ApiRoute}{tableName}?$count=true&{query}";
            var response = await this.CrmHelper.CrmWebApiFormattedGetRequest(queryStr);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// Get metadata for entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>Task that yields the metadata.</returns>
        public async Task<DynamicsEntityInfo> GetMetadataAsync(string entityName)
        {
            var cacheName = $"metadata_{entityName}";
            if (this.MemoryCache.TryGetValue(cacheName, out DynamicsEntityInfo result))
            {
                return result;
            }

            var url = $"{this.Config.ApiRoute}EntityDefinitions(LogicalName='{entityName}')?$expand=ManyToOneRelationships($select=ReferencedAttribute,ReferencedEntity,ReferencingAttribute,ReferencingEntity),ManyToManyRelationships&$select=EntitySetName";
            var requestResult = await this.CrmHelper.CrmWebApiFormattedGetRequest(url);
            var returnedJson = await requestResult.Content.ReadAsStringAsync();
            var metadata = JsonConvert.DeserializeObject<DynamicsEntityInfo>(returnedJson);

            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            // Save data in cache.
            this.MemoryCache.Set(cacheName, metadata, cacheEntryOptions);

            return metadata;
        }

        /// <summary>
        /// Prepares and sends a list of batch changes to dynamics atomically.
        /// </summary>
        /// <param name="changes">Items to post to dynamics.</param>
        /// <param name="excludeUserfields">Leave out user related fields.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<List<HttpResponseMessage>> PrepareAndSendBatch(IEnumerable<EntityChanges> changes, bool excludeUserfields)
        {
            if (!changes.Any())
            {
                Log.Debug("No changes to apply.");
                return new List<HttpResponseMessage>();
            }

            changes = changes
                .OrderBy(c => c.EntityName.Equals("ptas_land") ? 0 : c.EntityName.Equals("ptas_parceldetail") ? 1 : 2)
                .ToArray();

            Log.Debug($"Applying changes: {JsonConvert.SerializeObject(changes)}");

            var returnedTasks = changes
                .Select((c, i) => this.RemapChangesAsync(c, excludeUserfields))
                .ToArray();

            var remappedValues = (await Task.WhenAll(returnedTasks))
                .Where(v => !string.IsNullOrEmpty(v.EntitysetName))
                .ToArray();
            if (remappedValues.Length == 0)
            {
                // nothing to send.
                return new List<HttpResponseMessage>();
            }

            string batchName = $"batch_{Guid.NewGuid()}";
            MultipartContent batchContent = new MultipartContent("mixed", batchName);
            string changesetName = $"changeset_{Guid.NewGuid()}";
            MultipartContent changesetContent = new MultipartContent("mixed", changesetName);

            changes.Select((c, i) =>
                {
                    string remapped = remappedValues[i].remappedChanges;
                    return DynamicsHelpers.CreateHttpMessageContent(remappedValues[i].EntitysetName, c.EntityId, i, remapped, this.Config.CRMUri, this.Config.ApiRoute);
                })
                .ToList()
                .ForEach(c =>
                {
                    changesetContent.Add(c);
                });

            batchContent.Add(changesetContent);
            return await this.SendBatch(batchContent);
        }

        /// <summary>
        /// Attempt to link 2 entities.
        /// </summary>
        /// <param name="data">Details of linkage.</param>
        /// <param name="wrapper">Crm wrapper.</param>
        /// <returns>Nothing.</returns>
        internal async Task LinkEntities(LinkEntitiesData data, CRMWrapper wrapper)
        {
            var (entityMetadata, counterPartMetadata) = await this.GetMetadataForRelatedEntities(data);

            var navigationProperty = entityMetadata.ManyToManyRelationships
                .Where(mmr => mmr.Entity2LogicalName == data.CounterpartEntityName)
                .Select(mmr => mmr.Entity1NavigationPropertyName)
                .FirstOrDefault();
            var url = $"{this.Config.CRMUri}{this.Config.ApiRoute}{counterPartMetadata.EntitySetName}({data.CounterparId})";
            var toSave = new DynamicsNTONRelationship(url);
            bool result = await wrapper.ExecuteNTONPost(entityMetadata.EntitySetName, toSave, data.EntityId, navigationProperty);
        }

        /// <summary>
        /// Attempt to link 2 entities.
        /// </summary>
        /// <param name="data">Details of linkage.</param>
        /// <param name="wrapper">Crm wrapper.</param>
        /// <returns>Nothing.</returns>
        internal async Task UnlinkEntities(LinkEntitiesData data, CRMWrapper wrapper)
        {
            var (entityMetadata, counterPartMetadata) = await this.GetMetadataForRelatedEntities(data);
            var navigationProperty = entityMetadata.ManyToManyRelationships
                .Where(mmr => mmr.Entity2LogicalName == data.CounterpartEntityName)
                .Select(mmr => mmr.Entity1NavigationPropertyName)
                .FirstOrDefault();
            string url = $"/api/data/v9.1/{entityMetadata.EntitySetName}({data.EntityId})/{navigationProperty}({data.CounterparId})/$ref";
            var httpResult = await this.CrmHelper.CrmWebApiFormattedDeleteRequest(url);
            if (!httpResult.IsSuccessStatusCode)
            {
                var r = await httpResult.Content.ReadAsStringAsync();
                throw new Exception("Failed unlinking. " + r);
            }
        }

        private async Task<(DynamicsEntityInfo entityMetadata, DynamicsEntityInfo counterPartMetadata)> GetMetadataForRelatedEntities(LinkEntitiesData data)
        {
            var r = await Task.WhenAll(
                this.GetMetadataAsync(data.EntityName),
                this.GetMetadataAsync(data.CounterpartEntityName));
            return (r[0], r[1]);
        }

        private async Task<List<HttpResponseMessage>> ReadHttpContents(MultipartMemoryStreamProvider body)
        {
            List<HttpResponseMessage> results = new List<HttpResponseMessage>();
            if (body?.Contents != null)
            {
                foreach (HttpContent c in body.Contents)
                {
                    if (c.IsMimeMultipartContent())
                    {
                        results.AddRange(await this.ReadHttpContents(await c.ReadAsMultipartAsync()));
                    }
                    else if (c.IsHttpResponseMessageContent())
                    {
                        HttpResponseMessage responseMessage = await c.ReadAsHttpResponseMessageAsync();
                        if (responseMessage != null)
                        {
                            results.Add(responseMessage);
                        }
                    }
                    else
                    {
                        HttpResponseMessage responseMessage =
                            await DynamicsHelpers.DeserializeToResponse(await c.ReadAsStreamAsync());
                        if (responseMessage != null)
                        {
                            results.Add(responseMessage);
                        }
                    }
                }
            }

            return results;
        }

        private async Task<(string EntitysetName, string remappedChanges)> RemapChangesAsync(EntityChanges c, bool excludeUserfields)
        {
            var md = await this.GetMetadataAsync(c.EntityName);
            var remappedDict = c.Changes.Select((itm, index) =>
            {
                var key = itm.Key;
                var value = itm.Value;

                if (key.EndsWith("_value") && key.StartsWith("_"))
                {
                    var fieldName = key.Substring(1).Replace("_value", string.Empty); // _abc_value becomes abc
                    var entityRef = md.ManyToOneRelationships
                        .Where(v => v.ReferencingAttribute == fieldName || v.ReferencedAttribute == fieldName)
                        .FirstOrDefault();
                    if (entityRef != null)
                    {
                        string referencedEntity = entityRef.ReferencedEntity == "owner" ? "systemuser" : entityRef.ReferencedEntity;
                        var plurarized = referencedEntity.Pluralize();

                        var cc = $"{itm.Value}".Trim();
                        if ((excludeUserfields && plurarized.Equals("systemusers")) || string.IsNullOrEmpty(cc))
                        {
                            return new KeyValuePair<string, object>(string.Empty, null);
                        }

                        return new KeyValuePair<string, object>($"{entityRef.ReferencingAttribute}@odata.bind", $"/{plurarized}({cc})");
                    }
                }

                return new KeyValuePair<string, object>(itm.Key, itm.Value);
            }).Where(itm => itm.Value != null).ToDictionary(ssk => ssk.Key, ssk => ssk.Value);
            if (remappedDict.Any())
            {
                return (md.EntitySetName, JsonConvert.SerializeObject(remappedDict));
            }

            return (string.Empty, string.Empty);
        }

        private async Task<List<HttpResponseMessage>> SendBatch(MultipartContent batchContent)
        {
            string requestUrl = $"{this.Config.CRMUri}/api/data/v9.1/$batch";
            HttpRequestMessage batchRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUrl),
            };
            batchRequest.Content = batchContent;
            HttpResponseMessage response = await DynamicsHelpers.SendRequest(batchRequest, this.Config, this.HttpContext);
            if (response.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider body = await response.Content.ReadAsMultipartAsync();
                List<HttpResponseMessage> contents = await this.ReadHttpContents(body);

                if (!response.IsSuccessStatusCode)
                {
                    var tasks = contents.Select(c => c.Content.ReadAsStringAsync());
                    var strings = await Task.WhenAll(tasks);
                    var jsons = strings.Select(s => JsonConvert.DeserializeObject(s));
                    var resultObj = new { location = "send batch", requestUrl, results = jsons };
                    throw new ObjBearingException(resultObj);
                }

                return contents;
            }

            return new List<HttpResponseMessage> { response };
        }
    }
}