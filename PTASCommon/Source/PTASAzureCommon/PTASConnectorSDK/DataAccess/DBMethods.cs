using System;
using System.Collections.Generic;
using System.Linq;
using PTASConnectorSDK.DataModel;
using System.Data.SqlClient;
using System.Data;
using FastMember;
using System.Threading.Tasks;
using PTASServicesCommon.TokenProvider;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PTASConnectorSDK.DataAccess
{
    public class DBMethods : IDBMethods
    {
        public enum changeSetType
        {
            deviceUpload = 1,
            backendUpload = 2,
            specificDevice = 3
        }

        public enum changeSetStatus
        {
            created = 1,
            inProgress = 2,
            ready = 3,
            exported = 4,
            downloadedToDevice = 5
        }

        public string cnnStr = "";
        public ClientCredential principalCredential = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBMethods" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DBMethods(string connectionString, ClientCredential principalCredentials)
        {
            cnnStr = connectionString;
            principalCredential = principalCredentials;
        }

        private string GetSQLToken(string cnnString)
        {
            string accessToken = null;
            if (!cnnString.ToLowerInvariant().Contains("password"))
            {
                string token = null;
                if (principalCredential == null)
                {
                    token = Task.Run(async () =>
                    {
                        return await new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider().GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantUrl);
                    }).Result;
                }
                else
                {
                    IServiceTokenProvider tokenProvider = new AzureTokenProvider();

                    token = Task.Run(async () =>
                    {
                        return await tokenProvider.GetAccessTokenAsync(AzureTokenProvider.DbTokenEndpoint, AzureTokenProvider.TenantId, principalCredential);
                    }).Result;
                }

                accessToken = token;
            }

            return accessToken;
        }


        /// <summary>
        /// Adds the upload data.
        /// </summary>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="dataToUpload">The data to upload.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        public void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket)
        {
            long maxOrder = 0;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("Select isNULL(Max(dataOrder_upl), 0) from UploadPool_mb Where changeSetID_upl = @ticket", cnn))
                {
                    DateTime currentUTCDate = DateTime.UtcNow;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@ticket", uploadTicket));
                    cnn.Open();
                    var result = cmd.ExecuteScalar();
                    maxOrder = (long)result;
                    cnn.Close();
                }

                List<BulkRecord> tmpDt = new List<BulkRecord>();

                foreach (Dictionary<string, object> currentFields in dataToUpload)
                {
                    maxOrder++;
                    string data = string.Join("&#!", currentFields.Select(x => x.Key + "&#$" + ((x.Value is DateTime) ? ((DateTime)x.Value).ToString("yyyy/MM/dd HH:mm:ss.fff") : x.Value)?.ToString()));
                    BulkRecord emp = new BulkRecord(uploadTicket, entityKind, data, maxOrder);
                    tmpDt.Add(emp);
                }

                var copyParameters = new[]
                {
                        nameof(BulkRecord.id_upl),
                        nameof(BulkRecord.changeSetID_upl),
                        nameof(BulkRecord.entityKind_upl),
                        nameof(BulkRecord.strData_upl),
                        nameof(BulkRecord.dataOrder_upl)
                    };

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnn))
                {
                    cnn.Open();
                    bulkCopy.BulkCopyTimeout = cnn.ConnectionTimeout;
                    bulkCopy.DestinationTableName = "UploadPool_mb";
                    using (var reader = ObjectReader.Create(tmpDt, copyParameters))
                    {
                        bulkCopy.WriteToServer(reader);
                    }
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Gets the upload ticket.
        /// </summary>
        /// <param name="deviceID">The device ID.</param>
        /// <param name="assignmentID">The assignment ID.</param>
        /// <returns></returns>
        public long GetUploadTicket(string deviceID, long? assignmentID)
        {
            long chngsetID = -1;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                Guid? devGuid = null;
                if (!string.IsNullOrEmpty(deviceID))
                {
                    using (SqlCommand cmd = new SqlCommand("Select guid_mb from dbo.Device_mb Where deviceId_mb = @deviceID", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@deviceID", deviceID));
                        cnn.Open();
                        devGuid = (Guid)cmd.ExecuteScalar();
                        cnn.Close();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, getutcdate(), round(cast((getutcdate() - '2001-01-01 00:00:00') as float) * 86400, 1000000), @deviceGuid, @statusTypeId, @assignmentId); SELECT SCOPE_IDENTITY();", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlParameter myParam = new SqlParameter("@changesetTypeId", SqlDbType.Int);
                    if (!string.IsNullOrEmpty(deviceID))
                        myParam.Value = (int)changeSetType.deviceUpload;
                    else
                        myParam.Value = (int)changeSetType.backendUpload;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                    if (!string.IsNullOrEmpty(deviceID))
                        myParam.Value = devGuid;
                    else
                        myParam.Value = DBNull.Value;

                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@statusTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetStatus.created;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@assignmentId", SqlDbType.BigInt);
                    if (assignmentID.HasValue)
                    {
                        myParam.Value = (long)assignmentID;
                    }
                    else
                    {
                        myParam.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    chngsetID = (long)((decimal)cmd.ExecuteScalar());
                    cnn.Close();
                }
            }

            return chngsetID;
        }

        /// <summary>
        /// Gets the upload ticket for specific device.
        /// </summary>
        /// <returns></returns>
        public long GetUploadTicketForSpecificDevice()
        {
            long chngsetID = -1;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, getutcdate(), round(cast((getutcdate() - '2001-01-01 00:00:00') as float) * 86400, 1000000), @deviceGuid, @statusTypeId, @assignmentId); SELECT SCOPE_IDENTITY();", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlParameter myParam = new SqlParameter("@changesetTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetType.specificDevice;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                    myParam.Value = DBNull.Value;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@statusTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetStatus.created;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@assignmentId", SqlDbType.BigInt);
                    myParam.Value = DBNull.Value;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    chngsetID = (long)((decimal)cmd.ExecuteScalar());
                    cnn.Close();
                }
            }

            return chngsetID;
        }

        public long GetUploadTicketForSpecificDevice(long assignmentID)
        {
            long chngsetID = -1;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, getutcdate(), round(cast((getutcdate() - '2001-01-01 00:00:00') as float) * 86400, 1000000), @deviceGuid, @statusTypeId, @assignmentId); SELECT SCOPE_IDENTITY();", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlParameter myParam = new SqlParameter("@changesetTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetType.specificDevice;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                    myParam.Value = DBNull.Value;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@statusTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetStatus.created;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@assignmentId", SqlDbType.BigInt);
                    myParam.Value = assignmentID;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    chngsetID = (long)((decimal)cmd.ExecuteScalar());
                    cnn.Close();
                }
            }

            return chngsetID;
        }

        /// <summary>
        /// DBs the get guidsfor objects.
        /// </summary>
        /// <param name="objectList">The object list.</param>
        /// <returns></returns>
        public List<GetGuidObject> DBGetGuidsforObjects(List<GetGuidObject> objectList)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                Guid currentSetId = Guid.NewGuid();
                long maxOrder = 0;

                List<QueryRecord> tmpDt = new List<QueryRecord>();

                //Inserts the requested objects
                foreach (GetGuidObject currentObj in objectList)
                {
                    foreach (Dictionary<string, object> currentFields in currentObj.keyFields)
                    {
                        maxOrder++;
                        string data = string.Join("&#!", currentFields.Select(x => x.Key + "&#$" + x.Value));
                        QueryRecord emp = new QueryRecord(currentSetId, currentObj.kind, data, maxOrder);
                        tmpDt.Add(emp);
                    }
                }

                var copyParameters = new[]
                    {
                        nameof(QueryRecord.guidSetID_gq),
                        nameof(QueryRecord.entityKind_gq),
                        nameof(QueryRecord.strData_gq),
                        nameof(QueryRecord.dataOrder_gq)
                    };

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnn))
                {
                    cnn.Open();
                    bulkCopy.DestinationTableName = "GuidQuery_mb";
                    using (var reader = ObjectReader.Create(tmpDt, copyParameters))
                    {
                        bulkCopy.WriteToServer(reader);
                    }
                    cnn.Close();
                }

                //Executes the SP to assign the Guids
                using (SqlCommand cmd = new SqlCommand("PopulateEntityGuids", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    SqlParameter myParam = new SqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
                    myParam.Value = currentSetId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    cmd.ExecuteScalar();
                    cnn.Close();
                }

                //Reads the results
                List<GetGuidObject> resultList = new List<GetGuidObject>();
                GetGuidObject resultObj = null;
                List<Dictionary<string, object>> resultFields = new List<Dictionary<string, object>>();
                using (SqlCommand cmd = new SqlCommand("Select * from GuidQuery_mb Where guidSetID_gq = @guidSetID", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    SqlParameter myParam = new SqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
                    myParam.Value = currentSetId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (resultObj == null)
                        {
                            resultObj = new GetGuidObject();
                            resultObj.kind = (string)rdr["entityKind_gq"];
                        }
                        else if (resultObj.kind != (string)rdr["entityKind_gq"])
                        {
                            resultObj.keyFields = resultFields;
                            resultList.Add(resultObj);
                            resultObj = new GetGuidObject();
                            resultObj.kind = (string)rdr["entityKind_gq"];
                            resultFields = new List<Dictionary<string, object>>();
                        }
                        resultFields.Add(((string)rdr["strData_gq"]).Split(new string[] { "&#!" }, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Split(new string[] { "&#$" }, StringSplitOptions.None)).ToDictionary(split => split[0], split => (object)split[1]));
                    }

                    resultObj.keyFields = resultFields;
                    resultList.Add(resultObj);
                    cnn.Close();
                }

                //Purge the table
                using (SqlCommand cmd = new SqlCommand("Delete dbo.GuidQuery_mb where guidSetID_gq = @guidSetID", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    SqlParameter myParam = new SqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
                    myParam.Value = currentSetId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    cmd.ExecuteScalar();
                    cnn.Close();
                }

                return resultList;
            }
        }

        /// <summary>
        /// Processes the data for ticket.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        /// <param name="fromDevice">if set to <c>true</c> [from device].</param>
        /// <param name="lastExecution">if set to <c>true</c> [last execution].</param>
        public void ProcessDataForTicket(long changeSetID, bool fromDevice, bool lastExecution)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                //Store the imported data in to the tables, also update relationships and guids.
                using (SqlCommand cmd = new SqlCommand("ProcessData", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@changeSetID", changeSetID));
                    cmd.Parameters.Add(new SqlParameter("@fromDevice", fromDevice));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                if (lastExecution)
                {
                    //Marks the changeset as completed.
                    using (SqlCommand cmd = new SqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@statusTypeId", (long)changeSetStatus.ready));
                        cmd.Parameters.Add(new SqlParameter("@idChangeset", changeSetID));
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Set Changeset state to completed.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        public void ChangesetCompleted(long changeSetID)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@statusTypeId", (long)changeSetStatus.ready));
                    cmd.Parameters.Add(new SqlParameter("@idChangeset", changeSetID));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Set Changeset state to downloaded.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        public void ChangesetDownloaded(long changeSetID)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@statusTypeId", (long)changeSetStatus.downloadedToDevice));
                    cmd.Parameters.Add(new SqlParameter("@idChangeset", changeSetID));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Adds the root entity in to assigment.
        /// </summary>
        /// <param name="assignmentId">The root assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        public void AddRootInToAssigment(long assignmentId, string rootId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                long numRecs = 0;
                using (SqlCommand cmd = new SqlCommand("Select count(1) from dbo.Root_Assignment_Detail where assignmentHeader = @assignmentId and rootId_mb = @rootId", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlParameter myParam = new SqlParameter("@assignmentId", SqlDbType.BigInt);
                    myParam.Value = (long)assignmentId;
                    cmd.Parameters.Add(myParam);

                    myParam = new SqlParameter("@rootId", SqlDbType.NVarChar);
                    myParam.Value = rootId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    numRecs = (int)cmd.ExecuteScalar();
                    cnn.Close();
                }
                if (numRecs == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Root_Assignment_Detail(assignmentHeader, rootId_mb, editStatus)VALUES(@assignmentHeader, @rootId, @editStatus)", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;

                        SqlParameter myParam = new SqlParameter("@assignmentHeader", SqlDbType.BigInt);
                        myParam.Value = (long)assignmentId;
                        cmd.Parameters.Add(myParam);

                        myParam = new SqlParameter("@rootId", SqlDbType.NVarChar);
                        myParam.Value = rootId;
                        cmd.Parameters.Add(myParam);

                        myParam = new SqlParameter("@editStatus", SqlDbType.Int);
                        myParam.Value = -1;
                        cmd.Parameters.Add(myParam);

                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the last import date.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastImportDate()
        {
            DateTime? importDate = null;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select syncDateImport from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    importDate = (DateTime?)cmd.ExecuteScalar();
                    cnn.Close();
                }
            }

            return importDate;
        }

        /// <summary>
        /// Verify if the import process is running
        /// </summary>
        /// <returns>true if is running, otherwise returns false</returns>
        public bool IsRunning()
        {
            DateTime? importDate = null;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select syncDateImport from dbo.SynchronizationDate_mb where isRunning = 1", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    importDate = (DateTime?)cmd.ExecuteScalar();
                    cnn.Close();
                    if (importDate.HasValue && importDate.Value >= DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the last import date for the entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        public DateTime? GetLastImportEntityDate(string entityName)
        {
            DateTime? importDate = null;

            if (!string.IsNullOrEmpty(entityName))
            {
                string sqlToken = GetSQLToken(cnnStr);
                using (var cnn = new SqlConnection(cnnStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("select lastDate from entityDateSync_mb where entityName = @entityName", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@entityName", entityName);
                        cnn.Open();
                        importDate = (DateTime?)cmd.ExecuteScalar();
                        cnn.Close();
                    }
                }
            }

            return importDate;
        }

        /// <summary>
        /// Gets the last export date.
        /// </summary>
        /// <returns></returns>
        public float GetLastExportDate()
        {
            float exportDate;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select isnull(syncDateExport, 0) from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    exportDate = (float)cmd.ExecuteScalar();  //or double
                    cnn.Close();
                }
            }

            return exportDate;
        }

        /// <summary>
        /// Sets the import date.
        /// </summary>
        /// <param name="importDate">The import date.</param>
        public void SetImportDate(DateTime importDate)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select 1 from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    int? value = (int?)cmd.ExecuteScalar();
                    if (value.HasValue)
                    {
                        using (SqlCommand updateCmd = new SqlCommand("update dbo.SynchronizationDate_mb set syncDateImport = @date", cnn))
                        {
                            updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                            updateCmd.Parameters.AddWithValue("@date", importDate);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("insert into dbo.SynchronizationDate_mb (syncDateImport) values (@date)", cnn))
                        {
                            insertCmd.CommandTimeout = cnn.ConnectionTimeout;
                            insertCmd.Parameters.AddWithValue("@date", importDate);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Set the running status
        /// </summary>
        /// <param name="importDate">The update date</param>
        /// <param name="isRunning">The status</param>
        public void SetRunningStatus(DateTime importDate, bool isRunning)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select 1 from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    int? value = (int?)cmd.ExecuteScalar();
                    if (value.HasValue)
                    {
                        using (SqlCommand updateCmd = new SqlCommand("update dbo.SynchronizationDate_mb set syncDateImport = @date, isRunning = @isRunning", cnn))
                        {
                            updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                            updateCmd.Parameters.AddWithValue("@date", importDate);
                            updateCmd.Parameters.AddWithValue("@isRunning", isRunning);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("insert into dbo.SynchronizationDate_mb (syncDateImport, isRunning) values (@date, @isRunning)", cnn))
                        {
                            insertCmd.CommandTimeout = cnn.ConnectionTimeout;
                            insertCmd.Parameters.AddWithValue("@date", importDate);
                            insertCmd.Parameters.AddWithValue("@isRunning", isRunning);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Sets the last import date for the entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="lastDate">The last date</param>
        public void SetImportEntityDate(string entityName, DateTime lastDate)
        {
            if (!string.IsNullOrEmpty(entityName) && lastDate != null)
            {
                string sqlToken = GetSQLToken(cnnStr);
                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("select 1 from entityDateSync_mb where entityName = @entityName", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@entityName", entityName);
                        cnn.Open();
                        int? value = (int?)cmd.ExecuteScalar();
                        if (value.HasValue)
                        {
                            using (SqlCommand updateCmd = new SqlCommand("update dbo.EntityDateSync_mb set lastDate = @lastDate where entityName = @entityName", cnn))
                            {
                                updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                                updateCmd.Parameters.AddWithValue("@lastDate", lastDate);
                                updateCmd.Parameters.AddWithValue("@entityName", entityName);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand insertCmd = new SqlCommand("insert into dbo.EntityDateSync_mb values(@entityName, @lastDate)", cnn))
                            {
                                insertCmd.CommandTimeout = cnn.ConnectionTimeout;
                                insertCmd.Parameters.AddWithValue("@lastDate", lastDate);
                                insertCmd.Parameters.AddWithValue("@entityName", entityName);
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        cnn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the export date.
        /// </summary>
        /// <param name="exportDate">The export date.</param>
        public void SetExportDate(float exportDate)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select 1 from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    int? value = (int?)cmd.ExecuteScalar();
                    if (value.HasValue)
                    {
                        using (SqlCommand updateCmd = new SqlCommand("update dbo.SynchronizationDate_mb set syncDateExport = @date", cnn))
                        {
                            updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                            updateCmd.Parameters.AddWithValue("@date", exportDate);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand("insert into dbo.SynchronizationDate_mb (syncDateExport) values (@date)", cnn))
                        {
                            insertCmd.CommandTimeout = cnn.ConnectionTimeout;
                            insertCmd.Parameters.AddWithValue("@date", exportDate);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Gets the modified entity data.
        /// </summary>
        /// <param name="rootId">The root Id.</param>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetModifiedEntityData(string rootId, string entityKind, Guid deviceGuid)
        {
            List<Dictionary<string, object>> modifiedEntities = new List<Dictionary<string, object>>();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                string commandString = "select * from dbo." + entityKind + "_Exp where deviceGuid_cgs = '" + deviceGuid.ToString() + "'";
                if (!String.IsNullOrEmpty(rootId))
                {
                    commandString += " and rootId_mb = '" + rootId + "'";
                }

                using (SqlCommand cmd = new SqlCommand(commandString, cnn))
                {
                    cnn.Open();
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> entity = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            entity.Add(reader.GetName(i), reader[i]);
                        }

                        modifiedEntities.Add(entity);
                    }

                    cnn.Close();
                }
            }

            return modifiedEntities;
        }

        /// <summary>
        /// Sets the changesets as exported.
        /// </summary>
        /// <param name="changesetList">The changeset list.</param>
        public void SetChangesetsExported(List<string> changesetList)
        {
            string changesets = string.Empty;
            foreach (string changeset in changesetList)
            {
                changesets += changeset + ",";
            }

            if (changesets.Length > 0)
            {
                changesets = changesets.Substring(0, changesets.Length - 1);
                string sqlToken = GetSQLToken(cnnStr);
                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("update dbo.Changesets_mb set statusTypeId_cgs = @status where id_cgs in (" + changesets + ")", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@status", (int)changeSetStatus.exported);
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the device GUID list.
        /// </summary>
        /// <returns>The device guid list</returns>
        public List<Guid> GetDeviceGuidList()
        {
            List<Guid> deviceList = new List<Guid>();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select guid_mb from Device_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        deviceList.Add(Guid.Parse(reader["guid_mb"].ToString()));
                    }

                    cnn.Close();
                }
            }

            return deviceList;
        }

        /// <summary>
        /// Gets a dictionary with the value pair of device guid and username of the device.
        /// </summary>
        /// <returns>Dictionay that contains the device guid and username of the device</returns>
        public Dictionary<Guid, string> GetDeviceInformation()
        {
            Dictionary<Guid, string> deviceDic = new Dictionary<Guid, string>();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select guid_mb, userName_mb from Device_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        deviceDic.Add(Guid.Parse(reader["guid_mb"].ToString()), reader["userName_mb"].ToString());
                    }

                    cnn.Close();
                }
            }

            return deviceDic;
        }

        /// <summary>
        /// Gets the user of the device.
        /// </summary>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns>The user of the device</returns>
        public string GetUser(Guid deviceGuid)
        {
            object userName = null;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select userName_mb from Device_mb where guid_mb = '" + deviceGuid.ToString() + "'", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    userName = cmd.ExecuteScalar();
                    cnn.Close();
                }
            }

            if (userName != null)
                return userName.ToString();
            else
                return null;
        }

        /// <summary>
        /// Sets the log header.
        /// </summary>
        /// <param name="fromBackend">if set to <c>true</c> [from backend].</param>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <param name="changesetID">The changeset ID.</param>
        /// <param name="groupIdentifier">The group identifier.</param>
        /// <returns></returns>
        public long SetLogHeader(bool fromBackend, DateTime date, string message, long? changesetID, string groupIdentifier)
        {
            long logHeaderID = -1;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("insert into dbo.ConnectorsLogHeader_mb (fromBackend_chl, date_chl, message_chl, changesetId_chl, groupIdentifier_chl) values (@fromBackend, @date, @message, @changesetID, @groupIdentifier); select scope_identity();", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@fromBackend", fromBackend);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@message", message);
                    if (changesetID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@changesetID", changesetID);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@changesetID", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@groupIdentifier", groupIdentifier);
                    cnn.Open();
                    logHeaderID = (long)((decimal)cmd.ExecuteScalar());
                    cnn.Close();
                }
            }

            return logHeaderID;
        }

        /// <summary>
        /// Sets the log detail.
        /// </summary>
        /// <param name="logHeaderId">The log header id.</param>
        /// <param name="message">The message.</param>
        /// <param name="result">The result.</param>
        /// <param name="messageType">The type of the message (Information = 1, Warning = 2, Error = 3).</param>
        public void SetLogDetail(long logHeaderId, string message, string result, logMessageType messageType)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("insert into dbo.ConnectorsLogDetail_mb (connectorsLogHeaderId_cdl, message_cdl, result_cdl, messageType_cdl) values (@logHeaderId, @message, @result, @messageType)", cnn))           
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@logHeaderId", logHeaderId);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@result", result);
                    cmd.Parameters.AddWithValue("@messageType", (int)messageType);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Cleans the old log information.
        /// </summary>
        /// <param name="daysNumber">The days number.</param>
        public void CleanOldLogInformation(int daysNumber)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("CleanOldLogInformation", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@daysNumber", daysNumber));
                    cnn.Open();
                    cmd.ExecuteNonQueryAsync();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Assigns the root entities to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        public void AssignRootEntitiesToUser(string userName, List<string> entityIdentifierList)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                long? assingmentHeaderID;
                using (SqlCommand cmd = new SqlCommand("select assignmentId from Root_Assignment_Header where userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    assingmentHeaderID = (long?)cmd.ExecuteScalar();
                    cnn.Close();
                }

                if (!assingmentHeaderID.HasValue)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into Root_Assignment_Header(assignmentDate, sourceDetail, userName_mb, currentProgressStatus, type, reasonId) values(@assignmentDate, @sourceDetail, @userName_mb, @currentProgressStatus, @type, @reasonId); select scope_identity();", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@assignmentDate", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@sourceDetail", "backend");
                        cmd.Parameters.AddWithValue("@userName_mb", userName);
                        cmd.Parameters.AddWithValue("@currentProgressStatus", 1);
                        cmd.Parameters.AddWithValue("@type", 1);
                        cmd.Parameters.AddWithValue("@reasonId", 1);
                        cnn.Open();
                        assingmentHeaderID = ((long?)(decimal?)cmd.ExecuteScalar());
                        cnn.Close();
                    }
                }

                foreach (string identifier in entityIdentifierList)
                {
                    AddEntityToAssignment(assingmentHeaderID.Value, identifier);
                }
            }
        }
        public long? GetAssignmentID(string userName)
        {
            long? assingmentHeaderID;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select assignmentId from Root_Assignment_Header where userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    assingmentHeaderID = (long?)cmd.ExecuteScalar();
                    cnn.Close();
                }

                if (!assingmentHeaderID.HasValue)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into Root_Assignment_Header(assignmentDate, sourceDetail, userName_mb, currentProgressStatus, type, reasonId) values(@assignmentDate, @sourceDetail, @userName_mb, @currentProgressStatus, @type, @reasonId); select scope_identity();", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@assignmentDate", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@sourceDetail", "backend");
                        cmd.Parameters.AddWithValue("@userName_mb", userName);
                        cmd.Parameters.AddWithValue("@currentProgressStatus", 1);
                        cmd.Parameters.AddWithValue("@type", 1);
                        cmd.Parameters.AddWithValue("@reasonId", 1);
                        cnn.Open();
                        assingmentHeaderID = ((long?)(decimal?)cmd.ExecuteScalar());
                        cnn.Close();
                    }
                }
            }

            return assingmentHeaderID;
        }

        /// <summary>
        /// Adds the entity to assignment.
        /// </summary>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        public void AddEntityToAssignment(long assignmentId, string rootId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                string commandString = @"declare @assignmentDetailId bigint
                                         select @assignmentDetailId = assignmentDetailId from Root_Assignment_Detail where assignmentHeader = @assignmentId and rootId_mb = @rootId
                                         if (@assignmentDetailId is not null) begin
                                            update Root_Assignment_Detail set editStatus = 0 where assignmentDetailId = @assignmentDetailId
                                         end
                                         else begin
                                            insert into Root_Assignment_Detail(assignmentHeader, rootId_mb, editStatus) values(@assignmentHeader, @rootId, @editStatus)
                                         end";

                using (SqlCommand cmd = new SqlCommand(commandString, cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@assignmentId", assignmentId);
                    cmd.Parameters.AddWithValue("@rootId", rootId);
                    cmd.Parameters.AddWithValue("@assignmentHeader", assignmentId);
                    cmd.Parameters.AddWithValue("@editStatus", 0);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Gets the root entities assigned to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public List<string> GetRootEntitiesAssignedToUser(string userName)
        {
            List<string> entities = new List<string>();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select rootId_mb from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and D.editStatus = 0 and H.userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        entities.Add(reader["rootId_mb"].ToString());
                    }

                    cnn.Close();
                }
            }

            return entities;
        }

        /// <summary>
        /// Deletes root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        public void DeleteRootEntitiesFromUser(string userName, List<string> entityIdentifierList)
        {
            string entities = string.Empty;
            string myJSon = "[";
            long assignmentID = -1;
            foreach (string entity in entityIdentifierList)
            {
                entities += "'" + entity + "', ";
                myJSon += "\"" + entity + "\", ";
            }

            if (entities.Length > 0)
            {
                entities = entities.Substring(0, entities.Length - 2);
                myJSon = myJSon.Substring(0, myJSon.Length - 2) + "]";
            }

            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("delete Root_Assignment_Detail from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName and D.rootId_mb in (" + entities + "); select assignmentId from Root_Assignment_Header where userName_mb = @userName;", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    assignmentID = (long)cmd.ExecuteScalar();
                    cnn.Close();
                }
            }

            AddToSynCommand(1, myJSon, assignmentID);
        }

        /// <summary>
        /// Deletes all root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void DeleteAllRootEntitiesFromUser(string userName)
        {
            List<string> entities = new List<string>();
            string myJSon = "[";
            long assignmentID = -1;
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select rootId_mb from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        entities.Add(reader["rootId_mb"].ToString());
                    }

                    cnn.Close();
                }

                if (entities.Count > 0)
                {
                    foreach (string entity in entities)
                    {
                        myJSon += "\"" + entity + "\", ";
                    }

                    myJSon = myJSon.Substring(0, myJSon.Length - 2) + "]";
                }

                using (SqlCommand cmd = new SqlCommand("delete Root_Assignment_Detail from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName; select assignmentId from Root_Assignment_Header where userName_mb = @userName;", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    assignmentID = (long)cmd.ExecuteScalar();
                    cnn.Close();
                }

                AddToSynCommand(1, myJSon, assignmentID);
            }
        }

        /// <summary>
        /// Adds to syn command table.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="myJSon">My Json.</param>
        public void AddToSynCommand(int commandType, string myJSon, long assignmentID)
        {
            long uploadTicket = GetUploadTicketForSpecificDevice(assignmentID);
            Guid newGuid = Guid.NewGuid();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("insert into SyncCommand_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", newGuid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (SqlCommand cmd = new SqlCommand("insert into SyncCommand_mb_Data(guid_mb, command_mb, param_mb, changesetId_mb) values(@guid_mb, @command_mb, @param_mb, @changesetId_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", newGuid);
                    cmd.Parameters.AddWithValue("@command_mb", commandType);
                    cmd.Parameters.AddWithValue("@param_mb", myJSon);
                    cmd.Parameters.AddWithValue("@changesetId_mb", uploadTicket);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }

            ChangesetCompleted(uploadTicket);
        }



        /// <summary>
        /// Creates the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="changesetId">The changeset id.</param>
        public void CreateLayer(Layer layer, long changesetId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                Guid mapGuid;
                using (SqlCommand cmd = new SqlCommand("select top 1 guid_mb from Maps_mb_Key", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result is DBNull)
                    {
                        mapGuid = CreateMap(changesetId);
                    }
                    else
                    {
                        mapGuid = Guid.Parse(result.ToString());
                    }

                    cnn.Close();
                }

                using (SqlCommand cmd = new SqlCommand("insert into Layers_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layer.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (SqlCommand cmd = new SqlCommand("insert into Layers_mb_Data(guid_mb, details, entityRootKeyField, geoColumnName, isOn, isRootEntityLayer, layerKeyFieldName, maximumScale, minimumScale, name, [order], shapeType, spatialReference, supportClipping, tableName, type, uniqueName, changesetId_mb, fk_layerSource, fk_map, fk_style) values(@guid_mb, @details, @entityRootKeyField, @geoColumnName, @isOn, @isRootEntityLayer, @layerKeyFieldName, @maximumScale, @minimumScale, @name, @order, @shapeType, @spatialReference, @supportClipping, @tableName, @type, @uniqueName, @changesetId_mb, @fk_layerSource, @fk_map, @fk_style)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layer.Identifier);
                    cmd.Parameters.AddWithValue("@details", layer.Details);
                    cmd.Parameters.AddWithValue("@entityRootKeyField", layer.EntityRootKeyField);
                    cmd.Parameters.AddWithValue("@geoColumnName", layer.GeoColumnName);
                    cmd.Parameters.AddWithValue("@isOn", layer.IsOn);
                    cmd.Parameters.AddWithValue("@isRootEntityLayer", layer.IsRootEntityLayer);
                    cmd.Parameters.AddWithValue("@layerKeyFieldName", layer.LayerKeyFieldName);
                    cmd.Parameters.AddWithValue("@maximumScale", layer.MaximumScale);
                    cmd.Parameters.AddWithValue("@minimumScale", layer.MinimumScale);
                    cmd.Parameters.AddWithValue("@name", layer.Name);
                    cmd.Parameters.AddWithValue("@order", layer.Order);
                    cmd.Parameters.AddWithValue("@shapeType", layer.ShapeType);
                    cmd.Parameters.AddWithValue("@spatialReference", layer.SpatialReference);
                    cmd.Parameters.AddWithValue("@supportClipping", layer.SupportClipping);
                    cmd.Parameters.AddWithValue("@tableName", layer.TableName);
                    cmd.Parameters.AddWithValue("@type", layer.Type);
                    cmd.Parameters.AddWithValue("@uniqueName", layer.UniqueName);
                    cmd.Parameters.AddWithValue("@changesetId_mb", changesetId);
                    cmd.Parameters.AddWithValue("@fk_layerSource", layer.LayerSourceIdentifier);
                    cmd.Parameters.AddWithValue("@fk_map", mapGuid);
                    cmd.Parameters.AddWithValue("@fk_style", layer.StyleIdentifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Creates the map.
        /// </summary>
        /// <param name="changesetId">The changeset id.</param>
        /// <returns></returns>
        public Guid CreateMap(long changesetId)
        {
            Guid mapGuid = Guid.NewGuid();
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("insert into Maps_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", mapGuid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (SqlCommand cmd = new SqlCommand("insert into Maps_mb_Data(guid_mb, createdDate, defaultScale, details, lastModifiedDate, name, tags, changesetId_mb) values(@guid_mb, getutcdate(), 2000, 'map 2', getutcdate(), 'Map 2', 'na', @changesetId)"))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", mapGuid);
                    cmd.Parameters.AddWithValue("@changesetId", changesetId);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }

            return mapGuid;
        }


        /// <summary>
        /// Creates the layer source.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <param name="changesetId">The changeset id.</param>
        public void CreateLayerSource(LayerSource layerSource, long changesetId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("insert into LayerSource_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layerSource.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (SqlCommand cmd = new SqlCommand("insert into LayerSource_mb_Data(guid_mb, createdDate, fileType, isLinked, lastModifiedDate, name, path, size, changesetId_mb) values(@guid_mb, @createdDate, @fileType, @isLinked, @lastModifiedDate, @name, @path, @size, @changesetId_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layerSource.Identifier);
                    cmd.Parameters.AddWithValue("@createdDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@fileType", layerSource.FileType);
                    cmd.Parameters.AddWithValue("@isLinked", layerSource.IsLinked);
                    cmd.Parameters.AddWithValue("@lastModifiedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@name", layerSource.Name);
                    cmd.Parameters.AddWithValue("@path", layerSource.Path);
                    cmd.Parameters.AddWithValue("@size", layerSource.Size);
                    cmd.Parameters.AddWithValue("@changesetId_mb", changesetId);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Updates the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="changesetId">The changeset id.</param>
        public void UpdateLayer(Layer layer, long changesetId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("update Layers_mb_Data set details = @details, entityRootKeyField = @entityRootKeyField, geoColumnName = @geoColumnName, isOn = @isOn, isRootEntityLayer = @isRootEntityLayer, " +
                                                       "layerKeyFieldName = @layerKeyFieldName, maximumScale = @maximumScale, minimumScale = @minimumScale, name = @name, [order] = @order, shapeType = @shapeType, " +
                                                       "spatialReference = @spatialReference, supportClipping = @supportClipping, tableName = @tableName, type = @type, uniqueName = @uniqueName, changesetId_mb = @changesetId_mb, " +
                                                       "fk_layerSource = @fk_layerSource, fk_style = @fk_style where guid_mb = @guid_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@details", layer.Details);
                    cmd.Parameters.AddWithValue("@entityRootKeyField", layer.EntityRootKeyField);
                    cmd.Parameters.AddWithValue("@geoColumnName", layer.GeoColumnName);
                    cmd.Parameters.AddWithValue("@isOn", layer.IsOn);
                    cmd.Parameters.AddWithValue("@isRootEntityLayer", layer.IsRootEntityLayer);
                    cmd.Parameters.AddWithValue("@layerKeyFieldName", layer.LayerKeyFieldName);
                    cmd.Parameters.AddWithValue("@maximumScale", layer.MaximumScale);
                    cmd.Parameters.AddWithValue("@minimumScale", layer.MinimumScale);
                    cmd.Parameters.AddWithValue("@name", layer.Name);
                    cmd.Parameters.AddWithValue("@order", layer.Order);
                    cmd.Parameters.AddWithValue("@shapeType", layer.ShapeType);
                    cmd.Parameters.AddWithValue("@spatialReference", layer.SpatialReference);
                    cmd.Parameters.AddWithValue("@supportClipping", layer.SupportClipping);
                    cmd.Parameters.AddWithValue("@tableName", layer.TableName);
                    cmd.Parameters.AddWithValue("@type", layer.Type);
                    cmd.Parameters.AddWithValue("@uniqueName", layer.UniqueName);
                    cmd.Parameters.AddWithValue("@changesetId_mb", changesetId);
                    cmd.Parameters.AddWithValue("@fk_layerSource", layer.LayerSourceIdentifier);
                    cmd.Parameters.AddWithValue("@fk_style", layer.StyleIdentifier);
                    cmd.Parameters.AddWithValue("@guid_mb", layer.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Updates the layer source.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <param name="changesetId">The changeset id.</param>
        public void UpdateLayerSource(LayerSource layerSource, long changesetId)
        {
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("update LayerSource_mb_Data set fileType = @fileType, isLinked = @isLinked, lastModifiedDate = @lastModifiedDate, name = @name, path = @path, size = @size, changesetId_mb = @changesetId_mb where guid_mb = @guid_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@fileType", layerSource.FileType);
                    cmd.Parameters.AddWithValue("@isLinked", layerSource.IsLinked);
                    cmd.Parameters.AddWithValue("@lastModifiedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@name", layerSource.Name);
                    cmd.Parameters.AddWithValue("@path", layerSource.Path);
                    cmd.Parameters.AddWithValue("@size", layerSource.Size);
                    cmd.Parameters.AddWithValue("@changesetId_mb", changesetId);
                    cmd.Parameters.AddWithValue("@guid_mb", layerSource.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the map configuration entities.
        /// </summary>
        /// <param name="entitiesList">The entities list.</param>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="changesetId">The changeset id.</param>
        public void DeleteMapConfigurationEntities(string entitiesList, string entityKind, long changesetId)
        {
            if (!string.IsNullOrEmpty(entitiesList))
            {
                string sqlToken = GetSQLToken(cnnStr);
                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("update " + entityKind + "_Data set rowStatus_mb = 'D', changesetId_mb = @changesetId where guid_mb in (" + entitiesList + ")", cnn))
                    {
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.AddWithValue("@changesetId", changesetId);
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Layerses to delete.
        /// </summary>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        public string LayersToDelete(List<Layer> layers)
        {
            string layersToDelete = string.Empty;
            foreach (Layer layer in layers.Where(l => l.RowStatus == "d"))
            {
                layersToDelete += "'" + layer.Identifier.ToString() + "', ";
            }

            if (layersToDelete.Length > 0)
                layersToDelete = layersToDelete.Substring(0, layersToDelete.Length - 2);

            return layersToDelete;
        }


        public List<Dictionary<string, object>> GetModificationReasonForEntities(List<string> entitiesGuidList, Guid deviceGuid)
        {
            List<Dictionary<string, object>> modifiedEntities = new List<Dictionary<string, object>>();
            string entitiesGuidString = string.Empty;
            foreach (string guid in entitiesGuidList)
            {
                entitiesGuidString += "'" + guid + "',";
            }

            entitiesGuidString = entitiesGuidString.Substring(0, entitiesGuidString.Length - 1);
            string sqlToken = GetSQLToken(cnnStr);
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.AccessToken = sqlToken;
                string commandString = "select cld.entityGuid, cld.openKey5 reasonCode from dbo.ChangeLog_mb_Exp cld inner join " +
                                       "(select max(ChangeDate) changeDate, entityGuid from dbo.ChangeLog_mb_Exp where deviceGuid_cgs = @deviceGuid " +
                                       "and entityGuid in (" + entitiesGuidString + ") group by entityGuid) Temp on cld.changeDate = Temp.changeDate " +
                                       "and cld.entityGuid = Temp.entityGuid";

                using (SqlCommand cmd = new SqlCommand(commandString, cnn))
                {
                    cnn.Open();
                    cmd.Parameters.AddWithValue("@deviceGuid", deviceGuid);
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> entity = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            entity.Add(reader.GetName(i), reader[i]);
                        }

                        modifiedEntities.Add(entity);
                    }

                    cnn.Close();
                }
            }

            return modifiedEntities;
        }

        public Dictionary<string, object> GetFlightPlanInformation(string flightId)
        {
            return null;
        }

        public List<Dictionary<string, object>> GetFlightPlanLinesInformation(string flightId)
        {
            return null;
        }

        public List<Dictionary<string, object>> GetFlightPlanFilesInformation(string flightId)
        {
            return null;
        }

        public Dictionary<string, object> GetFlightPlanFileInformation(Guid fileUuid)
        {
            return null;
        }
    }
}
