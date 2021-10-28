// <copyright file="DbModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using ConnectorService;
    using ConnectorService.Utilities;

    /// <summary>DbModel.</summary>
    public class DbModel : IDbModel
    {
        /// <summary>Get the database model from the .dbx file.</summary>
        /// <returns>The database model.</returns>
        public DatabaseModel GetDatabaseModel()
        {
            return DatabaseModelHelper.GetDatabaseModel();
        }
    }
}
