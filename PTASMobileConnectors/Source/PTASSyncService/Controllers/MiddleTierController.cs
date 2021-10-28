using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PTASConnectorSDK;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace PTASSyncService.Controllers
{
    /// <summary>
    /// Class to handle synchronization between device and PostgreSQL.
    /// </summary>
    public class MiddleTierController : Controller
    {
        /// <summary>
        /// App configuration settings.
        /// </summary>
        private readonly Settings Configuration;
        private bool isDBDirect = false;

        private ConnectorSDK connectorSDK;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration settings</param>
        public MiddleTierController(Settings config)
        {
            this.Configuration = config;
            connectorSDK = new ConnectorSDK(Environment.GetEnvironmentVariable("connectionString"), SQLServerType.MSSQL);
        }

        [Route("api/MiddleTier/GetUploadTicketForBackend")]
        [SwaggerOperation(OperationId = "GetUploadTicketForBackend")]
        [HttpGet]
        public IActionResult GetUploadTicketForBackend()
        {
            try
            {
                long uploadTicket = connectorSDK.GetUploadTicketForBackend();
                return Ok(uploadTicket);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("api/MiddleTier/AddUploadData")]
        [SwaggerOperation(OperationId = "AddUploadData")]
        [HttpPost]
        public IActionResult AddUploadData([FromBody] Newtonsoft.Json.Linq.JObject json)
        {
            try
            {
                string entityName = json.GetValue("entityName").ToString();
                long uploadTicket = (long)json.GetValue("uploadTicket");
                var jsonUploadList = (JArray)json.GetValue("uploadList");

                List<Dictionary<string, object>> upLoadList = new List<Dictionary<string, object>>();

                foreach (JToken child in jsonUploadList.Children())
                {
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(child.ToString());
                    upLoadList.Add(values);
                }

                connectorSDK.AddUploadData(entityName, upLoadList, uploadTicket);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("api/MiddleTier/ProcessDataForTicket")]
        [SwaggerOperation(OperationId = "ProcessDataForTicket")]
        [HttpPost]
        public IActionResult ProcessDataForTicket([FromBody] Newtonsoft.Json.Linq.JObject json)
        {
            try
            {
                long uploadTicket = (long)json.GetValue("uploadTicket");

                connectorSDK.ProcessDataForTicket(uploadTicket, false, false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [Route("api/MiddleTier/GetDeviceGuidList")]
        [SwaggerOperation(OperationId = "GetDeviceGuidList")]
        [HttpGet]
        public IActionResult GetDeviceGuidList()
        {
            try
            {
                List<Guid> deviceList = connectorSDK.GetDeviceGuidList();
                var output = JsonConvert.SerializeObject(deviceList);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("api/MiddleTier/GetModifiedEntityData")]
        [SwaggerOperation(OperationId = "GetModifiedEntityData")]
        [HttpPost]
        public IActionResult GetModifiedEntityData([FromBody] Newtonsoft.Json.Linq.JObject json)
        {
            try
            {
                string rootId = json.GetValue("rootId").ToString();
                string entityKind = json.GetValue("entityKind").ToString();
                Guid deviceGuid = Guid.Parse(json.GetValue("deviceGuid").ToString());

                List<Dictionary<string, object>> list = connectorSDK.GetModifiedEntityData(rootId, entityKind, deviceGuid);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
