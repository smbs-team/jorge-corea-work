// <copyright file="GenericDynamicsControllerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json.Linq;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Base for controllers that need to apply dynamics.
    /// </summary>
    public abstract class GenericDynamicsControllerBase : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly ITokenManager tokenManager;
        private DynamicsTransactionHelper transactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDynamicsControllerBase"/> class.
        /// </summary>
        /// <param name="config">App config.</param>
        /// <param name="memoryCache">Memory Cache.</param>
        /// <param name="tokenManager">Token.</param>
        protected GenericDynamicsControllerBase(IConfigurationParams config, IMemoryCache memoryCache, ITokenManager tokenManager)
        {
            this.Config = config;
            this.memoryCache = memoryCache;
            this.tokenManager = tokenManager;
        }

        /// <summary>
        /// Gets app config.
        /// </summary>
        protected IConfigurationParams Config { get; }

        /// <summary>
        /// Gets local transaction helper.
        /// </summary>
        protected DynamicsTransactionHelper TransactionHelper
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
        /// Executes a query against dynamics.
        /// </summary>
        /// <param name="entityRequest">Item to executed.</param>
        /// <returns>Result of execution of query.</returns>
        protected async Task<string> ExecuteQuery(EntityRequest entityRequest)
        {
            string queryString = null;
            if (!string.IsNullOrEmpty(entityRequest.EntityId))
            {
                queryString = $"$filter={entityRequest.EntityName}id eq '{entityRequest.EntityId}'";
            }

            if (!string.IsNullOrEmpty(entityRequest.Query))
            {
                queryString = entityRequest.Query;
            }

            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentException("No conditions provided for query.");
            }

            return await this.TransactionHelper.ExecuteQuery(entityRequest.EntityName.Pluralize(), queryString);
        }

        /// <summary>
        /// Get a list of items.
        /// </summary>
        /// <param name="requests">Items to get.</param>
        /// <param name="onlyPtas">Only return fields prefixed with ptas_.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected async Task<EntityRequestResult[]> FetchItems(EntityRequest[] requests, bool onlyPtas)
        {
            var tasks = requests
                .Select(itm => this.ExecuteQuery(itm));
            var jsonResults = await Task.WhenAll(tasks);
            var results = jsonResults
                .Select(jsonText => JObject.Parse(jsonText))
                .Select((dynamic parsedObject)
                    => parsedObject.value as JArray)
                .Select((JArray items, int idx)
                    => (items, related: requests[idx]))
                .Select(resultTuple
                    =>
                {
                    return resultTuple.items.Select(currItem => new EntityRequestResult
                    {
                        EntityId = resultTuple.related.EntityId,
                        EntityName = resultTuple.related.EntityName,
                        Changes = JsonHelper.RemoveEmptyChildren(currItem, resultTuple.related.EntityName, new string[] { }, onlyPtas),
                    });
                })
                .SelectMany(_ => _)
                .ToArray();
            return results;
        }
    }
}