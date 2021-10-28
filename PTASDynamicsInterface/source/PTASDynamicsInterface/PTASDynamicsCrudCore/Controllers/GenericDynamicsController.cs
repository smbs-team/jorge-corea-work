// <copyright file="GenericDynamicsController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json;

    using PTASCRMHelpers;

    using PTASDynamicsCrudCore.Classes;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Applies a set of items to dynamics.
    /// </summary>
    [Route("v1/api/[controller]")]
    public class GenericDynamicsController : GenericDynamicsControllerBase
    {
        private const string TrueStr = "true";
        private readonly CRMWrapper wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDynamicsController"/> class.
        /// </summary>
        /// <param name="config">Sys config.</param>
        /// <param name="memoryCache">Memory cache.</param>
        /// <param name="tokenManager">Token.</param>
        /// <param name="wrapper">CRM wrapper.</param>
        public GenericDynamicsController(IConfigurationParams config, IMemoryCache memoryCache, ITokenManager tokenManager, CRMWrapper wrapper)
            : base(config, memoryCache, tokenManager)
        {
            this.wrapper = wrapper;
        }

        /// <summary>
        /// Gets a list of items.
        /// </summary>
        /// <param name="requests">List of items to get.</param>
        /// <returns>Got items if found.</returns>
        [HttpPost("GetItems")]
        public async Task<ActionResult> GetItems([FromBody] EntitysetRequests requests)
        {
            try
            {
                var requestStr = this.Request.Query["onlyPtas"].ToString();
                var onlyPtas = TrueStr.Equals(requestStr == string.Empty ? TrueStr : requestStr);
                var processPipe = await this.FetchItems(requests.Requests, onlyPtas);
                return new OkObjectResult(new { Items = processPipe });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Attempts to apply json to dynamics.
        /// </summary>
        /// <param name="changes">Json to apply.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("UpdateItems")]
        [SwaggerOperation(OperationId = "UpdateItems")]
        public async Task<ActionResult> UpdateItems([FromBody] EntitysetChanges changes)
        {
            try
            {
                return new OkObjectResult(await this.TransactionHelper.PrepareAndSendBatch(
                 changes.Items.Select(c => new EntityChanges
                 {
                     Changes = c.Changes,
                     EntityId = c.EntityId,
                     EntityName = c.EntityName,
                 }), false));
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Attempts to apply json to dynamics.
        /// </summary>
        /// <param name="data">Data received.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("LinkEntities")]
        [SwaggerOperation(OperationId = "LinkEntities")]
        public async Task<ActionResult> LinkEntities([FromBody] LinkEntitiesData data)
        {
            try
            {
                await this.TransactionHelper.LinkEntities(data, this.wrapper);
                return new OkObjectResult(new { result = "Ok" });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Attempts to apply json to dynamics.
        /// </summary>
        /// <param name="data">Data received.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("UnlinkEntities")]
        [SwaggerOperation(OperationId = "UnlinkEntities")]
        public async Task<ActionResult> UnlinkEntities([FromBody] LinkEntitiesData data)
        {
            try
            {
                await this.TransactionHelper.UnlinkEntities(data, this.wrapper);
                return new OkObjectResult(new { result = "Ok" });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Deletes items by ID from dynamics.
        /// Warning: deletions are final.
        /// </summary>
        /// <param name="deletions">List of deletions.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("DeleteItems")]
        [SwaggerOperation(OperationId = "DeleteItems")]
        public async Task<ActionResult> DeleteItems([FromBody] EntityDeletions deletions)
        {
            try
            {
                bool[] boolResults = await Task.WhenAll(deletions.Items.ToArray().Select(s => this.wrapper.ExecuteDelete(s.EntityName.Pluralize(), s.EntityId)));
                var results = boolResults.Select((result, i) => new
                {
                    Id = deletions.Items.ToArray()[i].EntityId,
                    result,
                });
                return new OkObjectResult(new
                {
                    results,
                });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        private ObjectResult ReportError(Exception ex)
            => ex.Message.StartsWith("{") ?
            this.StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.DeserializeObject(ex.Message)) :
            this.StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message,
            });
    }
}