using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace PTASSyncService.Controllers
{
    [Authorize]
    /// <summary>
    /// Class to handle synchronization between device and SQL Server.
    /// </summary>
    public class ConfigurationController : Controller
    {
        /// <summary>
        /// Updates the xml model and creates the SQL database
        /// </summary>
        /// <param name="request">The request message that contains the xml model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Configuration/PublishPostgreSQLDB")]
        [SwaggerOperation(OperationId = "PublishPostgreSQLDB")]
        public List<string> PostRawPublishPostgreSQLDB(bool forceCreate)
        {
            List<string> messageResult = new List<string>();
            return messageResult;
        }
    }
}
