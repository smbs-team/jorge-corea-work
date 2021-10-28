// <copyright file="FileAttachmentsMetadataController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle File Attachment Metadata Entity.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class FileAttachmentsMetadataController : Controller
    {
        private const string TableName = "ptas_fileattachmentmetadatas";
        private const string KeyField = "ptas_fileattachmentmetadataid";
        private const string NoDataFoundValue = "No Data Found";
        private readonly CRMWrapper wrapper;
        private readonly IMapper mapper;
        private string formerSeniorExemptionApplicationId;
        private string formerSeniorExemptionApplicationDetailId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttachmentsMetadataController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public FileAttachmentsMetadataController(CRMWrapper wrapper, IMapper mapper)
        {
            this.wrapper = wrapper;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets a particular FormFileAttachmentMetadata by FileAttachmentMetadata id.
        /// </summary>
        /// <param name="id">Id to search for in field FileAttachmentMetadataid.</param>
        /// <returns>Found item or null.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormFileAttachmentMetadataByMetadataId")]
        public virtual async Task<ActionResult<FormFileAttachmentMetadata>> Get(string id)
        {
            return await ControllerExtensions<FormFileAttachmentMetadata>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteGet<DynamicsFileAttachmentMetadata>(TableName, $"$filter={KeyField} eq '{id}'");

                if (result == null || result.Length == 0)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returns not data on reading {TableName} with key {id}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                if (result != null && result.Length > 0)
                {
                    return new JsonResult(this.mapper.Map<FormFileAttachmentMetadata>(result[0]));
                }

                return new JsonResult((FormFileAttachmentMetadata)null);
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A FormFileAttachmentMetadata<see cref="FormFileAttachmentMetadata"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostFormFileAttachmentMetadata")]
        public virtual async Task<ActionResult<FormFileAttachmentMetadata>> Post([FromBody] FormFileAttachmentMetadata value)
        {
            return await ControllerExtensions<FormFileAttachmentMetadata>.HandleExceptions(async () =>
            {
                value.SetId();
                this.FixEntityRelations(value);

                bool result = await this.wrapper.ExecutePost(TableName, this.mapper.Map<DynamicsFileAttachmentMetadataForSave>(value), KeyField);
                this.FixFormerValues(value);
                return result ? new JsonResult(value) : new JsonResult((FormFileAttachmentMetadata)null);
            });
        }

        /// <summary>
        /// Updates FileAttachmentMetadata.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormFileAttachmentMetadata<see cref="FormFileAttachmentMetadata"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchFormFileAttachmentMetadata")]
        public virtual async Task<ActionResult<FormFileAttachmentMetadata>> Patch(string id, [FromBody] FormFileAttachmentMetadata value)
        {
            return await ControllerExtensions<FormFileAttachmentMetadata>.HandleExceptions(async () =>
            {
                this.FixEntityRelations(value);
                var result = await this.wrapper.ExecutePatch(TableName, this.mapper.Map<DynamicsFileAttachmentMetadataForSave>(value), $"{KeyField}={id}");
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                this.FixFormerValues(value);
                return new JsonResult(value);
            });
        }

        /// <summary>
        /// Delete FileAttachmentMetadata.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteFormFileAttachmentMetadata")]
        public virtual async Task<ActionResult<FormFileAttachmentMetadata>> Delete(string id)
        {
            return await ControllerExtensions<FormFileAttachmentMetadata>.HandleExceptions(async () =>
            {
                var result = await this.wrapper.ExecuteDelete(TableName, id);
                if (!result)
                {
                    throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
                }

                JsonResult jsonResult = new JsonResult((FormFileAttachmentMetadata)null)
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
        /// <returns>A FormFileAttachmentMetadata<see cref="FormFileAttachmentMetadata"/> with the relations in the proper format.</returns>
        private FormFileAttachmentMetadata FixEntityRelations(FormFileAttachmentMetadata value)
        {
            this.formerSeniorExemptionApplicationId = value.SeniorExemptionApplicationId;
            this.formerSeniorExemptionApplicationDetailId = value.SeniorExemptionApplicationDetailId;
            value.SeniorExemptionApplicationId = string.IsNullOrEmpty(value.SeniorExemptionApplicationId) ? value.SeniorExemptionApplicationId : $"/ptas_seapplications({value.SeniorExemptionApplicationId})";
            value.SeniorExemptionApplicationDetailId = string.IsNullOrEmpty(value.SeniorExemptionApplicationDetailId) ? value.SeniorExemptionApplicationDetailId : $"/ptas_seappdetails({value.SeniorExemptionApplicationDetailId})";
            return value;
        }

        /// <summary>
        /// Fix former values por navigation properties.
        /// </summary>
        /// <param name="value">Entity to fix.</param>
        /// <returns>A FormFileAttachmentMetadata<see cref="FormFileAttachmentMetadata"/> with the relations in the proper format.</returns>
        private FormFileAttachmentMetadata FixFormerValues(FormFileAttachmentMetadata value)
        {
            value.SeniorExemptionApplicationId = this.formerSeniorExemptionApplicationId;
            value.SeniorExemptionApplicationDetailId = this.formerSeniorExemptionApplicationDetailId;
            return value;
        }
    }
}