// <copyright file="BlobMoveToSharepointController.cs" company="King County">
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

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// BlobMoveToSharepointController.
    /// </summary>
    public class BlobMoveToSharepointController : BlobMoveAbstractController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobMoveToSharepointController"/> class.
        /// </summary>
        /// <param name="cloud">Cloud.</param>
        /// <param name="config">Config.</param>
        public BlobMoveToSharepointController(ICloudStorageProvider cloud, IConfigParams config)
            : base(cloud, config)
        {
        }

        /// <summary>
        /// Inserts into this target.
        /// </summary>
        /// <param name="info">Info to insert.</param>
        /// <param name="blobs">Blobs to insert into.</param>
        /// <returns>Array of tuples of responses and original blob.</returns>
        protected override (BlobDocumentContainer blobContainer, InsertResponse insertResponse)[] InsertIntoTarget(BlobMoveInfo info, List<BlobDocumentContainer> blobs)
        {
            return blobs
              .Select(blobContainer => this.SaveToSharepoint(info, blobContainer)).ToArray();
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <returns>True, always, it's not sharepoint.</returns>
        protected override bool IsSharePoint() => true;

        private (BlobDocumentContainer blobContainer, InsertResponse insertResponse) SaveToSharepoint(BlobMoveInfo info, BlobDocumentContainer blobContainer)
        {
            ReceivedFileInfo[] blobFiles = GetFilesForContainer(blobContainer);
            var filesList = this.SplitFilesIntoSections(blobFiles);
            InsertResponse insertResponse = null;
            filesList.ToList().ForEach(files =>
            {
                var response = this.SaveToSharepoint(files, info);

                // Make sure to return the error if any.
                insertResponse = insertResponse == null ? response : insertResponse.Error ? insertResponse : response;
            });

            return (blobContainer, insertResponse);
        }

        private InsertResponse SaveToSharepoint(ReceivedFileInfo[] files, BlobMoveInfo info)
        {
            var id = Guid.NewGuid().ToString();
            var url = string.Format(this.Config.SharepointApiURL, "PutFileCollection", id);

            using (var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10),
            })
            {
                var formData = new MultipartFormDataContent();
                files.ToList().ForEach(file =>
                {
                    formData.Add(new ByteArrayContent(file.FileBits), Path.GetFileNameWithoutExtension(file.FileName), file.FileName);
                });

                // todo add access token
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = formData,
                };

                var authHeader = JWTDecoder.GetAuthHeader();
                if (authHeader != null)
                {
                    requestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), authHeader);
                }

                var result = client.SendAsync(requestMessage).Result;
                if (result.IsSuccessStatusCode)
                {
                    return new InsertResponse
                    {
                        AssignedId = id,
                        Error = false,
                        ErrorMessage = string.Empty,
                    };
                }
                else
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    return new InsertResponse
                    {
                        AssignedId = default,
                        Error = true,
                        ErrorMessage = content,
                    };
                }
            }
        }

        /// <summary>
        /// This function will take a list of files and split it in lists of less than 30 MB.
        /// There is a limit of 30 MB on payloads for Azure functions on version ~3. Issue is still not resolved.
        /// See https://github.com/Azure/azure-functions-host/issues/5540.
        /// </summary>
        private IEnumerable<ReceivedFileInfo[]> SplitFilesIntoSections(ReceivedFileInfo[] files)
        {
            IList<ReceivedFileInfo> newSubList = new List<ReceivedFileInfo>();
            int filesSize = 0;
            foreach (var file in files)
            {
                filesSize += file.FileBits.Length;
                if (filesSize >= 30000000)
                {
                    yield return newSubList.ToArray();
                    newSubList.Clear();
                    filesSize = file.FileBits.Length;
                }

                newSubList.Add(file);
            }

            if (newSubList.Any())
            {
                yield return newSubList.ToArray();
            }
        }
    }
}