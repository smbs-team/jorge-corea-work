// <copyright file="SEAppLookupByContactIdController.cs" company="King County">
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

    using PTASDynamicsCrudCore.Classes;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to get Senior Exemption Application for a contact.
    /// </summary>
    [Route("v1/api/seniorexemptionapplications")]
    [ApiController]
    public class SEAppLookupByContactIdController : ControllerBase
    {
        private const string TableName = "ptas_seapplications";
        private readonly IMapper mapper;
        private readonly SecurityChecker securityChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppLookupByContactIdController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper to use.</param>
        /// /// <param name="mapper">IMapper mapper to use.</param>
        /// <param name="contactManager">Contact manager.</param>
        public SEAppLookupByContactIdController(CRMWrapper wrapper, IMapper mapper, IContactManager contactManager)
        {
            this.Wrapper = wrapper;
            this.mapper = mapper;
            this.securityChecker = new SecurityChecker(contactManager);
        }

        private CRMWrapper Wrapper { get; }

        /// <summary>
        /// Api to get the senior application exceptions for a given contact.
        /// </summary>
        /// <param name="id">Id (currently email) of the contact.</param>
        /// <returns>The applications exceptions for the given contact or empty.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationByContactId")]
        public async Task<ActionResult<FormSeniorExemptionApplication[]>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplication[]>.HandleExceptions(async () =>
            {
                if (!await this.securityChecker.CheckSecurity(this.HttpContext, id))
                {
                    throw new DynamicsInterfaceException(
                        $"Unauthorized.",
                        (int)HttpStatusCode.Unauthorized,
                        null);
                }

                var result = this.mapper.Map<FormSeniorExemptionApplication[]>(await this.Wrapper.ExecuteGet<DynamicsSeniorExemptionApplication>(TableName, $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter=ptas_contactid/contactid eq '{id}'"));
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