// <copyright file="IDynamicsUtility.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsSyncLibrary
{
    using System.Threading.Tasks;

    using D2SSyncHelpers.Models;

    using Microsoft.Extensions.Logging;

    using PTASDynamicsDigesterLibrary.Classes;

    /// <summary>
    /// Interface for dynamics utility.
    /// </summary>
    public interface IDynamicsUtility
    {
        /// <summary>
        /// Attmpts to sync an entity.
        /// </summary>
        /// <param name="dbTable">DB Table name.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="timeout">Milliseconds before stopping process.</param>
        /// <param name="batchCopy">Use batch copy.</param>
        /// <param name="chunkSize">Number of records to process in parallel.</param>
        /// <param name="waitWhenFailMS">How many MS to wait after a failure.
        /// If zero it will exit on failure.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task Sync(string dbTable, string entityName, int timeout, bool batchCopy, int chunkSize, int waitWhenFailMS);

        /// <summary>
        /// Clears the sync state for entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="resetSQLTable">Reset sql table.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task ClearSyncState(string entityName, bool resetSQLTable);

        /// <summary>
        /// Retrieves dependent entitiste.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<EntityManyToManyReference[]> GetDependentEntities(string entityName);

        /// <summary>
        /// Attempts to update a record in dynamics from a json double quoted string.
        /// </summary>
        /// <param name="payload">JSON Payload to apply.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<(ReceivedEntityInfo info, string result)> UpdateDbRecord(string payload);
    }
}