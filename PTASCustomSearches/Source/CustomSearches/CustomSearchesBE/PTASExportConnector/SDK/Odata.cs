// <copyright file="Odata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using PTASCRMHelpers;
    using PTASCRMHelpers.Exceptions;
    using PTASExportConnector.Exceptions;

    /// <summary>Wrapper class for the OData functionality.</summary>
    /// <seealso cref="PTASExportConnector.SDK.IOdata" />
    public class Odata : IOdata
    {
        private PTASCRMHelpers.Utilities.CrmOdataHelper odata;

        /// <summary>Instantiates a OData connection.</summary>
        /// <param name="crmUri">The CRM Uri.</param>
        /// <param name="authUri">
        ///   <para>The authentication Uri.</para>
        /// </param>
        /// <param name="clientId">The Client ID.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <inheritdoc cref="Odata.Init(string, string, string, string)" />
        public void Init(string crmUri, string authUri, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(crmUri))
            {
                throw new ExportConnectorException("Error while initiating OData connection.", StatusCodes.Status400BadRequest, new ArgumentException("crmUri is null or empty."));
            }

            if (string.IsNullOrEmpty(authUri))
            {
                throw new ExportConnectorException("Error while initiating OData connection.", StatusCodes.Status400BadRequest, new ArgumentException("authUri is null or empty."));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ExportConnectorException("Error while initiating OData connection.", StatusCodes.Status400BadRequest, new ArgumentException("clientId is null or empty."));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ExportConnectorException("Error while initiating OData connection.", StatusCodes.Status400BadRequest, new ArgumentException("clientSecret is null or empty."));
            }

            TokenManager token = new TokenManager();
            this.odata = new PTASCRMHelpers.Utilities.CrmOdataHelper(crmUri, authUri, clientId, clientSecret, token);
        }

        /// <summary>Sends a create requests.</summary>
        /// <param name="apiRequest">The web API URL and the entity to create.</param>
        /// <param name="toSave">The JSON containing the data to create.</param>
        /// <param name="selectFields">Optional select filter.</param>
        /// <returns>The response of the operation.</returns>
        /// <inheritdoc cref="Odata.Create(string, object, string)" />
        public async Task<HttpResponseMessage> Create(string apiRequest, object toSave, string selectFields)
        {
            if (string.IsNullOrEmpty(apiRequest))
            {
                throw new ExportConnectorException("OData - Error while trying to send a create request, apiRequest is null or empty.", StatusCodes.Status400BadRequest);
            }

            if (toSave == null)
            {
                throw new ExportConnectorException("OData - Error while trying to send a create request, JSON was not provided.", StatusCodes.Status400BadRequest);
            }

            try
            {
                return await this.odata.CrmWebApiFormattedPostRequest(apiRequest, toSave, selectFields);
            }
            catch (DynamicsHttpRequestException ex)
            {
                throw new ExportConnectorException("OData - Error while trying send a create request. Message: " + ex.Message, StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("OData - Error while trying send a create request.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
        }

        /// <summary>Sends an update request.</summary>
        /// <typeparam name="T">The JSON containing the data to update.</typeparam>
        /// <param name="queryStr">The web API URL.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="keyStr">The entity primary key.</param>
        /// <inheritdoc cref="Odata.Update{T}(string, T, string)" />
        public async Task<HttpResponseMessage> Update<T>(string queryStr, T entity, string keyStr)
        {
            if (string.IsNullOrEmpty(queryStr))
            {
                throw new ExportConnectorException("OData - Error while trying to send an update request.", StatusCodes.Status400BadRequest, new ArgumentException("queryStr is null or empty."));
            }

            if (entity == null)
            {
                throw new ExportConnectorException("OData - Error while trying to send an update request, JSON was not provided.", StatusCodes.Status400BadRequest, new InvalidOperationException("T entity is null."));
            }

            if (string.IsNullOrEmpty(keyStr))
            {
                throw new ExportConnectorException("OData - Error while trying to send an update request.", StatusCodes.Status400BadRequest, new ArgumentException("keyStr"));
            }

            try
            {
               return await this.odata.CrmWebApiFormattedPatchRequest(queryStr, entity, keyStr);
            }
            catch (DynamicsHttpRequestException ex)
            {
                throw new ExportConnectorException("OData - Error while trying to send an update request. Message: " + ex.Message, StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("OData - Error while trying to send an update request.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
        }

        /// <summary>Sends a delete request.</summary>
        /// <param name="apiRequest">The web API URL, entity name and primary key.</param>
        /// <inheritdoc cref="Odata.Delete(string)" />
        public async Task<HttpResponseMessage> Delete(string apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest))
            {
                throw new ExportConnectorException("OData - Error while trying to send a delete request.", StatusCodes.Status400BadRequest, new ArgumentException("apiRequest is null or empty."));
            }

            try
            {
                return await this.odata.CrmWebApiFormattedDeleteRequest(apiRequest);
            }
            catch (DynamicsHttpRequestException ex)
            {
                throw new ExportConnectorException("OData - Error while trying to send a delete request. Message: " + ex.Message, StatusCodes.Status400BadRequest, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("OData - Error while trying to send a delete request.", StatusCodes.Status400BadRequest, ex.InnerException);
            }
        }



    }
}
