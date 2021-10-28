// <copyright file="SEAppNoteLookupController.cs" company="King County">
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
    /// Controller to lookup a SEAppNote by Senior Exemption Application Id.
    /// </summary>
    [Route("v1/api/seappnotelookup")]
    [ApiController]
    public class SEAppNoteLookupController : ControllerBase
    {
        private readonly ISEAppNoteManager sEAppNoteManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppNoteLookupController"/> class.
        /// </summary>
        /// <param name="sEAppNoteManager">To get the SEAppNote info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public SEAppNoteLookupController(ISEAppNoteManager sEAppNoteManager, IMapper mapper)
        {
            this.sEAppNoteManager = sEAppNoteManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a SEAppNote from the senior exemption application id.
        /// </summary>
        /// <param name="id">senior exemption application id.</param>
        /// <returns>a list of SEAppNote rows that contains de sEAppId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSEAppNoteByAppId")]
        public async Task<ActionResult<FormSEAppNote[]>> Get(string id)
        {
            return await ControllerExtensions<FormSEAppNote[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormSEAppNote[]>((await this.sEAppNoteManager.GetSEAppNoteFromSEAppId(id)).ToArray());
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