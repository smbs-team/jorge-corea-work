// <copyright file="SEApplicationFinancialController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle Seniour exemption applications Details.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEApplicationFinancialController : Controller
    {
        private const string KeyField = "ptas_sefinancialformsid";
        private const string NoDataFoundValue = "No Data Found";

        private const string TableName = "ptas_sefinancialformses";
        private readonly IMapper mapper;
        private readonly CRMWrapper wrapper;
        private readonly IConfigurationParams config;
        private string formerMedicarePlanId;
        private string formerSEAppDetailId;
        private string formerYearId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEApplicationFinancialController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        /// <param name="config">System params.</param>
        public SEApplicationFinancialController(CRMWrapper wrapper, IMapper mapper, IConfigurationParams config)
        {
            this.wrapper = wrapper;
            this.mapper = mapper;
            this.config = config;
        }

        /// <summary>
        /// Delete FormSeniorExemptionApplicationFinancial.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteFormSeniorExemptionApplicationFinancial")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationFinancial>> Delete(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial>.HandleExceptions(async () =>
             {
                 var result = await this.wrapper.ExecuteDelete(TableName, id);
                 if (!result)
                 {
                     throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                 }

                 JsonResult jsonResult = new JsonResult((FormSeniorExemptionApplicationFinancial)null)
                 {
                     StatusCode = (int)HttpStatusCode.OK,
                 };
                 return jsonResult;
             });
        }

        /// <summary>
        /// Gets a particular FormSeniorExemptionApplicationDetail by SeniorExemptionApplicationDetail id.
        /// </summary>
        /// <param name="id">Id to search for in field sefinancialformsid.</param>
        /// <returns>Found item or null.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationFinancialByFinancialFormId")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationFinancial>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(TableName, $"$filter={KeyField} eq '{id}'");
                if (result == null || result.Length == 0)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returns not data on reading {TableName} with key {KeyField}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                if (result != null && result.Length > 0)
                {
                    var occCtl = new SEAOccupantLookupByFinancialFormIdController(this.wrapper, this.mapper);
                    FormSeniorExemptionApplicationFinancial value = this.mapper.Map<FormSeniorExemptionApplicationFinancial>(result[0]);
                    var occupantList = await occCtl.GetOccupants(value.SEFinancialFormsId);
                    if (occupantList.Length > 0)
                    {
                        value.OccupantId = occupantList[0].SEAppOccupantId;
                    }

                    return new JsonResult(value);
                }

                return new JsonResult((FormSeniorExemptionApplicationFinancial)null);
            });
        }

        /// <summary>
        /// Updates SeniorExemptionApplicationFinancial.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormSeniorExemptionApplicationFinancial<see cref="FormSeniorExemptionApplicationFinancial"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchFormSeniorExemptionApplicationFinancial")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationFinancial>> Patch(string id, [FromBody] FormSeniorExemptionApplicationFinancial value)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial>.HandleExceptions(async () =>
            {
                this.FixEntityRelations(value);
                var result = await this.wrapper.ExecutePatch(TableName, this.mapper.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(value), $"{KeyField}={id}");
                this.FixFormerValues(value);
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                this.FixFormerValues(value);
                return new JsonResult(value);
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="receivedValues">Input information to save.</param>
        /// <returns>A FormSeniorExemptionApplicationFinancial<see cref="FormSeniorExemptionApplicationFinancial"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostFormSeniorExemptionApplicationFinancial")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationFinancial>> Post([FromBody] FormSeniorExemptionApplicationFinancial receivedValues)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationFinancial>.HandleExceptions(async () =>
            {
                receivedValues.SetId();
                this.FixEntityRelations(receivedValues);
                DynamicsSeniorExemptionApplicationFinancialForSave toSave = this.mapper.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(receivedValues);
                bool result = await this.wrapper.ExecutePost(TableName, toSave, KeyField);
                if (result)
                {
                    var occCtl = this.CreateOccupantController();
                    var postResult = await occCtl.SaveOccupantRelationship(receivedValues.SEFinancialFormsId, new FormSEAFinancialFormsSEAOpccupantsRelationship
                    {
                        SEAppOccupantId = receivedValues.OccupantId,
                    });
                }

                this.FixFormerValues(receivedValues);

                return result ? new JsonResult(receivedValues) : new JsonResult((FormSeniorExemptionApplicationFinancial)null);
            });
        }

        private SEAFinancialFormsSEAOpccupantsRelationshipController CreateOccupantController()
        {
            return new SEAFinancialFormsSEAOpccupantsRelationshipController(this.wrapper, this.config);
        }

        /// <summary>
        /// Fix entity relations before create or update.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormSeniorExemptionApplicationFinancial<see cref="FormSeniorExemptionApplicationFinancial"/> with the relations in the proper format.</returns>
        private FormSeniorExemptionApplicationFinancial FixEntityRelations(FormSeniorExemptionApplicationFinancial value)
        {
            if (value.MedicarePlanId == string.Empty)
            {
                value.MedicarePlanId = null;
            }

            this.formerYearId = value.YearId;
            this.formerSEAppDetailId = value.SEAppDetailId;
            this.formerMedicarePlanId = value.MedicarePlanId;
            value.YearId = string.IsNullOrEmpty(value.YearId) ? value.YearId : $"/ptas_years({value.YearId})";
            value.SEAppDetailId = string.IsNullOrEmpty(value.SEAppDetailId) ? value.SEAppDetailId : $"/ptas_seappdetails({value.SEAppDetailId})";
            value.MedicarePlanId = string.IsNullOrEmpty(value.MedicarePlanId) ? value.MedicarePlanId : $"/ptas_medicareplans({value.MedicarePlanId})";
            value.ModifiedBy = null;
            value.CreatedBy = null;
            value.CreatedOnBehalfBy = null;
            return value;
        }

        /// <summary>
        /// Fix former values por navigation properties.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormSeniorExemptionApplicationFinancial<see cref="FormSeniorExemptionApplicationFinancial"/> with the relations in the proper format.</returns>
        private FormSeniorExemptionApplicationFinancial FixFormerValues(FormSeniorExemptionApplicationFinancial value)
        {
            value.YearId = this.formerYearId;
            value.SEAppDetailId = this.formerSEAppDetailId;
            value.MedicarePlanId = this.formerMedicarePlanId;
            return value;
        }
    }
}