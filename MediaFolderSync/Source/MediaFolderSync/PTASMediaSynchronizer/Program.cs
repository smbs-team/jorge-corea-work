// <copyright file="Program.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASMediaSynchronizer
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using PTASMediaHelperClasses;

    /// <summary>
    /// Console program to load data from the database
    /// starting at a given date.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Current event logger.
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Main program run.
        /// </summary>
        /// <param name="args">System provided arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

                // if any of the args is string 'nl', choose the null logger.
                logger = args.Any(s => s == "nl") ? new NullLogger() : (ILogger)new EventLogLogger();

                GetParams(
                    out string docFile,
                    out string blobContainer,
                    out string connectionName,
                    out string rootFolder,
                    out string appId,
                    out string appKey,
                    out string tenantId,
                    out string secretIdentifier);

                string connectionString = GetDBConnectionString(connectionName);
                var one = await GetOne();
                string storageConnectionString = await GetStorageConnectionStringAsync(appId, appKey, tenantId, secretIdentifier);
                logger.OptionalOutput($"Output file: {docFile}");
                var dateGetter = new FileDateManager(docFile);

                // first argument is start date, if it is null, will use the one in the xml file.
                DateTime startTime;
                var gotDateFromArgs = false;
                if (GetDateFromArgs(args, out DateTime argsDate))
                {
                    startTime = argsDate;
                    gotDateFromArgs = true;
                }
                else
                {
                    startTime = dateGetter.GetDate();
                }

                IFileCopier mover = new ToAzureFileShareMover(storageConnectionString, logger);
                new PTASMediaDBLoaderClasses.DBRunner(mover, rootFolder, connectionString, logger).Run(startTime, LoadQuery());
                if (!gotDateFromArgs)
                {
                    dateGetter.UpdateDate();
                }

                Console.WriteLine("...done.");
            }
            catch (Exception ex)
            {
                logger.OptionalOutput($"{ex.Message} {ex.InnerException?.Message ?? string.Empty}");
            }
            finally
            {
#if DEBUG
                Console.ReadLine();
#endif
            }
        }

        private static string LoadQuery()
        {
            var s = Directory.GetCurrentDirectory() + "/query.sql";
            return File.ReadAllText(s);
        }

        private static bool GetDateFromArgs(string[] args, out DateTime argsDate)
        {
            if (args.Length == 0)
            {
                argsDate = DateTime.Now;
                return false;
            }

            // is the argument a date?
            string arg = args[0];
            if (DateTime.TryParse(arg, out argsDate))
            {
                // yes, so use it.
                return true;
            }

            // is the argument a number?
            if (int.TryParse(args[0], out int result))
            {
                // yes: then go back specified number of days.
                argsDate = DateTime.Today.AddDays(-result);
                return true;
            }

            // not date, not number.
            return false;
        }

        /// <summary>
        /// Get an item from configuration.
        /// </summary>
        /// <param name="key">Configuration key.</param>
        /// <returns>Item fetched from config file.</returns>
        private static string FromConfig(string key) => ConfigurationManager.AppSettings[key];

        /// <summary>
        /// Get the connections string from the config.
        /// </summary>
        /// <param name="connectionName">Which config parameter we are using.</param>
        /// <returns>The connection string fetched.</returns>
        private static string GetDBConnectionString(string connectionName) => ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

        /// <summary>
        /// Get the storage connection string.
        /// </summary>
        /// <returns>The connection string.</returns>
        private static async Task<string> GetStorageConnectionStringAsync(string appId, string appKey, string tenantId, string secredIdentifier)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider($"RunAs=App;AppId={appId};TenantId={tenantId};AppKey={appKey}");
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secret = await keyVaultClient.GetSecretAsync(secredIdentifier)
                    .ConfigureAwait(false);
            return secret.Value;
        }

        private static async Task<int> GetOne()
        {
            return await Task.FromResult(1);
        }

        /// <summary>
        /// Fetch all parameters.
        /// </summary>
        /// <param name="docFile">Return for document file name.</param>
        /// <param name="blobContainer">Return for document blob container.</param>
        /// <param name="connectionName">Return for connection name.</param>
        /// <param name="rootFolder">Return for document root folder.</param>
        private static void GetParams(out string docFile, out string blobContainer, out string connectionName, out string rootFolder, out string appId, out string appKey, out string tenantId, out string secretIdentifier)
        {
            docFile = FromConfig("save-file");
            blobContainer = FromConfig("container-name");
            connectionName = "localDB";
            rootFolder = FromConfig("file-root");
            appId = FromConfig("app-id");
            appKey = FromConfig("app-key");
            tenantId = FromConfig("tenant-id");
            secretIdentifier = FromConfig("secret-identifier");
        }

        /// <summary>
        ///  gets triggered when there is an unhandled exception.
        /// </summary>
        /// <param name="sender">Who generated it.</param>
        /// <param name="e">The event.</param>
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            string output = $"UNHANDLED ERROR: " + e.ExceptionObject.ToString();
            logger.OptionalOutput(output);
            logger.WriteError(output);
            Environment.Exit(1);
        }
    }
}