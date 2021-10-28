// <copyright file="Constants.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    /// <summary>
    /// System wide constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant settings.
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Gets the allowed  origins.
            /// </summary>
            public static string AllowedOrigins => "allowed-origins";

            /// <summary>
            /// Authentication class.
            /// </summary>
            public static class Auth
            {
                /// <summary>
                /// Gets issuer info.
                /// </summary>
                public static string Issuer => "oauth2:issuer";

                /// <summary>
                /// Gets audience info.
                /// </summary>
                public static string Audience => "oauth2:audience";

                /// <summary>
                /// Gets cert thumbprint.
                /// </summary>
                public static string CertThumbprint => "oauth2:cert-thumbprint";
            }
        }
    }
}