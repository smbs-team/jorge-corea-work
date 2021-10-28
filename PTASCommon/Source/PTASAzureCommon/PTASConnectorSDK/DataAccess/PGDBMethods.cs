using System;
using System.Collections.Generic;
using System.Linq;
using PTASConnectorSDK.DataModel;
using System.Data;
using FastMember;
using Npgsql;

namespace PTASConnectorSDK.DataAccess
{
    class PGDBMethods : IDBMethods
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DBMethods" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PGDBMethods(string connectionString)
        {
            cnnStr = connectionString;
        }


        public long? GetAssignmentID(string userName)
        {
            return 0;
        }

        /// <summary>
        /// Adds the upload data.
        /// </summary>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="dataToUpload">The data to upload.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        public void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket)
        {
            try
            {
                long maxOrder = 0;
                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("Select coalesce(Max(dataOrder_upl), 0) from UploadPool_mb Where changeSetID_upl = @ticket", cnn))
                    {
                        DateTime currentUTCDate = DateTime.UtcNow;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new NpgsqlParameter("@ticket", uploadTicket));
                        cnn.Open();
                        var result = cmd.ExecuteScalar();
                        maxOrder = (long)result;
                        cnn.Close();
                    }

                    
                    cnn.Open();
                    using (var writer = cnn.BeginBinaryImport("COPY UploadPool_mb (changeSetID_upl, entityKind_upl, strData_upl, dataOrder_upl) FROM STDIN (FORMAT BINARY)"))
                    {
                        foreach (Dictionary<string, object> currentFields in dataToUpload)
                        {
                            maxOrder++;
                            string data = string.Join("&#!", currentFields.Select(x => x.Key + "&#$" + x.Value));

                            writer.StartRow();
                            writer.Write(uploadTicket, NpgsqlTypes.NpgsqlDbType.Bigint); //check if long is equivalent to Numeric type
                            writer.Write(entityKind, NpgsqlTypes.NpgsqlDbType.Varchar);
                            writer.Write(data, NpgsqlTypes.NpgsqlDbType.Varchar);
                            writer.Write(maxOrder, NpgsqlTypes.NpgsqlDbType.Bigint); //check if long is equivalent to Numeric type

                        }
                    }

