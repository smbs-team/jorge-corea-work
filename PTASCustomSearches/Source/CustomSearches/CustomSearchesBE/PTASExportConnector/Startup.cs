// <copyright file="Startup.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

//using System.IO.Abstractions;
//using Microsoft.Azure.Functions.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//using PTASExportConnector;
//using PTASExportConnector.SDK;

//[assembly: FunctionsStartup(typeof(Startup))]

//namespace PTASExportConnector
//{
//    /// <summary>Entry point of the application.</summary>
//    public class Startup : FunctionsStartup
//    {
//        /// <summary>Configure method.</summary>
//        /// <param name="builder"> Builder.</param>
//        public override void Configure(IFunctionsHostBuilder builder)
//        {
//            builder.Services.AddSingleton<IConnector, Connector>();
//            builder.Services.AddSingleton<IDbService, DbService>();
//            builder.Services.AddSingleton<IExporters, Exporters>();
//            builder.Services.AddSingleton<IOdata, Odata>();
//            builder.Services.AddSingleton<IFileSystem, FileSystem>();
//        }
//    }
// }
