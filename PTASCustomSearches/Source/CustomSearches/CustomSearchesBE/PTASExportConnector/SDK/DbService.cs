// <copyright file="DbService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using ConnectorService;
    using ConnectorService.Utilities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASExportConnector.Exceptions;
    using PTASServicesCommon.TokenProvider;

    /// <summary>Contains database related methods.</summary>
    /// <seealso cref="PTASExportConnector.SDK.IDbService" />
    public class DbService : IDbService
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const string ConnectionStringPasswordSection = "password";

        private string sqlConnectionString;
        private string token;

        /// <summary>Initializes a new instance of the <see cref="DbService"/> class.</summary>
        /// <param name="sqlConnectionString">SQLConnection.</param>
        /// <param name="clientCredential">Credential.</param>
        public DbService(string sqlConnectionString, ClientCredential clientCredential)
        {
            this.sqlConnectionString = sqlConnectionString;

            if (string.IsNullOrEmpty(this.sqlConnectionString))
            {
                throw new ExportConnectorException("Error while trying to initiate the database service, sqlConnectionString is null or empty.", StatusCodes.Status400BadRequest);
            }

            ClientCredential principalCredentials = clientCredential;
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();

            string token = null;
            if (principalCredentials == null)
            {
                token = Task.Run(async () =>
                {
                    return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                }).Result;
            }
            else
            {
                token = Task.Run(async () =>
                {
                    return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, principalCredentials);
                }).Result;
            }

            this.token = token;
        }

        /// <summary>Cleans the exported data.</summary>
        public void CleanExportedData()
        {
            using var middleTierConnection = new SqlConnection(this.sqlConnectionString);
            middleTierConnection.AccessToken = this.token;
            using SqlCommand middleTierCommand = new SqlCommand("CleanExportedData", middleTierConnection);

            try
            {
                middleTierCommand.CommandType = System.Data.CommandType.StoredProcedure;
                middleTierConnection.Open();
                middleTierCommand.ExecuteNonQuery();
                middleTierConnection.Close();
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying to clear the exported data. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("Error while cleaning the exported data.", StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        /// <summary>Gets the dynamic GUID based on the Middle-tier GUID.</summary>
        /// <param name="entityGuid">The middle tier GUID.</param>
        /// <param name="entityKind">The entity name (table name of the database).</param>
        /// <returns>The dynamics GUID.</returns>
        public Dictionary<string, Guid> GetEntityKeys(Guid entityGuid, string entityKind)
        {
            if (entityGuid == Guid.Empty)
            {
                throw new ExportConnectorException("Error while trying to get the entity keys.", StatusCodes.Status400BadRequest, new InvalidOperationException("entityGuid is null."));
            }

            if (string.IsNullOrEmpty(entityKind))
            {
                throw new ExportConnectorException("Error while trying to get the entity keys", StatusCodes.Status400BadRequest, new ArgumentException("entityKind is null or empty."));
            }

            using SqlConnection middleTierConnection = new SqlConnection(this.sqlConnectionString);
            middleTierConnection.AccessToken = this.token;
            using SqlCommand middleTierCommand = new SqlCommand("select * from " + entityKind + "_Key where guid_mb = '" + entityGuid.ToString() + "'", middleTierConnection);

            try
            {
                Dictionary<string, Guid> entityKeys = new Dictionary<string, Guid>();
                middleTierConnection.Open();
                SqlDataReader middletierReader = middleTierCommand.ExecuteReader();
                if (middletierReader.Read())
                {
                    for (int i = 0; i < middletierReader.FieldCount; i++)
                    {
                        if (middletierReader.GetName(i) != "guid_mb")
                        {
                            entityKeys.Add(middletierReader.GetName(i), (Guid)middletierReader[i]);
                        }
                    }
                }

                middleTierConnection.Close();
                return entityKeys;
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying to get the entity keys. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("Error while trying to get the entity keys.", StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        /// <summary>Gets the database model.</summary>
        /// <returns>The model.</returns>
        /// <inheritdoc cref="DbService.GetDatabaseModel" />
        public DatabaseModel GetDatabaseModel()
        {
            try
            {
                return DatabaseModelHelper.GetDatabaseModel();
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("Error while trying get the database model.", StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        /// <summary>Updates the entity keys.</summary>
        /// <param name="entityGuid">The GUID.</param>
        /// <param name="entityKind">The name of the entity.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldValue">The field value.</param>
        public void UpdateEntityKeys(Guid entityGuid, string entityKind, string fieldName, Guid fieldValue)
        {
            if (entityGuid == Guid.Empty)
            {
                throw new ExportConnectorException("Error while trying to get the entity keys.", StatusCodes.Status400BadRequest, new InvalidOperationException("entityGuid is null."));
            }

            if (string.IsNullOrEmpty(entityKind))
            {
                throw new ExportConnectorException("Error while trying to get the entity keys.", StatusCodes.Status400BadRequest, new ArgumentException("entityKind is null or empty."));
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ExportConnectorException("Error while trying to get the entity keys, fieldName is null or empty", StatusCodes.Status400BadRequest, new ArgumentException("fieldName is null or empty."));
            }

            if (fieldValue == Guid.Empty)
            {
                throw new ExportConnectorException("Error while trying to get the entity keys, fieldValue is null or empty", StatusCodes.Status400BadRequest, new InvalidOperationException("entityGuid is null."));
            }

            using SqlConnection middleTierConnection = new SqlConnection(this.sqlConnectionString);
            middleTierConnection.AccessToken = this.token;
            using SqlCommand middleTierCommand = new SqlCommand("update " + entityKind + "_Key set " + fieldName + " = '" + fieldValue.ToString() + "' where guid_mb = '" + entityGuid.ToString() + "'", middleTierConnection);

            try
            {
                middleTierConnection.Open();
                middleTierCommand.ExecuteNonQuery();
                middleTierConnection.Close();
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying update the entity keys. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ExportConnectorException("Error while trying to update the entity keys.", StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }
    }
}
