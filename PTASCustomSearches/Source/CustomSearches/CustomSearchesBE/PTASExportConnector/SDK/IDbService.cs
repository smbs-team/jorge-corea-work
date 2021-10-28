// <copyright file="IDbService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using ConnectorService;

    /// <summary>Declares the methods for the operations related to the database.</summary>
    public interface IDbService
    {
        /// <inheritdoc cref = "DbService.CleanExportedData()"/>
        void CleanExportedData();

        /// <inheritdoc cref = "DbService.GetEntityKeys(Guid, string, string)"/>
        Dictionary<string, Guid> GetEntityKeys(Guid entityGuid, string entityKind);

        /// <inheritdoc cref = "DbService.GetDatabaseModel"/>
        DatabaseModel GetDatabaseModel();

        /// <inheritdoc cref = "DbService.UpdateEntityKeys(Guid, string, string, Guid, string)"/>
        void UpdateEntityKeys(Guid entityGuid, string entityKind, string fieldName, Guid fieldValue);
    }
}