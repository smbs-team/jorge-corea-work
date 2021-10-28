// <copyright file="SignHtmlController.cs" company="King County">
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

    using Newtonsoft.Json;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using static DocuSign.eSign.Client.Auth.OAuth.UserInfo;

    /// <summary>
    /// Send an html to docusign to sign.
    /// </summary>
    public class SignHtmlController : ApiController
    {
        private const string Email = "email";
        private const string FullName = "fullName";
        private const string HtmlContent = "htmlContent";
        private const string ParamRequiredMessage = "Param is required";
        private const string ReturnUrl = "returnUrl";
        private const string SignerClientId = "signerClientId";
        private const int TOKENREPLACEMENTINSECONDS = 10 * 60;
        private static ApiClient apiClient = null;
        private static DateTime expires = DateTime.Now;
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignHtmlController"/> class.
        /// </summary>
        /// <param name="config">Initial system configuration.</param>
        public SignHtmlController(IConfigParams config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            apiClient = apiClient ?? new ApiClient(this.config.DocuSignApiUrl);
        }

        private static string AccessToken { get; set; }

        /// <summary>
        /// Post an html to be signed.
        /// </summary>
        /// <returns>Object with result.</returns>
        public async Task<object> PostAsync()
        {
            var requestContent = await this.GetRequestContent();
            if (string.IsNullOrEmpty(requestContent))
            {
                throw new ArgumentException(ParamRequiredMessage, "Content");
            }

            Dictionary<string, string> form;
            try
            {
                form = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestContent);
            }
            catch (JsonReaderException ex)
            {
                throw new Classes.Exceptions.BadJsonException(requestContent, ex);
            }

            if (!form.TryGetValue(FullName, out string fullName))
            {
                throw new ArgumentException(ParamRequiredMessage, FullName);
            }

            if (!form.TryGetValue(Email, out string email))
            {
                throw new ArgumentException(ParamRequiredMessage, Email);
            }

            if (!form.TryGetValue(ReturnUrl, out string returnUrl))
            {
                throw new ArgumentException(ParamRequiredMessage, ReturnUrl);
            }

            if (!form.TryGetValue(SignerClientId, out string signerClientId))
            {
                throw new ArgumentException(ParamRequiredMessage, SignerClientId);
            }

            if (!form.TryGetValue(HtmlContent, out string html))
            {
                throw new ArgumentException(ParamRequiredMessage, HtmlContent);
            }

            //// html = html ?? this.LoadFile();
            ////html = Controllers.HtmlContent.html.Replace("'", "\"") ?? html;
            this.CheckToken();
            var apiCallResult = this.SendHtmlToSign(fullName, email, returnUrl, signerClientId, html);
            return new
            {
                RedirectUrl = apiCallResult.Url,
            };
        }

        /// <summary>
        /// Get form values.
        /// </summary>
        /// <returns>Name value collection with form results.</returns>
        protected virtual NameValueCollection GetForm() => HttpContext.Current.Request.Form;

        /// <summary>
        /// Get content from request.
        /// </summary>
        /// <returns>Content retrieved from http request.</returns>
        protected virtual async Task<string> GetRequestContent()
          => await this.Request.Content.ReadAsStringAsync();

        /// <summary>
        /// Send document to DocuSign for signing and return URL to client.
        /// </summary>
        /// <param name="fullName">Full name of the user.</param>
        /// <param name="email">Email address of the user.</param>
        /// <param name="returnUrl">Url DocuSign returns to.</param>
        /// <param name="signerClientId">DocuSign Client Id.</param>
        /// <param name="html">Html to send to DocuSign.</param>
        /// <returns>The url to return to.</returns>
        protected virtual ViewUrl SendHtmlToSign(string fullName, string email, string returnUrl, string signerClientId, string html)
        {
            var envelope = this.MakeEnvelope(email, fullName, signerClientId, html);
            var docusignConfig = new Configuration(apiClient);
            docusignConfig.AddDefaultHeader("Authorization", $"Bearer {AccessToken}");
            EnvelopesApi envelopesApi = new EnvelopesApi(docusignConfig);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(this.config.DocuSignAccountId, envelope);
            string envelopeId = results.EnvelopeId;

            string returnUrl1 = returnUrl.AddParametersToUrl("envelopeId", envelopeId);
            RecipientViewRequest viewRequest = this.MakeRecipientViewRequest(email, fullName, returnUrl1, signerClientId);

            // call the CreateRecipientView API
            ViewUrl apiCallResult = envelopesApi.CreateRecipientView(this.config.DocuSignAccountId, envelopeId, viewRequest);
            return apiCallResult;
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

        private string LoadFile()
        {
            var fileName = "~/templates/senior-application.html";
            string path = HttpContext.Current.Server.MapPath(fileName);
            return File.ReadAllText(path);
        }

        private EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string signerClientId, string html)
        {
            SignHere signHere = new SignHere
            {
                AnchorString = "/sn1/",
                AnchorUnits = "pixels",
                AnchorXOffset = "10",
                AnchorYOffset = "20",
                DocumentId = "1",
                PageNumber = "1",
                Name = "SignHere",
                TabLabel = "Please Sign Here",
            };
            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = "1",
                RoleName = "Signer1",
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
                Documents = new List<Document>
        {
          new Document
          {
            HtmlDefinition = new DocumentHtmlDefinition(Source: html),
            Name = "SignDocument",
            FileExtension = "HTML",
            DocumentId = "1",
          },
        },
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

        private void UpdateToken()
        {
            // Un-escape web config escaped values.
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
    }
}