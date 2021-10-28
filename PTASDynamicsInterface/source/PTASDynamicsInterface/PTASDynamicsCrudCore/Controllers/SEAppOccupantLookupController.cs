// <copyright file="SEAppOccupantLookupController.cs" company="King County">
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
    /// Controller to lookup a SEAppOccupant by Senior Exemption Application Id.
    /// </summary>
    [Route("v1/api/seappoccupantlookup")]
    [ApiController]
    public class SEAppOccupantLookupController : ControllerBase
    {
        private readonly ISEAppOccupantManager sEAppOccupantManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppOccupantLookupController"/> class.
        /// </summary>
        /// <param name="sEAppOccupantManager">To get the SEAppOccupant info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public SEAppOccupantLookupController(ISEAppOccupantManager sEAppOccupantManager, IMapper mapper)
        {
            this.sEAppOccupantManager = sEAppOccupantManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a SEAppOccupant from the senior exemption application id.
        /// </summary>
        /// <param name="id">senior exemption application id.</param>
        /// <returns>a list of SEAppOccupant rows that contains de sEAppId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSEAppOccupantByAppId")]
        public async Task<ActionResult<FormSEAppOccupant[]>> Get(string id)
        {
            return await ControllerExtensions<FormSEAppOccupant[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormSEAppOccupant[]>(await this.sEAppOccupantManager.GetSEAppOccupantFromSEAppId(id));
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