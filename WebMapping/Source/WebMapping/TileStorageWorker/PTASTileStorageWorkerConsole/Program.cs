namespace PTASTileStorageWorkerConsole
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;
    using PTASTileStorageWorkerLibrary.SystemProcess;
    using PTASTileStorageWorkerLibrary.TileStorage;
    using Serilog;
    using Serilog.Extensions.Logging;

    /// <summary>
    /// Tile storage worker program class.
    /// </summary>
    public class Program
    {
        private const int NoJobsDelay = 1000;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true, true)
                .Build();

            IConfigurationSection values = config.GetSection("Values");

            DbContextOptions<TileStorageJobDbContext> options = new DbContextOptionsBuilder<TileStorageJobDbContext>().UseSqlServer(values["SQLServerConnectionString"], null).Options;
            TileStorageJobDbContext dbContext = new TileStorageJobDbContext(options);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Trace()
                .CreateLogger();

            SerilogLoggerProvider serilogLoggerProvider = new SerilogLoggerProvider(Log.Logger);
            Microsoft.Extensions.Logging.ILogger seriLogLogger = serilogLoggerProvider.CreateLogger("SerilogLogger");

            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(values["StorageConnectionString"]);
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();
            ICloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider, tokenProvider);
            ICloudStorageSharedSignatureProvider sharedSignatureProvider = new BlobSharedSignatureProvider(storageProvider);
            IProcessFactory processFactory = new ProcessFactory();

            TileStorageWorker worker = new TileStorageWorker(dbContext, storageProvider, sharedSignatureProvider, processFactory, config, seriLogLogger);

            while (true)
            {
                bool result = false;
                try
                {
                    result = worker.ExecuteNextJob();
                }
                catch (System.Exception ex)
                {
                    seriLogLogger.LogInformation($"Exception while processing jobs: {ex.ToString()}");
                    result = false;
                }

                if (!result)
                {
                    seriLogLogger.LogInformation("Waiting for jobs...");
                    System.Threading.Thread.Sleep(NoJobsDelay);
                }
            }
        }
    }
}
