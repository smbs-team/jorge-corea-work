﻿// <copyright file="Startup.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

using System.IO;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PTASIlinxService.Startup))]

namespace PTASIlinxService
{
    // More information about https://github.com/drwatson1/AspNet-WebApi/wiki

    /// <summary>
    /// Application startup class. Is invoked on initialization.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure the application.
        /// </summary>
        /// <param name="app">Application builder object, add stuff to it.</param>
        public void Configuration(IAppBuilder app)
        {
            // Use DotNetEnv v1.1.0 due to it is the only version with out dependencies
            var envFilePath = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, ".env");
            if (File.Exists(envFilePath))
            {
                DotNetEnv.Env.Load(envFilePath);
            }

            // Set the CORS options
            var corsOptions = CorsConfig.ConfigureCors(Settings.AllowedOrigins);
            app.UseCors(corsOptions);

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();

            config.Filters.Add(new ExceptionFilter());

            AutofacConfig.Configure(config);

            FormatterConfig.Configure(config);
            RouteConfig.Configure(config);
            LoggerConfig.Configure(config);
            OptionsMessageHandlerConfig.Configure(config);
            SwaggerConfig.Configure(config);

            app.UseAutofacMiddleware(AutofacConfig.Container);
            app.UseAutofacWebApi(config);

            app.PreventResponseCaching();

            app.UseAuthentication();
            app.UseWebApi(config);
        }
    }
}
