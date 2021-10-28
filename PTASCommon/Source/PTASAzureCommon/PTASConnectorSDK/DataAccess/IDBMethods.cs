using PTASConnectorSDK.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTASConnectorSDK.DataAccess
{
    public interface IDBMethods
    {
        /// <summary>
        /// Adds the upload data.
        /// </summary>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="dataToUpload">The data to upload.</param>
        /// <param name="uploadTicket">The upload ticket.</param>
        void AddUploadData(string entityKind, List<Dictionary<string, object>> dataToUpload, long uploadTicket);

        /// <summary>
        /// Gets the upload ticket.
        /// </summary>
        /// <param name="deviceID">The device ID.</param>
        /// <param name="assignmentID">The assignment ID.</param>
        /// <returns></returns>
        long GetUploadTicket(string deviceID, long? assignmentID);

        /// <summary>
        /// Gets the upload ticket for specific device.
        /// </summary>
        /// <returns></returns>
        long GetUploadTicketForSpecificDevice();

        long GetUploadTicketForSpecificDevice(long assignmentID);

        /// <summary>
        /// DBs the get guidsfor objects.
        /// </summary>
        /// <param name="objectList">The object list.</param>
        /// <returns></returns>
        List<GetGuidObject> DBGetGuidsforObjects(List<GetGuidObject> objectList);

        /// <summary>
        /// Processes the data for ticket.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        /// <param name="fromDevice">if set to <c>true</c> [from device].</param>
        /// <param name="lastExecution">if set to <c>true</c> [last execution].</param>
        void ProcessDataForTicket(long changeSetID, bool fromDevice, bool lastExecution);

        /// <summary>
        /// Set Changeset state to completed.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        void ChangesetCompleted(long changeSetID);

        /// <summary>
        /// Set Changeset state to downloaded.
        /// </summary>
        /// <param name="changeSetID">The change set ID.</param>
        void ChangesetDownloaded(long changeSetID);

        /// <summary>
        /// Adds the root entity in to assigment.
        /// </summary>
        /// <param name="assignmentId">The root assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        void AddRootInToAssigment(long assignmentId, string rootId);

        /// <summary>
        /// Gets the last import date.
        /// </summary>
        /// <returns></returns>
        DateTime? GetLastImportDate();

        /// <summary>
        /// Verify if the import process is running
        /// </summary>
        /// <returns>true if is running, otherwise returns false</returns>
        bool IsRunning();

        /// <summary>
        /// Get the last import date for the entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        DateTime? GetLastImportEntityDate(string entityName);

        /// <summary>
        /// Gets the last export date.
        /// </summary>
        /// <returns></returns>
        float GetLastExportDate();

        /// <summary>
        /// Sets the import date.
        /// </summary>
        /// <param name="importDate">The import date.</param>
        void SetImportDate(DateTime importDate);

        /// <summary>
        /// Set the running status
        /// </summary>
        /// <param name="importDate">The update date</param>
        /// <param name="isRunning">The status</param>
        void SetRunningStatus(DateTime importDate, bool isRunning);

        /// <summary>
        /// Set the import date for the entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="lastDate">The import date</param>
        void SetImportEntityDate(string entityName, DateTime lastDate);

        /// <summary>
        /// Sets the export date.
        /// </summary>
        /// <param name="exportDate">The export date.</param>
        void SetExportDate(float exportDate);

        /// <summary>
        /// Gets the modified entity data.
        /// </summary>
        /// <param name="rootId">The root Id.</param>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns></returns>
        List<Dictionary<string, object>> GetModifiedEntityData(string rootId, string entityKind, Guid deviceGuid);

        /// <summary>
        /// Sets the changesets as exported.
        /// </summary>
        /// <param name="changesetList">The changeset list.</param>
        void SetChangesetsExported(List<string> changesetList);

        /// <summary>
        /// Gets the device GUID list.
        /// </summary>
        /// <returns>The device guid list</returns>
        List<Guid> GetDeviceGuidList();

        /// <summary>
        /// Gets a dictionary with the value pair of device guid and username of the device.
        /// </summary>
        /// <returns>Dictionay that contains the device guid and username of the device</returns>
        Dictionary<Guid, string> GetDeviceInformation();


        /// <summary>
        /// Gets the user of the device.
        /// </summary>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <returns>The user of the device</returns>
        string GetUser(Guid deviceGuid);

        /// <summary>
        /// Sets the log header.
        /// </summary>
        /// <param name="fromBackend">if set to <c>true</c> [from backend].</param>
        /// <param name="date">The date.</param>
        /// <param name="message">The message.</param>
        /// <param name="changesetID">The changeset ID.</param>
        /// <param name="groupIdentifier">The group identifier.</param>
        /// <returns></returns>
        long SetLogHeader(bool fromBackend, DateTime date, string message, long? changesetID, string groupIdentifier);

        /// <summary>
        /// Sets the log detail.
        /// </summary>
        /// <param name="logHeaderId">The log header id.</param>
        /// <param name="message">The message.</param>
        /// <param name="result">The result.</param>
        /// <param name="messageType">The type of the message (Information = 1, Warning = 2, Error = 3).</param>
        void SetLogDetail(long logHeaderId, string message, string result, logMessageType messageType);


        /// <summary>
        /// Cleans the old log information.
        /// </summary>
        /// <param name="daysNumber">The days number.</param>
        void CleanOldLogInformation(int daysNumber);

        /// <summary>
        /// Assigns the root entities to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        void AssignRootEntitiesToUser(string userName, List<string> entityIdentifierList);

        long? GetAssignmentID(string userName);

        /// <summary>
        /// Adds the entity to assignment.
        /// </summary>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="rootId">The root Id.</param>
        void AddEntityToAssignment(long assignmentId, string rootId);

        /// <summary>
        /// Gets the root entities assigned to user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        List<string> GetRootEntitiesAssignedToUser(string userName);

        /// <summary>
        /// Deletes root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityIdentifierList">The entity identifier list.</param>
        void DeleteRootEntitiesFromUser(string userName, List<string> entityIdentifierList);


        /// <summary>
        /// Deletes all root entities from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        void DeleteAllRootEntitiesFromUser(string userName);


        /// <summary>
        /// Adds to syn command table.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="myJSon">My Json.</param>
        void AddToSynCommand(int commandType, string myJSon, long assignmentID);


        /// <summary>
        /// Creates the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="changesetId">The changeset id.</param>
        void CreateLayer(Layer layer, long changesetId);

        /// <summary>
        /// Creates the map.
        /// </summary>
        /// <param name="changesetId">The changeset id.</param>
        /// <returns></returns>
        Guid CreateMap(long changesetId);

        /// <summary>
        /// Creates the layer source.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <param name="changesetId">The changeset id.</param>
        void CreateLayerSource(LayerSource layerSource, long changesetId);

        /// <summary>
        /// Updates the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="changesetId">The changeset id.</param>
        void UpdateLayer(Layer layer, long changesetId);

        /// <summary>
        /// Updates the layer source.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <param name="changesetId">The changeset id.</param>
        void UpdateLayerSource(LayerSource layerSource, long changesetId);

        /// <summary>
        /// Deletes the map configuration entities.
        /// </summary>
        /// <param name="entitiesList">The entities list.</param>
        /// <param name="entityKind">Kind of the entity.</param>
        /// <param name="changesetId">The changeset id.</param>
        void DeleteMapConfigurationEntities(string entitiesList, string entityKind, long changesetId);

        /// <summary>
        /// Layerses to delete.
        /// </summary>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        string LayersToDelete(List<Layer> layers);
        
        List<Dictionary<string, object>> GetModificationReasonForEntities(List<string> entitiesGuidList, Guid deviceGuid);

        Dictionary<string, object> GetFlightPlanInformation(string flightId);

        List<Dictionary<string, object>> GetFlightPlanLinesInformation(string flightId);

        List<Dictionary<string, object>> GetFlightPlanFilesInformation(string flightId);

        Dictionary<string, object> GetFlightPlanFileInformation(Guid fileUuid);
    }
}
