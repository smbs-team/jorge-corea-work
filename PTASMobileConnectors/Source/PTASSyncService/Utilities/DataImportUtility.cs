using PTASConnectorSDK;
using PTASSyncService.Models;
using PTASSyncService.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace ConnectorService.Utilities
{
    class DataImportUtility
    {
        private ConnectorSDK connectorSDK;
        private readonly PTASSyncService.Settings Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataImportUtility" /> class.
        /// </summary>
        public DataImportUtility()
        {
            // Initialize a new instance of the connector SDK
            connectorSDK = new ConnectorSDK(Environment.GetEnvironmentVariable("connectionString"), SQLServerType.MSSQL);
        }
        /// <summary>
        /// Moves the edited entity data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        /// <param name="rootEntityList">The root entity list.</param>
        /// <param name="entityNameList">The entity name list.</param>
        private void MoveEditedEntityData(EntityModel entity, long uploadTicket, string rootEntityList, string entityNameList)
        {
            Debug.Assert(entity != null, "The entity should not be null");
            Debug.Assert(uploadTicket > 0, "The uploadTicket is wrong");
            Debug.Assert(!string.IsNullOrEmpty(entityNameList), "The entityNameList should not be null or empty");
            Debug.Assert(!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("backendConnectionString")), "The backend connection string in the config file is wrong");
            Debug.Assert(Configuration.chunkSize > 0, "The chunkSize in the config file is wrong");
            // If the entity is contained into the list of entities to process
            if (entityNameList.Contains(entity.Name.ToLower()))
            {
                string backConnStr = Environment.GetEnvironmentVariable("backendConnectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(backConnStr);
                using (SqlConnection backendConnection = new SqlConnection(backConnStr))
                {
                    backendConnection.AccessToken = sqlToken;
                    List<Dictionary<string, object>> uploadList = new List<Dictionary<string, object>>();
                    int counter = 1;
                    int chunkSize = Configuration.chunkSize;
                    // If the entity is rootRelated
                    if (entity.IsRootRelated)
                    {
                        string fieldToFilter = "rootID_mb";
                        if (entity.BackendQuery.Contains("rootID_indx"))
                            fieldToFilter = "rootID_indx";

                        // Using the backend query of the entity obtains his data, with a filter by the root id that will be in the rootEntityList
                        string backendQuery = "select * from (" + entity.BackendQuery + ") as BQ where " + fieldToFilter + " in (" + rootEntityList + ")";
                        using (SqlCommand backendCommand = new SqlCommand(backendQuery, backendConnection))
                        {
                            connectorSDK.SetLogHeader(true, DateTime.UtcNow, "Importing " + (entity.FriendlyName != null ? (string.IsNullOrEmpty(entity.FriendlyName) ? entity.Name : entity.FriendlyName) : entity.Name) + " data", uploadTicket, uploadTicket.ToString());
                            backendCommand.CommandTimeout = backendConnection.ConnectionTimeout;
                            backendConnection.Open();
                            SqlDataReader backendReader = backendCommand.ExecuteReader();
                            while (backendReader.Read())
                            {
                                // Creates a data dictionary with pair <fieldName, value>
                                Dictionary<string, object> resultDict = new Dictionary<string, object>();
                                for (int i = 0; i < backendReader.FieldCount; i++)
                                {
                                    resultDict.Add(backendReader.GetName(i), backendReader[i]);
                                }

                                //Add the dictionary to the list to send to the middle tier database
                                uploadList.Add(resultDict);
                                // If the chunkSize is reached then send the data, send data by chunks
                                if (counter == chunkSize)
                                {
                                    // Invoke the method of the connector to send the data
                                    connectorSDK.AddUploadData(entity.Name, uploadList, uploadTicket);
                                    // Invoke the method of the connector to process the data
                                    connectorSDK.ProcessDataForTicket(uploadTicket, false, false);
                                    counter = 1;
                                    uploadList = new List<Dictionary<string, object>>();
                                }
                                else
                                {
                                    counter++;
                                }
                            }

                            if (uploadList.Count > 0)
                            {
                                // Invoke the method of the connector to send the data
                                connectorSDK.AddUploadData(entity.Name, uploadList, uploadTicket);
                                // Invoke the method of the connector to process the data
                                connectorSDK.ProcessDataForTicket(uploadTicket, false, false);
                            }

                            backendConnection.Close();
                        }
                    }
                    // If the entity is not rootRelated
                    else
                    {
                        // Using the backend query of the entity obtains his data
                        using (SqlCommand backendCommand = new SqlCommand(entity.BackendQuery, backendConnection))
                        {
                            connectorSDK.SetLogHeader(true, DateTime.UtcNow, "Importing " + (entity.FriendlyName != null ? (string.IsNullOrEmpty(entity.FriendlyName) ? entity.Name : entity.FriendlyName) : entity.Name) + " data", uploadTicket, uploadTicket.ToString());
                            backendCommand.CommandTimeout = backendConnection.ConnectionTimeout;
                            backendConnection.Open();
                            SqlDataReader backendReader = backendCommand.ExecuteReader();
                            while (backendReader.Read())
                            {
                                // Creates a data dictionary with pair <fieldName, value>
                                Dictionary<string, object> resultDict = new Dictionary<string, object>();
                                for (int i = 0; i < backendReader.FieldCount; i++)
                                {
                                    resultDict.Add(backendReader.GetName(i), backendReader[i]);
                                }

                                //Add the dictionary to the list to send to the middle tier database
                                uploadList.Add(resultDict);
                                // If the chunkSize is reached then send the data, send data by chunks
                                if (counter == chunkSize)
                                {
                                    // Invoke the method of the connector to send the data
                                    connectorSDK.AddUploadData(entity.Name, uploadList, uploadTicket);
                                    // Invoke the method of the connector to process the data
                                    connectorSDK.ProcessDataForTicket(uploadTicket, false, false);
                                    counter = 1;
                                    uploadList = new List<Dictionary<string, object>>();
                                }
                                else
                                {
                                    counter++;
                                }
                            }

                            if (uploadList.Count > 0)
                            {
                                // Invoke the method of the connector to send the data
                                connectorSDK.AddUploadData(entity.Name, uploadList, uploadTicket);
                                // Invoke the method of the connector to process the data
                                connectorSDK.ProcessDataForTicket(uploadTicket, false, false);
                            }

                            backendConnection.Close();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Loads the root entities into SyncDB.
        /// </summary>
        /// <param name="loadRootEntityMdl">Model obj that contains the requested root entities.</param>
        public long LoadRootEntities(LoadRootEntityModel loadRootEntityMdl, long assignmentID)
        {
            Debug.Assert(loadRootEntityMdl != null, "The loadRootEntityMdl should not be null");
            Debug.Assert(assignmentID > 0, "The assignmentID is wrong");
            
            long uploadTicket = -1;
            DatabaseModel databaseModel = DatabaseModelHelper.GetDatabaseModel();
            string rootEntitiesList = string.Empty;

            foreach (string currentRootEntity in loadRootEntityMdl.RootEntityList)
            {
                connectorSDK.AddRootInToAssigment(assignmentID, currentRootEntity);
                rootEntitiesList = rootEntitiesList + "'" + currentRootEntity + "'" + ",";
            }

            if (rootEntitiesList.Length > 0)
            {
                uploadTicket = connectorSDK.GetUploadTicketForSpecificDevice();  //*Verify this
                rootEntitiesList = rootEntitiesList.Substring(0, rootEntitiesList.Length - 1);

                var entityCollection = databaseModel.Entities.Where(e => e.IsRootRelated == true).OrderBy(e => e.SyncOrder);
                foreach (EntityModel entity in entityCollection)
                {
                    if (!String.IsNullOrEmpty(entity.BackendQuery))
                    {
                        MoveEditedEntityData(entity, uploadTicket, rootEntitiesList, entity.Name.ToLower());
                    }
                }

                string connStr = Environment.GetEnvironmentVariable("connectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                using (SqlConnection middletierConnection = new SqlConnection(connStr))
                {
                    middletierConnection.AccessToken = sqlToken;
                    using (SqlCommand updateCommand = new SqlCommand("update Root_Assignment_Header set currentProgressStatus = 1 where assignmentId = @assignmentId", middletierConnection))
                    {
                        updateCommand.CommandTimeout = middletierConnection.ConnectionTimeout;
                        middletierConnection.Open();
                        updateCommand.Parameters.AddWithValue("@assignmentId", assignmentID);
                        updateCommand.ExecuteNonQuery();
                        middletierConnection.Close();
                    }
                }
            }

            return uploadTicket;
        }
    }
}