// <copyright file="OptionsMessageHandlerConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
  using System;
  using System.Web.Http;

  /// <summary>
  /// Options message handler configuration.
  /// </summary>
  public static class OptionsMessageHandlerConfig
  {
    /// <summary>
    /// Configure the message handler.
    /// </summary>
    /// <param name="config">Application configuration.</param>
    public static void Configure(HttpConfiguration config)
    {
      if (config == null)
      {
        throw new ArgumentNullException(nameof(config));
      }

      config.MessageHandlers.Add(new OptionsHttpMessageHandler());
    }
  }
}
