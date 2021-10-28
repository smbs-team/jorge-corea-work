// <copyright file="IExporters.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASExportConnector.SDK;

    /// <summary>Declares the methods for initiating the export process.</summary>
    public interface IExporters
    {
        /// <inheritdoc cref = "Exporters.ExportAsync(IConnector, Guid, string)"/>
        Task<List<string>> ExportAsync(Connector connector, Guid deviceGuid, string entityName);
    }
}