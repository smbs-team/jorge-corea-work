// <copyright file="BackendService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ConnectorService;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Polly;
    using PTASImportConnector.Exceptions;
    using PTASServicesCommon.TokenProvider;

    /// <summary>It has the logic for retrieving data to send to middle tier.</summary>
    public class BackendService : IBackendService
    {
        /// <summary>
        /// The connection string password section.
        /// </summary>
        private const int Retries = 5;
        private const string ConnectionStringPasswordSection = "password";
        private string pKs = string.Empty;
        private int count;
        private List<Dictionary<string, object>> changesList = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> insertsList = new List<Dictionary<string, object>>();
        private string pkName = string.Empty;
        private TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(24);
        private int countOfRetries = 1;
        private string accessToken = null;
        private ILogger logger;
        private ClientCredential principalCredentials;

        /// <summary>Gets if the bulk was already done.</summary>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        public void DataKeyIsEmptyAsync(string sqlConnectionString, EntityModel entity)
        {
            var policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
            {
                if (ex.Message.ToLower().Contains("login failed for user"))
                {
                    this.GetToken();
                }
                else
                {
                    this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DataKeyIsEmpty. Excepcion: " + ex.Message);
                    this.countOfRetries++;
                }
            });

            policy.Execute(() =>
            {
                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    con.AccessToken = this.accessToken;
                    con.Open();
                    string query = "SELECT TOP 1 1 FROM dbo." + entity.Name + "_Key;";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        this.count = command.ExecuteScalar() == null ? 0 : 1;
                    }
                }

                this.countOfRetries = 1;
            });
        }

        /// <summary>Disable or Rebuild Index, for optimize the BulkCopy.</summary>
        /// <param name="sqlConnectionString"> Connection.</param>
        /// <param name="entityName"> Entity of the index.</param>
        /// <param name="option"> If option = 1, disable the index, or 2, rebuild all index.</param>
        public void DisableorRebuildIndex(string sqlConnectionString, string entityName, int option)
        {
            var policy = Policy
               .Handle<Exception>()
               .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
               {
                   if (ex.Message.ToLower().Contains("login failed for user"))
                   {
                       this.GetToken();
                   }
                   else
                   {
                       this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DisableIndex. Excepcion: " + ex.Message);
                       this.countOfRetries++;
                   }
               });

            policy.Execute(async () =>
            {
                if (option == 1)
                {
                    using (SqlConnection con = new SqlConnection(sqlConnectionString))
                    {
                        con.AccessToken = this.accessToken;
                        con.Open();
                        string query = "ALTER INDEX ALL ON dbo." + entityName + "_Data DISABLE;";
                        string query2 = "ALTER INDEX [PK_" + entityName + "_Data_guid_mb_changesetId_mb] ON dbo." + entityName + "_Data REBUILD;";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.ExecuteScalar();
                        }

                        using (SqlCommand command = new SqlCommand(query2, con))
                        {
                            command.ExecuteScalar();
                        }
                    }
                }
                else if (option == 2)
                {
                    using (SqlConnection con = new SqlConnection(sqlConnectionString))
                    {
                        con.AccessToken = this.accessToken;
                        con.Open();
                        string query = "ALTER INDEX ALL ON dbo." + entityName + "_Data REBUILD;";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            await command.ExecuteScalarAsync();
                        }
                    }
                }

                this.countOfRetries = 1;
            });
        }

        /// <summary>Get data to send to middle tier database.</summary>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        /// <param name="backendConnectionString"> Location where the data is located.</param>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="lastDate"> Date used to filter the data, to retrieve the most recent data.</param>
        /// <param name="connector"> The connector instance. </param>
        /// <param name="uploadTicket"> The ticket used to do the upload of data. </param>
        /// <param name="isBulkInsert"> Flag to indicate if it is a Bulk Insert or not. </param>
        /// <param name="logger"> Looger for messages. </param>
        /// <param name="principalCredentials"> Credentials. </param>
        /// <returns>List containing the data.</returns>
        public List<Dictionary<string, object>> GetData(EntityModel entity, string backendConnectionString, string sqlConnectionString, DateTime? lastDate, IConnector connector, long uploadTicket, bool isBulkInsert, ILogger logger, ClientCredential principalCredentials)
        {
            this.logger = logger;
            this.principalCredentials = principalCredentials;

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (string.IsNullOrEmpty(backendConnectionString))
            {
                throw new ArgumentException("backendConnectionString is null or empty.");
            }

            try
            {
                this.GetToken();

                // this.DisableorRebuildIndex(sqlConnectionString, entity.Name, 1);
                using (var backendConnection = new SqlConnection(backendConnectionString))
                {
                    backendConnection.AccessToken = this.accessToken;

                    // Using the back-end query of the entity obtains his data
                    using (var backendCommand = new SqlCommand(entity.BackendQuery, backendConnection))
                    {
                        backendCommand.CommandTimeout = 0;
                        if (entity.BackendQuery.Contains("@lastSyncDate"))
                        {
                            backendCommand.Parameters.AddWithValue("@lastSyncDate", lastDate ?? new DateTime(1900, 01, 01));
                        }

                        var policy = Policy
                                    .Handle<Exception>()
                                    .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                                    {
                                        this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method GetData. Excepcion: " + ex.Message);
                                        this.countOfRetries++;
                                    });

                        var uploadList = new List<Dictionary<string, object>>();
                        backendConnection.Open();
                        SqlDataReader backendReader = policy.Execute(() =>
                        {
                            return backendCommand.ExecuteReader();
                        });
                        this.countOfRetries = 1;
                        this.count = -1;
                        while (backendReader.Read())
                        {
                            if (this.count < 0)
                            {
                                this.DataKeyIsEmptyAsync(sqlConnectionString, entity);
                            }

                            // Creates a data dictionary with pair <fieldName, value>
                            var resultDict = new Dictionary<string, object>();
                            for (int i = 0; i < backendReader.FieldCount; i++)
                            {
                                resultDict.Add(backendReader.GetName(i), backendReader[i]);
                            }

                            // Add the dictionary to the list to send to the middle tier database
                            uploadList.Add(resultDict);

                            // If its not the first time, add in this list to remove after
                            if (this.count != 0)
                            {
                                this.changesList.Add(resultDict);
                            }

                            if (uploadList.Count >= 30000)
                            {
                                if (!isBulkInsert)
                                {
                                    policy.Execute(() =>
                                    {
                                        connector.AddUploadData(entity.Name, uploadList, uploadTicket);
                                        return Task.CompletedTask;
                                    });
                                }
                                else
                                {
                                    this.BulkProcess(backendConnectionString, sqlConnectionString, entity, uploadTicket, uploadList, connector);
                                }

                                uploadList.Clear();
                            }
                        }

                        return uploadList;
                    }
                }
            }
            catch (SqlException ex)
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

                throw new ImportConnectorException("Error while getting the data to send to the middle tier." + "\n" + errorMessages.ToString());
            }
        }

        /// <summary>Get the PK for Insert or Update.</summary>
        /// <param name="uploadList"> List the attributtes.</param>
        /// <param name="entityName"> Name of the atribute.</param>
        public void GetPK(List<Dictionary<string, object>> uploadList, string entityName)
        {
            foreach (Dictionary<string, object> keyValues in uploadList)
            {
                foreach (var item in keyValues)
                {
                    if (item.Key.ToString().ToLower() == this.pkName.ToLower())
                    {
                        this.pKs = this.pKs + "'" + item.Value.ToString() + "',";
                    }
                }
            }
        }

        /// <summary>Get the PK for Insert or Update.</summary>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        /// <param name="uploadTicket"> The ticket used to do the upload of data. </param>
        /// <param name="connector"> The connector instance. </param>
        public void ComparePK(string sqlConnectionString, EntityModel entity, long uploadTicket, IConnector connector)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            this.pKs = this.pKs.Remove(this.pKs.Length - 1, 1);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                    {
                        if (ex.Message.ToLower().Contains("login failed for user"))
                        {
                            this.GetToken();
                        }
                        else
                        {
                            this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method ComparePK. Excepcion: " + ex.Message);
                            this.countOfRetries++;
                        }
                    });

                    policy.Execute(() =>
                    {
                        con.AccessToken = this.accessToken;
                        con.Open();
                        string query = "SELECT guid_mb, " + this.pkName + " FROM dbo." + entity.Name + "_Key WHERE " + this.pkName + " IN (" + this.pKs + ");";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                pairs.Add(string.Empty + Guid.Parse(reader["guid_mb"].ToString()), string.Empty + Guid.Parse(reader[this.pkName].ToString()));
                            }
                        }
                    });
                    this.countOfRetries = 1;
                }

                List<Dictionary<string, object>> tempList = new List<Dictionary<string, object>>();

                foreach (Dictionary<string, object> keyValues in this.changesList)
                {
                    tempList.Add(keyValues);
                }

                // If pairs not contains a GUID like any in the List of new Update, that Dictionary its a new Insert
                foreach (Dictionary<string, object> keyValues in tempList)
                {
                    foreach (var item in keyValues)
                    {
                        if (item.Key.ToLower().Equals(this.pkName) && !pairs.ContainsValue(item.Value.ToString().ToLower()))
                        {
                            this.insertsList.Add(keyValues);
                            this.changesList.Remove(keyValues);
                            break;
                        }
                    }
                }

                if (!pairs.Any())
                {
                    this.insertsList = this.changesList;
                }

                this.AddListBulk(sqlConnectionString, entity, uploadTicket, this.insertsList, connector);

                if (pairs.Any())
                {
                    this.UpdateListBulk(sqlConnectionString, entity, uploadTicket, connector, pairs);
                }

                this.changesList.Clear();
                tempList.Clear();
                this.insertsList.Clear();
                this.pKs = string.Empty;
            }
            catch (Exception ex)
            {
                throw new ImportConnectorException("Error in ComparePK: " + ex.Message, ex.InnerException);
            }
        }

        /// <summary>Save the uploadList in the BulkInsert way.</summary>
        /// <param name="backendConnectionstring"> Location where the data is located.</param>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        /// <param name="uploadTicket"> The ticket used to do the upload of data. </param>
        /// <param name="uploadList"> List to save.</param>
        /// <param name="connector"> The connector instance. </param>
        public void BulkProcess(string backendConnectionstring, string sqlConnectionString, EntityModel entity, long uploadTicket, List<Dictionary<string, object>> uploadList, IConnector connector)
        {
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                    {
                        if (ex.Message.ToLower().Contains("login failed for user"))
                        {
                            this.GetToken();
                        }
                        else
                        {
                            this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method BulkProcess. Exception: " + ex.Message);
                            this.countOfRetries++;
                        }
                    });
                policy.Execute(() =>
                {
                    con.AccessToken = this.accessToken;
                    con.Open();
                    string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + entity.Name + "_Key" + "' AND TABLE_SCHEMA = 'dbo' AND COLUMN_NAME != 'guid_mb';";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        this.pkName = command.ExecuteScalar().ToString();
                    }
                });
                this.countOfRetries = 1;
            }

            if (this.count == 0)
            {
                this.AddListBulk(sqlConnectionString, entity, uploadTicket, uploadList, connector);
            }
            else
            {
                this.GetPK(uploadList, entity.Name);
                this.ComparePK(sqlConnectionString, entity, uploadTicket, connector);
            }
        }

        /// <summary>Save the uploadList in the BulkInsert way.</summary>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        /// <param name="uploadTicket"> The ticket used to do the upload of data. </param>
        /// <param name="connector"> The connector instance. </param>
        /// <param name="pairs"> List with GUID_mb and PK. </param>
        public void UpdateListBulk(string sqlConnectionString, EntityModel entity, long uploadTicket, IConnector connector, Dictionary<string, string> pairs)
        {
            try
            {
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                    {
                        this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method UpdateListBulk. Exception: " + ex.Message);
                        this.countOfRetries++;
                    });

                while (this.changesList.Any())
                {
                    var partOfList = this.changesList.Take(15000).ToList();
                    policy.Execute(() =>
                    {
                        this.DataTableData(sqlConnectionString, partOfList, entity.Name, null, uploadTicket, pairs, true);
                    });
                    this.countOfRetries = 1;
                    this.changesList = this.changesList.Except(partOfList).ToList();
                    policy.Execute(() =>
                    {
                        connector.SetRunningStatus(DateTime.UtcNow, true);
                    });
                    this.countOfRetries = 1;
                }
            }
            catch (Exception ex)
            {
                throw new ImportConnectorException("Error in UpdateListBulk: " + ex.Message, ex.InnerException);
            }
        }

        /// <summary>Save the uploadList in the BulkInsert way.</summary>
        /// <param name="sqlConnectionString"> Location where the data is to save.</param>
        /// <param name="entity"> It has the necessary database model functionality.</param>
        /// <param name="uploadTicket"> The ticket used to do the upload of data. </param>
        /// <param name="uploadList"> List to save.</param>
        /// <param name="connector"> The connector instance. </param>
        public void AddListBulk(string sqlConnectionString, EntityModel entity, long uploadTicket, List<Dictionary<string, object>> uploadList, IConnector connector)
        {
            try
            {
                var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                {
                    this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method AddListBulk. Exception: " + ex.Message);
                    this.countOfRetries++;
                });
                while (uploadList.Any())
                {
                    var partOfList = uploadList.Take(15000).ToList();
                    List<Guid> list = policy.Execute(() =>
                    {
                        return this.DataTableKey(sqlConnectionString, partOfList, entity.Name);
                    });
                    this.countOfRetries = 1;
                    policy.Execute(() =>
                    {
                        this.DataTableData(sqlConnectionString, partOfList, entity.Name, list, uploadTicket, null, false);
                    });
                    this.countOfRetries = 1;
                    uploadList = uploadList.Except(partOfList).ToList();
                    policy.Execute(() =>
                    {
                        connector.SetRunningStatus(DateTime.UtcNow, true);
                    });
                    this.countOfRetries = 1;
                }
            }
            catch (Exception ex)
            {
                throw new ImportConnectorException("Error in AddListBulk: " + ex.Message, ex.InnerException);
            }
        }

        /// <summary>Fill the key table.</summary>
        /// <param name="connectionString"> ConnectionString.</param>
        /// <param name="uploadList"> List the attributtes.</param>
        /// <param name="entityName"> Name of the atribute.</param>
        /// <returns>List.</returns>
        public List<Guid> DataTableKey(string connectionString, List<Dictionary<string, object>> uploadList, string entityName)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            List<Guid> guids = new List<Guid>();
            DataTable dataTable;
            SqlDataReader reader = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                {
                    if (ex.Message.ToLower().Contains("login failed for user"))
                    {
                        this.GetToken();
                    }
                    else
                    {
                        this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DataTableKey. Exception: " + ex.Message);
                        this.countOfRetries++;
                    }
                });
                policy.Execute(() =>
                {
                    con.AccessToken = this.accessToken;
                    con.Open();
                    string query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + entityName + "_Key" + "' AND TABLE_SCHEMA = 'dbo'";
                    SqlCommand command = new SqlCommand(query, con);
                    reader = command.ExecuteReader();
                });
                this.countOfRetries = 1;

                while (reader.Read())
                {
                    pairs.Add(reader.GetString(0), reader.GetString(1));
                }

                dataTable = this.GetTypes(pairs);
                dataTable.TableName = entityName.ToLower() + "_Key";

                foreach (Dictionary<string, object> keyValues in uploadList)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var item in keyValues)
                    {
                        if (item.Key.ToString().ToLower() == this.pkName.ToLower())
                        {
                            row[item.Key] = item.Value;
                        }
                    }

                    Guid guid = Guid.NewGuid();
                    row["guid_mb"] = guid;
                    guids.Add(guid);
                    dataTable.Rows.Add(row);
                }

                con.Close();
                this.SaveDataTable(dataTable, connectionString);
                return guids;
            }
        }

        /// <summary>Fill the data table.</summary>
        /// <param name="connectionString"> ConnectionString.</param>
        /// <param name="uploadList"> List the attributtes.</param>
        /// <param name="entityName"> Name of the atribute.</param>
        /// <param name="guids">Saved guids.</param>
        /// <param name="uploadTicket">Tickets.</param>
        /// <param name="guidsUpdate">PK and GUIDS only for update.</param>
        /// <param name="isUpdate">Flag for updates only.</param>
        public void DataTableData(string connectionString, List<Dictionary<string, object>> uploadList, string entityName, List<Guid> guids, long uploadTicket, Dictionary<string, string> guidsUpdate, bool isUpdate)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            DataTable dataTable;
            SqlDataReader reader = null;
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                {
                    if (ex.Message.ToLower().Contains("login failed for user"))
                    {
                        this.GetToken();
                    }
                    else
                    {
                        this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DataTableData. Exception: " + ex.Message);
                        this.countOfRetries++;
                    }
                });

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                policy.Execute(() =>
                {
                    con.AccessToken = this.accessToken;
                    con.Open();
                    string query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + entityName + "_Data" + "' AND TABLE_SCHEMA = 'dbo'";
                    SqlCommand command = new SqlCommand(query, con);
                    reader = command.ExecuteReader();
                });
                this.countOfRetries = 1;
                while (reader.Read())
                {
                    pairs.Add(reader.GetString(0).ToLower(), reader.GetString(1));
                }

                dataTable = this.GetTypes(pairs);
                dataTable.TableName = entityName.ToLower() + "_Data";
                int i = 0;

                foreach (Dictionary<string, object> keyValues in uploadList)
                {
                    DataRow row = dataTable.NewRow();
                    if (dataTable.Columns.Contains("changesetId_mb"))
                    {
                        row["changesetId_mb"] = uploadTicket;
                    }

                    foreach (var item in keyValues)
                    {
                        if (item.Key.ToLower().Equals(this.pkName.ToLower()) && isUpdate)
                        {
                            row["guid_mb"] = Guid.Parse(guidsUpdate.FirstOrDefault(x => x.Value.ToLower() == item.Value.ToString().ToLower()).Key);
                        }
                        else if (item.Key.ToLower().Equals(this.pkName.ToLower()))
                        {
                            row["guid_mb"] = guids[i];
                        }

                        if (dataTable.Columns.Contains(item.Key))
                        {
                            row[item.Key] = this.ConvertData(item.Value, pairs.GetValueOrDefault(item.Key.ToLower()));
                        }
                    }

                    dataTable.Rows.Add(row);
                    i++;
                }

                if (isUpdate)
                {
                    this.DeleteGuids(guidsUpdate, connectionString, entityName, uploadTicket);
                }

                con.Close();
                this.SaveDataTable(dataTable, connectionString);
            }
        }

        /// <summary>Delete old guids with updates.</summary>
        /// <param name="guidsUpdate"> DataTable to save.</param>
        /// <param name="sqlConnectionString"> ConnectionString.</param>
        /// <param name="entityName"> Entity.</param>
        /// <param name="uploadTicket"> Changeset.</param>
        private void DeleteGuids(Dictionary<string, string> guidsUpdate, string sqlConnectionString, string entityName, long uploadTicket)
        {
            string guidDelete = string.Empty;
            int index = 0;
            foreach (var item in guidsUpdate)
            {
                guidDelete = guidDelete + "'" + item.Key.ToString() + "',";
                index++;
                if (index == 1000)
                {
                    guidDelete = guidDelete.Remove(guidDelete.Length - 1, 1);
                    index = 0;

                    try
                    {
                        var policy = Policy
                            .Handle<Exception>()
                            .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                            {
                                if (ex.Message.ToLower().Contains("login failed for user"))
                                {
                                    this.GetToken();
                                }
                                else
                                {
                                    this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DeleteGuids1. Exception: " + ex.Message);
                                    this.countOfRetries++;
                                }
                            });
                        policy.Execute(() =>
                        {
                            using (SqlConnection con = new SqlConnection(sqlConnectionString))
                            {
                                con.AccessToken = this.accessToken;
                                con.Open();
                                string query = "DELETE FROM " + entityName + "_Data WHERE guid_mb IN (" + guidDelete + ") AND changesetID_mb < " + uploadTicket + ";";
                                using (SqlCommand command = new SqlCommand(query, con))
                                {
                                    command.CommandTimeout = con.ConnectionTimeout;
                                    int rows = command.ExecuteNonQuery();
                                }
                            }
                        });
                        this.countOfRetries = 1;
                    }
                    catch (Exception ex)
                    {
                        throw new ImportConnectorException("Error in DeleteGuids: " + ex.Message, ex.InnerException);
                    }

                    guidDelete = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(guidDelete))
            {
                guidDelete = guidDelete.Remove(guidDelete.Length - 1, 1);
                try
                {
                    var policy = Policy
                            .Handle<Exception>()
                            .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                            {
                                if (ex.Message.ToLower().Contains("login failed for user"))
                                {
                                    this.GetToken();
                                }
                                else
                                {
                                    this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method DeleteGuids2. Exception: " + ex.Message);
                                    this.countOfRetries++;
                                }
                            });
                    policy.Execute(() =>
                    {
                        using (SqlConnection con = new SqlConnection(sqlConnectionString))
                        {
                            con.AccessToken = this.accessToken;
                            con.Open();
                            string query = "DELETE FROM " + entityName + "_Data WHERE guid_mb IN (" + guidDelete + ") AND changesetID_mb < " + uploadTicket + ";";
                            using (SqlCommand command = new SqlCommand(query, con))
                            {
                                command.CommandTimeout = con.ConnectionTimeout;
                                int rows = command.ExecuteNonQuery();
                            }
                        }
                    });
                    this.countOfRetries = 1;
                }
                catch (Exception ex)
                {
                    throw new ImportConnectorException("Error in DeleteGuids: " + ex.Message, ex.InnerException);
                }
            }
        }

        /// <summary>Bulk the data table.</summary>
        /// <param name="dataTable"> DataTable to save.</param>
        /// <param name="connectionString"> ConnectionString.</param>
        private void SaveDataTable(DataTable dataTable, string connectionString)
        {
            // IEnumerable de datos de la tabla
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.AccessToken = this.accessToken;
                con.Open();
                using (SqlBulkCopy bulkInsert = new SqlBulkCopy(
                    con,
                    SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction,
                    null))
                {
                    bulkInsert.DestinationTableName = string.Format("[{0}].[{1}]", "dbo", dataTable.TableName);
                    try
                    {
                        bulkInsert.BulkCopyTimeout = 0;
                        bulkInsert.BatchSize = 3000;
                        bulkInsert.WriteToServer(dataTable);
                        string query = "update SynchronizationDate_mb set syncDateImport = getutcdate(), isRunning = 1";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            var policy = Policy
                            .Handle<Exception>()
                            .WaitAndRetry(Retries, i => this.pauseBetweenFailures, (ex, pauseBetweenFailures) =>
                            {
                                this.logger.LogError("Error in the attemp " + this.countOfRetries + ", in Method SaveDataTable Update. Exception: " + ex.Message);
                                this.countOfRetries++;
                            });
                            policy.Execute(() =>
                            {
                                command.ExecuteNonQuery();
                            });
                            this.countOfRetries = 1;
                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new ImportConnectorException("Error in SaveDataTable: " + ex.Message, ex.InnerException);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        /// <summary>Gives format and types to Datatable.</summary>
        /// <param name="pairs"> List the attributtes.</param>
        /// <returns>Datatable.</returns>
        private DataTable GetTypes(Dictionary<string, string> pairs)
        {
            DataTable dataTable = new DataTable();
            int i = 0;

            foreach (var item in pairs)
            {
                dataTable.Columns.Add(item.Key);
                if (item.Value.Contains("char") ||
                    item.Value.Contains("text"))
                {
                    dataTable.Columns[i].DataType = typeof(string);
                }

                if (item.Value.Contains("float"))
                {
                    dataTable.Columns[i].DataType = typeof(float);
                }

                if (item.Value.Contains("datetime"))
                {
                    dataTable.Columns[i].DataType = typeof(DateTime);
                }

                if (item.Value.Equals("int"))
                {
                    dataTable.Columns[i].DataType = typeof(int);
                }

                if (item.Value.Equals("bigint"))
                {
                    dataTable.Columns[i].DataType = typeof(long);
                }

                if (item.Value.Equals("smallint"))
                {
                    dataTable.Columns[i].DataType = typeof(int);
                }

                if (item.Value.Contains("binary") ||
                    item.Value.Contains("image"))
                {
                    dataTable.Columns[i].DataType = typeof(byte[]);
                }

                if (item.Value.Equals("tinyint"))
                {
                    dataTable.Columns[i].DataType = typeof(byte);
                }

                if (item.Value.Equals("bit"))
                {
                    dataTable.Columns[i].DataType = typeof(bool);
                }

                if (item.Value.Equals("decimal") ||
                    item.Value.Equals("money") ||
                    item.Value.Equals("smallmoney"))
                {
                    dataTable.Columns[i].DataType = typeof(decimal);
                }

                if (item.Value.Equals("uniqueidentifier"))
                {
                    dataTable.Columns[i].DataType = typeof(Guid);
                }

                if (item.Value.Equals("real"))
                {
                    dataTable.Columns[i].DataType = typeof(SqlSingle);
                }

                i++;
            }

            return dataTable;
        }

        /// <summary>Gives format and types to Datatable.</summary>
        /// <param name="items"> Item to convert.</param>
        /// <param name="type"> Type.</param>
        /// <returns>Object.</returns>
        private object ConvertData(object items, string type)
        {
            var data = items;

            if (!data.ToString().Equals(string.Empty))
            {
                if (type.ToString().Contains("char") ||
                    type.ToString().Contains("text"))
                {
                    data = data.ToString();
                }

                if (type.ToString().Contains("float"))
                {
                    data = float.Parse(data.ToString());
                }

                if (type.ToString().Contains("datetime"))
                {
                    data = DateTime.Parse(data.ToString());
                }

                if (type.Equals("int") || type.Equals("smallint"))
                {
                    data = int.Parse(data.ToString());
                }

                if (type.Equals("bigint"))
                {
                    data = long.Parse(data.ToString());
                }

                if (type.ToString().Contains("binary") ||
                    type.ToString().Contains("image"))
                {
                    data = Encoding.Default.GetBytes(data.ToString());
                }

                if (type.Equals("tinyint"))
                {
                    data = byte.Parse(data.ToString());
                }

                if (type.Equals("bit"))
                {
                    data = bool.Parse(data.ToString());
                }

                if (type.Equals("decimal") ||
                    type.Equals("money") ||
                    type.Equals("smallmoney"))
                {
                    data = decimal.Parse(data.ToString());
                }

                if (type.Equals("uniqueidentifier"))
                {
                    data = Guid.Parse(data.ToString());
                }

                if (type.Equals("real"))
                {
                    data = SqlSingle.Parse(data.ToString());
                }
            }

            return data;
        }

        /// <summary>Create or update token when expired.</summary>
        /// <param name="sqlConnectionString"> Connection for the token.</param>
        private void GetToken()
        {
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();

            this.accessToken = Task.Run(async () =>
            {
                return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, this.principalCredentials);
            }).Result;
        }
    }
}
