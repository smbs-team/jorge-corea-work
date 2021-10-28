// <copyright file="SEAppOtherPropsController.cs" company="King County">
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
    /// Controller to handle SEAppOtherProp Entity.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEAppOtherPropsController : Controller
    {
        private const string TableName = "ptas_seappotherprops";
        private const string KeyField = "ptas_seappotherpropid";
        private const string NoDataFoundValue = "No Data Found";
        private readonly CRMWrapper wrapper;
        private readonly IMapper mapper;
        private string formerSEApplicationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppOtherPropsController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public SEAppOtherPropsController(CRMWrapper wrapper, IMapper mapper)
        {
            this.wrapper = wrapper;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets a particular SEAppOtherProp by SEAppOtherProp id.
        /// </summary>
        /// <param name="id">Id to search for in field SEAppOtherPropid.</param>
        /// <returns>Found item or null.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormSEAppOtherPropByOtherPropId")]
        public virtual async Task<ActionResult<FormSEAppOtherProp>> Get(string id)
        {
            return await ControllerExtensions<FormSEAppOtherProp>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteGet<DynamicsSEAppOtherProp>(TableName, $"$filter={KeyField} eq '{id}'");
                if (result == null || result.Length == 0)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returns not data on reading {TableName} with key {KeyField}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                if (result != null && result.Length > 0)
                {
                    return new JsonResult(this.mapper.Map<FormSEAppOtherProp>(result[0]));
                }

                return new JsonResult((FormSEAppOtherProp)null);
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A FormSEAppOtherProp<see cref="FormSEAppOtherProp"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostFormSEAppOtherProp")]
        public virtual async Task<ActionResult<FormSEAppOtherProp>> Post([FromBody] FormSEAppOtherProp value)
        {
            return await ControllerExtensions<FormSEAppOtherProp>.HandleExceptions(async () =>
            {
                value.SetId();
                string formerSEApplicationId = value.SEApplicationId;
                this.FixEntityRelations(value);
                bool result = await this.wrapper.ExecutePost(TableName, this.mapper.Map<DynamicsSEAppOtherPropForSave>(value), KeyField);
                value.SEApplicationId = formerSEApplicationId;
                return result ? new JsonResult(value) : new JsonResult((FormSEAppOtherProp)null);
            });
        }

        /// <summary>
        /// Updates SEAppOtherProp.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormSEAppOtherProp<see cref="FormSEAppOtherProp"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchFormSEAppOtherProp")]
        public virtual async Task<ActionResult<FormSEAppOtherProp>> Patch(string id, [FromBody] FormSEAppOtherProp value)
        {
            return await ControllerExtensions<FormSEAppOtherProp>.HandleExceptions(async () =>
            {
                string formerSEApplicationId = value.SEApplicationId;
                this.FixEntityRelations(value);
                var result = await this.wrapper.ExecutePatch(TableName, this.mapper.Map<DynamicsSEAppOtherPropForSave>(value), $"{KeyField}={id}");
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                this.FixFormerValues(value);
                return new JsonResult(value);
            });
        }

        /// <summary>
        /// Delete SEAppOtherProp.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteFormSEAppOtherProp")]
        public virtual async Task<ActionResult<FormSEAppOtherProp>> Delete(string id)
        {
            return await ControllerExtensions<FormSEAppOtherProp>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteDelete(TableName, id);
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                JsonResult jsonResult = new JsonResult((FormSEAppOtherProp)null)
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
        /// <returns>A FormSEAppOtherProp<see cref="FormSEAppOtherProp"/> with the relations in the proper format.</returns>
        private FormSEAppOtherProp FixEntityRelations(FormSEAppOtherProp value)
        {
            this.formerSEApplicationId = value.SEApplicationId;
            value.SEApplicationId = string.IsNullOrEmpty(value.SEApplicationId) ? value.SEApplicationId : $"/ptas_seapplications({value.SEApplicationId})";
            value.ModifiedBy = null;
            value.CreatedBy = null;
            return value;
        }

        /// <summary>
        /// Fix former values por navigation properties.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormFileAttachmentMetadata<see cref="FormFileAttachmentMetadata"/> with the relations in the proper format.</returns>
        private FormSEAppOtherProp FixFormerValues(FormSEAppOtherProp value)
        {
            value.SEApplicationId = this.formerSEApplicationId;
            return value;
        }
    }
}