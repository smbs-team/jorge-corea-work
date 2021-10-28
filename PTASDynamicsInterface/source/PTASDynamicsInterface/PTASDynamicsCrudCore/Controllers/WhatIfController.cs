// <copyright file="WhatIfController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASCRMHelpers;
    using PTASCRMHelpers.Exception;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    using Serilog;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller for what if analisys on parcels.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class WhatIfController : ControllerBase
    {
        private const string LandName = "ptas_lands";
        private const string ObjectId = "objectId";
        private const string ParcelDetailsKeyField = "ptas_parceldetailid";
        private const string ParcelDetailsName = "ptas_parceldetails";
        private readonly ITokenManager tokenManager;
        private readonly IMemoryCache memoryCache;
        private DynamicsTransactionHelper transactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhatIfController"/> class.
        /// </summary>
        /// <param name="config">Sys config.</param>
        /// <param name="memoryCache">Memory cache.</param>
        /// <param name="tokenManager">Token.</param>
        public WhatIfController(IConfigurationParams config, IMemoryCache memoryCache, ITokenManager tokenManager)
        {
            this.Config = config;
            this.memoryCache = memoryCache;
            this.tokenManager = tokenManager;
        }

        /// <summary>
        /// Gets app configuration.
        /// </summary>
        public IConfigurationParams Config { get; }

        private DynamicsTransactionHelper TransactionHelper
        {
            get
            {
                if (this.transactionHelper == null)
                {
                    this.transactionHelper = new DynamicsTransactionHelper(this.Config, this.memoryCache, this.HttpContext, this.tokenManager);
                }

                return this.transactionHelper;
            }
        }

        /// <summary>
        /// Check in parcel for what if.
        /// </summary>
        /// <param name="id">Parcel Id.</param>
        /// <param name="applyChanges">Does it apply the changes.</param>
        /// <param name="excludeFields">Fields to exclude.</param>
        /// <param name="data">Data to save.</param>
        /// <returns>Applied info.</returns>
        [HttpPost("receive/{id}")]
        [SwaggerOperation(OperationId = "WhatIfReceiver")]
        public async Task<ActionResult> Receive(string id, [FromQuery] bool applyChanges, [FromQuery] string excludeFields, [FromBody] DynamicsToDynamicsData[] data = null)
        {
            try
            {
                excludeFields = excludeFields ?? string
                    .Empty;
                Log.Debug($"About to process changes for Parcel {id}. Value: {JsonConvert.SerializeObject(data)}");
                string[] excludeFieldsArray = excludeFields.Split(',', StringSplitOptions.RemoveEmptyEntries);
                List<DynamicsToDynamicsData> dest = await this.GetParcelItems(id, excludeFieldsArray);
                var changes = DynamicsHelpers.GetChanges(data, dest, excludeFieldsArray).ToList();
                if (applyChanges)
                {
                    var r = await this.TransactionHelper.PrepareAndSendBatch(changes, excludeUserfields: true);
                    if (!r.Any())
                    {
                        changes.Clear();
                    }
                }

                return this.Ok(changes);
            }
            catch (Exception ex)
            {
                return this.ReportError(ex, "Receive");
            }
        }

        /// <summary>
        /// Reverts to previous version. Will send a request to other dynamics to send the record back.
        /// </summary>
        /// <param name="id">Parcel to revert.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("revert/{id}")]
        [SwaggerOperation(OperationId = "WhatIfRevert")]
        public async Task<ActionResult> Revert(string id)
        {
            try
            {
                Log.Debug($"About to revert changes for Parcel {id}.");
                if (this.Config.IsWhatifEnvironment)
                {
                    var url = DynamicsHelpers.CreateURI(this.Config.ParcelWhatIfUri, "send", id);
                    url = $"{url}?applyChanges=true";
                    var client = new WebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string authHeader = JWTDecoder.GetAuthHeader(this.HttpContext);
                    client.Headers[HttpRequestHeader.Authorization] = authHeader;
                    var result = await client.UploadStringTaskAsync(new Uri(url), "POST", JsonConvert.SerializeObject(new { }));
                    return this.Ok(result);
                }

                throw new Exception("Must run in what-if environment only.");
            }
            catch (WebException ex)
            {
                var t = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return this.ReportError(new Exception(t, ex), "Revert");
            }
            catch (Exception ex)
            {
                return this.ReportError(ex, "Revert");
            }
        }

        /// <summary>
        /// Get what if information.
        /// </summary>
        /// <param name="id">Parcel Id.</param>
        /// <param name="applyChanges">Does it apply the changes.</param>
        /// <param name="excludeFields">Comma separated list of fields to exclude.</param>
        /// <returns>Object.</returns>
        [HttpPost("send/{id}")]
        [SwaggerOperation(OperationId = "WhatIfSender")]
        public async Task<ActionResult> Send(string id, [FromQuery] bool? applyChanges, [FromForm] string excludeFields)
        {
            var exclude = excludeFields ?? string.Empty;
            if (applyChanges is null || !applyChanges.HasValue)
            {
                throw new ArgumentNullException(nameof(applyChanges));
            }

            try
            {
                Log.Debug($"About to send changes for Parcel {id}. Should apply changes: {applyChanges}");
                string result = await this.SendToOtherDynamics(id, applyChanges.Value, exclude);
                return this.Ok(new { result });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex, "Send");
            }
        }

        private async Task<List<DynamicsToDynamicsData>> GetParcelItems(string id, string[] excludeFields)
        {
            var parcelItems = new List<DynamicsToDynamicsData>();

            string[] loadedResponses = await this.LoadAllItems(id);
            if (loadedResponses.Any())
            {
                string parcelStr = loadedResponses.First();
                dynamic originalParcel = this.LoadParcel(id, parcelItems, parcelStr, excludeFields);

                var relatedItems = loadedResponses.Skip(1).Select((itm, index) =>
                {
                    (string entity, string keyfield, string relatingField) = DynamicsTransactionHelper.RelatedEntities[index];
                    return DynamicsHelpers.GetLinkedItems(itm, entity, keyfield);
                }).SelectMany(itm => itm).ToList();

                parcelItems.AddRange(relatedItems);

                await this.LoadLandInfo(parcelItems, originalParcel, excludeFields);
            }

            return parcelItems;
        }

        private async Task<string[]> LoadAllItems(string id)
        {
            var atasks = DynamicsTransactionHelper.RelatedEntities.Select(s => this.TransactionHelper.ExecuteQuery(s.entity, $"$filter=_ptas_{s.relatingField}_value eq '{id}'")).ToList();

            Task<string> parcelTask = this.TransactionHelper.ExecuteQuery(ParcelDetailsName, $"$filter={ParcelDetailsKeyField} eq '{id}'");
            atasks.Insert(0, parcelTask);

            string[] taskResults = await Task.WhenAll(atasks);
            return taskResults;
        }

        private async Task LoadLandInfo(List<DynamicsToDynamicsData> toMoveList, dynamic originalParcel, string[] excludeFields)
        {
            var landId = originalParcel._ptas_landid_value;
            var landStr = await this.TransactionHelper.ExecuteQuery(LandName, $"$filter=ptas_landid eq '{landId}'");
            dynamic landResult = JObject.Parse(landStr);

            if (landResult.value is JArray returnedArray && returnedArray.Any())
            {
                dynamic landInfoObj = returnedArray.First();
                dynamic land = JsonHelper.RemoveEmptyChildren(landInfoObj, LandName, excludeFields);

                toMoveList.Add(new DynamicsToDynamicsData
                {
                    EntityName = LandName,
                    EntityId = landId,
                    EntityJSON = JsonConvert.SerializeObject(
                        land,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        }),
                });
            }
        }

        private dynamic LoadParcel(string id, List<DynamicsToDynamicsData> toMoveList, string parcelStr, string[] excludeFields)
        {
            dynamic parcelInfo = JObject.Parse(parcelStr);
            if (parcelInfo.error != null)
            {
                throw new DynamicsHttpRequestException($"Code: {parcelInfo.error.code}, message: {parcelInfo.error.message}", null);
            }

            dynamic originalParcel;

            if (!(parcelInfo.value is JArray returnedParcelArray) || !returnedParcelArray.Any())
            {
                throw new DynamicsHttpRequestException("Could not find parcel info.", null);
            }

            originalParcel = returnedParcelArray[0];

            dynamic parcel = JsonHelper.RemoveEmptyChildren(originalParcel, ParcelDetailsName, excludeFields);
            toMoveList.Add(new DynamicsToDynamicsData
            {
                EntityId = id,
                EntityName = ParcelDetailsName,
                EntityJSON = JsonConvert.SerializeObject(
                    parcel,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                    }),
            });
            return originalParcel;
        }

        private ActionResult ReportError(Exception ex, string method)
        {
            Log.Error($"{ex.Message}\nStack: {ex.StackTrace}");
            try
            {
                var t = JsonConvert.DeserializeObject(ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, t);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new { method, ex.Message });
            }
        }

        private async Task<string> SendToOtherDynamics(string id, bool applyChanges, string excludeFieldsStr)
        {
            var excludeFields = excludeFieldsStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<DynamicsToDynamicsData> toMoveList = await this.GetParcelItems(id, excludeFields);
            string tail = $"?applyChanges={applyChanges}&excludeFields={excludeFieldsStr}";
            string uri = DynamicsHelpers.CreateURI(this.Config.ParcelWhatIfUri, "receive", id);
            var url = $"{uri}{tail}";

            string authHeader = JWTDecoder.GetAuthHeader(this.HttpContext)?.Replace("Bearer ", string.Empty);
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue(
               "Bearer",
               authHeader);
            var client = DynamicsHelpers.CreateClient(authenticationHeaderValue);
            string serializedContent = JsonConvert.SerializeObject(toMoveList);
            var content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await client.PostAsync(url, content);
            var returned = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                dynamic rot = JsonConvert.DeserializeObject(returned);
                throw new ObjBearingException(rot);
            }

            return returned;
        }
    }
}