// <copyright file="StatusCodesController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to get the combination value of StatusCode and StateCode in diferente dynamics entities.
    /// </summary>
    [Route("v1/api/statuscodes")]
    [ApiController]
    public class StatusCodesController : ControllerBase
    {
        private const string TableNameForState = "statusmaps";
        private const string TableNameForStatus = "stringmaps";
        private readonly CRMWrapper wrapper;

        /// <summary>
        /// mapping of friendly names to PTAS dynamics specific names.
        /// </summary>
        private readonly Dictionary<string, string> tableMappings = new Dictionary<string, string>
    {
      { "seniorapp", "ptas_seapplication" },
      { "seniordetail", "ptas_seappdetail" },
    };

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodesController"/> class.
        /// </summary>
        /// <param name="wrapper">Wrapper to connect to CRM.</param>
        public StatusCodesController(CRMWrapper wrapper) => this.wrapper = wrapper;

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="id">Id that identifies the entity where the status/state code is required to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetTheStatusCodeSetList")]
        public async Task<ActionResult<IEnumerable<StatusCodeSet>>> Get(string id)
        {
            return await ControllerExtensions<IEnumerable<StatusCodeSet>>.HandleExceptions(async () =>
            {
                if (this.tableMappings.TryGetValue(id.ToLower(), out string objecttypecode))
                {
                    IEnumerable<OptionSet> statusValues = (await this.wrapper.ExecuteGet<OptionSet>(
                        TableNameForStatus,
                        $"$filter=objecttypecode eq '{objecttypecode}' and attributename eq 'statuscode'")).OrderBy(itm => itm.AttributeValue);
                    IEnumerable<StatusCodeSet> stateValues = (await this.wrapper.ExecuteGet<StatusCodeSet>(
                        TableNameForState,
                        $"$filter=objecttypecode eq '{objecttypecode}'")).OrderBy(itm => itm.Status);

                    var result = (from x in statusValues
                                  join y in stateValues
                                  on x.AttributeValue equals y.Status
                                  select new StatusCodeSet { Status = x.AttributeValue.GetValueOrDefault(), Value = x.Value, State = y.State }).AsEnumerable();
                    JsonResult jResult = new JsonResult(result);
                    if (result.Count() == 0)
                    {
                        jResult.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                    else
                    {
                        jResult.StatusCode = (int)HttpStatusCode.OK;
                    }

                    return jResult;
                }

                throw new DynamicsInterfaceException(
                $"Request returns not data, entity with value {id} is not allowed.",
                (int)HttpStatusCode.NotFound,
                null);
            });
        }
    }
}