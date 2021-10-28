// <copyright file="BlobMoveAbstractController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Microsoft.Azure.Management.CognitiveServices.Models;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json.Linq;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Generic class to move blobs.
    /// </summary>
    public abstract class BlobMoveAbstractController : ApiController
    {
        private readonly HttpClient client = new HttpClient();
        private readonly NotificationSender notifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobMoveAbstractController"/> class.
        /// </summary>
        /// <param name="cloud">Cloud storage provider.</param>
        /// <param name="config">System configuration.</param>
        public BlobMoveAbstractController(ICloudStorageProvider cloud, IConfigParams config)
        {
            this.BlobContainer = cloud.GetCloudBlobContainer(config.BlobStorageContainer);
            this.notifier = new NotificationSender(config);
            this.Config = config;
        }

        /// <summary>
        /// Gets blob container.
        /// </summary>
        public CloudBlobContainer BlobContainer { get; }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfigParams Config { get; }

        /// <summary>
        /// Main blob move call point.
        /// </summary>
        /// <param name="info">Info required to move a blob to ILinx.</param>
        /// <returns>Status of request.</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        public async Task<MoveDocResults> PostAsync(BlobMoveInfo info)
        {
            try
            {
                // Step 1. Recursively enumerate blob files.
                var blobs = this.GetBlobs(info.BlobId);
                this.CheckTrue(!(blobs == null || blobs.Count() == 0), "Blob not found.");

                // Step 2. Move them 1 by 1 to Target, either ILinx or Sharepoint.
                (BlobDocumentContainer blobContainer, InsertResponse insertResponse)[] insertResults = this.InsertIntoTarget(info, blobs);
                var s1 = insertResults.Where(s => s.insertResponse?.Error == true).Select(s => s.insertResponse.ErrorMessage);
                var s2 = string.Join(", ", s1);
                this.CheckTrue(insertResults.Any(i => !(i.insertResponse?.Error ?? false)), $"Could not insert files. {s2}");

                // Step 2.5. Report errors
                var errorMessages =
                    insertResults
                    .Where(t => t.insertResponse.Error)
                    .Select(t => $"URL: {t.blobContainer.Url}. Message: {t.insertResponse.ErrorMessage}");
                if (errorMessages.Any())
                {
                    this.ReportErrors(string.Join("\n", errorMessages));
                }

                // Step 3. Mark as done in dynamics.
                var marked = await this.MarkAllAsDone(insertResults);

                // what should it do if the dynamics does not work or returns an error?

                // Step 4. Delete from blob.
                ////this.DeleteMovedBlobs(blobs, marked);

                // Done. Return results.
                return new MoveDocResults { Error = false, InsertResults = insertResults.Select(c => c.insertResponse) };
            }
            catch (Exception ex)
            {
                // report error.
                return new MoveDocResults { Error = true, Message = ex.Message };
            }
        }

        /// <summary>
        /// Gets Files For Container.
        /// </summary>
        /// <param name="documentContainer">Container to get the files from.</param>
        /// <returns>Array of received files.</returns>
        protected static ReceivedFileInfo[] GetFilesForContainer(BlobDocumentContainer documentContainer)
        {
            return documentContainer.BlobFiles.Select(itm =>
            {
                return new ReceivedFileInfo
                {
                    FileBits = itm.Bytes,
                    FileExtension = Path.GetExtension(itm.FileName),
                    FileName = itm.FileName,
                };
            }).ToArray();
        }

        /// <summary>
        /// Delete the blobs after they are moved.
        /// </summary>
        /// <param name="blobs">blob list to delete.</param>
        /// <param name="processed">Elements that were processed.</param>
        protected virtual void DeleteMovedBlobs(List<BlobDocumentContainer> blobs, bool[] processed)
        {
            var r = blobs
                .Select((b, i) => new { blob = b, proc = processed[i] })
                .Where(t => t.proc)
                .Select(x => x.blob);

            var blobFiles = r.SelectMany(blob => blob.BlobFiles).ToList();

            List<CloudBlockBlob> toDelete = blobFiles.Select(file => file.Blob)
              .ToList();
            toDelete.ForEach(blob => blob.Delete());
        }

        /// <summary>
        /// Gets a list of blobs for a document.
        /// </summary>
        /// <param name="blobId">Id of the document to fetch.</param>
        /// <returns>Enumerable of Blob Document container.</returns>
        protected virtual List<BlobDocumentContainer> GetBlobs(Guid blobId)
        {
            return this.BlobContainer
            .GetDirectoryReference($"{blobId}")
            .ListBlobs(true)
            .Select(blob
            =>
            {
                CloudBlockBlob cloudBlockBlob = blob as CloudBlockBlob;
                if (blob != null)
                {
                    var parts = cloudBlockBlob.Name.Split('/');

                    Guid detailId = Guid.Empty;

                    //// 0 id/1 detailid/2 sect/3 doc/4 filenam
                    //// id/sect/doc/filename
                    var lastItem = parts.Length;
                    if (lastItem >= 4 && (lastItem != 5 || Guid.TryParse(parts[1], out detailId)))
                    {
                        string fileName = parts[lastItem - 1];
                        string document = parts[lastItem - 2];
                        string section = parts[lastItem - 3];
                        return new
                        {
                            id = blobId,
                            detailId,
                            fileName,
                            document,
                            section,
                            url = $@"${blobId}/{detailId}/{section}/{document}",
                            blob = cloudBlockBlob,
                            bytes = this.GetBytesFromBlob(cloudBlockBlob),
                        };
                    }
                }

                return null;
            })
            .Where(b => b != null)
            .GroupBy(a => a.url)
            .Select((grouping, item) =>
            {
                var first = grouping.First();
                return new BlobDocumentContainer(
                first.id,
                first.detailId,
                first.section,
                first.document,
                first.url,
                grouping.Select(itm => new BlobFileDetails(
                itm.fileName,
                itm.blob,
                itm.bytes)).ToList());
            })
            .ToList();
        }

        /// <summary>
        /// Insert documents into ILinx.
        /// </summary>
        /// <param name="info">Blob move parameters.</param>
        /// <param name="blobs">Blobs to move.</param>
        /// <returns>Array of tuples with the container and the response.</returns>
        protected abstract (BlobDocumentContainer blobContainer, InsertResponse insertResponse)[] InsertIntoTarget(BlobMoveInfo info, List<BlobDocumentContainer> blobs);

        /// <summary>
        /// Are we a sharepoint implementation.
        /// </summary>
        /// <returns>true if sharepoint.</returns>
        protected abstract bool IsSharePoint();

        /// <summary>
        /// Mark all items as done in dynamics.
        /// </summary>
        /// <param name="insertResults">Results of the insertion.</param>
        /// <returns>The container and the insert response array of tuples.</returns>
        protected virtual async Task<bool[]> MarkAllAsDone((BlobDocumentContainer documentContainer, InsertResponse insertResponse)[] insertResults)
        {
            var tasks = insertResults.Select(async result =>
              {
                  if (result.insertResponse.Error)
                  {
                      return false;
                  }
                  else
                  {
                      return await this.MarkAsDone(
                              result.documentContainer.Id,
                              result.documentContainer.DetailId,
                              result.documentContainer.Section,
                              result.documentContainer.Document,
                              Guid.Parse(result.insertResponse.AssignedId));
                  }
              }).ToArray();
            bool[] results = await Task.WhenAll(tasks);
            return results;
        }

        private void CheckTrue(bool condition, string errorMsg)
        {
            if (!condition)
            {
                throw new CheckException(errorMsg);
            }
        }

        private byte[] GetBytesFromBlob(CloudBlockBlob cloudBlockBlob)
        {
            var stream = new MemoryStream();
            cloudBlockBlob.DownloadToStream(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Marks a document as moved to ilinx in dynamics.
        /// </summary>
        /// <param name="sEApplicationid">Senior application id.</param>
        /// <param name="sEAppDetailId">SEA detail Id.</param>
        /// <param name="section">Section id.</param>
        /// <param name="document">Socument id.</param>
        /// <param name="assignedDocumentId">Document that was assigned to blob container.</param>
        /// <returns>True if done.</returns>
        private async Task<bool> MarkAsDone(
          Guid sEApplicationid,
          Guid sEAppDetailId,
          string section,
          string document,
          Guid assignedDocumentId)
        {
            bool isSharePoint = this.IsSharePoint();
            var url = $"{this.Config.FinalizerUrl}?isSharePoint={isSharePoint}";
            var method = new HttpMethod("PATCH");
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                SEApplicationid = sEApplicationid,
                SEAppDetailId = sEAppDetailId,
                Document = document,
                Section = section,
                AssignedDocumentId = assignedDocumentId,
            });

            try
            {
                string authHeader = HttpContext.Current.Request.Headers.GetValues(HttpRequestHeader.Authorization.ToString()).FirstOrDefault();
                using (HttpRequestMessage requestMessage = new HttpRequestMessage(method, url))
                {
                    requestMessage.Content = new StringContent(
                        jsonStr,
                        Encoding.UTF8,
                        "application/json");
                    requestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), authHeader);
                    var response = await this.client.SendAsync(requestMessage);
                    var responseStr = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JObject.Parse(responseStr);

                    // TODO: check for response error, what to do?
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ReportErrors(string errorMessages)
        {
            this.notifier.SendNotification("Blob Move Controller Error", $"<h1>Errors</h1><pre>{errorMessages}</pre>");
        }
    }
}