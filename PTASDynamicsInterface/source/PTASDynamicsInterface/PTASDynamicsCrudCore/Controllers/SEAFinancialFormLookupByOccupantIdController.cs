// <copyright file="SEAFinancialFormLookupByOccupantIdController.cs" company="King County">
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
    [Route("v1/api/seafinancialformlookupbyoccupantid")]
    [ApiController]
    public class SEAFinancialFormLookupByOccupantIdController : ControllerBase
    {
        private const string TableName = "ptas_sefinancialformses";
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAFinancialFormLookupByOccupantIdController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper to use.</param>
        /// /// <param name="mapper">IMapper mapper to use.</param>
        public SEAFinancialFormLookupByOccupantIdController(CRMWrapper wrapper, IMapper mapper)
        {
            this.Wrapper = wrapper;
            this.mapper = mapper;
        }

        private CRMWrapper Wrapper { get; }

        /// <summary>
        /// Api to get the Senior Exemption Application Financial Form for a given OccupantId.
        /// </summary>
        /// <param name="id">Id  of the occupant.</param>
        /// <returns>The FormSeniorExemptionApplicationFinancial for the given occupant or empty.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationFinancialByOccupantId")]
        public async Task<ActionResult<FormSeniorExemptionApplicationFinancial[]>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial[]>.HandleExceptions(async () =>
            {
                string query = $"$expand=ptas_seappoccupant_ptas_sefinancialforms&$filter=ptas_seappoccupant_ptas_sefinancialforms/any(o: o/ptas_seappoccupantid eq '{id}')";
                var result = this.mapper.Map<FormSeniorExemptionApplicationFinancial[]>(await this.Wrapper.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(TableName, query));
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
    }
}