                    cnn.Close();

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                Guid? devGuid = null;
                if (!string.IsNullOrEmpty(deviceID))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("Select guid_mb from dbo.Device_mb Where deviceId_mb = @deviceID", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new NpgsqlParameter("@deviceID", deviceID));
                        cnn.Open();
                        devGuid = (Guid)cmd.ExecuteScalar();
                        cnn.Close();
                    }
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, NOW(), round(cast((cast(NOW() as date) - cast('2001-01-01 00:00:00' as date)) as float) * 86400), @deviceGuid, @statusTypeId, @assignmentId); SELECT currval('changesets_sec');", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlParameter myParam = new NpgsqlParameter("@changesetTypeId", SqlDbType.Int);
                    if (!string.IsNullOrEmpty(deviceID))
                        myParam.Value = (int)changeSetType.deviceUpload;
                    else
                        myParam.Value = (int)changeSetType.backendUpload;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                    if (!string.IsNullOrEmpty(deviceID))
                        myParam.Value = devGuid;
                    else
                        myParam.Value = DBNull.Value;

                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@statusTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetStatus.created;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@assignmentId", SqlDbType.BigInt);
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
                    chngsetID = (long)cmd.ExecuteScalar();
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
            try
            {
                long chngsetID = -1;
                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, NOW() , round( cast(cast(NOW() as date)- CAST('2001-01-01 00:00:00' as date) as float) * 86400), @deviceGuid, @statusTypeId, @assignmentId); SELECT currval('changesets_sec');", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        NpgsqlParameter myParam = new NpgsqlParameter("@changesetTypeId", SqlDbType.Int);
                        myParam.Value = (int)changeSetType.specificDevice;
                        cmd.Parameters.Add(myParam);

                        myParam = new NpgsqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                        myParam.Value = DBNull.Value;
                        cmd.Parameters.Add(myParam);

                        myParam = new NpgsqlParameter("@statusTypeId", SqlDbType.Int);
                        myParam.Value = (int)changeSetStatus.created;
                        cmd.Parameters.Add(myParam);

                        myParam = new NpgsqlParameter("@assignmentId", SqlDbType.BigInt);
                        myParam.Value = DBNull.Value;
                        cmd.Parameters.Add(myParam);

                        cnn.Open();
                        chngsetID = (long)cmd.ExecuteScalar();
                        cnn.Close();
                    }
                }

                return chngsetID;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return 0;
        }

        public long GetUploadTicketForSpecificDevice(long assignmentID)
        {
            long chngsetID = -1;
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Changesets_mb(changesetTypeId_cgs, date_cgs, fdate_cgs, deviceGuid_cgs, statusTypeId_cgs, assignmentId) Values(@changesetTypeId, CAST(NOW() at time zone 'utc'), round(cast((CAST(NOW() at time zone 'utc') - '2001-01-01 00:00:00') as float) * 86400, 1000000), @deviceGuid, @statusTypeId, @assignmentId); SELECT currval('changetsets_sec');", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlParameter myParam = new NpgsqlParameter("@changesetTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetType.specificDevice;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@deviceGuid", SqlDbType.UniqueIdentifier);
                    myParam.Value = DBNull.Value;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@statusTypeId", SqlDbType.Int);
                    myParam.Value = (int)changeSetStatus.created;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@assignmentId", SqlDbType.BigInt);
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                Guid currentSetId = Guid.NewGuid();
                long maxOrder = 0;

                foreach (GetGuidObject currentObj in objectList)
                {
                    cnn.Open();
                    using (var writer = cnn.BeginBinaryImport("COPY GuidQuery_mb (guidSetID_gq, entityKind_gq, strData_gq, dataOrder_gq) FROM STDIN (FORMAT BINARY)"))
                    {
                        foreach (Dictionary<string, object> currentFields in currentObj.keyFields)
                        {
                            string data = string.Join("&#!", currentFields.Select(x => x.Key + "&#$" + x.Value));

                            writer.StartRow();
                            writer.Write(currentSetId, NpgsqlTypes.NpgsqlDbType.Numeric); //check if long is equivalent to Numeric type
                            writer.Write(currentObj.kind);
                            writer.Write(data);
                            writer.Write(maxOrder, NpgsqlTypes.NpgsqlDbType.Numeric); //check if long is equivalent to Numeric type
                        }
                    }

                    cnn.Close();
                }

                //Executes the SP to assign the Guids
                using (NpgsqlCommand cmd = new NpgsqlCommand("PopulateEntityGuids", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    NpgsqlParameter myParam = new NpgsqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
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
                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from GuidQuery_mb Where guidSetID_gq = @guidSetID", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    NpgsqlParameter myParam = new NpgsqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
                    myParam.Value = currentSetId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
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
                using (NpgsqlCommand cmd = new NpgsqlCommand("Delete dbo.GuidQuery_mb where guidSetID_gq = @guidSetID", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;

                    NpgsqlParameter myParam = new NpgsqlParameter("@guidSetID", SqlDbType.UniqueIdentifier);
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
            try
            {
                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    //Store the imported data in to the tables, also update relationships and guids.
                    using (NpgsqlCommand cmd = new NpgsqlCommand("processdata", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;

                        NpgsqlParameter changeSetParam = new NpgsqlParameter("@v_changesetid", NpgsqlTypes.NpgsqlDbType.Bigint);
                        changeSetParam.Value = changeSetID;
                        cmd.Parameters.Add(changeSetParam);

                        NpgsqlParameter fromDeviceParam = new NpgsqlParameter("@v_fromdevice", NpgsqlTypes.NpgsqlDbType.Boolean);
                        fromDeviceParam.Value = fromDevice;
                        cmd.Parameters.Add(fromDeviceParam);
                        
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }

                    if (lastExecution)
                    {
                        //Marks the changeset as completed.
                        using (NpgsqlCommand cmd = new NpgsqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = cnn.ConnectionTimeout;

                            NpgsqlParameter statusParam = new NpgsqlParameter("@statusTypeId", NpgsqlTypes.NpgsqlDbType.Bigint);
                            statusParam.Value = (long)changeSetStatus.ready;
                            cmd.Parameters.Add(statusParam);

                            NpgsqlParameter changeSetParam = new NpgsqlParameter("@idChangeset", NpgsqlTypes.NpgsqlDbType.Bigint);
                            changeSetParam.Value = changeSetID;
                            cmd.Parameters.Add(changeSetParam);

                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            cnn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        /// <summary>
        /// Set Changeset state to completed.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        public void ChangesetCompleted(long changeSetID)
        {
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new NpgsqlParameter("@statusTypeId", (long)changeSetStatus.ready));
                    cmd.Parameters.Add(new NpgsqlParameter("@idChangeset", changeSetID));
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("Update dbo.Changesets_mb set statusTypeId_cgs = @statusTypeId where id_cgs = @idChangeset", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new NpgsqlParameter("@statusTypeId", (long)changeSetStatus.downloadedToDevice));
                    cmd.Parameters.Add(new NpgsqlParameter("@idChangeset", changeSetID));
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                long numRecs = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand("Select count(1) from dbo.Root_Assignment_Detail where assignmentHeader = @assignmentId and rootId_mb = @rootId", cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlParameter myParam = new NpgsqlParameter("@assignmentId", SqlDbType.BigInt);
                    myParam.Value = (long)assignmentId;
                    cmd.Parameters.Add(myParam);

                    myParam = new NpgsqlParameter("@rootId", SqlDbType.NVarChar);
                    myParam.Value = rootId;
                    cmd.Parameters.Add(myParam);

                    cnn.Open();
                    numRecs = (int)cmd.ExecuteScalar();
                    cnn.Close();
                }
                if (numRecs == 0)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Root_Assignment_Detail(assignmentHeader, rootId_mb, editStatus)VALUES(@assignmentHeader, @rootId, @editStatus)", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;

                        NpgsqlParameter myParam = new NpgsqlParameter("@assignmentHeader", SqlDbType.BigInt);
                        myParam.Value = (long)assignmentId;
                        cmd.Parameters.Add(myParam);

                        myParam = new NpgsqlParameter("@rootId", SqlDbType.NVarChar);
                        myParam.Value = rootId;
                        cmd.Parameters.Add(myParam);

                        myParam = new NpgsqlParameter("@editStatus", SqlDbType.Int);
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select syncDateImport from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    importDate = (DateTime?)cmd.ExecuteScalar();
                    cnn.Close();
                }
            }

            return importDate;
        }

        public bool IsRunning()
        {
            ///TBD
            return false;
        }

        public DateTime? GetLastImportEntityDate(string entityName)
        {
            ///TBD
            return null;
        }

        /// <summary>
        /// Gets the last export date.
        /// </summary>
        /// <returns></returns>
        public float GetLastExportDate()
        {
            float exportDate;
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select coalesce(syncDateExport, 0) from dbo.SynchronizationDate_mb", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select 1 from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    int? value = (int?)cmd.ExecuteScalar();
                    if (value.HasValue)
                    {
                        using (NpgsqlCommand updateCmd = new NpgsqlCommand("update dbo.SynchronizationDate_mb set syncDateImport = @date", cnn))
                        {
                            updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                            updateCmd.Parameters.AddWithValue("@date", importDate);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (NpgsqlCommand insertCmd = new NpgsqlCommand("insert into dbo.SynchronizationDate_mb (syncDateImport) values (@date)", cnn))
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

        public void SetRunningStatus(DateTime importDate, bool isRunning)
        {
            ///TBD;
        }

        public void SetImportEntityDate(string entityName, DateTime lastDate)
        {
            ///TBD;
        }

        /// <summary>
        /// Sets the export date.
        /// </summary>
        /// <param name="exportDate">The export date.</param>
        public void SetExportDate(float exportDate)
        {
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select 1 from dbo.SynchronizationDate_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    int? value = (int?)cmd.ExecuteScalar();
                    if (value.HasValue)
                    {
                        using (NpgsqlCommand updateCmd = new NpgsqlCommand("update dbo.SynchronizationDate_mb set syncDateExport = @date", cnn))
                        {
                            updateCmd.CommandTimeout = cnn.ConnectionTimeout;
                            updateCmd.Parameters.AddWithValue("@date", exportDate);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (NpgsqlCommand insertCmd = new NpgsqlCommand("insert into dbo.SynchronizationDate_mb (syncDateExport) values (@date)", cnn))
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
            List<Dictionary<string, object>> modifiedEntities = new List<Dictionary<string,object>>();
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                string commandString = "select * from dbo." + entityKind + "_Exp where deviceGuid_cgs = @deviceGuid";
                if (!String.IsNullOrEmpty(rootId))
                {
                    commandString += " and rootId_mb = '" + rootId + "'";
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                {
                    cnn.Open();
                    cmd.Parameters.AddWithValue("@deviceGuid", deviceGuid);
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> entity = new Dictionary<string,object>();
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
                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("update dbo.Changesets_mb set statusTypeId_cgs = @status where id_cgs in (" + changesets + ")", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select guid_mb from Device_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select guid_mb, userName_mb from Device_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select userName_mb from Device_mb where guid_mb = '" + deviceGuid.ToString() + "'", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into dbo.ConnectorsLogHeader_mb (fromBackend_chl, date_chl, message_chl, changesetId_chl, groupIdentifier_chl) values (@fromBackend, @date, @message, @changesetID, @groupIdentifier); select scope_identity();", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into dbo.ConnectorsLogDetail_mb (connectorsLogHeaderId_cdl, message_cdl, result_cdl, messageType_cdl) values (@logHeaderId, @message, @result, @messageType)", cnn))             
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("CleanOldLogInformation", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new NpgsqlParameter("@daysNumber", daysNumber));
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                long? assingmentHeaderID;
                using (NpgsqlCommand cmd = new NpgsqlCommand("select assignmentId from Root_Assignment_Header where userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    assingmentHeaderID = (long?)cmd.ExecuteScalar();
                    cnn.Close();
                }

                if (!assingmentHeaderID.HasValue)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("insert into Root_Assignment_Header(assignmentDate, sourceDetail, userName_mb, currentProgressStatus, type, reasonId) values(@assignmentDate, @sourceDetail, @userName_mb, @currentProgressStatus, @type, @reasonId); select scope_identity();", cnn))
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

        /// <summary>
        /// Adds the entity to assignment.
        /// </summary>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        public void AddEntityToAssignment(long assignmentId, string rootId)
        {
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                string commandString = @"declare @assignmentDetailId bigint
                                         select @assignmentDetailId = assignmentDetailId from Root_Assignment_Detail where assignmentHeader = @assignmentId and rootId_mb = @rootId
                                         if (@assignmentDetailId is not null) begin
                                            update Root_Assignment_Detail set editStatus = 0 where assignmentDetailId = @assignmentDetailId
                                         end
                                         else begin
                                            insert into Root_Assignment_Detail(assignmentHeader, rootId_mb, editStatus) values(@assignmentHeader, @rootId, @editStatus)
                                         end";

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select rootId_mb from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and D.editStatus = 0 and H.userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
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

            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("delete Root_Assignment_Detail from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName and D.rootId_mb in (" + entities + "); select assignmentId from Root_Assignment_Header where userName_mb = @userName;", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select rootId_mb from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cnn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
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

                using (NpgsqlCommand cmd = new NpgsqlCommand("delete Root_Assignment_Detail from Root_Assignment_Detail D inner join Root_Assignment_Header H on D.assignmentHeader = H.assignmentId and H.userName_mb = @userName; select assignmentId from Root_Assignment_Header where userName_mb = @userName;", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into SyncCommand_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", newGuid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into SyncCommand_mb_Data(guid_mb, command_mb, param_mb, changesetId_mb) values(@guid_mb, @command_mb, @param_mb, @changesetId_mb)", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                Guid mapGuid;
                using (NpgsqlCommand cmd = new NpgsqlCommand("select top 1 guid_mb from Maps_mb_Key", cnn))
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

                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into Layers_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layer.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into Layers_mb_Data(guid_mb, details, entityRootKeyField, geoColumnName, isOn, isRootEntityLayer, layerKeyFieldName, maximumScale, minimumScale, name, [order], shapeType, spatialReference, supportClipping, tableName, type, uniqueName, changesetId_mb, fk_layerSource, fk_map, fk_style) values(@guid_mb, @details, @entityRootKeyField, @geoColumnName, @isOn, @isRootEntityLayer, @layerKeyFieldName, @maximumScale, @minimumScale, @name, @order, @shapeType, @spatialReference, @supportClipping, @tableName, @type, @uniqueName, @changesetId_mb, @fk_layerSource, @fk_map, @fk_style)", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into Maps_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", mapGuid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into Maps_mb_Data(guid_mb, createdDate, defaultScale, details, lastModifiedDate, name, tags, changesetId_mb) values(@guid_mb, getutcdate(), 2000, 'map 2', getutcdate(), 'Map 2', 'na', @changesetId)"))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into LayerSource_mb_Key(guid_mb) values(@guid_mb)", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@guid_mb", layerSource.Identifier);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into LayerSource_mb_Data(guid_mb, createdDate, fileType, isLinked, lastModifiedDate, name, path, size, changesetId_mb) values(@guid_mb, @createdDate, @fileType, @isLinked, @lastModifiedDate, @name, @path, @size, @changesetId_mb)", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("update Layers_mb_Data set details = @details, entityRootKeyField = @entityRootKeyField, geoColumnName = @geoColumnName, isOn = @isOn, isRootEntityLayer = @isRootEntityLayer, " +
                                                       "layerKeyFieldName = @layerKeyFieldName, maximumScale = @maximumScale, minimumScale = @minimumScale, name = @name, [order] = @order, shapeType = @shapeType, " +
                                                       "spatialReference = @spatialReference, supportClipping = @supportClipping, tableName = @tableName, type = @type, uniqueName = @uniqueName, changesetId_mb = @changesetId_mb, " +
                                                       "fk_layerSource = @fk_layerSource, fk_style = @fk_style where guid_mb = @guid_mb", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.AddWithValue("@details", layer.Details);
                    cmd.Parameters.AddWithValue("@entityRootKeyField",layer.EntityRootKeyField);
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("update LayerSource_mb_Data set fileType = @fileType, isLinked = @isLinked, lastModifiedDate = @lastModifiedDate, name = @name, path = @path, size = @size, changesetId_mb = @changesetId_mb where guid_mb = @guid_mb", cnn))
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
                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("update " + entityKind + "_Data set rowStatus_mb = 'D', changesetId_mb = @changesetId where guid_mb in (" + entitiesList + ")", cnn))
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
            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                string commandString = "select cld.entityGuid, cld.openKey5 reasonCode from dbo.ChangeLog_mb_Exp cld inner join "+
                                       "(select max(ChangeDate) changeDate, entityGuid from dbo.ChangeLog_mb_Exp where deviceGuid_cgs = @deviceGuid " +
                                       "and entityGuid in (" + entitiesGuidString + ") group by entityGuid) Temp on cld.changeDate = Temp.changeDate " +
                                       "and cld.entityGuid = Temp.entityGuid";
                
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                {
                    cnn.Open();
                    cmd.Parameters.AddWithValue("@deviceGuid", deviceGuid);
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlDataReader reader = cmd.ExecuteReader();
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
            Dictionary<string, object> entity = new Dictionary<string, object>();

            using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
            {
                string commandString = "select flight_plan_id, description as project_name, flight_plan_type, flight_number, flight_plan_name, full_flight_plan_name, " +
                    "flight_plan_is_completed, data_source_cd, flight_plans.created_date, flight_plans.created_by_user_id, flight_plans.modified_date, flight_plans.modified_by_user_id, " +
                    "flight_plan_uuid, flight_plan_xml, first_flight_datetime, last_flight_datetime, cpid, capture_number, capture_set, grid_name, metro, spec, reason, " +
                    "checkout_by, reassign_to, total_flight_time, total_flight_length, altitude, estimated_time from flight_plans left outer join projects " +
                    "on flight_plans.project_name = projects.project_name where flight_plan_id = " + flightId;


                using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                {
                    cnn.Open();
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            entity.Add(reader.GetName(i), reader[i]);
                        }

                        break;
                    }

                    cnn.Close();
                }
            }

            return entity;
        }

        public List<Dictionary<string, object>> GetFlightPlanLinesInformation(string flightId)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    string commandString = "select * from flight_plan_lines_detail where flight_plan_id = " + flightId;

                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        NpgsqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Dictionary<string, object> entity = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                entity.Add(reader.GetName(i), reader[i]);
                            }

                            list.Add(entity);
                        }

                        cnn.Close();
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }

        public List<Dictionary<string, object>> GetFlightPlanFilesInformation(string flightId)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    string commandString = "select * from files where flight_plan_id = " + flightId;

                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                    {
                        cnn.Open();
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        NpgsqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Dictionary<string, object> entity = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                entity.Add(reader.GetName(i), reader[i]);
                            }

                            list.Add(entity);
                        }

                        cnn.Close();
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }

        public Dictionary<string, object> GetFlightPlanFileInformation(Guid fileUuid)
        {
            try
            {
                Dictionary<string, object> entity = new Dictionary<string, object>();

                using (NpgsqlConnection cnn = new NpgsqlConnection(cnnStr))
                {
                    string commandString = "select * from files where file_uuid = @fileUuid";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@fileUuid", fileUuid);
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        NpgsqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            entity = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                entity.Add(reader.GetName(i), reader[i]);
                            }
                        }

                        cnn.Close();
                    }
                }

                return entity;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
    }
}
