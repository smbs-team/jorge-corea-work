// <copyright file="BlobToIlinxFinalizerController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Castle.DynamicProxy.Generators;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Serilog;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Api endpoint to mark a Dynamics ptas_fileattachmentmetadata as moved from blob to ILinx.
    /// </summary>
    [Route("v1/api/BlobToIlinxFinalizer")]
    [ApiController]
    public class BlobToIlinxFinalizerController : ControllerBase
    {
        private const string PTASFileattachmentmetadatas = "ptas_fileattachmentmetadatas";
        private const string ErrorResultVal = "Error";
        private const string OKResultVal = "Ok";
        private const string ErrorMessageNoElementsFound = "No elements found during Blob To Ilinx Finalizer.";
        private const string HeaderLogDynamicException = "API Web Dynamics Interface/DynamicException: ";
        private const string HeaderLogSystemException = "API Web Dynamics Interface/System Exception: ";
        private readonly CRMWrapper dynamicsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobToIlinxFinalizerController"/> class.
        /// </summary>
        /// <param name="dynamicsWrapper">CRM interface wrapper.</param>
        public BlobToIlinxFinalizerController(CRMWrapper dynamicsWrapper)
        {
            this.dynamicsWrapper = dynamicsWrapper;
        }

        /// <summary>
        /// Post action, updates metadata for to ILinx.
        /// </summary>
        /// <returns>Result of update.</returns>
        /// <param name="info">Info to locate and update the metadata record.</param>
        /// <param name="isSharepoint">Optional parameter if info was saved in sharepoint.</param>
        [HttpPatch]
        [SwaggerOperation(OperationId = "PatchBlobToIlinxFinalize")]
        public async Task<BlobToIlinxFinalizerOperationResult> Patch([FromBody] BlobToIlinxFinalizerParams info, [FromQuery] bool isSharepoint = false)
        {
            try
            {
                string value = info.SEAppDetailId.Equals(default) ? "null" : info.SEAppDetailId.ToString();

                /*
                 * conditions:
                 * 1. applicationId = param
                 * 2. application detail id = param
                 * 3. isBlob = true
                 * 4. isILinx = false  agregar or Sharepoint= false
                 * 5. portal document = param.document
                 * 6. portal section = param.section
                 */

                string query = $"$filter=(ptas_seniorexemptionapplication/ptas_seapplicationid eq {info.SEApplicationid}) and (ptas_seniorexemptionapplicationdetail/ptas_seappdetailid eq {value}) and (ptas_isblob eq true) and ((ptas_isilinx eq false) or (ptas_issharepoint eq false)) and (ptas_portaldocument eq '{info.Document}') and (ptas_portalsection eq '{info.Section}')";

                var matchingAttachmentMetadata = await this.dynamicsWrapper.ExecuteGet<SEApplicationIdAndOriginalFileName>(PTASFileattachmentmetadatas, query);
                if (!matchingAttachmentMetadata.Any())
                {
                    Log.Error($"{HeaderLogDynamicException}: status code: {(int)HttpStatusCode.NotFound}, message: {ErrorMessageNoElementsFound}, inner exception: null");
                    return new BlobToIlinxFinalizerOperationResult
                    {
                        Result = ErrorResultVal,
                        Message = $"{ErrorMessageNoElementsFound}",
                        Count = 0,
                    };
                }

                SEApplicationIdAndOriginalFileName foundAttachmentMetadata = matchingAttachmentMetadata.FirstOrDefault(md => IsCorrectOriginalFileName(md.OriginalFileName)) ?? matchingAttachmentMetadata.First();

                var rest = matchingAttachmentMetadata.Where(md => md.Id != foundAttachmentMetadata.Id).ToArray();

                var results = await Task.WhenAll(rest.Select(md
                    => this.dynamicsWrapper.ExecuteDelete(PTASFileattachmentmetadatas, md.Id.ToString())));

                string originalFileName = ParseFileName(foundAttachmentMetadata.OriginalFileName);

                // Fix for bug: 126097, keeping the code in case is needed again in the future.
                // string sharepointUrl = string.IsNullOrEmpty(originalFileName) ? string.Empty : $"{this.config.SharepointDocsUri}/{this.config.SharepointSite}/{this.config.SharepointDrive}/{info.AssignedDocumentId}/{originalFileName}";
                string sharepointUrl = null;
                var newItem = new SEApplicationUpdateFields
                {
                    DocumentId = info.AssignedDocumentId,
                    IsBlob = false,
                    IsIlinx = !isSharepoint,
                    IsSharepoint = isSharepoint,
                    RedactionUrl = this.dynamicsWrapper.RedactionToolUrl(info.AssignedDocumentId),
                    SharepointUrl = sharepointUrl,
                };

                await this.dynamicsWrapper.ExecutePatch(PTASFileattachmentmetadatas, newItem, $"ptas_fileattachmentmetadataid={foundAttachmentMetadata.Id}");

                // EexecutePatch throws an exception if the patch failed
                return new BlobToIlinxFinalizerOperationResult
                {
                    Result = OKResultVal,
                    Message = string.Empty,
                    Count = 1,
                };
            }
            catch (DynamicsInterfaceException diex)
            {
                Log.Error($"{HeaderLogDynamicException}: status code: {diex.StatusCode}, message: {diex.Message}, inner exception: {diex.InnerException}");
                return new BlobToIlinxFinalizerOperationResult { Result = ErrorResultVal, Message = diex.Message, Count = 0 };
            }
            catch (System.Exception ex)
            {
                Log.Error($"{HeaderLogSystemException}: status code: {(int)HttpStatusCode.InternalServerError}, message: {ex.Message}, inner excepcion: {ex.InnerException})");
                return new BlobToIlinxFinalizerOperationResult { Result = ErrorResultVal, Message = ex.Message, Count = 0 };
            }
        }

        private static string ParseFileName(string originalFileName)
            => !IsCorrectOriginalFileName(originalFileName) || !(JsonConvert.DeserializeObject(originalFileName) is JArray valuesArray)
                ? string.Empty
                : valuesArray.FirstOrDefault()?.ToString() ?? string.Empty;

        private static bool IsCorrectOriginalFileName(string originalFileName)
            => !string.IsNullOrEmpty(originalFileName) && originalFileName.StartsWith("[") && originalFileName.EndsWith("]");
    }
}