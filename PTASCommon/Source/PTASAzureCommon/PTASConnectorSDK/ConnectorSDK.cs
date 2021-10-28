using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTASConnectorSDK.DataAccess;
using PTASConnectorSDK.DataModel;
using System.Diagnostics;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PTASConnectorSDK
{
    public enum SQLServerType
    {
        MSSQL,
        PostgreSQL
    }
    public class ConnectorSDK
    {
        private string usrName = "";
        private string pass = "";
        private bool isDBDirect = false;
        private string dbCnnString = "";
        private string servURL = "";
        private IDBMethods databaseAccesMethods = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorSDK" /> class.
        /// </summary>
        /// <param name="dbConnectionString">The db connection string.</param>
        public ConnectorSDK(string dbConnectionString, SQLServerType type, ClientCredential principalCredentials = null)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(dbConnectionString), "The connection string for the database should not be null or empty");
            isDBDirect = true;
            dbCnnString = dbConnectionString;
            if (type == SQLServerType.MSSQL)
            {
                databaseAccesMethods = new DBMethods(dbConnectionString, principalCredentials);
            }
            else if (type == SQLServerType.PostgreSQL)
            { 
                databaseAccesMethods = new PGDBMethods(dbConnectionString);
            }
        }

        

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorSDK" /> class.
        /// </summary>
        /// <param name="serviceURL">The service URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public ConnectorSDK(string serviceURL, string userName, string password)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(serviceURL), "The service URL should not be null or empty");
            Debug.Assert(!string.IsNullOrWhiteSpace(userName), "The userName should not be null or empty");
            Debug.Assert(!string.IsNullOrWhiteSpace(password), "The password should not be null or empty");
            isDBDirect = false;
            servURL = serviceURL;
            usrName = userName;
            pass = password;
        }

        /// <summary>
        /// Gets the guids for objects.
        /// </summary>
        /// <param name="objectList">The object list.</param>
        /// <returns></returns>
        public List<GetGuidObject> GetGuidsforObjects(List<GetGuidObject> objectList)
        {
            Debug.Assert(objectList != null, "The object list should not be null");
            if (isDBDirect)
            {
                return databaseAccesMethods.DBGetGuidsforObjects(objectList);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the upload ticket for device.
        /// </summary>
        /// <param name="deviceID">The device ID.</param>
        /// <param name="assignmentID">The assignment ID.</param>
        /// <returns></returns>
        public long GetUploadTicketForDevice(string deviceID, long? assignmentID)
        {
            Debug.Assert(!string.IsNullOrEmpty(deviceID), "The device id should not be null or empty");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetUploadTicket(deviceID, assignmentID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the upload ticket for backend.
        /// </summary>
        /// <returns></returns>
        public long GetUploadTicketForBackend()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetUploadTicket(string.Empty, null);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the upload ticket for specific device.
        /// </summary>
        /// <returns></returns>
        public long GetUploadTicketForSpecificDevice()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetUploadTicketForSpecificDevice();
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the upload ticket for specific device.
        /// </summary>
        /// <param name="assignmentID">The assignment ID.</param>
        /// <returns>ChangeSet</returns>
        public long GetUploadTicketForSpecificDevice(long assignmentID)
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetUploadTicketForSpecificDevice(assignmentID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Adds the upload data.
        /// </summary>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="dataToUpload">The data to upload.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        public void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket)
        {
            Debug.Assert(!string.IsNullOrEmpty(entityKind), "The entity kind should not be null or empty");
            Debug.Assert(dataToUpload != null, "The data to upload object should not be null");
            Debug.Assert(uploadTicket > 0, "The upload ticket should be greater than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.AddUploadData(entityKind, dataToUpload, uploadTicket);
            }
            else
            {
                return;
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
            Debug.Assert(changeSetID > 0, "The changeset id should be greater than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.ProcessDataForTicket(changeSetID, fromDevice, lastExecution);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Set Changeset state to completed.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        public void ChangesetCompleted(long changeSetID)
        {
            Debug.Assert(changeSetID > 0, "The changeset id should be greater than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.ChangesetCompleted(changeSetID);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Set Changeset state to downloaded.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        public void ChangesetDownloaded(long changeSetID)
        {
            Debug.Assert(changeSetID > 0, "The changeset id should be greater than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.ChangesetDownloaded(changeSetID);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Adds the root entity in to assigment.
        /// </summary>
        /// <param name="assignmentId">The root assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        public void AddRootInToAssigment(long assignmentId, string rootId)
        {
            Debug.Assert(assignmentId > 0, "The assignment id should be greater than 0");
            Debug.Assert(!string.IsNullOrEmpty(rootId), "The root Id should not be null or empty");
            if (isDBDirect)
            {
                databaseAccesMethods.AddRootInToAssigment(assignmentId, rootId);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gets the last import date.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastImportDate()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetLastImportDate();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Verify if the import process is running
        /// </summary>
        /// <returns>true if is running, otherwise returns false</returns>
        public bool IsRunning()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.IsRunning();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the last import date for the entity.
        /// </summary>
        /// <param name="entityName">the entity name</param>
        /// <returns></returns>
        public DateTime? GetLastImportEntityDate(string entityName)
        {
            Debug.Assert(!string.IsNullOrEmpty(entityName), "The entity name should not be null");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetLastImportEntityDate(entityName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the last export date.
        /// </summary>
        /// <returns></returns>
        public float GetLastExportDate()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetLastExportDate();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets the import date.
        /// </summary>
        /// <param name="importDate">The import date.</param>
        public void SetImportDate(DateTime importDate)
        {
            Debug.Assert(importDate != null, "The import date should not be null");
            if (isDBDirect)
            {
                databaseAccesMethods.SetImportDate(importDate);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Set the running status
        /// </summary>
        /// <param name="importDate">The update date</param>
        /// <param name="isRunning">The status</param>
        public void SetRunningStatus(DateTime importDate, bool isRunning)
        {
            Debug.Assert(importDate != null, "The import date should not be null");
            if (isDBDirect)
            {
                databaseAccesMethods.SetRunningStatus(importDate, isRunning);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Sets the import date for the entity
        /// </summary>
        /// <param name="entityName">the entity name</param>
        /// <param name="lastDate">the import date</param>
        public void SetImportEntityDate(string entityName, DateTime lastDate)
        {
            Debug.Assert(lastDate != null, "The import date should not be null");
            Debug.Assert(!string.IsNullOrEmpty(entityName), "The entity name should not be null");
            if (isDBDirect)
            {
                databaseAccesMethods.SetImportEntityDate(entityName, lastDate);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Sets the export date.
        /// </summary>
        /// <param name="exportDate">The export date.</param>
        public void SetExportDate(float exportDate)
        {
            Debug.Assert(exportDate > 0, "The export date should be greater than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.SetExportDate(exportDate);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gets the modified entity data.
        /// </summary>
        /// <param name="rootId">The root id.</param>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetModifiedEntityData(string rootId, string entityKind, Guid deviceGuid)
        {
            Debug.Assert(!string.IsNullOrEmpty(entityKind), "The entity kind should not be null or empty");
            Debug.Assert(deviceGuid != null, "The device guid should not be null");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetModifiedEntityData(rootId, entityKind, deviceGuid);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the changesets as exported.
        /// </summary>
        /// <param name="changesetList">The changeset list.</param>
        public void SetChangesetsExported(List<string> changesetList)
        {
            Debug.Assert(changesetList != null, "The changeset list should not be null");
            Debug.Assert(changesetList.Count > 0, "The changeset list should have at least 1 element");
            if (isDBDirect)
            {
                databaseAccesMethods.SetChangesetsExported(changesetList);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gets the device GUID list.
        /// </summary>
        /// <returns></returns>
        public List<Guid> GetDeviceGuidList()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetDeviceGuidList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a dictionary with the value pair of device guid and username of the device.
        /// </summary>
        /// <returns>Dictionay that contains the device guid and username of the device</returns>
        public Dictionary<Guid, string> GetDeviceInformation()
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetDeviceInformation();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns></returns>
        public string GetUser(Guid deviceGuid)
        {
            Debug.Assert(deviceGuid != null, "The device guid should not be null");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetUser(deviceGuid);
            }
            else
            {
                return null;
            }
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
            Debug.Assert(date != null, "The date should not be null");
            Debug.Assert(!string.IsNullOrEmpty(message), "The message should not be null or empty");
            Debug.Assert(!string.IsNullOrEmpty(groupIdentifier), "The group identifier should not be null or empty");
            if (isDBDirect)
            {
                return databaseAccesMethods.SetLogHeader(fromBackend, date, message, changesetID, groupIdentifier);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets the log detail.
        /// </summary>
        /// <param name="logHeaderId">The log header id.</param>
        /// <param name="message">The message.</param>
        /// <param name="result">The result.</param>
        public void SetLogDetail(long logHeaderId, string message, string result, logMessageType messageType)
        {
            Debug.Assert(logHeaderId > 0, "The log header id should be greater than 0");
            Debug.Assert(!string.IsNullOrEmpty(message), "The message should not be null or empty");
            Debug.Assert(!string.IsNullOrEmpty(result), "The result should not be null or empty");
            if (isDBDirect)
            {
                databaseAccesMethods.SetLogDetail(logHeaderId, message, result, messageType);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Cleans the old log information.
        /// </summary>
        /// <param name="daysNumber">The days number.</param>
        public void CleanOldLogInformation(int daysNumber)
        {
            Debug.Assert(daysNumber >= 0, "The days number should be greater or equal than 0");
            if (isDBDirect)
            {
                databaseAccesMethods.CleanOldLogInformation(daysNumber);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Assigns the root entities to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        public void AssignRootEntitiesToUser(string userName, List<string> entityIdentifierList)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The userName should not be null or empty");
            Debug.Assert(entityIdentifierList != null, "The list of entity identifiers should not be null");
            if (isDBDirect)
            {
                databaseAccesMethods.AssignRootEntitiesToUser(userName, entityIdentifierList);
            }
            else
            {
                return;
            }
        }

        

        public long? GetAssignmentID(string userName)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The userName should not be null or empty");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetAssignmentID(userName);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the root entities assigned to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public List<string> GetRootEntitiesAssignedToUser(string userName)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The userName should not be null or empty");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetRootEntitiesAssignedToUser(userName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deletes the root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        public void DeleteRootEntitiesFromUser(string userName, List<string> entityIdentifierList)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The userName should not be null or empty");
            Debug.Assert(entityIdentifierList != null, "The list of entity identifiers should not be null");
            if (isDBDirect)
            {
                databaseAccesMethods.DeleteRootEntitiesFromUser(userName, entityIdentifierList);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Deletes all root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void DeleteAllRootEntitiesFromUser(string userName)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The userName should not be null or empty");
            if (isDBDirect)
            {
                databaseAccesMethods.DeleteAllRootEntitiesFromUser(userName);
            }
            else
            {
                return;
            }
        }

 
  
        public List<Dictionary<string, object>> GetModificationReasonForEntities(List<string> entitiesGuidList, Guid deviceGuid)
        {
            Debug.Assert(entitiesGuidList != null, "The entities guid list should not be null");
            Debug.Assert(entitiesGuidList.Count() > 0, "The entities guid list should not be empty");
            Debug.Assert(deviceGuid != null, "The device guid should not be null");
            if (isDBDirect)
            {
                return databaseAccesMethods.GetModificationReasonForEntities(entitiesGuidList, deviceGuid);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, object> GetFlightPlanInformation(string flightId)
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetFlightPlanInformation(flightId);
            }
            else
            {
                return null;
            }
        }

        public List<Dictionary<string, object>> GetFlightPlanLinesInformation(string flightId)
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetFlightPlanLinesInformation(flightId);
            }
            else
            {
                return null;
            }
        }

        public List<Dictionary<string, object>> GetFlightPlanFilesInformation(string flightId)
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetFlightPlanFilesInformation(flightId);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, object> GetFlightPlanFileInformation(Guid fileUuid)
        {
            if (isDBDirect)
            {
                return databaseAccesMethods.GetFlightPlanFileInformation(fileUuid);
            }
            else
            {
                return null;
            }
        }
    }
}