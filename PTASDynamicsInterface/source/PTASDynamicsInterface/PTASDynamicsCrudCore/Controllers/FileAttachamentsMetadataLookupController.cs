// <copyright file="FileAttachamentsMetadataLookupController.cs" company="King County">
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
    /// Controller to lookup a File Attachment Metadata by Senior Exemtion Application Id.
    /// </summary>
    [Route("v1/api/fileattachmentmetadatalookup")]
    [ApiController]
    public class FileAttachamentsMetadataLookupController : ControllerBase
    {
        private readonly IFileAttachmentMetadataManager fileAttachmentMetadataManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttachamentsMetadataLookupController"/> class.
        /// </summary>
        /// <param name="fileAttachamentMetadataManager">To get the FileAttachamentsMetadata info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public FileAttachamentsMetadataLookupController(IFileAttachmentMetadataManager fileAttachamentMetadataManager, IMapper mapper)
        {
            this.fileAttachmentMetadataManager = fileAttachamentMetadataManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a FileAttachamentsMetadata from the senior exemption application id.
        /// </summary>
        /// <param name="id">senior exemption application id.</param>
        /// <returns>a list of file attachement metadata rows that contains de sEAppId.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFormFileAttachmentMetadataByAppId")]
        public async Task<ActionResult<FormFileAttachmentMetadata[]>> Get(string id)
        {
            return await ControllerExtensions<FormFileAttachmentMetadata[]>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<FormFileAttachmentMetadata[]>((await this.fileAttachmentMetadataManager.GetFileAttchamentMetadataFromSEAppId(id)).ToArray());
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