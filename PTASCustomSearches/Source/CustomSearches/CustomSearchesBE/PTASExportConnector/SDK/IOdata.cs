// <copyright file="IOdata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>Declares the methods for the management of the data.</summary>
    public interface IOdata
    {
        /// <inheritdoc cref = "Odata.Create(string, object, string)"/>
        Task<HttpResponseMessage> Create(string apiRequest, object toSave, string selectFields);

        /// <inheritdoc cref = "Odata.Delete(string)"/>
        Task<HttpResponseMessage> Delete(string apiRequest);

        /// <inheritdoc cref = "Odata.Init(string, string, string, string)"/>
        void Init(string crmUri, string authUri, string clientId, string clientSecret);

        /// <inheritdoc cref = "Odata.Update{T}(string, T, string)"/>
        Task<HttpResponseMessage> Update<T>(string queryStr, T entity, string keyStr);
    }
}