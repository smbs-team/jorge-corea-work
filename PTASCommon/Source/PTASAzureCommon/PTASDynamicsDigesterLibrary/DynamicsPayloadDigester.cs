// <copyright file="DynamicsPayloadDigester.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsDigesterLibrary
{
    using System;
    using System.Threading.Tasks;

    using D2SSyncHelpers.Models;
    using D2SSyncHelpers.Services;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using PTASDynamicsDigesterLibrary.Classes;

    /// <summary>
    /// Dynamics Processor Base.
    /// </summary>
    public class DynamicsPayloadDigester
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private readonly string sqlConnectionString;

        private readonly string organizationId;
        private readonly string organizationName;
        private readonly ClientCredential principalCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsPayloadDigester"/> class.
        /// </summary>
        /// <param name="sqlConnectionString">Connection.</param>
        /// <param name="organizationId">Organization.</param>
        /// <param name="organizationName">Organization name.</param>
        /// <param name="principalCredentials">Principal credentials.</param>
        public DynamicsPayloadDigester(string sqlConnectionString, string organizationId, string organizationName, ClientCredential principalCredentials)
        {
            this.sqlConnectionString = sqlConnectionString;
            this.organizationId = organizationId;
            this.organizationName = organizationName;
            this.principalCredentials = principalCredentials;
            DapperConfig.ConfigureMapper(typeof(DBTable), typeof(DBField), typeof(TableChange));
        }

        /// <summary>
        /// Process string from payload.
        /// </summary>
        /// <param name="queueItem">Queue item to process. Assumes a twice quoted json string.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<(ReceivedEntityInfo info, string result)> DigestItem(string queueItem)
        {
            try
            {
                DataAccessLibrary dbAccess = DynamicsStaticHelper.CreateDBAccess(this.sqlConnectionString, this.principalCredentials);
                ReceivedEntityInfo info = DynamicsStaticHelper.GetDynamicsMessage(queueItem);
                if (!string.IsNullOrEmpty(this.organizationId) && !(info.OrganizationId == this.organizationId && info.OrganizationName == this.organizationName))
                {
                    throw new Exception($"Ignored record for organization {info.OrganizationId}");
                }

                var wrapper = new ReceivedInfoSimplfier(info);
                if (wrapper.IsUpdate)
                {
                    var (records, result) = await DynamicsStaticHelper.UpdateRecord(dbAccess, wrapper);
                    bool hadRecords = records > 0;
                    if (hadRecords)
                    {
                        return (info, $"OK: Updated Record:\n{queueItem}. {result}");
                    }
                }

                if (wrapper.IsUpdate || wrapper.IsInsert)
                {
                    await DynamicsStaticHelper.InsertRecord(dbAccess, wrapper);
                    return (info, $"OK: Inserted Record:\n{queueItem}");
                }

                throw new Exception($"Did not process, item was not insert or update.");
            }
            catch (Exception ex)
            {
                return (null, $"ERROR: {ex.Message}\n{ex.InnerException?.Message}\n{queueItem}");
            }
        }
    }
}