// <copyright file="IConnector.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System;
    using System.Collections.Generic;

    /// <summary>Declares connector SDK methods.</summary>
    public interface IConnector
    {
        /// <inheritdoc cref = "Connector.Init()"/>
        void Init();

        /// <inheritdoc cref = "Connector.Init()"/>
        void Init(string sqlConnectionString, ClientCredential principalCredentials);

        /// <inheritdoc cref = "Connector.GetDeviceGuidList"/>
        List<Guid> GetDeviceGuidList();

        /// <inheritdoc cref = "Connector.GetModifiedEntityData(string, string, Guid)"/>
        List<Dictionary<string, object>> GetModifiedEntityData(string rootId, string entityKind, Guid deviceGuid);

        /// <inheritdoc cref = "Connector.SetChangesetsExported(List{string}) />
        void SetChangesetsExported(List<string> changesetIds);
    }
}