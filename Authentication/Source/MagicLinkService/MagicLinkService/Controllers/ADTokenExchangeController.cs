//-----------------------------------------------------------------------
// <copyright file="ADTokenExchangeController.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;
    using PTAS.MagicLinkService.Models;

    /// <summary>
    /// Controller that emits b2c tokens hints when presented with a QR token.  (QR tokens are validated against last login).
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class ADTokenExchangeController : IdentityController
    {
        /// <summary>
        /// The HTTP message handler.  Used for unit testing purposes.
        /// </summary>
        private HttpClientHandler httpMessageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ADTokenExchangeController" /> class.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="logger">The logger.</param>
        public ADTokenExchangeController(IOptions<AppSettingsModel> appSettings, IHostingEnvironment hostingEnvironment, ILogger<IdentityController> logger)
            : base(appSettings, hostingEnvironment, logger)
        {
        }

        /// <summary>
        /// Get method for the controller.  Exchanges an AD Token sent as Authorization for B2C Id Token with the user UPN as email.
        /// </summary>
        /// <returns>The Id Token for the AD User.</returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            string authHeader = this.Request.Headers["Authorization"];
            string[] authHeaderParts = authHeader?.Split(" ");
            if (string.IsNullOrWhiteSpace(authHeader) || authHeaderParts == null || authHeaderParts.Length != 2 || authHeaderParts[0] != "Bearer")
            {
                return this.StatusCode(
                    (int)HttpStatusCode.Unauthorized,
                    new B2CResponseModel("No authorization token.", HttpStatusCode.Unauthorized));
            }

            try
            {
                string userEmail = await this.ValidateJWT(authHeaderParts);
                if (!string.IsNullOrWhiteSpace(userEmail))
                {
                    var bearerToken = await this.GetBearerToken(userEmail);
                    if (!string.IsNullOrEmpty(bearerToken))
                    {
                        return this.Ok(bearerToken);
                    }
                    else
                    {
                        return this.StatusCode(
                        (int)HttpStatusCode.Conflict,
                        new B2CResponseModel("AD Token was valid but could not obtain bearer from B2C. ", HttpStatusCode.Conflict));
                    }
                }
                else
                {
                    return this.StatusCode(
                           (int)HttpStatusCode.Unauthorized,
                           new B2CResponseModel("Invalid authorization token.", HttpStatusCode.Unauthorized));
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("AD Token Exchange Exception: " + ex.ToString());

                return this.StatusCode(
                    (int)HttpStatusCode.Conflict,
                    new B2CResponseModel("An unexpected error occurred. ", HttpStatusCode.Conflict));
            }
        }

        /// <summary>
        /// Gets the bearer token from B2C.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <returns>A Bearer Token.</returns>
        private async Task<string> GetBearerToken(string userEmail)
        {
            string token = this.BuildIdToken(userEmail);
            string qrToken = string.Empty;
            string link = this.BuildUrl(token, qrToken, null);

            var httpMessageHandler = this.httpMessageHandler ?? new HttpClientHandler();

            try
            {
                using (HttpClient client = new HttpClient(httpMessageHandler))
                using (HttpResponseMessage response = await client.GetAsync(link))
                using (HttpContent content = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var redirectQueryString = response.RequestMessage.RequestUri.Query.Remove(0, 1);
                        var parameters = redirectQueryString.Split('&');
                        var bearerToken = string.Empty;
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var keypair = parameters[i].Split('=');
                            if (keypair[0] == "access_token")
                            {
                                return keypair[1];
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException requestException)
            {
                // Logging needed here.
                return null;
            }

            return null;
        }

        /// <summary>
        /// Validates the JWT.
        /// </summary>
        /// <param name="authHeaderParts">The authentication header parts.</param>
        /// <returns>
        /// The user email if the token is valid.
        /// </returns>
        private async Task<string> ValidateJWT(string[] authHeaderParts)
        {
            ISecurityTokenValidator tokenValidator;
            tokenValidator = new JwtSecurityTokenHandler();
            string aadInstance = $"https://login.microsoftonline.com/{this.AppSettings.KCTenant}/v2.0";

            IList<string> validissuers = new List<string>()
            {
                $"https://login.microsoftonline.com/{this.AppSettings.KCTenantId}/",
                $"https://login.microsoftonline.com/{this.AppSettings.KCTenant}/",
                $"https://login.microsoftonline.com/{this.AppSettings.KCTenantId}/v2.0",
                $"https://login.microsoftonline.com/{this.AppSettings.KCTenant}/v2.0",
                $"https://login.windows.net/{this.AppSettings.KCTenantId}/",
                $"https://login.windows.net/{this.AppSettings.KCTenant}/",
                $"https://login.microsoft.com/{this.AppSettings.KCTenantId}/",
                $"https://login.microsoft.com/{this.AppSettings.KCTenant}/",
                $"https://sts.windows.net/{this.AppSettings.KCTenantId}/",
                $"https://sts.windows.net/{this.AppSettings.KCTenant}/"
            };

            string configurationEndpoint = $"{aadInstance}/.well-known/openid-configuration";

            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(configurationEndpoint, new OpenIdConnectConfigurationRetriever());

            try
            {
                var config = await configManager.GetConfigurationAsync();

                // Initialize the token validation parameters
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    // App Id URI and AppId of this service application are both valid audiences.
                    ValidAudiences = new[] { this.AppSettings.PTASAppRegistrationId },

                    // Support Azure AD V1 and V2 endpoints.
                    ValidIssuers = validissuers,

                    // Signing keys from the tenant
                    IssuerSigningKeys = config.SigningKeys,
                };

                // Validate token.
                SecurityToken securityToken;
                var claimsPrincipal = tokenValidator.ValidateToken(authHeaderParts[1], validationParameters, out securityToken);
                var emailClaim = claimsPrincipal.Claims.
                    Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").FirstOrDefault();

                return emailClaim.Value;
            }
            catch (SecurityTokenValidationException stex)
            {
                return null;
            }
        }
    }
}
