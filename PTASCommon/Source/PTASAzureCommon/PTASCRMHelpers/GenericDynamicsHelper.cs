// <copyright file="GenericDynamicsHelper.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASCRMHelpers.Exceptions;
    using PTASCRMHelpers.Models;
    using PTASCRMHelpers.Utilities;

    using Serilog;

    /// <summary>
    /// Generic helper for dynamics.
    /// </summary>
    public class GenericDynamicsHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDynamicsHelper"/> class.
        /// </summary>
        /// <param name="apiRoute">API Route.</param>
        /// <param name="cRMUri">CRM address.</param>
        /// <param name="authUri">Windows authorization URI.</param>
        /// <param name="clientId">Client Id for security.</param>
        /// <param name="clientSecret">Client Secret.</param>
        public GenericDynamicsHelper(string apiRoute, string cRMUri, string authUri, string clientId, string clientSecret)
        {
            this.CrmHelper = new CrmOdataHelper(cRMUri, authUri, clientId, clientSecret);
            this.ApiRoute = apiRoute;
            this.CRMUri = cRMUri;
        }

        private string ApiRoute { get; }

        private CrmOdataHelper CrmHelper { get; }

        private string CRMUri { get; }

        /// <summary>
        /// Apply item changes to dynamics.
        /// </summary>
        /// <param name="items">items to apply.</param>
        /// <param name="skipUserFields">drop any user-related fields.</param>
        /// <param name="impersonateUserId">The impersonation user identifier.</param>
        /// <returns>The task.</returns>
        public async Task ApplyToDynamics(IEnumerable<EntityChanges> items, bool skipUserFields, Guid? impersonateUserId = null)
        {
            var converted = items.Select(c => new EntityChanges
            {
                Changes = c.Changes,
                EntityId = c.EntityId,
                EntityName = c.EntityName.Pluralize(),
            });
            var results = await this.PrepareAndSendBatch(converted, skipUserFields, impersonateUserId);
        }

        /// <summary>
        /// Gets a list of items.
        /// </summary>
        /// <param name="requests">List of items to get.</param>
        /// <returns>Got items if found.</returns>
        public async Task<IEnumerable<EntityRequestResult>> GetItems(IEnumerable<EntityRequest> requests)
        {
            var processPipe = await this.FetchItems(requests);
            return processPipe;
        }

        /// <summary>
        /// Prepares and sends a list of batch changes to dynamics atomically.
        /// </summary>
        /// <param name="changes">Items to post to dynamics.</param>
        /// <param name="excludeUserfields">Leave out user related fields.</param>
        /// <param name="impersonateUserId">The impersonation user identifier.</param>
        /// <returns>
        /// A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.
        /// </returns>
        public async Task<IEnumerable<HttpResponseMessage>> PrepareAndSendBatch(IEnumerable<EntityChanges> changes, bool excludeUserfields, Guid? impersonateUserId = null)
        {
            if (!changes.Any())
            {
                Log.Debug("No changes to apply.");
                return new List<HttpResponseMessage>();
            }

            Log.Debug($"Applying changes: {JsonConvert.SerializeObject(changes)}");

            var returnedTasks = changes.Select((c, i) =>
            {
                return this.RemapChangesAsync(c, excludeUserfields);
            }).ToArray();

            var remappedValues = (await Task.WhenAll(returnedTasks))
                .Where(v => !string.IsNullOrEmpty(v))
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
                string remapped = remappedValues[i];
                return Helpers.CreateHttpMessageContent(c.EntityName, c.EntityId, i, remapped, this.CRMUri, this.ApiRoute);
            })
                .ToList()
                .ForEach(c =>
                {
                    changesetContent.Add(c);
                });

            batchContent.Add(changesetContent);
            try
            {
                return await this.SendBatch(batchContent, impersonateUserId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                var allSentValues = changes.Select((c, i) => $"{c.EntityName} {remappedValues[i]}\n");
                string values = string.Join("\n", allSentValues);
                throw new DynamicsHttpRequestException("Sent info: " + values, ex);
            }
        }

        /// <summary>
        /// Executes a query against dynamics.
        /// </summary>
        /// <param name="entityRequest">Item to executed.</param>
        /// <returns>Result of execution of query.</returns>
        protected Task<string> ExecuteQuery(EntityRequest entityRequest)
            => this.CrmHelperExecuteQuery(entityRequest.EntityName.Pluralize(), $"$filter={entityRequest.EntityName}id eq '{entityRequest.EntityId}'");

        /// <summary>
        /// Get a list of items.
        /// </summary>
        /// <param name="requests">Items to get.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected async Task<EntityRequestResult[]> FetchItems(IEnumerable<EntityRequest> requests)
        {
            var arr = requests.ToArray();
            var tasks = arr
                .Select(itm => this.ExecuteQuery(itm));
            var jsonResults = await Task.WhenAll(tasks);
            var results = jsonResults
                .Select(jsonText => JObject.Parse(jsonText))
                .Select((dynamic parsedObject)
                    => parsedObject.value is JArray returnedArray && returnedArray.Any() ? returnedArray.First() : null)
                .Select((dynamic firstItem, int idx)
                    => (itm: firstItem, related: arr[idx]))
                .Select(v
                    =>
                {
                    dynamic item = v.itm == null ? null : Helpers.RemoveEmptyChildren(v.itm, v.related.EntityName);
                    return new EntityRequestResult
                    {
                        EntityId = v.related.EntityId,
                        EntityName = v.related.EntityName,
                        Changes = item,
                    };
                })
                .ToArray();
            return results;
        }

        /// <summary>
        /// Create and httpclient to send.
        /// </summary>
        /// <returns>Created client.</returns>
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            this.CrmHelper.SetupHeaders(httpClient);
            return httpClient;
        }

        private async Task<string> CrmHelperExecuteQuery(string tableName, string query)
        {
            var queryStr = $"{this.ApiRoute}{tableName}?$count=true&{query}";
            var response = await this.CrmHelper.CrmWebApiFormattedGetRequest(queryStr);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private async Task<IEnumerable<EntityReference>> GetMetadataAsync(string entityName)
        {
            if (entityName.EndsWith("s"))
            {
                entityName = entityName.Substring(0, entityName.Length - 1);
            }

            var url = $"{this.ApiRoute}EntityDefinitions(LogicalName='{entityName}')/ManyToOneRelationships?$select=ReferencedAttribute,ReferencedEntity,ReferencingAttribute,ReferencingEntity";
            var r = await this.CrmHelper.CrmWebApiFormattedGetRequest(url);
            var returnedJson = await r.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<DynamicsWrapper>(returnedJson);

            return items.Value;
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
                            await Helpers.DeserializeToResponse(await c.ReadAsStreamAsync());
                        if (responseMessage != null)
                        {
                            results.Add(responseMessage);
                        }
                    }
                }
            }

            return results;
        }

        private async Task<string> RemapChangesAsync(EntityChanges c, bool excludeUserfields)
        {
            IEnumerable<EntityReference> metadata = await this.GetMetadataAsync(c.EntityName);
            var remappedDict = c.Changes.Select((itm, index) =>
            {
                var key = itm.Key;
                var value = itm.Value;

                if (key.EndsWith("_value") && key.StartsWith("_"))
                {
                    var fieldName = key.Substring(1).Replace("_value", string.Empty); // _abc_value becomes abc
                    var entityRef = metadata.Where(v => v.ReferencingAttribute == fieldName || v.ReferencedAttribute == fieldName).FirstOrDefault();
                    if (entityRef != null)
                    {
                        var plurarized = entityRef.ReferencedEntity.Pluralize();

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
                return JsonConvert.SerializeObject(remappedDict);
            }

            return string.Empty;
        }

        /// <summary>
        /// Sends the batch.
        /// </summary>
        /// <param name="batchContent">Content of the batch.</param>
        /// <param name="impersonateUserId">The impersonation user identifier.</param>
        /// <returns>The response of the HttpMessage sent.</returns>
        private async Task<List<HttpResponseMessage>> SendBatch(MultipartContent batchContent, Guid? impersonateUserId = null)
        {
            string requestUrl = $"{this.CRMUri}/api/data/v9.1/$batch";
            try
            {
                HttpRequestMessage batchRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUrl),
                };
                batchRequest.Content = batchContent;

                if (impersonateUserId != null)
                {
                    batchRequest.Headers.Add("CallerObjectId", impersonateUserId.Value.ToString());
                }

                HttpResponseMessage response = await this.SendRequest(batchRequest);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    throw new DynamicsHttpRequestException($"Error calling {requestUrl}. Message: {msg}", null);
                }

                MultipartMemoryStreamProvider body = await response.Content.ReadAsMultipartAsync();
                List<HttpResponseMessage> contents = await this.ReadHttpContents(body);

                return contents;
            }
            catch (Exception ex)
            {
                throw new DynamicsHttpRequestException($"Failed calling url: {requestUrl}. {ex.Message}.", ex);
            }
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage batchRequest)
        {
            HttpClient httpClient = this.CreateClient();
            var response = await httpClient.SendAsync(batchRequest);
            return response;
        }
    }
}