// <copyright file="WhatIfHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASCRMHelpers.Exception;

    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Helper methods for whatif.
    /// </summary>
    public static class DynamicsHelpers
    {
        /// <summary>
        /// Pluralize an entity name.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <returns>The pluralized name.</returns>
        public static string Pluralize(this string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                return string.Empty;
            }

            if (entityName.EndsWith("set") && entityName.Count(c => c == '_') > 1)
            {
                return entityName;
            }

            // Strange exception for the rule in dynamics, sketch is pluralized incorrectly as sketchs.
            if (entityName.EndsWith("etch"))
            {
                return entityName + "s";
            }

            // If the singular noun ends in ‑s, -ss, -sh, -ch, -x, or - z, add ‑es to the end to make it plural.
            if (entityName.EndsWith("s") || entityName.EndsWith("ss") || entityName.EndsWith("sh") || entityName.EndsWith("ch") || entityName.EndsWith("x") || entityName.EndsWith("z"))
            {
                return entityName + "es";
            }

            if (entityName.EndsWith("y"))
            {
                return entityName.Substring(0, entityName.Length - 1) + "ies";
            }

            return entityName + "s";
        }

        /// <summary>
        /// Check for id match.
        /// </summary>
        /// <param name="searchValue">Format: numbers and dashes.</param>
        /// <returns>True if it is numbers and dashes.</returns>
        public static bool CouldBeId(this string searchValue) =>
            Regex.Match(searchValue, @"^[0-9-]*$").Success;

        /// <summary>
        /// Get all messages from exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns>All nested messages.</returns>
        public static string AllInnerMessages(this Exception ex)
            => ex == null ? string.Empty : $"Message: {ex.Message}.\n {ex.InnerException.AllInnerMessages()}".Trim();

        /// <summary>
        /// Get all messages from exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns>All nested messages.</returns>
        public static string AllMessages(this Exception ex)
            => ex == null ? string.Empty : $"{ex.Message}";

        /// <summary>
        /// Calculate changes from one set of attributes to another.
        /// </summary>
        /// <param name="src">Source items.</param>
        /// <param name="dst">Destination Items.</param>
        /// <param name="entityName">Entity name.</param>
        /// <param name="excludeFields">Exclude fields.</param>
        /// <returns>Changed dictionary of items.</returns>
        public static Dictionary<string, object> CalcChanges(DynamicsToDynamicsData src, DynamicsToDynamicsData dst, string entityName, string[] excludeFields)
        {
            var srcDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(src.EntityJSON);
            var destDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(dst.EntityJSON);
            var valuesDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(src.EntityJSON);
            var fieldsWithDifferentValues =
                (from srcItm in srcDict
                 join dstItem in destDict on srcItm.Key equals dstItem.Key
                 where dstItem.Value != srcItm.Value && JsonHelper.IsValidName(dstItem.Key, entityName, excludeFields, true) // values don't match.
                 select (srcItm.Key, Value: valuesDict[srcItm.Key]))
                 .ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
            return fieldsWithDifferentValues;
        }

        /// <summary>
        /// Changes generated from a new none existing item.
        /// </summary>
        /// <param name="src">Source json to clean.</param>
        /// <param name="entityName">Entity name.</param>
        /// <param name="excludeFields">Fields to exclude.</param>
        /// <returns>Changes for all entities.</returns>
        public static Dictionary<string, object> CreateChangesForNewItem(string src, string entityName, string[] excludeFields)
        {
            return JsonConvert
                               .DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(JsonHelper.RemoveEmptyChildren(JObject.Parse(src), entityName, excludeFields)));
        }

        /// <summary>
        /// Create and httpclient to send.
        /// </summary>
        /// <param name="authenticationHeaderValue">The authentication we need.</param>
        /// <returns>Created client.</returns>
        public static HttpClient CreateClient(AuthenticationHeaderValue authenticationHeaderValue)
        {
            var httpClient = new HttpClient
            {
                Timeout = new TimeSpan(0, 2, 0),
            };
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
            httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(@"*/*"));
            ////AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue(
            ////    "Bearer",
            ////    this.GetTokenUsingClientIdSecret());
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            return httpClient;
        }

        /// <summary>
        /// Create content for a batch send.
        /// </summary>
        /// <param name="tableName">Name of the entity to apply to.</param>
        /// <param name="itemId">Sequential Id of the item.</param>
        /// <param name="contentId">Id of the content.</param>
        /// <param name="content">Content to post.</param>
        /// <param name="cRMUri">Uri of the CRM.</param>
        /// <param name="apiRoute">Route to the api.</param>
        /// <returns>Created message content.</returns>
        public static HttpMessageContent CreateHttpMessageContent(
            string tableName,
            string itemId,
            int contentId,
            string content,
            string cRMUri,
            string apiRoute)
        {
            var requestUri = $"{cRMUri}{apiRoute}{tableName}({itemId})";
            ////var requestUri = $"{cRMUri}{apiRoute}{tableName}";
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri)
            {
                Version = new Version("1.1"),
            };
            HttpMessageContent messageContent = new HttpMessageContent(requestMessage);
            messageContent.Headers.Remove("Content-Type");
            messageContent.Headers.Add("Content-Type", "application/http");
            messageContent.Headers.Add("Content-Transfer-Encoding", "binary");

            if (!string.IsNullOrEmpty(content))
            {
                StringContent stringContent = new StringContent(content);
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;type=entry");
                requestMessage.Content = stringContent;
            }

            messageContent.Headers.Add("Content-ID", contentId.ToString());

            return messageContent;
        }

        /// <summary>
        /// Create URI.
        /// </summary>
        /// <param name="url">Url to resource.</param>
        /// <param name="path">Path.</param>
        /// <param name="id">Id to resource.</param>
        /// <returns>Created URI.</returns>
        public static string CreateURI(string url, string path, string id) => $@"{url}{path}/{id}";

        /// <summary>
        /// Deserialize a composite response.
        /// </summary>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> DeserializeToResponse(Stream stream)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            response.Content = new ByteArrayContent(memoryStream.ToArray());
            response.Content.Headers.Add("Content-Type", "application/http;msgtype=response");
            return await response.Content.ReadAsHttpResponseMessageAsync();
        }

        /// <summary>
        /// Get the changed items from two list of DynamicsToDynamicsData lists.
        /// </summary>
        /// <param name="source">Source list.</param>
        /// <param name="dest">Destination list.</param>
        /// <param name="excludeFields">Fields to exclude.</param>
        /// <returns>Changes between lists.</returns>
        public static IEnumerable<EntityChanges> GetChanges(IEnumerable<DynamicsToDynamicsData> source, IEnumerable<DynamicsToDynamicsData> dest, string[] excludeFields)
        {
            var changedItems = (from entityChanges in
                                    from joined in from src in source
                                                   join dst in dest on src.EntityId equals dst.EntityId
                                                   select (src, dst)
                                    select new EntityChanges
                                    {
                                        EntityName = joined.src.EntityName,
                                        EntityId = joined.src.EntityId,
                                        Changes = CalcChanges(joined.src, joined.dst, joined.src.EntityName, excludeFields),
                                    }
                                where entityChanges.Changes.Any()
                                select entityChanges).ToList();
            var newItems = from itm in source
                           where !dest.Any(f => f.EntityId == itm.EntityId)
                           select new EntityChanges
                           {
                               EntityName = itm.EntityName,
                               EntityId = itm.EntityId,
                               Changes = CreateChangesForNewItem(itm.EntityJSON, itm.EntityName, excludeFields),
                           };
            return changedItems.Concat(newItems);
        }

        /// <summary>
        /// Get items linked to an item.
        /// </summary>
        /// <param name="jsonContents">String of contents.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="keyField">Field to get.</param>
        /// <returns>Linked items.</returns>
        public static List<DynamicsToDynamicsData> GetLinkedItems(string jsonContents, string entityName, string keyField)
        {
            if (string.IsNullOrEmpty(jsonContents.Trim()))
            {
                return new List<DynamicsToDynamicsData>();
            }

            dynamic returnedObject = JObject.Parse(jsonContents);
            if (!(returnedObject.value is JArray returnedItems))
            {
                return new List<DynamicsToDynamicsData>();
            }

            var entities = returnedItems
                .Select(j => JsonHelper.RemoveEmptyChildren(j, entityName, null))
                .Select((dynamic j) =>
                {
                    string entityId = j.Value<string>(keyField);
                    return new DynamicsToDynamicsData
                    {
                        EntityId = entityId,
                        EntityName = entityName,
                        EntityJSON = Newtonsoft.Json.JsonConvert.SerializeObject(
                          j,
                          new JsonSerializerSettings
                          {
                              NullValueHandling = NullValueHandling.Include,
                          }),
                    };
                }).ToList();
            return entities;
        }

        /// <summary>
        /// GetTokenUsingClientIdSecret:It gets the toker for the connection with OData/Dynamics.
        /// </summary>
        /// <param name="config">Sys configuration.</param>
        /// <returns>The token for the connection.</returns>
        public static string GetTokenUsingClientIdSecret(IConfigurationParams config)
        {
            try
            {
                var authority = config.AuthUri;
                var authContext = new AuthenticationContext(authority, false);
                var credentials = new ClientCredential(config.ClientId, config.ClientSecret);
                var tokenResult = authContext.AcquireTokenAsync(config.CRMUri, credentials).Result;
                return tokenResult.AccessToken;
            }
            catch (AdalServiceException ex)
            {
                string error = string.Format($"Error trying to authenticate credentials from Dynamics odata service. (Error code: {ex.ErrorCode}, error message: {ex.Message})");
                throw new DynamicsHttpRequestException(error, ex);
            }
            catch (AggregateException ex) when (ex.InnerException is AdalServiceException)
            {
                var ex1 = ex.InnerException as AdalServiceException;
                string error = string.Format($"Azure Active Directory Autentication Exception. (Error code: {ex1.ErrorCode}, error message: {ex1.Message})");
                throw new DynamicsHttpRequestException(error, ex1);
            }
        }

        /// <summary>
        /// Send a composite request to dynamics.
        /// </summary>
        /// <param name="batchRequest">Message to send.</param>
        /// <param name="config">System configuration.</param>
        /// <param name="context">Http context.</param>
        /// <returns>A <see cref="Task{HttpResponseMessage}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> SendRequest(HttpRequestMessage batchRequest, IConfigurationParams config, HttpContext context)
        {
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue(
                "Bearer",
                GetTokenUsingClientIdSecret(config));

            HttpClient httpClient = CreateClient(authenticationHeaderValue);

            if (!config.IsWhatifEnvironment)
            {
                var t = JWTDecoder.GetOidFromToken(context);
                httpClient.DefaultRequestHeaders.Add("CallerObjectId", t);
            }

            var response = await httpClient.SendAsync(batchRequest);
            return response;
        }
    }
}