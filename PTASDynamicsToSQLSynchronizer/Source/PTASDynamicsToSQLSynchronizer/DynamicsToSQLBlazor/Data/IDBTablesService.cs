// <copyright file="IDBTablesService.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
    }
}