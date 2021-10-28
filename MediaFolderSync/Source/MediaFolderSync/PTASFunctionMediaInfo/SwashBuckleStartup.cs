// <copyright file="SwashBuckleStartup.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

using MediaInfo;

using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(SwashBuckleStartup))]

namespace MediaInfo
{
    using System.Reflection;

    ////using AzureFunctions.Extensions.Swashbuckle;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;

    /// <summary>
    /// Startup swagger.
    /// </summary>
    public class SwashBuckleStartup : IWebJobsStartup
    {
        /// <summary>
        /// Configure swagger.
        /// </summary>
        /// <param name="builder">Injected builder.</param>
        public void Configure(IWebJobsBuilder builder)
        {
            // Register the extension.
            ////builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}