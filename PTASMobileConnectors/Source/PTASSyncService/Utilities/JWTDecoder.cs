// <copyright file="JWTDecoder.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSyncService.Utilities
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Decodes JWT header.
    /// </summary>
    public static class JWTDecoder
    {
        /// <summary>
        /// Get authorization token.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>Token if possible.</returns>
        public static JwtSecurityToken GetAuthToken(HttpContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context), "Context is null");
            }

            var authHeader = string.Empty;
            try
            {
                authHeader = GetAuthHeader(context);
                return authHeader != null
                    ? new JwtSecurityTokenHandler()
                            .ReadJwtToken(authHeader.Replace("Bearer ", string.Empty))
                    : null;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Failed on GetAuthToken. Auth header: {authHeader}.", ex);
            }
        }

        /// <summary>
        /// Get Authorization header.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>Header string.</returns>
        public static string GetAuthHeader(HttpContext context)
            => context.Request.Headers[HttpRequestHeader.Authorization.ToString()].First();

        /// <summary>
        /// Attempts to get an email from jwt auth token.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>email if found or null.</returns>
        public static string GetEmailFromHeader(HttpContext context)
        {
            var authToken = GetAuthToken(context);
            if (authToken != null)
            {
                IEnumerable<Claim> claims = authToken.Claims;

                var try1 = claims
                    .Where(s => s.Value.IsValidEmail())
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

                //This code is exclusive just for desktop sync app
                Microsoft.Extensions.Primitives.StringValues email;
                if (context.Request.Headers.TryGetValue("email", out email))
                {
                    if (email.FirstOrDefault().IsValidEmail())
                    {
                        return email;
                    }
                }

            }

            return null;
        }

        /// <summary>
        /// Get the oid from a token.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>Object Id if found.</returns>
        public static string GetOidFromToken(HttpContext context)
        {
            var authToken = GetAuthToken(context);
            if (authToken != null)
            {
                IEnumerable<Claim> claims =
                    authToken.Claims;
                var try1 = claims
                    .Where(s => s.Type == "oid")
                    .FirstOrDefault()?.Value;
                return try1;
            }

            return null;
        }
    }
}