//-----------------------------------------------------------------------
// <copyright file="IdentityController.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Controllers
{
    using System;
    using System.Net;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PTAS.MagicLinkService.CryptoHelper;
    using PTAS.MagicLinkService.Models;
    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Controller that emits b2c tokens hints when presented with a QR token.  (QR tokens are validated against last login).
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class QrTokenController : IdentityControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QrTokenController" /> class.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="logger">The logger.</param>
        public QrTokenController(IOptions<AppSettingsModel> appSettings, IHostingEnvironment hostingEnvironment, ILogger<IdentityController> logger)
            : base(appSettings, hostingEnvironment, logger)
        {
        }

        /// <summary>
        /// Post method for the controller.  Handles the sending of the email with the magic link.
        /// </summary>
        /// <param name="userEmail">The email of the user.</param>
        /// <param name="qrToken">The qr token.</param>
        /// <returns>
        /// A token hint.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery]string userEmail, [FromQuery]string qrToken)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Email null or empty.", HttpStatusCode.BadRequest));
            }

            if (string.IsNullOrEmpty(qrToken))
            {
                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("QRToken null or empty.", HttpStatusCode.BadRequest));
            }

            try
            {
                if (await this.ValidateQrToken(userEmail, qrToken))
                {
                    string token = this.BuildIdToken(userEmail);
                    return this.Ok(token);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.Forbidden, new B2CResponseModel("Qr token not valid.", HttpStatusCode.Forbidden));
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("QR Token Exchange Exception: " + ex.ToString());

                return this.StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("An unexpected error occurred. ", HttpStatusCode.Conflict));
            }
        }

        /// <summary>
        /// Builds the qr token.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="qrToken">The qr token.</param>
        /// <returns>
        /// True if the presented token is valid.
        /// </returns>
        private async Task<bool> ValidateQrToken(string email, string qrToken)
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

            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

            string plaintext = cryptoHelper.Decrypt(qrToken, key, iv);
            string[] tokens = plaintext.Split('/');
            if (tokens.Length != 3)
            {
                return false;
            }

            // Expiration validation.
            int tokenDuration = 0;
            bool isValidTokenDuration = int.TryParse(tokens[1], out tokenDuration);

            long emittedTimeTicks = 0;
            bool isValidEmittedTime = long.TryParse(tokens[2], out emittedTimeTicks);

            Guid qrGuid;
            bool isValidQrGuid = Guid.TryParse(tokens[0], out qrGuid);

            if (!isValidTokenDuration || !isValidEmittedTime || !isValidQrGuid)
            {
                return false;
            }

            System.DateTime emittedTime = new System.DateTime(emittedTimeTicks);
            if (emittedTime + new TimeSpan(0, 0, tokenDuration) < System.DateTime.Now)
            {
                return false;
            }

            ITableStorageProvider storageProvider = this.GetTableStorageProvider();

            string qrGuidString = qrGuid.ToString();
            QrTokenEvidenceEntity evidence = await storageProvider.GetEntityAsync<QrTokenEvidenceEntity>(
                QrTokenEvidenceEntity.QrTokenEvidencePartitionKey,
                qrGuidString,
                QrTokenEvidenceEntity.TableName);

            // Email comparison.
            if (string.Compare(email, evidence.Email) != 0)
            {
                return false;
            }

            return true;
        }
    }
}
