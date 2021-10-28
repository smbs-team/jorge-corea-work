using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTASSyncService.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using PTASSyncService.Utilities;
using PTASConnectorSDK;
using ConnectorService.Utilities;
using ConnectorService;
using Microsoft.AspNetCore.Cors;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace PTASSyncService.Controllers
{
    /// <summary>
    /// Changeset types
    /// </summary>
    public enum changeSetType
    {
        deviceUpload = 1,
        backendUpload = 2,
        specificDevice = 3
    }

    /// <summary>
    /// Change set status
    /// </summary>
    public enum changeSetStatus
    {
        created = 1,
        inProgress = 2,
        ready = 3,
        exported = 4,
        downloadedToDevice = 5
    }

    /// <summary>
    /// Class to handle synchronization between device and SQL Server.
    /// </summary>
    public class SyncController : Controller
    {
        /// <summary>
        /// App configuration settings.
        /// </summary>
        private readonly Settings Configuration;

        private bool isDBDirect = false;

        private ConnectorSDK myConnectorObj;
        //private ConnectorSDK backendConnectorObj;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration settings</param>
        public SyncController(Settings config)
        {
            this.Configuration = config;
            myConnectorObj = new ConnectorSDK(Environment.GetEnvironmentVariable("connectionString"), SQLServerType.MSSQL);
            //backendConnectorObj = new ConnectorSDK(Configuration.backendConnectionString, SQLServerType.MSSQL);

        }

        /// <summary>
        /// Verifies the service is up.
        /// </summary>
        /// <returns>Ok message</returns>
        [Route("api/Sync/HealthCheck")]
        [HttpGet]
        [SwaggerOperation(OperationId = "HealthCheck")]
        public string Get()
        {
            return "Endpoint is OK";
        }

        /// <summary>
        /// Gets the database status
        /// </summary>
        /// <param name="cnnName">Connection name</param>
        /// <returns>Dictionary with the connection string.</returns>
        // GET api/Sync/GetDBStatus
        [Route("api/Sync/GetDBStatus")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetDBStatus")]
        public Dictionary<string, string> DBStatus(string cnnName)
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            resultDict.Add(cnnName, this.testConnectionString(cnnName));

            return resultDict;
        }

        /// <summary>
        /// Tests the connection string.
        /// </summary>
        /// <param name="cnnStrName">Connection string name</param>
        /// <returns>Ok message</returns>
        private string testConnectionString(string cnnStrName)
        {
            try
            {
                string cnnStr = Environment.GetEnvironmentVariable(cnnStrName);
                if (cnnStr != null && cnnStr.Trim().Length > 0)
                {
                    var scnstr = new SqlConnectionStringBuilder(cnnStr);
                    scnstr.ConnectTimeout = 5;
                    string sqlToken = SQLTokenUtility.GetSQLToken(scnstr.ConnectionString);
                    using (SqlConnection cnn = new SqlConnection(scnstr.ConnectionString))
                    {
                        cnn.AccessToken = sqlToken;
                        cnn.Open();
                        cnn.Close();
                        return "Connection OK";
                    }
                }
                else
                {
                    return "Connection Missing or Empty";
                }
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "testConnectionString", ex, Configuration);
                return "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Gets a download ticket.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <param name="lastSyncDate">Last sync date</param>
        /// <param name="assignmentID">Assignment id</param>
        /// <returns>Ok if successful</returns>
        [Route("api/Sync/GetDownloadTicket")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetDownloadTicket")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetDownloadTicket(string deviceID, string area, double lastSyncDate, long assignmentID)
        {
            try
            {
                var userName = JWTDecoder.GetEmailFromHeader(this.HttpContext);
                List<GetNumberOfRecordsToDownloadModel> resultList = new List<GetNumberOfRecordsToDownloadModel>();
                string connStr = Environment.GetEnvironmentVariable("backendConnectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    cnn.AccessToken = sqlToken;
                    //Prepare the download package for the device returning the downloadID
                    using (SqlCommand cmd = new SqlCommand("GetNumberOfRecordsToDownload", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@lastSyncDate", lastSyncDate));
                        cmd.Parameters.Add(new SqlParameter("@userName", userName));
                        cmd.Parameters.Add(new SqlParameter("@area", area));
                        cmd.Parameters.Add(new SqlParameter("@assignmentId", assignmentID));
                        cnn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            GetNumberOfRecordsToDownloadModel resultObj = new GetNumberOfRecordsToDownloadModel();

                            resultObj.NumRecs = (int?)rdr["NumRecs"];
                            resultObj.SourceTable = (string)rdr["SourceTable"];
                            resultObj.downloadId = (long?)rdr["downloadId"];
                            resultObj.syncDate = (double?)rdr["syncDate"];
                            resultList.Add(resultObj);
                        }
                        cnn.Close();
                    }
                }
                return Ok(resultList);
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "GetDownloadTicket", ex, Configuration);
                var result = Json("{error:" + ex.Message + "}");
                result.StatusCode = 500;
                return result;
            }
        }

        /// <summary>
        /// Marks a changeset as downloaded.
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="deviceID">Device id</param>
        /// <returns>Ok if successful</returns>
        // GET api/Sync/GetMarkChangesetAsDownloaded
        [Route("api/Sync/GetMarkChangesetAsDownloaded")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetMarkChangesetAsDownloaded")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetMarkChangesetAsDownloaded(long ticket, string deviceID)
        {
            try
            {
                string connStr = Environment.GetEnvironmentVariable("connectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    cnn.AccessToken = sqlToken;
                    //Read the changesetID related with the download.
                    using (SqlCommand cmd = new SqlCommand("Select changesetId_mb From Sync_Download_mb Where Id_mb = @downloadID", cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;

                        SqlParameter myParam = new SqlParameter("@downloadID", SqlDbType.BigInt);
                        myParam.Value = ticket;
                        cmd.Parameters.Add(myParam);

                        cnn.Open();
                        var changesetID = cmd.ExecuteScalar();
                        cnn.Close();
                        if (changesetID != null)
                            myConnectorObj.ChangesetDownloaded((long)changesetID);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "GetMarkChangesetAsDownloaded", ex, Configuration);
                var result = Json("{error:" + ex.Message + "}");
                result.StatusCode = 500;
                return result;
            }
        }

        /// <summary>
        /// Gets the process data.
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="isDevice">True whether is a device.</param>
        /// <returns>Ok if successful</returns>
        // GET api/Sync/GetProcessData
        [Route("api/Sync/GetProcessData")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetProcessData")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetProcessData(long ticket, int isDevice)
        {
            try
            {
                myConnectorObj.ProcessDataForTicket(ticket, isDevice == 0 ? false : true, true);
                string connStr = Environment.GetEnvironmentVariable("connectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("AfterSyncProcess", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@changesetId", ticket));
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "GetProcessData", ex, Configuration);
                var result = Json("{error:" + ex.Message + "}");
                result.StatusCode = 500;
                return result;
            }
        }

        /// <summary>
        /// Returns the upload ticket number.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <param name="assignmentID">Assignment id</param>
        /// <returns>Changeset id</returns>
        // GET api/Sync/GetUploadTicket
        [Route("api/Sync/GetUploadTicket")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetUploadTicket")]
        //[Authorize]
        [AllowAnonymous]
        public long GetUploadTicket(string deviceID, long assignmentID)
        {
            try
            {
                long chngsetID = -1;


                chngsetID = myConnectorObj.GetUploadTicketForDevice(deviceID, assignmentID);

                return chngsetID;
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "GetUploadTicket", ex, Configuration);
                return 0;
            }
        }

        // GET api/Sync
        [Route("api/Sync")]
        [SwaggerOperation(OperationId = "GetSync")]
        [HttpGet]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult Get(string entityKind, long from, long to, long ticket)
        {
            string tbl = string.Empty;
            var now = DateTime.Now;
            try
            {
                DatabaseModel databaseModel = DatabaseModelHelper.GetDatabaseModel();
                tbl = (from ent in databaseModel.Entities
                       where ent.Name == entityKind
                       select ent.Name).FirstOrDefault();
            }
            catch (Exception ex1)
            {
                LogClass.ReportError("SyncController", "Get entityKind, from, to , ticket", ex1, Configuration);
                var result = Json("{error:" + ex1.Message + "}");
                result.StatusCode = 500;
                return result;
            }

            if (string.IsNullOrEmpty(tbl))
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
                    string connStr = Environment.GetEnvironmentVariable("connectionString");
                    string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                    using (SqlConnection cnn = new SqlConnection(connStr))
                    {
                        cnn.AccessToken = sqlToken;
                        using (SqlCommand cmd = new SqlCommand("Select * from " + entityKind + "_View Where downloadId_mb = " + ticket.ToString() + " and sortOrder_mb > " + from.ToString() + " and sortOrder_mb <= " + to.ToString(), cnn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = cnn.ConnectionTimeout;
                            cnn.Open();
                            SqlDataReader rdr = cmd.ExecuteReader();

                            // iterate through results, printing each to console
                            while (rdr.Read())
                            {
                                Dictionary<string, object> resultDict = new Dictionary<string, object>();
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {
                                    if (rdr.IsDBNull(i))
                                    {
                                        resultDict.Add(rdr.GetName(i), null);
                                    }
                                    else
                                    {
                                        if (rdr[i].GetType() == typeof(Boolean))
                                        {
                                            resultDict.Add(rdr.GetName(i), ((Boolean)rdr[i] == false) ? 0 : 1);
                                        }
                                        else
                                        {
                                            resultDict.Add(rdr.GetName(i), rdr[i]);
                                        }
                                    }
                                }

                                resultList.Add(resultDict);
                            }
                            cnn.Close();
                        }

                    }


                    return Ok(resultList);

                }
                catch (Exception ex)
                {

                    LogClass.ReportError("SyncController", "Get entityKind, from, to , ticket", ex, Configuration);
                    var result = Json("{error:" + ex.Message + "}");
                    result.StatusCode = 500;
                    return result;
                }
                finally {
                    var end = DateTime.Now;
                    TimeSpan span = end - now;
                    Console.WriteLine((int)span.TotalMilliseconds);
                }
            }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        // GET api/Sync/GetLayerNumberOfRecordsToDownload
        // Returns the number of rows to download for the layer.
        [Route("api/Sync/GetLayerNumberOfRecordsToDownload")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetLayerNumberOfRecordsToDownload")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetLayerNumberOfRecordsToDownload(string LayerName, string deviceID)
        {
            long numRecs = 0;
            return Ok(numRecs);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        // GET api/Sync/GetTileNumberOfRecordsToDownload
        // Returns the number of rows to download for the Tile layer.
        [Route("api/Sync/GetTileNumberOfRecordsToDownload")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTileNumberOfRecordsToDownload")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetTileNumberOfRecordsToDownload(string LayerName, string deviceID, long assignmentID)
        {
            long numRecs = 0;
            return Ok(numRecs);
        }

        /// <summary>
        /// Not implemented. Returns rows details for a tile.
        /// </summary>
        // GET api/Sync/GetTile
        [Route("api/Sync/GetTile")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTile")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetTile(string LayerName, long from, long to, string deviceID, long assignmentID)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
            return Ok(resultList);
        }

        /// <summary>
        /// Not implemented. Returns the user list.
        /// </summary>
        // GET api/Sync/GetUserList
        [Route("api/Sync/GetUserList")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetUserList")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetUserList()
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
            return Ok(resultList);
        }

        /// <summary>
        /// Not implemented. Returns rows details for a layer.
        /// </summary>
        // GET api/Sync/GetLayer
        [Route("api/Sync/GetLayer")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetLayer")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetLayer(string LayerName, long from, long to, string deviceID)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
            return Ok(resultList);
        }

        // GET api/Sync/GetObliquesToDownload
        // Not implemented. Returns the obliques to download for one assigment
        [Route("api/Sync/GetObliquesToDownload")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetObliquesToDownload")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetObliquesToDownload(string deviceID, long assignmentID)
        {
            List<object> headerList = new List<object>();
            return Ok(headerList);
        }

        /// <summary>
        /// Not implemented. Returns the count of the parcels to download
        /// </summary>
        // GET api/Sync/GetParcelCountToDownload
        [Route("api/Sync/GetParcelCountToDownload")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetParcelCountToDownload")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetParcelCountToDownload(string deviceID, long downloadID)
        {
            int numRecs = 0;
            return Ok(numRecs);
        }

        // GET api/Sync/GetParcelandLayerCountToDownload
        /// <summary>
        /// Not implemented. Returns the count of the parcels and layers to download
        /// </summary>
        [Route("api/Sync/GetParcelandLayerCountToDownload")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetParcelandLayerCountToDownload")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult GetParcelandLayerCountToDownload(string deviceID, long downloadID)
        {
            Dictionary<string, int> resultDict = new Dictionary<string, int>();
            return Ok(resultDict);
        }

        /// <summary>
        /// Uploads data.
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="isDevice">True whether is a device.</param>
        /// <param name="UploadModelList">Upload data</param>
        /// <returns>Ok if successful.</returns>
        // POST api/Sync
        //[Authorize]
        [AllowAnonymous]
        [HttpPost("api/Sync")]
        [SwaggerOperation(OperationId = "PostSync")]
        public IActionResult Post([FromQuery]long ticket, [FromQuery]int isDevice, [FromBody] List<UploadEntityModel> UploadModelList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                foreach (UploadEntityModel currentModel in UploadModelList)
                {
                    myConnectorObj.AddUploadData(currentModel.kind, currentModel.jsonEntities, ticket);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "Post Upload data", ex, Configuration);
                var result = Json("{error:" + ex.Message + "}");
                result.StatusCode = 500;
                return result;
            }
        }

        // POST api/Sync
        [Route("api/Sync/ReturnDBGuids")]
        [HttpPost]
        [SwaggerOperation(OperationId = "ReturnDBGuids")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult PostReturnDBGuids([FromBody] List<GetGuidObject> UploadModelList)
        {
            List<GetGuidObject> resultList = new List<GetGuidObject>();
            return Ok(resultList);
        }

        /// <summary>
        /// Register a device in the system.
        /// </summary>
        /// <param name="deviceMdl">Device data.</param>
        /// <returns>Ok if successful.</returns>
        // POST api/Sync/RegisterDevice
        [Route("api/Sync/RegisterDevice")]
        [SwaggerOperation(OperationId = "RegisterDevice")]
        [HttpPost]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult PostRegisterDevice([FromBody] RegisterDeviceModel deviceMdl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var userName = JWTDecoder.GetEmailFromHeader(this.HttpContext);

                long assignmentHeaderID = 0;
                string connStr = Environment.GetEnvironmentVariable("connectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    cnn.AccessToken = sqlToken;
                    using (SqlCommand cmd = new SqlCommand("RegisterDevice_mb", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@deviceID", deviceMdl.identifierForVendor));
                        cmd.Parameters.Add(new SqlParameter("@deviceName", deviceMdl.name));
                        cmd.Parameters.Add(new SqlParameter("@userName", userName));
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();

                    }
                    string sqlScript = @"declare @result as bigint
                                    if (Select count(1) from Root_Assignment_Header Where userName_mb = @usernName) = 0
                                    begin 
                                        INSERT INTO Root_Assignment_Header(assignmentDate, sourceDetail, userName_mb, currentProgressStatus, [type], reasonId, WGS84Coverage)
                                        Values(getutcdate(), 'file', @usernName, 1, 1, 1, null)
                                        SELECT @result = SCOPE_IDENTITY()
                                    end else begin
                                        Select @result = assignmentId from Root_Assignment_Header Where userName_mb = @usernName
                                    end
                                    Select @result";

                    using (SqlCommand cmd = new SqlCommand(sqlScript, cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;
                        SqlParameter myParam = new SqlParameter("@usernName", SqlDbType.NVarChar);
                        myParam.Value = userName;
                        cmd.Parameters.Add(myParam);

                        cnn.Open();
                        var resultHeaderID = cmd.ExecuteScalar();
                        assignmentHeaderID = (long)resultHeaderID;
                        cnn.Close();
                    }
                }

                return Ok(assignmentHeaderID);
                
            }

            catch (Exception ex)
            {
                LogClass.ReportError("SyncController", "PostRegisterDevice", ex, Configuration);
                var result = Json("{error:" + ex.Message + "}");
                result.StatusCode = 500;

                return result;
            }
        }


        /////////// <summary>
        /////////// Reads the Root Entities from Backend and let them ready for device download.
        /////////// </summary>
        /////////// <param name="deviceID">Device id</param>
        /////////// <param name="assignmentID">Assignment id</param>
        /////////// <param name="loadRootEntityMdl">Root entity model</param>
        /////////// <returns>Ok if successfull</returns>
        ////////// POST api/Sync/LoadRootEntities
        ////////[Route("api/Sync/LoadRootEntities")]
        ////////[HttpPost]
        ////////[SwaggerOperation(OperationId = "LoadRootEntities")]
        //////////[Authorize]
        ////////[AllowAnonymous]
        ////////public IActionResult PostLoadRootEntities(/*[FromUri]*/ string deviceID, string area, long assignmentID, [FromBody] LoadRootEntityModel loadRootEntityMdl)
        ////////{
        ////////    return ExecuteLoadRootEntities(deviceID, area, assignmentID, null, loadRootEntityMdl);
        ////////}

        /// <summary>
        /// Reads the Root Entities from Backend and let them ready for device download.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <param name="assignmentID">Assigment id</param>
        /// <param name="lastSyncDate">Last sync date</param>
        /// <param name="loadRootEntityMdl">Root entity model</param>
        /// <returns>Ok if successful</returns>
        // POST api/Sync/LoadRootEntities
        [Route("api/Sync/LoadRootEntities")]
        [HttpPost]
        [SwaggerOperation(OperationId = "SyncLoadRootEntities")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult PostLoadRootEntities(/*[FromUri]*/ string deviceID, string area, long assignmentID, [FromBody] LoadRootEntityModel loadRootEntityMdl, double? lastSyncDate = null)
        {
            return ExecuteLoadRootEntities(deviceID, area, assignmentID, lastSyncDate, loadRootEntityMdl);
        }


        private IActionResult  ExecuteLoadRootEntities(string deviceID, string area, long assignmentID, double? lastSyncDate, LoadRootEntityModel loadRootEntityMdl)
        {
            List<GetNumberOfRecordsToDownloadModel> resultList = new List<GetNumberOfRecordsToDownloadModel>();

            var routedController = new RoutedController(Configuration);
            var chnSetID = routedController.PostLoadRootEntitiesInternal(loadRootEntityMdl, assignmentID);

            myConnectorObj.ProcessDataForTicket(chnSetID, false, true);
            var userName = JWTDecoder.GetEmailFromHeader(this.HttpContext);

            string connStr = Environment.GetEnvironmentVariable("connectionString");
            string sqlToken = SQLTokenUtility.GetSQLToken(connStr);
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                cnn.AccessToken = sqlToken;
                //Prepare the download package for the device returning the downloadID
                using (SqlCommand cmd = new SqlCommand("GetNumberOfRecordsToDownloadWithChangeset", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cmd.Parameters.Add(new SqlParameter("@changesetID", chnSetID));
                    // cmd.Parameters.Add(new SqlParameter("@userName", HttpContext.Current.User.Identity.Name));
                    cmd.Parameters.Add(new SqlParameter("@userName", userName));
                    cmd.Parameters.Add(new SqlParameter("@area", area));
                    cmd.Parameters.Add(new SqlParameter("@assignmentId", assignmentID));
                    cnn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GetNumberOfRecordsToDownloadModel resultObj = new GetNumberOfRecordsToDownloadModel();

                        resultObj.NumRecs = (int?)rdr["NumRecs"];
                        resultObj.SourceTable = (string)rdr["SourceTable"];
                        resultObj.downloadId = (long?)rdr["downloadId"];
                        resultObj.syncDate = (double?)rdr["syncDate"];
                        resultList.Add(resultObj);
                    }
                    cnn.Close();
                }
            }

         
            return Ok(resultList);
        }

        

        /// <summary>
        /// Gets the connector status.
        /// </summary>
        /// <returns>Ok if successful</returns>
        [Route("api/Sync/ConnectorStatus")]
        [HttpGet]
        [SwaggerOperation(OperationId = "ConnectorStatus")]
        public async Task<IActionResult> GetConnectorStatus()
        {
            var url = Configuration.connectorServiceUrl;
            string status = "Invalid";
            if (!string.IsNullOrEmpty(url))
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("content-type", "application/json");
                var response = await client.GetAsync("");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var result = await response.Content.ReadAsStreamAsync())
                    {
                        StreamReader reader = new StreamReader(result);
                        string jsonString = reader.ReadToEnd();
                        status = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(jsonString);
                    }
                }
            }

            return Ok(status);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        private string getLayerTableName(string LayerName)
        {
            string tableName = string.Empty;
            return tableName;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        private Dictionary<string, object> geRootLayerInfo()
        {
            Dictionary<string, object> resultDict = new Dictionary<string, object>();
            return resultDict;
        }


        [Route("api/Sync/AssignRootEntityToUser")]
        [SwaggerOperation(OperationId = "AssignRootEntityToUser")]
        [HttpPost]
        [EnableCors("SiteCorsPolicy")]
        public IActionResult AssignRootEntityToUser([FromBody] AssignToUserModel assignToUser)
        {
            myConnectorObj.AssignRootEntitiesToUser(assignToUser.userName, assignToUser.entityIdentifierList);
            return Ok();
        }
  
        [Route("api/Sync/GetUploadTicketForSpecificDevice")]
        [SwaggerOperation(OperationId = "GetUploadTicketForSpecificDevice")]
        [HttpPost]
        public IActionResult GetUploadTicketForSpecificDevice([FromBody] long assignmentID)
        {
            long changesetId = myConnectorObj.GetUploadTicketForSpecificDevice(assignmentID);
            return Ok(changesetId);
        }


        
    }
}
