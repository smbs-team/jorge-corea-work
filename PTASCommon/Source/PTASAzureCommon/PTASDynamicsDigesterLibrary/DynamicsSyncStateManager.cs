// <copyright file="DynamicsSyncStateManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsDigesterLibrary
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Services;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Dynamics Sync State Manager.
    /// </summary>
    public class DynamicsSyncStateManager
    {
        private readonly DataAccessLibrary dbAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsSyncStateManager"/> class.
        /// </summary>
        /// <param name="sqlConnectionString">Connection string.</param>
        /// <param name="principalCredentials">Principal credentials.</param>
        public DynamicsSyncStateManager(string sqlConnectionString, ClientCredential principalCredentials)
        {
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                throw new ArgumentException($"'{nameof(sqlConnectionString)}' cannot be null or empty.", nameof(sqlConnectionString));
            }

            this.dbAccess = DynamicsStaticHelper.CreateDBAccess(sqlConnectionString, principalCredentials);
        }

        /// <summary>
        /// Clear Sync State.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>Void task.</returns>
        public async Task ClearSyncState(string entityName)
        {
            await this.dbAccess.SaveData(
                @"  delete from dbo.DynamicsDeltaSyncState
                    where EntityName = @EntityName", new { EntityName = entityName });
        }

        /// <summary>
        /// Gets the last sync url for an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>A task that yields a string when complete.</returns>
        public async Task<string> GetSyncUrlForEntityFromDB(string entityName)
        {
            var result = await this.dbAccess.LoadData<SyncRecord, dynamic>(
                "select SyncUrl from dbo.DynamicsDeltaSyncState where EntityName = @EntityName", new { EntityName = entityName });
            return result.FirstOrDefault()?.SyncUrl;
        }

        /// <summary>
        /// Reset a table.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>Task with no content.</returns>
        public async Task ResetTable(string entityName)
        {
            await this.dbAccess.SaveData<dynamic>(
               "delete from dynamics." + entityName, new { });
        }

        /// <summary>
        /// Saves the sync url for an entity in the DB.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="syncUrl">Delta Url returned by dynamics.</param>
        /// <returns>Task.</returns>
        public async Task SetSyncUrlForEntity(string entityName, string syncUrl)
        {
            await this.ClearSyncState(entityName);
            await this.dbAccess.SaveData<dynamic>(
               "insert into dbo.DynamicsDeltaSyncState (EntityName, SyncUrl) values (@EntityName, @SyncUrl)", new { EntityName = entityName, SyncUrl = syncUrl });
        }

        private class SyncRecord
        {
            public string SyncUrl { get; internal set; }
        }
    }
}