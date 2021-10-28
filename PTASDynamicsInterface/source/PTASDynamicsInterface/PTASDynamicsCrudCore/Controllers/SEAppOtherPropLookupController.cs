// <copyright file="SEAppOtherPropLookupController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to lookup a SEAppOtherProp by Senior Exemption Application Id.
    /// </summary>
    [Route("v1/api/seappotherproplookup")]
    [ApiController]
    public class SEAppOtherPropLookupController : ControllerBase
    {
        private readonly ISEAppOtherPropManager sEAppOtherPropManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppOtherPropLookupController"/> class.
        /// </summary>
        /// <param name="sEAppOtherPropManager">To get the SEAppOtherProp info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public SEAppOtherPropLookupController(ISEAppOtherPropManager sEAppOtherPropManager, IMapper mapper)
        {
            this.sEAppOtherPropManager = sEAppOtherPropManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a SEAppOtherProp from the senior exemption application id.
        /// </summary>
        /// <param name="id">senior exemption application id.</param>
        /// <returns>a list of SEAppOtherPropt rows that contains de sEAppId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSEAppOtherPropByAppId")]
        public async Task<ActionResult<FormSEAppOtherProp[]>> Get(string id)
        {
            return await ControllerExtensions<FormSEAppOtherProp[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormSEAppOtherProp[]>((await this.sEAppOtherPropManager.GetSEAppOtherPropFromSEAppId(id)).ToArray());
                JsonResult jResult = new JsonResult(result);
                if (result.Length == 0)
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