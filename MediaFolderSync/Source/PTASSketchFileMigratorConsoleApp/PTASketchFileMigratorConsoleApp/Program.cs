// <copyright file="Program.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using global::Ninject;
    using Serilog;

    /// <summary>Entry point of the application.</summary>
    internal class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.EventLog("PTASSketchFileMigrator", manageEventSource: true)
                    .WriteTo.Console()
                    .CreateLogger();

                IKernel kernel = new StandardKernel(new Ninject());
                var sketchFileMigrator = kernel.Get<SketchFileMigrator>();
                sketchFileMigrator.Run();
            }
            catch (Exception ex)
            {
                Log.Error("Error: {0}", ex.GetBaseException());
                Log.CloseAndFlush();
                throw;
            }
        }
    }
}
