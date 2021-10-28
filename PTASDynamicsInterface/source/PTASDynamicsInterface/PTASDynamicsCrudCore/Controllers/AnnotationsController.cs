// <copyright file="AnnotationsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Annotations Controller.
    /// </summary>
    [Route("v1/api/[controller]")]
    public class AnnotationsController : GenericDynamicsControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationsController"/> class.
        /// </summary>
        /// <param name="config">Sys config.</param>
        /// <param name="memoryCache">Memory cache.</param>
        /// <param name="tokenManager">Token.</param>
        public AnnotationsController(IConfigurationParams config, IMemoryCache memoryCache, ITokenManager tokenManager)
            : base(config, memoryCache, tokenManager)
        {
        }

        /// <summary>
        /// Attempts to apply json to dynamics.
        /// </summary>
        /// <param name="changes">Json to apply.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "Post")]
        public async Task<ActionResult> Post([FromForm] Payload changes)
        {
            try
            {
                if (string.IsNullOrEmpty(changes.AnnotationId))
                {
                    changes.AnnotationId = Guid.NewGuid().ToString();
                }

                MemoryStream m = new MemoryStream();
                changes.File.CopyTo(m);
                byte[] filebytes = m.ToArray();
                string base64String = Convert.ToBase64String(filebytes);
                var t = new EntityChanges
                {
                    EntityId = changes.AnnotationId,
                    EntityName = "annotation",
                    Changes = new Dictionary<string, object>()
                    {
                            { "notetext", changes.NoteText },
                            { "filename", changes.File.FileName },
                            { "documentbody", base64String },
                            { "isdocument", true },
                            { "mimetype", changes.File.ContentType },
                            {
                                $"objectid_{changes.ObjectIdType}@odata.bind",
                                $"/{changes.ObjectIdType.Pluralize()}({changes.ObjectId})"
                            },
                    },
                };
                var cc = new EntityChanges[] { t };
                var result = (await this.TransactionHelper.PrepareAndSendBatch(cc, false)).FirstOrDefault();
                if (result.IsSuccessStatusCode)
                {
                    return new OkObjectResult(new { result = "Ok" });
                }
                else
                {
                    var msg = await result.Content.ReadAsStringAsync();
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message,
                });
            }
        }

        /// <summary>
        /// Payload input for the annotiation api.
        /// </summary>
        public class Payload
        {
            /// <summary>
            /// Gets or sets annotation Id. Leave null if new.
            /// </summary>
            public string AnnotationId { get; set; }

            /// <summary>
            /// Gets or sets file.
            /// </summary>
            public IFormFile File { get; set; }

            /// <summary>
            /// Gets or sets note Text.
            /// </summary>
            public string NoteText { get; set; }

            /// <summary>
            /// Gets or sets object Id.
            /// </summary>
            public string ObjectId { get; set; }

            /// <summary>
            /// Gets or sets object Id Type.
            /// </summary>
            public string ObjectIdType { get; set; }
        }
    }
}