// <copyright file="Program.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsTranfer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CommandLine;

    using D2SSyncHelpers.Models;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Main program.
    /// </summary>
    public partial class Program
    {
        private enum ExitCode : int
        {
            Success = 0,
            InvalidArgs = 1,
            InvalidFilename = 2,
            UnknownError = 10,
        }

        private static IConfigurationRoot GetConfig(Options options)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return new MyConfig(configuration, options);
        }

        private static async Task HandleParseError(IEnumerable<Error> errs)
        {
            await Task.FromResult(errs);
            Environment.Exit((int)ExitCode.UnknownError);
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Number of arguments: " + args.Length);
            foreach (string arg in args)
            {
                Console.WriteLine("Received Argument: " + arg);
            }

            await Parser.Default
                .ParseArguments<Options>(args)
                .MapResult(
                    async (opts) => await RunOptionsAndReturnExitCode(opts),
                    async err => await HandleParseError(err));
        }

        private static async Task RunOptionsAndReturnExitCode(Options opts)
        {
            DapperConfig.ConfigureMapper(typeof(DBTable), typeof(DBField), typeof(TableChange));
            var finished = false;
            var retries = 0;
            while (!finished)
            {
                try
                {
                    Console.WriteLine($"Starting process. Retries: {retries}");
                    var entityName = opts.EntityName;
                    int chunkSize = opts.ChunkSize;
                    int useBulkInsert = opts.useBulkInsert;
                    await new DataProcessor().ProcessEntityAsync(entityName, chunkSize, GetConfig(opts), useBulkInsert);
                    finished = true;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    retries += 1;
                    if (retries == 3)
                    {
                        Environment.Exit((int)ExitCode.UnknownError);
                    }
                }
            }

            Console.WriteLine("Finished");
            Environment.Exit((int)ExitCode.Success);
        }
    }
}