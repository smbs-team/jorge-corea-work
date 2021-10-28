// <copyright file="SEAppDetailLookupController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to lookup a Senior Exemption Application Detail by Senior Exemtion Application Id.
    /// </summary>
    [Route("v1/api/seappdetaillookup")]
    [ApiController]
    public class SEAppDetailLookupController : ControllerBase
    {
        private readonly ISEAppDetailManager sEAppDetailManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppDetailLookupController"/> class.
        /// </summary>
        /// <param name="sEAppDetailManager">To get the SEAppDetailManager info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public SEAppDetailLookupController(ISEAppDetailManager sEAppDetailManager, IMapper mapper)
        {
            this.sEAppDetailManager = sEAppDetailManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a SEAppDetailManager from the senior exemption application id.
        /// </summary>
        /// <param name="id">senior exemption application id.</param>
        /// <returns>a list of SEAppDetailManager rows that contains de sEAppId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationDetailByAppId")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationDetail[]>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationDetail[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormSeniorExemptionApplicationDetail[]>((await this.sEAppDetailManager.GetSEAppDetailFromSEAppId(id)).ToArray());
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