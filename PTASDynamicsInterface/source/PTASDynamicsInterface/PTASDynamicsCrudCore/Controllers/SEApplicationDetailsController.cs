// <copyright file="SEApplicationDetailsController.cs" company="King County">
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
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle Seniour exemption applications Details.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEApplicationDetailsController : Controller
    {
        private const string TableName = "ptas_seappdetails";
        private const string KeyField = "ptas_seappdetailid";
        private const string NoDataFoundValue = "No Data Found";
        private readonly CRMWrapper wrapper;
        private readonly IMapper mapper;
        private string formerContactId;
        private string formerParcelId;
        private string formerSeApplicationId;
        private string formerYearId;
        private string formerDecisionReasonId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEApplicationDetailsController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public SEApplicationDetailsController(CRMWrapper wrapper, IMapper mapper)
        {
            this.wrapper = wrapper;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets a particular FormSeniorExemptionApplicationDetail by SeniorExemptionApplicationDetail id.
        /// </summary>
        /// <param name="id">Id to search for in field SEAppdetailid.</param>
        /// <returns>Found item or null.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationDetailByAppDetailId")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationDetail>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationDetail>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(TableName, $"$filter={KeyField} eq '{id}'");
                if (result == null || result.Length == 0)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returns no data on reading {TableName} with key {KeyField}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                if (result != null && result.Length > 0)
                {
                    return new JsonResult(this.mapper.Map<FormSeniorExemptionApplicationDetail>(result[0]));
                }

                return new JsonResult((FormSeniorExemptionApplicationDetail)null);
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A FormSeniorExemptionApplicationDetail<see cref="FormSeniorExemptionApplicationDetail"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostFormSeniorExemptionApplicationDetail")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationDetail>> Post([FromBody] FormSeniorExemptionApplicationDetail value)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationDetail>.HandleExceptions(async () =>
            {
                value.SetId();
                this.FixEntityRelations(value);
                bool result = await this.wrapper.ExecutePost(TableName, this.mapper.Map<DynamicsSeniorExemptionApplicationDetailForSave>(value), KeyField);
                this.FixFormerValues(value);
                return result ? new JsonResult(value) : new JsonResult((FormSeniorExemptionApplicationDetail)null);
            });
        }

        /// <summary>
        /// Updates SeniorExemptionApplicationDetail.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormParcelDetail<see cref="FormSeniorExemptionApplicationDetail"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchFormSeniorExemptionApplicationDetail")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationDetail>> Patch(string id, [FromBody] FormSeniorExemptionApplicationDetail value)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationDetail>.HandleExceptions(async () =>
            {
                this.FixEntityRelations(value);
                var result = await this.wrapper.ExecutePatch(TableName, this.mapper.Map<DynamicsSeniorExemptionApplicationDetailForSave>(value), $"{KeyField}={id}");
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                this.FixFormerValues(value);
                return new JsonResult(value);
            });
        }

        /// <summary>
        /// Delete SeniorExemptionApplicationDetail.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteFormSeniorExemptionApplicationDetail")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplicationDetail>> Delete(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplicationDetail>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteDelete(TableName, id);
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                JsonResult jsonResult = new JsonResult((FormSeniorExemptionApplicationDetail)null)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return jsonResult;
            });
        }

        /// <summary>
        /// Fix entity relations before create or update.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormSeniorExemptionApplicationDetail<see cref="FormSeniorExemptionApplicationDetail"/> with the relations in the proper format.</returns>
        private FormSeniorExemptionApplicationDetail FixEntityRelations(FormSeniorExemptionApplicationDetail value)
        {
            this.formerContactId = value.ContactId;
            this.formerParcelId = value.ParcelId;
            this.formerSeApplicationId = value.SeApplicationId;
            this.formerYearId = value.YearId;
            this.formerDecisionReasonId = value.DecisionReasonId;
            value.ContactId = string.IsNullOrEmpty(value.ContactId) ? value.ContactId : $"/contacts({value.ContactId})";
            value.ParcelId = string.IsNullOrEmpty(value.ParcelId) ? value.ParcelId : $"/ptas_parceldetails({value.ParcelId})";
            value.SeApplicationId = string.IsNullOrEmpty(value.SeApplicationId) ? value.SeApplicationId : $"/ptas_seapplications({value.SeApplicationId})";
            value.YearId = string.IsNullOrEmpty(value.YearId) ? value.YearId : $"/ptas_years({value.YearId})";
            value.DecisionReasonId = string.IsNullOrEmpty(value.DecisionReasonId) ? value.DecisionReasonId : $"/ptas_seexemptionreasons({value.DecisionReasonId})";
            value.ModifiedBy = null;
            value.CreatedBy = null;
            value.CreatedOnBehalfBy = null;
            return value;
        }

        /// <summary>
        /// Fix former values por navigation properties.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormSeniorExemptionApplicationDetail<see cref="FormSeniorExemptionApplicationDetail"/> with the relations in the proper format.</returns>
        private FormSeniorExemptionApplicationDetail FixFormerValues(FormSeniorExemptionApplicationDetail value)
        {
            value.ContactId = this.formerContactId;
            value.ParcelId = this.formerParcelId;
            value.SeApplicationId = this.formerSeApplicationId;
            value.YearId = this.formerYearId;
            value.DecisionReasonId = this.formerDecisionReasonId;
            return value;
        }
    }
}