// <copyright file="IConnector.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System;
    using System.Collections.Generic;

    /// <summary>Declares connector SDK methods.</summary>
    public interface IConnector
    {
        /// <inheritdoc cref = "Connector.GetLastImportDate"/>
        DateTime? GetLastImportDate();

        /// <inheritdoc cref = "Connector.IsRunning"/>
        bool IsRunning();

        /// <inheritdoc cref = "Connector.GetLastImportEntityDate(string)"/>
        DateTime? GetLastImportEntityDate(string entityName);

        /// <inheritdoc cref = "Connector.GetUploadTicketForBackend"/>
        long GetUploadTicketForBackend();

        /// <inheritdoc cref = "Connector.Init(string)"/>
        void Init(string sqlConnectionString, ClientCredential principalCredentials);

        /// <inheritdoc cref = "Connector.AddUploadData(string, List{Dictionary{string, object}}, long)"/>
        void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket);

        /// <inheritdoc cref = "Connector.ProcessDataForTicket(long, bool, bool)"/>
        void ProcessDataForTicket(long changeSetID, bool fromDevice, bool lastExecution);

        /// <inheritdoc cref = "Connector.SetImportDate(DateTime)"/>
        void SetImportDate(DateTime importDate);

        /// <inheritdoc cref = "Connector.SetRunningStatus(DateTime, bool)"/>
        void SetRunningStatus(DateTime importDate, bool isRunning);

        /// <inheritdoc cref = "Connector.SetImportEntityDate(string, DateTime)"/>
        void SetImportEntityDate(string entityName, DateTime importDate);
    }
}