// <copyright file="JWTDecoder.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Decodes JWT header.
    /// </summary>
    public class JWTDecoder
    {
        /// <summary>
        /// Get authorization token.
        /// </summary>
        /// <returns>Token if possible.</returns>
        public static JwtSecurityToken GetAuthToken()
        {
            var authHeader = GetAuthHeader();
            return authHeader != null
                ? new JwtSecurityTokenHandler()
                        .ReadJwtToken(authHeader.Replace("Bearer ", string.Empty))
                : null;
        }

        /// <summary>
        /// Get Authorization header.
        /// </summary>
        /// <returns>Header string.</returns>
        public static string GetAuthHeader() =>
            HttpContext.Current.Request.Headers.GetValues(HttpRequestHeader.Authorization.ToString())?.FirstOrDefault();

        /// <summary>
        /// Attempts to get an email from jwt auth token.
        /// </summary>
        /// <returns>email if found or null.</returns>
        public static string GetEmailFromHeader()
        {
            var authToken = GetAuthToken();
            if (authToken != null)
            {
                IEnumerable<System.Security.Claims.Claim> claims =
                    authToken.Claims;
                var try1 = claims
                    .Where(s => s.Type == "sub")
                    .FirstOrDefault()?.Value ?? string.Empty;
                if (try1.IsValidEmail())
                {
                    return try1;
                }

                var try2 = claims
                    .Where(s => s.Type == "emails").FirstOrDefault()?.Value ?? string.Empty;
                if (try2.IsValidEmail())
                {
                    return try2;
                }
            }

            return null;
        }
    }
}