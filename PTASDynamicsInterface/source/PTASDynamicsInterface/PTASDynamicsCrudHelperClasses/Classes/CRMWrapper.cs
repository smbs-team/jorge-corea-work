// <copyright file="CRMWrapper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Wrapper for dynamics CRM.
    /// </summary>
    public class CRMWrapper
    {
        private const int HttpReturnStatusCode = 502;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRMWrapper"/> class.
        /// </summary>
        /// <param name="config">Configuration params.</param>
        /// <param name="tokenManager">Token Manager.</param>
        public CRMWrapper(IConfigurationParams config, ITokenManager tokenManager)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(config.CRMUri))
            {
                throw new ArgumentNullException(nameof(config.CRMUri));
            }

            if (string.IsNullOrEmpty(config.AuthUri))
            {
                throw new ArgumentNullException(nameof(config.AuthUri));
            }

            if (string.IsNullOrEmpty(config.ClientId))
            {
                throw new ArgumentNullException(nameof(config.ClientId));
            }

            if (string.IsNullOrEmpty(config.ClientSecret))
            {
                throw new ArgumentNullException(nameof(config.ClientSecret));
            }

            this.CRMHelper = new CrmOdataHelper(config.CRMUri, config.AuthUri, config.ClientId, config.ClientSecret, tokenManager);
            this.Config = config;
        }

        private IConfigurationParams Config { get; }

        private CrmOdataHelper CRMHelper { get; }

        /// <summary>
        /// Delete a row in table name identified by id.
        /// </summary>
        /// <param name="tableName">Name of entity.</param>
        /// <param name="id">id of the row.</param>
        /// <returns>true if delete is done, false if not.</returns>
        public virtual async Task<bool> ExecuteDelete(string tableName, string id)
        {
            var queryStr = $"{this.Config.ApiRoute}{tableName}({id})";
            try
            {
                var response = await this.CRMHelper.CrmWebApiFormattedDeleteRequest(queryStr);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        throw new DynamicsInterfaceException($"Can't delete on cascade on:{this.Config.CRMUri}{queryStr}. Details:{response.ReasonPhrase}", (int)HttpStatusCode.MethodNotAllowed, null);
                    }

                    DynamicsHttpExceptions.ThrowException(response, this.Config.CRMUri + queryStr);
                }

                var s = await CrmOdataHelper.GetResponseString(response);
                return response.IsSuccessStatusCode;
            }
            catch (PTASCRMHelpers.Exception.DynamicsHttpRequestException ex)
            {
                throw new DynamicsInterfaceException("HttpRequest error on " + this.Config.CRMUri + queryStr + "(" + ex.Message + ")", HttpReturnStatusCode, ex);
            }
        }

        /// <summary>
        /// Gets optionsets for identity.
        /// </summary>
        /// <param name="tableId">Entity.</param>
        /// <param name="elementId">Element.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<OptionSet>> GetOptionsets(string tableId, string elementId)
        {
            var result = (await this.ExecuteGet<OptionSet>(
            "stringmaps",
            $"$filter=attributename eq '{elementId}' and objecttypecode eq '{tableId}'"))
                .OrderBy(itm => itm.Displayorder);
            return result;
        }

        /// <summary>
        /// Executes a get from the crm.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <param name="query">query to execute.</param>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <returns>Array of objects.</returns>
        public virtual async Task<T[]> ExecuteGet<T>(string tableName, string query = "")
        {
            query = string.IsNullOrEmpty(query) ? string.Empty : $"&{query}";
            var queryStr = $"{this.Config.ApiRoute}{tableName}?$count=true{query}";
            var response = await this.CRMHelper.CrmWebApiFormattedGetRequest(queryStr);
            if (!response.IsSuccessStatusCode)
            {
                DynamicsHttpExceptions.ThrowException(response, this.Config.CRMUri + queryStr);
            }

            var s = await CrmOdataHelper.GetResponseString(response);
            if (s == null)
            {
                return new T[] { };
            }

            var r = JsonConvert.DeserializeObject<CRMHeader<T>>(s);
            if (r.Count <= 0)
            {
                return new T[] { };
            }

            return r.Values;
        }

        /// <summary>
        /// Execute a N to N relationship post.
        /// </summary>
        /// <typeparam name="T">Item to save.</typeparam>
        /// <param name="tableName">On what table.</param>
        /// <param name="entity">Entity of type T to save.</param>
        /// <param name="keyValue">The key value on TableName for the N to N relationship.</param>
        /// <param name="navigationPropertyStr">Name of navitation property.</param>
        /// <returns>Nothing, just task.</returns>
        public virtual async Task<bool> ExecuteNTONPost<T>(string tableName, T entity, string keyValue, string navigationPropertyStr)
        {
            var queryStr = $"{this.Config.ApiRoute}{tableName}";
            try
            {
                var response = await this.CRMHelper.CrmWebApiFormattedNtoNPostRequest(queryStr, entity, keyValue, navigationPropertyStr);
                if (!response.IsSuccessStatusCode)
                {
                    DynamicsHttpExceptions.ThrowException(response, $"{this.Config.CRMUri}{queryStr}({keyValue})/{navigationPropertyStr}/$ref");
                }

                var s = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            catch (PTASCRMHelpers.Exception.DynamicsHttpRequestException ex)
            {
                throw new DynamicsInterfaceException("HttpRequest error on " + $"{this.Config.CRMUri}{queryStr}({keyValue})/{navigationPropertyStr}/$ref ({ex.Message})", HttpReturnStatusCode, ex);
            }
        }

        /// <summary>
        /// Execute a patch update.
        /// </summary>
        /// <typeparam name="T">Item to save.</typeparam>
        /// <param name="tableName">On what table.</param>
        /// <param name="entity">Entity of type T to save.</param>
        /// <param name="keyStr">Field for search.</param>
        /// <returns>Nothing, just task.</returns>
        public virtual async Task<bool> ExecutePatch<T>(string tableName, T entity, string keyStr)
        {
            var queryStr = $"{this.Config.ApiRoute}{tableName}";
            try
            {
                var response = await this.CRMHelper.CrmWebApiFormattedPatchRequest(queryStr, entity, keyStr);
                var s = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    DynamicsHttpExceptions.ThrowException(response, $"{this.Config.CRMUri}{queryStr}({keyStr})");
                }

                return response.IsSuccessStatusCode;
            }
            catch (PTASCRMHelpers.Exception.DynamicsHttpRequestException ex)
            {
                throw new DynamicsInterfaceException("HttpRequest error on " + $"{this.Config.CRMUri}{queryStr}({keyStr})  ({ex.Message})", HttpReturnStatusCode, ex);
            }
        }

        /// <summary>
        /// Execute a post.
        /// </summary>
        /// <typeparam name="T">Item to save.</typeparam>
        /// <param name="tableName">On what table.</param>
        /// <param name="entity">Entity of type T to save.</param>
        /// <param name="selectFields">fields to return.</param>
        /// <returns>Nothing, just task.</returns>
        public virtual async Task<bool> ExecutePost<T>(string tableName, T entity, string selectFields)
        {
            var queryStr = $"{this.Config.ApiRoute}{tableName}";
            try
            {
                var response = await this.CRMHelper.CrmWebApiFormattedPostRequest(queryStr, entity, selectFields);
                ////var content = await response.Content.ReadAsStringAsync();
                if (!(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.PreconditionFailed))
                {
                    DynamicsHttpExceptions.ThrowException(response, this.Config.CRMUri + queryStr + (string.IsNullOrEmpty(selectFields) ? string.Empty : selectFields));
                }

                var s = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            catch (PTASCRMHelpers.Exception.DynamicsHttpRequestException ex)
            {
                throw new DynamicsInterfaceException("HttpRequest error on " + this.Config.CRMUri + queryStr + (string.IsNullOrEmpty(selectFields) ? string.Empty : selectFields) + "(" + ex.Message + ")", HttpReturnStatusCode, ex);
            }
        }

        /// <summary>
        /// Redaction tool url in this dynamics instalation.
        /// </summary>
        /// <param name="assignedDocumentId">Assigned Document Id.</param>
        /// <returns>Created URL.</returns>
        public string RedactionToolUrl(Guid assignedDocumentId)
            => $"{this.Config.CRMUri}/WebResources/ptas_LandingPage?data=documentid%3d{assignedDocumentId}";
    }
}