// <copyright file="QRController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Newtonsoft.Json;

    using PTASIlinxService.Classes;

    using QRCoder;

    /// <summary>
    /// Controller to generate QRCodes.
    /// </summary>
    public class QRController : ApiController
    {
        /// <summary>
        /// Http post action.
        /// </summary>
        /// <returns>Generated qr code.</returns>
        public async Task<QRCodeResult> PostAsync()
        {
            var requestContent = await this.GetRequestContent();
            if (!string.IsNullOrEmpty(requestContent))
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestContent);
                if (data.Count() > 0)
                {
                    var content = string.Join(";", data.Select(itm => $"{itm.Key}={itm.Value}"));
                    return this.CreateQRCode(content);
                }
            }

            return new QRCodeResult { FileBytes = new byte[] { } };
        }

        /// <summary>
        /// Get content from request.
        /// </summary>
        /// <returns>Content retrieved from http request.</returns>
        protected virtual async Task<string> GetRequestContent()
          => await this.Request.Content.ReadAsStringAsync();

        /// <summary>
        /// Creates a QR Code based on the content.
        /// </summary>
        /// <param name="content">String content to convert to QR.</param>
        /// <returns>QRCode bytes.</returns>
        protected virtual QRCodeResult CreateQRCode(string content)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(4, Color.Black, Color.White, false);
                    return new QRCodeResult
                    {
                        FileBytes = qrCodeImage.ToByteArray(),
                    };
                }
            }
        }
    }
}