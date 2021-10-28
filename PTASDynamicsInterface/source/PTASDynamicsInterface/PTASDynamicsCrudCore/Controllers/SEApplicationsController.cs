// <copyright file="SEApplicationsController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudCore.Classes;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle Seniour exemption applications.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEApplicationsController : ControllerBase
    {
        private const string TableName = "ptas_seapplications";
        private const string KeyField = "ptas_seapplicationid";
        private const string NoDataFoundValue = "No Data Found";
        private readonly CRMWrapper wrapper;
        private readonly IMapper mapper;
        private readonly SecurityChecker securityChecker;
        private string formerContactid;
        private string formerParcelid;
        private string formerTransferredFrCounty;
        private string formerAddrCountryId;
        private string formerCheckAddressCountryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEApplicationsController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        /// <param name="contactManager">Contact manager.</param>
        public SEApplicationsController(CRMWrapper wrapper, IMapper mapper, IContactManager contactManager)
        {
            this.wrapper = wrapper;
            this.mapper = mapper;
            this.securityChecker = new SecurityChecker(contactManager);
        }

        /// <summary>
        /// Gets a particular FormSeniorExemptionApplication by ptas_fileattachmentmetadataid.
        /// </summary>
        /// <param name="id">Id to search for in field ptas_fileattachmentmetadataid.</param>
        /// <returns>Found item or null.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSeniorExemptionApplicationByAppId")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplication>> Get(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplication>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteGet<DynamicsSeniorExemptionApplication>(TableName, $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter={KeyField} eq '{id}'");
                if (result == null || result.Length == 0)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returned no data on reading {TableName} with key {KeyField} = {id}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                var cid = result[0].ContactId;
                if (!await this.securityChecker.CheckSecurity(this.HttpContext, cid))
                {
                    throw new DynamicsInterfaceException(
                        $"Unauthorized.",
                        (int)HttpStatusCode.Unauthorized,
                        null);
                }

                return new JsonResult(this.mapper.Map<FormSeniorExemptionApplication>(result[0]));
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A FormSeniorExemptionApplication<see cref="FormSeniorExemptionApplication"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostFormSeniorExemptionApplication")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplication>> Post([FromBody] FormSeniorExemptionApplication value)
        {
            return await ControllerExtensions<FormSeniorExemptionApplication>.HandleExceptions(async () =>
            {
                value.SetId();
                this.FixEntityRelations(value);
                bool result = await this.wrapper.ExecutePost(TableName, this.mapper.Map<DynamicsSeniorExemptionApplicationForSave>(value), KeyField);
                this.FixFormerValues(value);
                return result ? new JsonResult(value) : new JsonResult((FormSeniorExemptionApplication)null);
            });
        }

        /// <summary>
        /// Updates file metadata.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormSeniorExemptionApplication<see cref="FormSeniorExemptionApplication"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchFormSeniorExemptionApplication")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplication>> Patch(string id, [FromBody] FormSeniorExemptionApplication value)
        {
            return await ControllerExtensions<FormSeniorExemptionApplication>.HandleExceptions(async () =>
            {
                this.FixEntityRelations(value);
                var result = await this.wrapper.ExecutePatch(TableName, this.mapper.Map<DynamicsSeniorExemptionApplicationForSave>(value), value.SEAapplicationId);

                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                this.FixFormerValues(value);
                return new JsonResult(value);
            });
        }

        /// <summary>
        /// Delete Senior Exemption Application entity.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteFormSeniorExemptionApplication")]
        public virtual async Task<ActionResult<FormSeniorExemptionApplication>> Delete(string id)
        {
            return await ControllerExtensions<FormSeniorExemptionApplication>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteDelete(TableName, id);
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                JsonResult jsonResult = new JsonResult((FormSeniorExemptionApplication)null)
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
        /// <returns>A FormSeniorExemptionApplicationa<see cref="FormSeniorExemptionApplication"/> with the relations in the proper format.</returns>
        private FormSeniorExemptionApplication FixEntityRelations(FormSeniorExemptionApplication value)
        {
            this.formerContactid = value.ContactId;
            this.formerParcelid = value.ParcelId;
            this.formerTransferredFrCounty = value.TransferredFrCounty;
            this.formerAddrCountryId = value.AddrcountryId;
            this.formerCheckAddressCountryId = value.CheckAddressCountryId;
            value.ContactId = string.IsNullOrEmpty(value.ContactId) ? value.ContactId : $"/contacts({value.ContactId})";
            value.ParcelId = string.IsNullOrEmpty(value.ParcelId) ? value.ParcelId : $"/ptas_parceldetails({value.ParcelId})";

            value.CheckAddressCountryId = string.IsNullOrEmpty(value.CheckAddressCountryId) ? value.CheckAddressCountryId : $"/ptas_countries({value.CheckAddressCountryId})";

            value.TransferredFrCounty = string.IsNullOrEmpty(value.TransferredFrCounty) ? value.TransferredFrCounty : $"/ptas_counties({value.TransferredFrCounty})";
            value.AddrcountryId = string.IsNullOrEmpty(value.AddrcountryId) ? value.AddrcountryId : $"/ptas_countries({value.AddrcountryId})";
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
        private FormSeniorExemptionApplication FixFormerValues(FormSeniorExemptionApplication value)
        {
            value.ContactId = this.formerContactid;
            value.ParcelId = this.formerParcelid;
            value.TransferredFrCounty = this.formerTransferredFrCounty;
            value.AddrcountryId = this.formerAddrCountryId;
            value.CheckAddressCountryId = this.formerCheckAddressCountryId;
            return value;
        }
    }
}