// <copyright file="IBackendService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ConnectorService;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>Declares back-end service methods.</summary>
    public interface IBackendService
    {
        /// <inheritdoc cref = "BackendService.GetData(EntityModel, string, DateTime?, IConnector, long)"/>
        List<Dictionary<string, object>> GetData(EntityModel entity, string backendConnectionString, string sqlConnectionString, DateTime? lastDate, IConnector connector, long uploadTicket, bool isBulkInsert, ILogger looger, ClientCredential principalCredentials);

        /// <inheritdoc cref = "BackendService.BulkProcess(string, string, EntityModel, long)"/>
        void BulkProcess(string backendConnectionString, string sqlConnectionString, EntityModel entity, long uploadTicket, List<Dictionary<string, object>> uploadList, IConnector connector);

        /// <inheritdoc cref = "DataKeyIsEmpty(string sqlConnectionString, EntityModel entity)"/>
        void DataKeyIsEmptyAsync(string sqlConnectionString, EntityModel entity);
        void DisableorRebuildIndex(string sqlConnectionString, string name, int v);
    }
}