using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(PTASMapTileFunctions.Startup.Startup))]

namespace PTASMapTileFunctions.Startup
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using Microsoft.Extensions.Options;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.FileSystem;
    using PTASServicesCommon.Telemetry;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Startup class.
    /// </summary>
    /// <seealso cref="Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup" />
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// The storage connection string setting name.
        /// </summary>
        private const string StorageConnectionStringSettingName = "StorageConnectionString";

        /// <summary>
        /// The blob tile container name setting name.
        /// </summary>
        private const string BlobTileContainerNameSettingName = "BlotTileContainerName";

        /// <summary>
        /// The BLOB tile path mask setting name.
        /// </summary>
        private const string BlobTilePathMaskSettingName = "BlobTilePathMask";

        /// <summary>
        /// The SQL server connection string setting name.
        /// </summary>
        private const string SQLServerConnectionStringSettingName = "SQLServerConnectionString";

        /// <summary>
        /// Startup configuration.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>()?.Value;
            var appDirectory = executionContextOptions?.AppDirectory;

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();

            if (!string.IsNullOrWhiteSpace(appDirectory))
            {
                configBuilder = configBuilder.SetBasePath(appDirectory);
            }

            var config = configBuilder.AddJsonFile("app.settings.json", false)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();

            IConfigurationSection settingsSection = config.GetSection("Settings");

            // Blob provider configuration
            ICloudStorageConfigurationProvider storageConfigurationProvider =
                new CloudStorageConfigurationProvider(config[Startup.StorageConnectionStringSettingName]);
            builder.Services.AddSingleton<ICloudStorageConfigurationProvider>(storageConfigurationProvider);

            IBlobTileConfigurationProvider blobTileConfigurationProvider = new BlobTileConfigurationProvider(
                settingsSection[Startup.BlobTileContainerNameSettingName],
                settingsSection[Startup.BlobTilePathMaskSettingName]);

            builder.Services.AddSingleton<IBlobTileConfigurationProvider>(blobTileConfigurationProvider);

            // Tile provider factory configuration
            ITileProviderFactory tileProviderFactory = new TileProviderFactory();
            builder.Services.AddSingleton<ITileProviderFactory>(tileProviderFactory);

            // Tile feature data provider factory configuration
            ITileFeatureDataProviderFactory tileFeatureDataProviderFactory = new TileFeatureDataProviderFactory();
            builder.Services.AddSingleton<ITileFeatureDataProviderFactory>(tileFeatureDataProviderFactory);

            // File system provider singleton
            IFileSystemProvider fileSystemProvider = new FileSystemProvider();
            builder.Services.AddSingleton<IFileSystemProvider>(fileSystemProvider);

            // Token provider singleton
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();
            builder.Services.AddSingleton<IServiceTokenProvider>(tokenProvider);

            // Token provider singleton
            IFactory<TileFeatureDataDbContext> tileDbContextFactory = new Factory<TileFeatureDataDbContext>(() =>
            {
                DbContextOptionsBuilder<TileFeatureDataDbContext> optionsBuilder = new DbContextOptionsBuilder<TileFeatureDataDbContext>()
                    .UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));

                return new TileFeatureDataDbContext(optionsBuilder.Options, tokenProvider);
            });

            builder.Services.AddSingleton<IFactory<TileFeatureDataDbContext>>(tileDbContextFactory);

            // Add tile feature data db context
            builder.Services.AddDbContext<TileFeatureDataDbContext>(opts =>
            {
                opts.UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));
            });

            // Add storage job storage db context
            builder.Services.AddDbContext<TileStorageJobDbContext>(opts =>
            {
                opts.UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));
            });

            string instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
            if (!string.IsNullOrWhiteSpace(instrumentationKey))
            {
                TelemetryClient telemetryClient = TelemetryHelper.CreateTelemetryClient(
                    "PTASMapTileFunctions",
                    instrumentationKey);

                builder.Services.AddSingleton<TelemetryClient>(telemetryClient);
            }
        }
    }
}