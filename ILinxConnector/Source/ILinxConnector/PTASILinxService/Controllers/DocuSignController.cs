// <copyright file="DocuSignController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using DocuSign.eSign.Api;
    using DocuSign.eSign.Client;
    using DocuSign.eSign.Client.Auth;
    using DocuSign.eSign.Model;

    using Microsoft.WindowsAzure.Storage.Blob;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using static DocuSign.eSign.Client.Auth.OAuth.UserInfo;

    /// <summary>
    /// Docusign management controller.
    /// Serves as an intermediary between the dynamics app and docusign.
    /// </summary>
    public class DocuSignController : ApiController
    {
        private const string Email = "email";
        private const string FullName = "fullName";
        private const string NoFilesMessage = "Request must have at least one file.";
        private const string ParamRequiredMessage = "Param is required";
        private const string ReturnUrl = "returnUrl";
        private const string SignerClientId = "signerClientId";
        private const int TOKENREPLACEMENTINSECONDS = 10 * 60;
        private static ApiClient apiClient = null;

        private static DateTime expires = DateTime.Now;
        private readonly ICloudStorageProvider cloudProvider;
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocuSignController"/> class.
        /// </summary>
        /// <param name="config">Initial system configuration.</param>
        /// <param name="cloudProvider">Inyected cloud provider.</param>
        public DocuSignController(IConfigParams config, ICloudStorageProvider cloudProvider)
        {
            this.config = config;
            this.cloudProvider = cloudProvider ?? throw new ArgumentNullException(nameof(cloudProvider));
            apiClient = apiClient ?? this.CreateApiClient();
        }

        private static string AccessToken { get; set; }

        /// <summary>
        /// Patch finalized the request.
        /// </summary>
        /// <param name="envelopeId">envelopeId received from client.</param>
        /// <param name="seniorApplicationId">Signer Id.</param>
        /// <param name="section">Section to save to.</param>
        /// <param name="document">Document to save to.</param>
        /// <returns>Requester does not wait for response, so returns OK.</returns>
        public async Task<object> Patch(
          string envelopeId,
          string seniorApplicationId,
          string section,
          string document)
        {
            envelopeId = envelopeId ?? throw new ArgumentException(ParamRequiredMessage, nameof(envelopeId));
            seniorApplicationId = seniorApplicationId ?? throw new ArgumentNullException(nameof(seniorApplicationId));
            section = section ?? throw new ArgumentNullException(nameof(section));
            document = document ?? throw new ArgumentNullException(nameof(document));

            this.CheckToken();

            var config = new Configuration(apiClient);
            config.AddDefaultHeader("Authorization", $"Bearer {AccessToken}");
            EnvelopesApi envelopesApi = new EnvelopesApi(config);
            Stream docStream = await envelopesApi.GetDocumentAsync(this.config.DocuSignAccountId, envelopeId, "combined", null);

            ////Envelope envelopeInfo = await envelopesApi.GetEnvelopeAsync(this.config.DocuSignAccountId, envelopeId);

            CloudBlobContainer container = this.GetStorageContainer();

            var files = docStream.GetPDFFiles(this.config.SautinLicense);
            document = !string.IsNullOrEmpty(document) ? $"{document}/" : string.Empty;
            IEnumerable<Task<bool>> allTasks = files.Images.ToList().Select(async (itm, idx) =>
            {
                var blobRef = container.GetBlockBlobReference($"{seniorApplicationId}/{section}/{document}img-{idx + 1}.png");
                try
                {
                    await blobRef.UploadFromStreamAsync(new MemoryStream(itm));
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            var results = await Task.WhenAll(allTasks.ToArray());
            var errored = results.Count(r => !r);
            return new
            {
                error = errored > 0,
                errorMessage = "Some files could not be moved to blob.",
            };
        }

        /// <summary>
        /// Post info.
        /// </summary>
        /// <remarks>
        /// Post parameters:
        /// 1. files array
        /// 2. fullName: Name and last name of user.
        /// 3. email: email address of user.
        /// 4. returnUrl: url to return to after operation.
        /// 5. signerClientId: unique identifier of signer.
        /// </remarks>
        /// <returns>Result of post as json.</returns>
        public DocusignResponse Post()
        {
            try
            {
                // Get values from body
                HttpFileCollectionBase requestFiles = this.GetFiles();
                NameValueCollection form = this.GetForm();

                // Parameter checking
                if (requestFiles.Count == 0)
                {
                    throw new ArgumentException(NoFilesMessage, "files");
                }

                var fullName = form[FullName] ?? throw new ArgumentException(ParamRequiredMessage, FullName);
                var email = form[Email] ?? throw new ArgumentException(ParamRequiredMessage, Email);
                var returnUrl = form[ReturnUrl] ?? throw new ArgumentException(ParamRequiredMessage, ReturnUrl);
                var signerClientId = form[SignerClientId] ?? throw new ArgumentException(ParamRequiredMessage, SignerClientId);

                this.CheckToken();

                FileDefinition[] files = this.GetFilesFrom(requestFiles).ToArray();

                ViewUrl apiCallResult = this.SendDocsToSign(fullName, email, returnUrl, signerClientId, files);
                return new DocusignResponse
                {
                    RedirectUrl = apiCallResult.Url,
                };
            }
            catch (ApiException ex)
            {
                return new DocusignResponse
                {
                    Error = true,
                    ErrorMessage = "API ERROR: " + ex.Message,
                };
            }
            catch (ArgumentException ex)
            {
                return new DocusignResponse
                {
                    Error = true,
                    ErrorMessage = $"Argument exception. {ex.ParamName} must have a value.",
                };
            }
        }

        /// <summary>
        /// Create API client.
        /// </summary>
        /// <returns>Created API client.</returns>
        protected virtual ApiClient CreateApiClient()
        {
            return new ApiClient(this.config.DocuSignApiUrl);
        }

        /// <summary>
        /// Retrieve files from the request context.
        /// </summary>
        /// <returns>List of files.</returns>
        protected virtual HttpFileCollectionBase GetFiles() => new HttpFileCollectionWrapper(HttpContext.Current.Request.Files);

        /// <summary>
        /// Get form values.
        /// </summary>
        /// <returns>Name value collection with form results.</returns>
        protected virtual NameValueCollection GetForm() => HttpContext.Current.Request.Form;

        /// <summary>
        /// Get a storage container for further processing.
        /// </summary>
        /// <returns>A created storage container from config.</returns>
        protected virtual CloudBlobContainer GetStorageContainer() => this.cloudProvider.GetCloudBlobContainer(this.config.BlobStorageContainer);

        private static byte[] ToBytes(HttpPostedFileBase item)
        {
            var m = new MemoryStream();
            item.InputStream.CopyTo(m);
            return m.ToArray();
        }

        private void CheckToken()
        {
            if (AccessToken == null
                || (DateTime.Now > expires))
            {
                this.UpdateToken();
            }
        }

        private Account GetAccountInfo(OAuth.OAuthToken authToken)
        {
            apiClient.SetOAuthBasePath(this.config.DocuSignAuthServer);
            OAuth.UserInfo userInfo = apiClient.GetUserInfo(authToken.access_token);
            Account acct = null;

            var accounts = userInfo.Accounts;

            acct = accounts.FirstOrDefault(a => a.IsDefault == "true");

            return acct;
        }

        private IEnumerable<FileDefinition> GetFilesFrom(HttpFileCollectionBase files)
        {
            foreach (var key in files.AllKeys)
            {
                yield return new FileDefinition
                {
                    Bytes = ToBytes(files[key]),
                    FileName = files[key].FileName,
                };
            }
        }

        private EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string signerClientId, FileDefinition[] files)
        {
            SignHere signHere = new SignHere
            {
                AnchorString = "/sn1/",
                AnchorUnits = "pixels",
                AnchorXOffset = "10",
                AnchorYOffset = "20",
                DocumentId = "1",
                PageNumber = "1",
            };
            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = "1",
                Tabs = new Tabs
                {
                    SignHereTabs = new List<SignHere> { signHere, },
                },
            };
            EventNotification notification = new EventNotification
            {
                Url = this.Url.Link("DefaultApi", new { controller = "DocuSignWebhook" }),
            };
            return new EnvelopeDefinition
            {
                EmailSubject = "Please sign this document",
                Status = "sent",
                EventNotifications = new List<EventNotification> { notification },
                Documents = files.Select((f, i)
                  => new Document
                  {
                      DocumentBase64 = Convert.ToBase64String(f.Bytes),
                      Name = f.FileName,
                      FileExtension = Path.GetExtension(f.FileName),
                      DocumentId = $"{i + 1}",
                  }).ToList(),
                Recipients = new Recipients
                {
                    Signers = new List<Signer> { signer, },
                },
            };
        }

        private RecipientViewRequest MakeRecipientViewRequest(string signerEmail, string signerName, string returnUrl, string signerClientId)
          => new RecipientViewRequest
          {
              ReturnUrl = returnUrl,
              AuthenticationMethod = "none",
              Email = signerEmail,
              UserName = signerName,
              ClientUserId = signerClientId,
          };

        private void SaveTransactionForClient(string signerClientId, string envelopeId, FileDefinition[] files)
        {
            // TODO: do we want to save a reference to this transaction for later review?
        }

        private ViewUrl SendDocsToSign(string fullName, string email, string returnUrl, string signerClientId, FileDefinition[] files)
        {
            // Create envelope
            var envelope = this.MakeEnvelope(email, fullName, signerClientId, files);

            // Create config
            var docusignConfig = new Configuration(apiClient);

            docusignConfig.AddDefaultHeader("Authorization", $"Bearer {AccessToken}");

            EnvelopesApi envelopesApi = new EnvelopesApi(docusignConfig);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(this.config.DocuSignAccountId, envelope);
            string envelopeId = results.EnvelopeId;

            string returnUrl1 = returnUrl.AddParametersToUrl("envelopeId", envelopeId);
            RecipientViewRequest viewRequest = this.MakeRecipientViewRequest(email, fullName, returnUrl1, signerClientId);

            // call the CreateRecipientView API
            ViewUrl apiCallResult = envelopesApi.CreateRecipientView(this.config.DocuSignAccountId, envelopeId, viewRequest);
            this.SaveTransactionForClient(signerClientId, envelopeId, files);
            return apiCallResult;
        }

        private void UpdateToken()
        {
            string unescapedKey = this.config.DocusignPrivateKey.Replace("\\r", "\r").Replace("\\n", "\n");
            var authServer = this.config.DocuSignAuthServer;

            OAuth.OAuthToken authToken = apiClient.RequestJWTUserToken(
              this.config.DocuSignIntegratorId,
              this.config.DocuSignImpersonatedUserId,
              authServer,
              Encoding.UTF8.GetBytes(unescapedKey),
              1);
            AccessToken = authToken.access_token;
            var account = this.GetAccountInfo(authToken);
            apiClient = new ApiClient(account.BaseUri + "/restapi");
            expires = DateTime.Now.AddSeconds(TOKENREPLACEMENTINSECONDS);
        }

        private class FileDefinition
        {
            public byte[] Bytes { get; set; }

            public string FileName { get; set; }
        }
    }
}