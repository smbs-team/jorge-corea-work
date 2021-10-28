// <copyright file="SEAOccupantLookupByFinancialFormIdController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to get Senior Exemption Application Financial Form for a OccupantId.
    /// </summary>
    [Route("v1/api/seaoccupantlookupbyfinancialformId")]
    [ApiController]
    public class SEAOccupantLookupByFinancialFormIdController : ControllerBase
    {
        private const string TableName = "ptas_seappoccupants";
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAOccupantLookupByFinancialFormIdController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper to use.</param>
        /// /// <param name="mapper">IMapper mapper to use.</param>
        public SEAOccupantLookupByFinancialFormIdController(CRMWrapper wrapper, IMapper mapper)
        {
            this.Wrapper = wrapper;
            this.mapper = mapper;
        }

        private CRMWrapper Wrapper { get; }

        /// <summary>
        /// Api to get the Senior Exemption Application Financial Form for a given OccupantId.
        /// </summary>
        /// <param name="id">Id ot the  SEAppFinancialForm.</param>
        /// <returns>The occupants  for the given SEAppFinancialForm.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSEAppOccupantByFinancialFormId")]
        public async Task<ActionResult<FormSEAppOccupant[]>> Get(string id)
        {
            return await ControllerExtensions<FormSEAppOccupant[]>.HandleExceptions(async () =>
            {
                var result = await this.GetOccupants(id);
                JsonResult jResult = new JsonResult(result);
                if (result == null || result.Length == 0)
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
        /// Returns a list of occupants for the financial form.
        /// </summary>
        /// <param name="id">Financial form Id.</param>
        /// <returns>Retrieved objects.</returns>
        internal async Task<FormSEAppOccupant[]> GetOccupants(string id)
        {
            return this.mapper.Map<FormSEAppOccupant[]>(await this.Wrapper.ExecuteGet<DynamicsSEAppOccupant>(TableName, $"$expand=ptas_seappoccupant_ptas_sefinancialforms&$filter=ptas_seappoccupant_ptas_sefinancialforms/any(o: o/ptas_sefinancialformsid eq '{id}')"));
        }
    }
}