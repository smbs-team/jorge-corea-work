//-----------------------------------------------------------------------
// <copyright file="IdentityController.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage.Table;
    using PTAS.MagicLinkService.CryptoHelper;
    using PTAS.MagicLinkService.Models;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Controller that builds an authenticator (magic) link and sends it by email to the end user.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : IdentityControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityController" /> class.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="logger">The logger.</param>
        public IdentityController(IOptions<AppSettingsModel> appSettings, IHostingEnvironment hostingEnvironment, ILogger<IdentityController> logger)
            : base(appSettings, hostingEnvironment, logger)
        {
        }

        /// <summary>
        /// Post method for the controller.  Handles the sending of the email with the magic link.
        /// </summary>
        /// <param name="inputClaims">The input claims.</param>
        /// <returns>
        /// The status code depending on the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]InputClaimsModel inputClaims)
        {
            if (inputClaims == null)
            {
                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            // Check input email address.
            if (string.IsNullOrEmpty(inputClaims.Email))
            {
                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Email address is empty", HttpStatusCode.Conflict));
            }

            try
            {
                Guid qrGuid = Guid.NewGuid();
                string token = this.BuildIdToken(inputClaims.Email);
                string qrToken = this.BuildQrToken(qrGuid);
                string link = this.BuildUrl(token, qrToken, inputClaims.RedirectUrlOverride);
                string body = string.Empty;
                string htmlTemplate = System.IO.File.ReadAllText(Path.Combine(this.HostingEnvironment.ContentRootPath, "Template.html"));

                await this.SaveQrEvidence(qrGuid, inputClaims.Email);
                await this.SendEmail(inputClaims, token, link, body, htmlTemplate);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.Logger.LogError("Magic Link Identity Exception: " + ex.ToString());
                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("An unexpected error occurred. ", HttpStatusCode.Conflict));
            }
        }

        /// <summary>
        /// Builds the qr token.
        /// </summary>
        /// <param name="qrGuid">The guid that identifies the qr token.</param>
        /// <returns>The id token hint.</returns>
        protected string BuildQrToken(System.Guid qrGuid)
        {
            int ivLength = 16;
            int keyLength = 32;

            byte[] rsaPrivateKey = Convert.FromBase64String(this.AppSettings.QrTokenSigningKey);
            int ivStep = rsaPrivateKey.Length / ivLength;
            int keyStep = rsaPrivateKey.Length / keyLength;

            byte[] key = new byte[keyLength];
            byte[] iv = new byte[ivLength];

            for (int i = 0; i < ivLength; i++)
            {
                iv[i] = rsaPrivateKey[(i * ivStep) + 1];
            }

            for (int i = 0; i < keyLength; i++)
            {
                key[i] = rsaPrivateKey[i * keyStep];
            }

            AesCtrCryptoHelper cryptoHelper = new AesCtrCryptoHelper();

            string qrToken = $"{qrGuid.ToString()}/{this.AppSettings.QrTokenDurationInSeconds}/{System.DateTime.Now.Ticks}";

            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

            string cipher = cryptoHelper.Encrypt(qrToken, key, iv);

            return cipher;
        }

        /// <summary>
        /// Builds the URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="qrToken">The qr token.</param>
        /// <param name="redirectUrl">The redirect URL override coming from b2c.</param>
        /// <returns>
        /// B2c Url.
        /// </returns>
        protected string BuildUrl(string token, string qrToken, string redirectUrl)
        {
            string nonce = Guid.NewGuid().ToString("n");

            return string.Format(
                this.AppSettings.B2CSignUpUrl,
                this.AppSettings.B2CTenant,
                this.AppSettings.B2CPolicy,
                this.AppSettings.B2CClientId,
                this.AppSettings.Scope,
                Uri.EscapeDataString(redirectUrl ?? this.AppSettings.B2CRedirectUri),
                nonce,
                qrToken) + "&id_token_hint=" + token;
        }

        private async Task SendEmail(InputClaimsModel inputClaims, string token, string link, string body, string htmlTemplate)
        {
            Guid correlationID = Guid.NewGuid();
            AzureTokenProvider azureTokenProvider = new AzureTokenProvider();

            string notificationServiceToken = await azureTokenProvider.GetKcServiceAccessTokenAsync(
                this.AppSettings.ApimEndpoint,
                this.AppSettings.NotificatioServiceResourceId,
                this.AppSettings.ApimSubscriptionKey,
                this.AppSettings.MagicLinkServiceClientId,
                this.AppSettings.MagicLinkServiceClientSecret);

            var clientNotify = new HttpClient();

            // Request headers
            // Add unique correlation id
            clientNotify.DefaultRequestHeaders.Add("kc-correlation-id", correlationID.ToString());
            clientNotify.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.AppSettings.ApimSubscriptionKey);
            clientNotify.DefaultRequestHeaders.Add("Authorization", "Bearer " + notificationServiceToken);

            Uri apimUri = new Uri(this.AppSettings.ApimEndpoint);
            Uri tokenServiceUri = new Uri(apimUri, "/notifications/v1/email");

            HttpResponseMessage response;

            // Request body
            string emailBody = htmlTemplate.Replace("[0]", inputClaims.Email).Replace("[1]", link);

            // Some email body pre-processing to avoid breaking json later..
            emailBody = emailBody.Replace("\"", "\\\"");
            emailBody = emailBody.Replace("\n", string.Empty);

            string timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffff");
            var finalemailBody = "{\"notificationEvent\": {\"id\": \"5d883c77-831d-46e2-a91e-907e795518a2\",\"timestamp\": \"" + timeStamp + "\",\"type\": \"email\",\"notification\": {\"attachments\": [ ],\"bccList\": [], \"body\": \"" + emailBody + "\", \"ccList\": [], \"from\": \"" + this.AppSettings.SMTPFromAddress + " \", \"fromDisplayName\": \"" + this.AppSettings.FromDisplayName + "\", \"importance\": \"high\", \"isHtml\": true, \"replyToList\": [], \"subject\": \"" + this.AppSettings.SMTPSubject + "\", \"toList\": [\"" + inputClaims.Email + "\"], \"undeliverableEmail\": \"" + this.AppSettings.UndeliverableEmailRecipient + "\" }} }";
            byte[] byteData = Encoding.UTF8.GetBytes(finalemailBody);

            using (var content = new ByteArrayContent(byteData))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await clientNotify.PostAsync(tokenServiceUri.ToString(), content);
                if (!response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    throw new Exception("Error sending authentication email: " + responseString);
                }
            }
        }

        /// <summary>
        /// Saves the qr evidence for later check during qr token validation.
        /// </summary>
        /// <param name="qrGuid">The qr unique identifier.</param>
        /// <param name="email">The email.</param>
        private async Task SaveQrEvidence(System.Guid qrGuid, string email)
        {
            ITableStorageProvider storageProvider = this.GetTableStorageProvider();
            await storageProvider.EnsureTableExists(QrTokenEvidenceEntity.TableName);

            QrTokenEvidenceEntity evidence = new QrTokenEvidenceEntity
            {
                PartitionKey = QrTokenEvidenceEntity.QrTokenEvidencePartitionKey,
                RowKey = qrGuid.ToString(),
                Email = email
            };

            await storageProvider.InsertAsync<QrTokenEvidenceEntity>(evidence, QrTokenEvidenceEntity.TableName);
        }
    }
}
