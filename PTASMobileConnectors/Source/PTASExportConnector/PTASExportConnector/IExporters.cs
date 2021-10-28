// <copyright file="IExporters.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using PTASExportConnector.SDK;

    /// <summary>Declares the methods for initiating the export process.</summary>
    public interface IExporters
    {
        /// <inheritdoc cref = "Exporters.GetEntityList(string)"/>
        IList<Entity> GetEntityList(string route);

        /// <inheritdoc cref = "Exporters.Export(IConnector, Guid, string)"/>
        List<string> Export(IConnector connector, Guid deviceGuid, string entityName);
    }
}