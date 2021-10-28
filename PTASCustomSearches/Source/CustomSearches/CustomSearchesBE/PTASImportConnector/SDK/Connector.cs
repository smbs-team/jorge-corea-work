// <copyright file="Connector.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASConnectorSDK;
    using PTASImportConnector.Exceptions;

    /// <summary>ConnectorSDK wrapper.</summary>
    public class Connector : IConnector
    {
        private ConnectorSDK sdk;

        /// <summary>Creates a new instances of ConnectorSDK.</summary>
        /// <param name="sqlConnectionString">Connection string for the connector.</param>
        /// <param name="principalCredentials">Id and secret.</param>
        public void Init(string sqlConnectionString, ClientCredential principalCredentials)
        {
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                throw new ArgumentException("sqlConnectionString is null or empty.");
            }

            this.sdk = new ConnectorSDK(sqlConnectionString, SQLServerType.MSSQL, principalCredentials);
        }

        /// <summary>Creates a new instances of ConnectorSDK.</summary>
        /// <returns>Returns a DateTime representing the last import date.</returns>
        public DateTime? GetLastImportDate()
        {
            try
            {
                return this.sdk.GetLastImportDate();
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while getting the last import date." + "\n" + errorMessage);
            }
        }

        /// <summary>
        /// Check if the import connetors are running.
        /// </summary>
        /// <returns>true if is running, otherwise false.</returns>
        public bool IsRunning()
        {
            try
            {
                return this.sdk.IsRunning();
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while getting the status of import connectors." + "\n" + errorMessage);
            }
        }

        /// <summary>
        /// Get the last import date for the entity.
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        /// <returns>Returns the last import date for the entity.</returns>
        public DateTime? GetLastImportEntityDate(string entityName)
        {
            try
            {
                return this.sdk.GetLastImportEntityDate(entityName);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while getting the last import date for the entity " + entityName + ".\n" + errorMessage);
            }
        }

        /// <summary>Creates a new instances of ConnectorSDK.</summary>
        /// <returns>Returns the Upload ticket for the back-end.</returns>
        public long GetUploadTicketForBackend()
        {
            try
            {
                return this.sdk.GetUploadTicketForBackend();
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while getting the ticket for the back-end." + "\n" + errorMessage);
            }
        }

        /// <summary>Add the upload data.</summary>
        /// <param name="entityKind">The entity kind.</param>
        /// <param name="dataToUpload">The data to upload.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        public void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket)
        {
            if (string.IsNullOrEmpty(entityKind))
            {
                throw new ArgumentException("entityKind is null or empty.");
            }

            if (uploadTicket < 0)
            {
                throw new InvalidOperationException("The upload ticket should be greater than zero.");
            }

            try
            {
                this.sdk.AddUploadData(entityKind, dataToUpload, uploadTicket);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while adding the upload data." + "\n" + errorMessage);
            }
        }

        /// <summary>Process data for ticket.</summary>
        /// <param name="changeSetID">Change set id.</param>
        /// <param name="fromDevice">Where the data comes from, back-end or device.</param>
        /// <param name="lastExecution">Last execution call of process data of the current ticket.</param>
        public void ProcessDataForTicket(long changeSetID, bool fromDevice, bool lastExecution)
        {
            if (changeSetID < 0)
            {
                throw new InvalidOperationException("The change-set id should be greater than zero.");
            }

            try
            {
                this.sdk.ProcessDataForTicket(changeSetID, fromDevice, lastExecution);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while processing the data for ticket." + "\n" + errorMessage);
            }
        }

        /// <summary>Sets the import date..</summary>
        /// <param name="importDate">The import date.</param>
        public void SetImportDate(DateTime importDate)
        {
            try
            {
                this.sdk.SetImportDate(importDate);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while setting the import date." + "\n" + errorMessage);
            }
        }

        /// <summary>
        /// Set the status of import connectors.
        /// </summary>
        /// <param name="importDate">The date of the import.</param>
        /// <param name="isRunning">The status.</param>
        public void SetRunningStatus(DateTime importDate, bool isRunning)
        {
            try
            {
                this.sdk.SetRunningStatus(importDate, isRunning);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while setting the import connectors status." + "\n" + errorMessage);
            }
        }

        /// <summary>
        /// Sets the import date for the entity.
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        /// <param name="importDate">The date to set.</param>
        public void SetImportEntityDate(string entityName, DateTime importDate)
        {
            try
            {
                this.sdk.SetImportEntityDate(entityName, importDate);
            }
            catch (SqlException ex)
            {
                var errorMessage = this.SqlExceptionBuilder(ex);
                throw new ImportConnectorException("Error while setting the import date for the entity " + entityName + ".\n" + errorMessage);
            }
        }

        private string SqlExceptionBuilder(SqlException ex)
        {
            var errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "Error Number: " + ex.Errors[i].Number + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n");
            }

            return errorMessages.ToString();
        }
    }
}
