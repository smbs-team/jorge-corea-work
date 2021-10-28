// <copyright file="Connector.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Microsoft.AspNetCore.Http;
    using PTASConnectorSDK;
    using PTASExportConnector.Exceptions;

    /// <summary>ConnectorSDK wrapper.</summary>
    public class Connector : IConnector
    {
        private readonly string sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        private ConnectorSDK sdk;

        /// <summary>Creates a new instances of ConnectorSDK.</summary>
        public void Init()
        {
            if (string.IsNullOrEmpty(this.sqlConnectionString))
            {
                throw new ExportConnectorException("Connecting string should not be null or empty.", StatusCodes.Status400BadRequest);
            }

            this.sdk = new ConnectorSDK(this.sqlConnectionString, SQLServerType.MSSQL);
        }

        /// <summary>Return the list of device GUIDS.</summary>
        /// <returns>The list of GUIDS.</returns>
        public List<Guid> GetDeviceGuidList()
        {
            try
            {
                return this.sdk.GetDeviceGuidList();
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying to get the device list. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        /// <summary>Return data associated to provided entity.</summary>
        /// <param name="rootId">The root ID.</param>
        /// <param name="entityKind">The name of the entity.</param>
        /// <param name="deviceGuid">The GUID of the device.</param>
        /// <returns>The data of the requested entity.</returns>
        public List<Dictionary<string, object>> GetModifiedEntityData(string rootId, string entityKind, Guid deviceGuid)
        {
            if (string.IsNullOrEmpty(rootId))
            {
                throw new ExportConnectorException("Error while trying to get the modified entity data.", StatusCodes.Status400BadRequest, new ArgumentException("rootId is null or empty."));
            }

            if (string.IsNullOrEmpty(entityKind))
            {
                throw new ExportConnectorException("Error while trying to get the modified entity data.", StatusCodes.Status400BadRequest, new ArgumentException("entityKind is null or empty."));
            }

            if (deviceGuid == Guid.Empty)
            {
                throw new ExportConnectorException("Error while trying to get the modified entity data.", StatusCodes.Status400BadRequest, new ArgumentException("deviceGuid is null or empty."));
            }

            try
            {
                return this.sdk.GetModifiedEntityData(rootId, entityKind, deviceGuid);
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying to get the modified entity data. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        /// <summary>Sets the exported change-sets.</summary>
        /// <param name="changesetIds">The change-set ids.</param>
        public void SetChangesetsExported(List<string> changesetIds)
        {
            if (changesetIds.Count == 0)
            {
                throw new ExportConnectorException("Error while setting the change sets.", StatusCodes.Status400BadRequest, new InvalidOperationException("changesetIds is empty."));
            }

            try
            {
                this.sdk.SetChangesetsExported(changesetIds);
            }
            catch (SqlException ex)
            {
                var errorMessages = Helper.SqlExceptionBuilder(ex);
                throw new ExportConnectorException("Error while trying to set the exported change sets ids. SQL Error: " + errorMessages, StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }
    }
}
