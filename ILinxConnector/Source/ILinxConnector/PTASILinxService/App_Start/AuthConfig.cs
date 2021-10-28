// <copyright file="AuthConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
  using Owin;

  /// <summary>
  /// Authentication configuration.
  /// </summary>
  public static class AuthConfig
  {
    /// <summary>
    /// Initialize authentication.
    /// </summary>
    /// <param name="app">Application to add authorization to.</param>
    public static void UseAuthentication(this IAppBuilder app)
    {
      if (string.IsNullOrWhiteSpace(Settings.Auth.Issuer)
          || string.IsNullOrWhiteSpace(Settings.Auth.Audience)
          || string.IsNullOrWhiteSpace(Settings.Auth.IssuerCertThumbprint))
      {
        return;
      }

      app.UseJsonWebToken(
          issuer: Settings.Auth.Issuer,
          audience: Settings.Auth.Audience,
          signingKey: Settings.Auth.IssuerCertificate);
    }
  }
}