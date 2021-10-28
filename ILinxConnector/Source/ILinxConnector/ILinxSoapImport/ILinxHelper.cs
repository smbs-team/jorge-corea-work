// <copyright file="ILinxHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using ILinxSoapImport.EdmsService;
    using ILinxSoapImport.Exceptions;
    using PTASILinxConnectorHelperClasses.Models;
    using PTASLinxConnectorHelperClasses.Models;
    using PTASTiffs;

    /// <summary>
    /// Class to facilitate the access to ILinx soap interface.
    /// </summary>
    public class ILinxHelper : IILinxHelper
    {
        private const string AccountNumber = "ACCOUNTNUMBER";
        private const string RollSpaceYear = "ROLL YEAR";
        private const string RollYear = "ROLLYEAR";
        private const string DocType = "DOCTYPE";
        private const string DocumentDate = "DOCUMENTDATE";
        private const string UserId = "LOGINUSERID";
        private const string ScannerId = "SCANNERID";
        private const string RecId = "RECID";
        private const string PageSpaceCount = "PAGE COUNT";
        private const string PageCount = "PAGECOUNT";
        private const string ScanTime = "SCANDATE_TIME";
        private readonly ContentStoreHelper contentStore;
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ILinxHelper"/> class.
        /// </summary>
        /// <param name="config">Initial configuration.</param>
        public ILinxHelper(IConfigParams config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.contentStore = new ContentStoreHelper(config);
            this.config = config;
        }

        /// <summary>
        /// Get a document from iLinx.
        /// </summary>
        /// <param name="id">Id of the document to fetch.</param>
        /// <param name="returnTiffs">should it return tiffs as tiffs.</param>
        /// <returns>The document information.</returns>
        public DocumentResult FetchDocument(string id, bool returnTiffs = false)
        {
            var securityToken = this.contentStore.GetSecurityToken();
            using (var client = this.contentStore.GetContentStoreClient())
            {
                ContentStoreDocument documentResponse = client.RetrieveDocument(this.config.UserName, securityToken, new Guid(id), this.config.ApplicationName, out DiscussionMessage[] messages);
                if (documentResponse == null)
                {
                    throw new DocumentNotFoundException();
                }

                ContentStoreFile[] files = documentResponse.Files;
                if (files.Length == 0)
                {
                    throw new EmptyDocException();
                }

                DocumentResult result;

                var rxr = files.SelectMany((ContentStoreFile csFile) =>
                {
                    return csFile.FileExtension.ToLower().Contains("tif") && !returnTiffs
                        ? TiffManager.TiffToImages(new MemoryStream(csFile.FileBits)).Select((img, index) => new FileDetails
                        {
                            FileExtension = ".png",
                            FileId = $"{csFile.FileID}.{index}",
                            Size = img.Length,
                            FileBytes = img,
                            FileName = $"{id}.{csFile.FileID}.{index}.png",
                        })
                        : (new FileDetails[]
                        {
                            new FileDetails
                            {
                                FileExtension = csFile.FileExtension,
                                FileId = $"{csFile.FileID}.0",
                                Size = csFile.FileSizeInBytes,

                                // note: .. could result if the filextension already has a period.
                                FileName = $"{id}.{csFile.FileID}.0.{csFile.FileExtension}".Replace("..", "."),
                                FileBytes = csFile.FileBits,
                            },
                        });
                }).ToArray();

                result = new DocumentResult
                {
                    DocumentId = id,
                    FileCount = rxr.Length,
                    Files = rxr,
                };
                client.Close();
                return result;
            }
        }

        /// <summary>
        /// Attempts to update an iLinx document.
        /// </summary>
        /// <param name="documentId">Id of the document.</param>
        /// <param name="files">Files to update.</param>
        /// <returns>Result of update.</returns>
        public InsertResponse UpdateDocument(Guid documentId, ReceivedFileInfo[] files)
        {
            try
            {
                if (files.Length < 1)
                {
                    throw new NeedFileException();
                }

                var securityToken = this.contentStore.GetSecurityToken();
                using (var client = this.contentStore.GetContentStoreClient())
                {
                    ContentStoreDocument documentResponse = client.RetrieveDocument(this.config.UserName, securityToken, documentId, this.config.ApplicationName, out DiscussionMessage[] messages);

                    if (documentResponse == null)
                    {
                        throw new FileDoesNotExistException();
                    }

                    if (documentResponse.Files.Count() < 1)
                    {
                        throw new FilesDoNotMatchException(documentResponse.Files.Count(), files.Count());
                    }

                    ContentStoreFile toUpdate = documentResponse.Files[0];

                    var incomingIsTiff = files.First().FileExtension.ToLower().Contains("tif");

                    if (!incomingIsTiff && toUpdate.FileExtension.ToLower().Contains("tif"))
                    {
                        var withParts = files.Select(f =>
                        {
                            var parts = f.FileName.Split('.');
                            var fileId = parts[1];
                            var partNum = parts[2];
                            return (fileId, partNum, f);
                        });

                        var grouped = (from wp in withParts
                                       group wp by wp.fileId into g
                                       select (
                                           fileId: g.Key,
                                           files: g.OrderBy(h => h.fileId).ToArray())).ToList();

                        if (grouped.Count() != documentResponse.Files.Length)
                        {
                            throw new FilesDoNotMatchException(grouped.Count(), documentResponse.Files.Length);
                        }

                        var tiffs = grouped.Select(abc => abc.files.Select(cde => cde.f.FileBits).ImagesToTiff()).ToArray();

                        for (int i = 0; i < documentResponse.Files.Length; i++)
                        {
                            var file = files[i];
                            ContentStoreFile thisOne = documentResponse.Files[i];
                            thisOne.FileBits = tiffs[i];
                            thisOne.FileExtension = ".tif";
                            thisOne.FileSizeInBytes = tiffs[i].Length;
                        }
                    }
                    else
                    {
                        int filesLength = files.Length;
                        if (filesLength != documentResponse.Files.Length)
                        {
                            throw new FilesDoNotMatchException(documentResponse.Files.Count(), files.Count());
                        }

                        for (int i = 0; i < filesLength; i++)
                        {
                            var file = files[i];
                            ContentStoreFile thisOne = documentResponse.Files[i];
                            thisOne.FileBits = file.FileBits;
                            thisOne.FileExtension = file.FileExtension;
                            thisOne.FileSizeInBytes = file.FileBits.Length;
                        }
                    }

                    client.UpdateDocument(this.config.UserName, securityToken, documentResponse, new Guid[] { }, new DiscussionMessage[] { });
                    return new InsertResponse
                    {
                        AssignedId = documentId.ToString(),
                        Error = false,
                        ErrorMessage = string.Empty,
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new InsertResponse
                {
                    AssignedId = default,
                    Error = true,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// Attempts to insert a new document.
        /// </summary>
        /// <param name="accountNumber">Account number.</param>
        /// <param name="rollYear">Roll Year.</param>
        /// <param name="docType">Document type.</param>
        /// <param name="recId">RecId: note we don't know what rec stands for and is not documented.</param>
        /// <param name="files">Files to save.</param>
        /// <returns>Result of insertion.</returns>
        public InsertResponse SaveDocument(string accountNumber, string rollYear, string docType, string recId, ReceivedFileInfo[] files)
        {
            Guid newId = default;
            try
            {
                if (files.Length < 1)
                {
                    throw new NeedFileException();
                }

                var md =
                     new[]
                    {
                        this.GetMetaData(this.config, files.Length, accountNumber, rollYear, docType, recId, newId),
                    };
                ContentStoreDocument newDocument = new ContentStoreDocument
                {
                    RepositoryName = this.config.ApplicationName,
                    DocumentID = newId,
                    Files = files.Select(f =>
                    {
                        Guid guid = Guid.NewGuid();
                        return new ContentStoreFile
                        {
                            DocumentID = guid,
                            FileBits = f.FileBits,
                            FileExtension = f.FileExtension,
                            FileID = guid,
                        };
                    }).ToArray(),
                    MetaData = md,
                };

                var initialMessages = this.GetInitialMessages(newId);
                Guid response;
                using (ContentStoreContractClient client = this.contentStore.GetContentStoreClient())
                {
                    string securityToken = this.contentStore.GetSecurityToken();
                    response = client
                      .InsertDocument(
                          this.config.UserName,
                          securityToken,
                          newDocument,
                          initialMessages);

                    client.Close();
                }

                return new InsertResponse
                {
                    AssignedId = response.ToString(),
                    Error = false,
                    ErrorMessage = string.Empty,
                };
            }
            catch (System.Exception ex)
            {
                return new InsertResponse
                {
                    AssignedId = default(Guid).ToString(),
                    Error = true,
                    ErrorMessage = $"{ex.Message} {ex.InnerException?.Message}",
                };
            }
        }

        private DiscussionMessage[] GetInitialMessages(Guid newId) =>
          new[]
            {
            new DiscussionMessage
            {
              Message = "This document was added by PTASILinxConnector web app.",
              DocumentID = newId,
            },
            };

        private ContentStoreDocumentMetaData GetMetaData(
          IConfigParams config,
          int pages,
          string accountNumber,
          string rollYear,
          string docType,
          string recId,
          Guid newId)
        {
            return new ContentStoreDocumentMetaData
            {
                RepositoryName = config.ApplicationName,
                DocumentID = newId,
                FileID = newId,
                IndexInfo = new Dictionary<string, IndexField>()
        {
          { AccountNumber, new IndexField { FieldName = AccountNumber, FieldValue = accountNumber } },
          { RollSpaceYear, new IndexField { FieldName = RollYear, FieldValue = rollYear } },
          { DocType, new IndexField { FieldName = DocType, FieldValue = docType } },
          { DocumentDate, new IndexField { FieldName = DocumentDate, FieldValue = DateTime.Now.ToShortDateString() } },
          { UserId, new IndexField { FieldName = UserId, FieldValue = config.UserName } },
          { ScannerId, new IndexField { FieldName = ScannerId, FieldValue = string.Empty } },
          { RecId, new IndexField { FieldName = RecId, FieldValue = recId } },
          { PageSpaceCount, new IndexField { FieldName = PageSpaceCount, FieldValue = "2" } },
          { ScanTime, new IndexField { FieldName = ScanTime, FieldValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } },
        },
            };
        }
    }
}
