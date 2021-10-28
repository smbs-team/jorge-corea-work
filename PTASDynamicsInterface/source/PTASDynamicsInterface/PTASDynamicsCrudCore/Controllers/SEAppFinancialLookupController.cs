// <copyright file="SEAppFinancialLookupController.cs" company="King County">
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
    /// Controller to lookup a Senior Exemption Application Financial by Senior Exemtion Application Detail Id.
    /// </summary>
    [Route("v1/api/seappfinanciallookup")]
    [ApiController]
    public class SEAppFinancialLookupController : ControllerBase
    {
        private readonly ISEAppFinancialManager sEAppFinancialManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppFinancialLookupController"/> class.
        /// </summary>
        /// <param name="sEAppFinancialManager">To get the SEAppFinancial info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public SEAppFinancialLookupController(ISEAppFinancialManager sEAppFinancialManager, IMapper mapper)
        {
            this.sEAppFinancialManager = sEAppFinancialManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a SEAppFinancial from the senior exemption application detail id.
        /// </summary>
        /// <param name="id">senior exemption application detail id.</param>
        /// <returns>a list of SEAppFinancial rows that contains de sEAppDetailId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationFinancialByAppId")]
        public async Task<ActionResult<FormSeniorExemptionApplicationFinancial[]>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormSeniorExemptionApplicationFinancial[]>((await this.sEAppFinancialManager.GetSEAppFinancialFromSEAppDetailId(id)).ToArray());
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