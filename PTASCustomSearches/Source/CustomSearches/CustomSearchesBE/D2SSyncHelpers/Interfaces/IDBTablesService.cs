// <copyright file="IDBTablesService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using D2SSyncHelpers.Models;

    /// <summary>
    /// Service that loads tables from a service.
    /// </summary>
    public interface IDBTablesService
    {
        /// <summary>
        /// Get a list of tables from media.
        /// </summary>
        /// <returns>List of tables.</returns>
        Task<IEnumerable<DBTable>> GetTables();

        /// <summary>
        /// Fetches one table info.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Fetched Data.</returns>
        Task<DBTable> GetTable(string tableName);
    }
}