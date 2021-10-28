using System;

namespace PTASDynamicsTranfer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CommandLine;

    using D2SSyncHelpers.Models;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public partial class Start
    {
        private enum ExitCode : int
        {
            Success = 0,
            InvalidArgs = 1,
            InvalidFilename = 2,
            UnknownError = 10,
        }

        /// <summary>
        /// Run the transfer process.
        /// </summary>
        /// <param name="chunkSize">chunkSize.</param>
        /// <param name="connectionString">connectionString.</param>
        /// <param name="authURI">authURI.</param>
        /// <param name="dynamicsURL">dynamicsURL.</param>
        /// <param name="entityName">entityName.</param>
        /// <param name="clientID">clientID.</param>
        /// <param name="clientSecret">clientSecret.</param>
        /// <param name="useBulkInsert">useBulkInsert.</param>
        /// <param name="principalCredentials">principalCredentials.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Run(int chunkSize, string connectionString, string authURI, string dynamicsURL, string entityName, string clientID, string clientSecret, int useBulkInsert, ClientCredential principalCredentials)
        {
            Options options = new Options(chunkSize, connectionString, authURI, dynamicsURL, entityName, clientID, clientSecret, useBulkInsert);
            await RunOptionsAndReturnExitCode(options, principalCredentials);
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
            await Task.FromResult(0);
            Environment.Exit((int)ExitCode.UnknownError);
        }

        private static async Task RunOptionsAndReturnExitCode(Options opts, ClientCredential principalCredentials)
        {
            DapperConfig.ConfigureMapper(typeof(DBTable), typeof(DBField), typeof(TableChange));
            var finished = false;
            var retries = 0;
            string version = "2.0.0";
            Console.WriteLine("PTASDynamicsTransfer Version: " + version);
            while (!finished)
            {
                try
                {
                    Console.WriteLine($"Starting process. Retries: {retries}");
                    var entityName = opts.EntityName;
                    string connectionString = opts.ConnectionString;
                    int chunkSize = opts.ChunkSize;
                    int useBulkInsert = opts.useBulkInsert;
                    await new DataProcessor().ProcessEntityAsync(entityName, chunkSize, GetConfig(opts), useBulkInsert, connectionString, principalCredentials);
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

            // Environment.Exit((int)ExitCode.Success);
        }
    }
}