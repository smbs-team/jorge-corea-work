using PTASConnectorSDK;
using PTASSyncService.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ConnectorService
{
    /// <summary>
    /// Routed controller class
    /// </summary>
    public class RoutedController : Controller
    {
        private readonly PTASSyncService.Settings Configuration;

        private ConnectorSDK myConnectorObj;

        public RoutedController(PTASSyncService.Settings config)
        {
            this.Configuration = config;
            myConnectorObj = new ConnectorSDK(Environment.GetEnvironmentVariable("connectionString"), SQLServerType.MSSQL);
        }

        /// <summary>
        /// Gets the load root entities.
        /// </summary>
        /// <param name="loadRootEntityMdl">The load root entities list.</param>
        /// <param name="assignmentID">The assignment ID.</param>
        /// <returns></returns>
        [Route("Connectors/LoadRootEntities")]
        [HttpPost]
        [SwaggerOperation(OperationId = "ConnLoadRootEntities")]
        public IActionResult PostLoadRootEntities([FromBody] LoadRootEntityModel loadRootEntityMdl, long assignmentID)
        {
            ConnectorInstances connector = ConnectorInstances.Instances;
            long uploadTicket = connector.LoadRootEntities(loadRootEntityMdl, assignmentID);
            return Ok(uploadTicket);
        }

        public long PostLoadRootEntitiesInternal(LoadRootEntityModel loadRootEntityMdl, long assignmentID)
        {
            ConnectorInstances connector = ConnectorInstances.Instances;
            long uploadTicket = connector.LoadRootEntities(loadRootEntityMdl, assignmentID);
            return uploadTicket;
        }
    }
}