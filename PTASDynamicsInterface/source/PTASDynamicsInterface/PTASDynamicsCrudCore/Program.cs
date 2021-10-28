// <copyright file="Program.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;

    /// <summary>
    /// Main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program that gets run on startup. Runs the web host.
        /// </summary>
        /// <param name="args">Parameters passed by the operating system.</param>
        public static void Main(string[] args)
        {
            // Comment
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a web host builder object.
        /// </summary>
        /// <param name="args">Parameters passed by the operating system.</param>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .WriteTo.Trace()
                .ReadFrom.Configuration(hostingContext.Configuration));
    }
}