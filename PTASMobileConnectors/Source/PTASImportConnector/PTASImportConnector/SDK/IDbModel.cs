// <copyright file="IDbModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using ConnectorService;

    /// <summary>Declares data base model methods.</summary>
    public interface IDbModel
    {
        /// <inheritdoc cref = "DbModel.GetDatabaseModel"/>
        DatabaseModel GetDatabaseModel();
    }
}