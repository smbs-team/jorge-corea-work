// <copyright file="IOdata.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System.Threading.Tasks;
    using PTASketchFileMigratorConsoleApp;

    /// <summary>Defines Odata methods.</summary>
    public interface IOdata
    {
        /// <summary>Sends a create requests.</summary>
        /// <param name="entityName">The entity name to query.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>The response of the operation.</returns>
        Task<OdataResponse> Get(string entityName, string query);
    }
}