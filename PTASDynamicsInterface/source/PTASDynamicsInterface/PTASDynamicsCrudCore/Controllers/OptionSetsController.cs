// <copyright file="OptionSetsController.cs" company="King County">
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
    /// Controller to handle all option sets.
    /// </summary>
    [Route("v1/api/optionsets")]
    [ApiController]
    public class OptionSetsController : ControllerBase
    {
        /// <summary>
        /// mapping of friendly names to PTAS dynamics specific names.
        /// </summary>
        private readonly Dictionary<string, string> tableMappings = new Dictionary<string, string>
    {
      { "relationships", "ptas_occupanttype" },
      { "accounttypes", "ptas_accounttype" },
      { "exemptionsources", "ptas_source" },
      { "exemptiontypes", "ptas_exemptiontype" },
      { "incomelevels", "ptas_incomelevel" },
      { "mediatypes", "ptas_mediatype" },
      { "purposes", "ptas_purpose" },
      { "splitcodes", "ptas_splitcode" },
      { "disabilityincomesources", "ptas_incomedisabilitysrc" },
      { "financialfilertypes", "ptas_filertype" },
      { "financialformtypes", "ptas_financialformtype" },
      { "portalattachmentlocations", "ptas_portalattachmentlocation" },
    };

        private readonly CRMWrapper wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionSetsController"/> class.
        /// </summary>
        /// <param name="wrapper">Wrapper to connect to CRM.</param>
        public OptionSetsController(CRMWrapper wrapper) => this.wrapper = wrapper;

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="objectId">Object id.</param>
        /// <param name="id">Id of the optionset to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{objectId}/{id}")]
        [SwaggerOperation(OperationId = "GetByObjectAndAttribute")]
        public async Task<ActionResult<IEnumerable<OptionSet>>> GetByObjectAndAttribute(string objectId, string id)
        {
            return await ControllerExtensions<IEnumerable<OptionSet>>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.GetOptionsets(objectId, id);

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
            });
        }

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="id">Id of the optionset to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetOptionSetById")]
        public async Task<ActionResult<IEnumerable<OptionSet>>> Get(string id)
        {
            return await ControllerExtensions<IEnumerable<OptionSet>>.HandleExceptions(async () =>
            {
                string loweredId = id.ToLower();
                string useCode =
                    loweredId.StartsWith("ptas_")
                    ? loweredId
                    : this.tableMappings.TryGetValue(loweredId, out string objecttypecode)
                        ? objecttypecode
                        : throw new DynamicsInterfaceException($"Request returned no data, entity with value {loweredId} unknown.", (int)HttpStatusCode.NotFound, null);

                var result = (await this.wrapper.ExecuteGet<OptionSet>(
                            "stringmaps",
                            $"$filter=attributename eq '{useCode}'"))
                        .OrderBy(itm => itm.Displayorder);

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
            });
        }
    }
}