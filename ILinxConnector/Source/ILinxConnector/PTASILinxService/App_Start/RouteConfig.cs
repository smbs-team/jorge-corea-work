// <copyright file="RouteConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
  using System.Web.Http;

  /// <summary>
  /// Static class to configure the routing.
  /// </summary>
  public static class RouteConfig
  {
    /// <summary>
    /// Configure the routes.
    /// </summary>
    /// <param name="config">System supplied configuration initializer.</param>
    public static void Configure(HttpConfiguration config)
    {
      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1.0/api/{controller}/{defaultId}",
                defaults: new { defaultId = RouteParameter.Optional });
    }
  }
}
