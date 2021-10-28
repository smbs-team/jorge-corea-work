using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CustomSearchesFunctions.Startup.Startup))]

namespace CustomSearchesFunctions.Startup
{
    using System;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.ApplicationInsights;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.Telemetry;
    using PTASServicesCommon.TokenProvider;
    using SignalRManager;

    /// <summary>
    /// Startup class.
    /// </summary>
    /// <seealso cref="Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup" />
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// The LogDynamicSqlStatementsInDb setting name
        /// </summary>
        private const string LogDynamicSqlStatementsInDbSettingName = "LogDynamicSqlStatementsInDb";

        /// <summary>
        /// The SQL server connection string setting name.
        /// </summary>
        private const string SQLServerConnectionStringSettingName = "SQLServerConnectionString";

        /// <summary>
        /// The SQL server data connection string setting name.
        /// </summary>
        private const string SQLServerDataConnectionStringSettingName = "SQLServerDataConnectionString";

        /// <summary>
        /// The storage connection string setting name.
        /// </summary>
        private const string StorageConnectionStringSettingName = "StorageConnectionString";

        /// <summary>
        /// The premium storage connection string setting name.
        /// </summary>
        private const string PremiumStorageConnectionStringSettingName = "PremiumStorageConnectionString";

        /// <summary>
        /// The Azure SignalR connection string setting name.
        /// </summary>
        private const string AzureSignalRConnectionStringSettingName = "AzureSignalRConnectionString";

        /// <summary>
        /// The admin roles setting name.
        /// </summary>
        private const string AdminRolesSettingName = "AdminRoles";

        /// <summary>
        /// The Log Sql Statements setting name.
        /// </summary>
        private const string LogSqlStatementsSettingName = "LogSqlStatements";

        /// <summary>
        /// The SighalR hub endpoint setting name.
        /// </summary>
        private const string SignalRHubEndpointSettingName = "SignalRHubEndpoint";

        /// <summary>
        /// The active telemetry channels setting name.
        /// </summary>
        private const string ActiveTelemetryChannelsSettingName = "ActiveTelemetryChannels";

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

            IConfigurationSection values = config.GetSection("Values");

            // Token provider singleton
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();
            builder.Services.AddSingleton<IServiceTokenProvider>(tokenProvider);

            var loggerFactory = this.CreateLoggerFactory(builder);

            // Custom searches db context factory singleton
            IFactory<CustomSearchesDbContext> customSearchesDbContextFactory =
                this.CreateCustomSearchesDbContextFactory(tokenProvider, loggerFactory, config);

            builder.Services.AddSingleton<IFactory<CustomSearchesDbContext>>(customSearchesDbContextFactory);

            // Custom searches data db context factory singleton
            IFactory<CustomSearchesDataDbContext> customSearchesDataDbContextFactory =
                this.CreateCustomSearchesDataDbContextFactory(tokenProvider, loggerFactory, config);

            builder.Services.AddSingleton<IFactory<CustomSearchesDataDbContext>>(customSearchesDataDbContextFactory);

            // Custom searches gis db context factory singleton
            IFactory<GisDbContext> gisDbContextFactory =
                this.CreateGisDbContextFactory(tokenProvider, loggerFactory, config);

            builder.Services.AddSingleton<IFactory<GisDbContext>>(gisDbContextFactory);

            // Custom searches workerJob db context factory singleton
            IFactory<WorkerJobDbContext> workerJobDbContextFactory =
                this.CreateWorkerJobDbContextFactory(tokenProvider, loggerFactory, config);

            builder.Services.AddSingleton<IFactory<WorkerJobDbContext>>(workerJobDbContextFactory);

            // Cloud storage provider singleton
            ICloudStorageConfigurationProvider storageConfigurationProvider =
                new CloudStorageConfigurationProvider(config[Startup.StorageConnectionStringSettingName]);

            ICloudStorageProvider cloudStorageProvider = new CloudStorageProvider(
                storageConfigurationProvider,
                tokenProvider);

            // Premium Cloud storage provider singleton
            ICloudStorageConfigurationProvider premiumStorageConfigurationProvider =
                new CloudStorageConfigurationProvider(config[Startup.PremiumStorageConnectionStringSettingName]);

            ICloudStorageProvider premiumCloudStorageProvider = new CloudStorageProvider(
                premiumStorageConfigurationProvider,
                tokenProvider);

            builder.Services.AddSingleton<ICloudStorageProvider>(cloudStorageProvider);

            // Authorization factory singleton
            IFactory<IAuthProvider> authProviderFactory = new Factory<IAuthProvider>(() =>
            {
                return new AuthProvider(customSearchesDbContextFactory, values[Startup.AdminRolesSettingName]);
            });

            builder.Services.AddSingleton<IFactory<IAuthProvider>>(authProviderFactory);

            // Message publisher factory
            IMessagePublisherFactory messagePublisherFactory = new MessagePublisherFactory(config[Startup.AzureSignalRConnectionStringSettingName]);

            var activeChannels = config[Startup.ActiveTelemetryChannelsSettingName]?.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
            TelemetryHelper.SetActiveTelemetryChannels(activeChannels);

            TelemetryClient telemetryClient = TelemetryHelper.CreateTelemetryClient(
                "CustomSearchesFunctions",
                Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"));

            // Service context factory singleton
            ServiceContextFactory serviceContextFactory = new ServiceContextFactory(
                authProviderFactory,
                customSearchesDbContextFactory,
                gisDbContextFactory,
                customSearchesDataDbContextFactory,
                workerJobDbContextFactory,
                messagePublisherFactory,
                cloudStorageProvider,
                premiumCloudStorageProvider,
                appCredential: null,
                telemetryClient,
                appDirectory,
                values["JobQueueSuffix"],
                config[Startup.SignalRHubEndpointSettingName],
                logger: null);
            builder.Services.AddSingleton<IServiceContextFactory>(serviceContextFactory);

            bool sqlLoggingInDbEnabled = false;
            bool.TryParse(values[Startup.LogDynamicSqlStatementsInDbSettingName], out sqlLoggingInDbEnabled);

            DbTransientRetryPolicy.SetDynamicSqlLoggingInDbEnabled(sqlLoggingInDbEnabled);
        }

        /// <summary>
        /// Creates the logger factory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The logger factory.</returns>
        private ILoggerFactory CreateLoggerFactory(IFunctionsHostBuilder builder)
        {
            return LoggerFactory.Create(builder =>
            {
                builder
                .AddConsole((options) => { })
                .AddFilter((category, level) =>
                    (category == DbLoggerCategory.Database.Command.Name || category == DbLoggerCategory.Update.Name)
                    && level == LogLevel.Information);
            });
        }

        /// <summary>
        /// Creates the custom searches database context factory.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="config">Configuration settings.</param>
        /// <returns>
        /// The database factory.
        /// </returns>
        private IFactory<CustomSearchesDbContext> CreateCustomSearchesDbContextFactory(
            IServiceTokenProvider tokenProvider,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            IConfigurationSection values = config.GetSection("Values");

            // Sql Logging
            bool logSqlStatements = false;
            bool.TryParse(values[Startup.LogSqlStatementsSettingName], out logSqlStatements);

            return new Factory<CustomSearchesDbContext>(() =>
            {
                DbContextOptionsBuilder<CustomSearchesDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomSearchesDbContext>()
                    .UseSqlServer(
                        config[Startup.SQLServerConnectionStringSettingName],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.CommandTimeout(30);
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        });
                optionsBuilder.EnableSensitiveDataLogging(true);
                if (logSqlStatements)
                {
                    optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true);
                }

                return new CustomSearchesDbContext(optionsBuilder.Options, tokenProvider, null);
            });
        }

        /// <summary>
        /// Creates the custom searches data database context factory.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="config">Configuration settings.</param>
        /// <returns>
        /// The database factory.
        /// </returns>
        private IFactory<CustomSearchesDataDbContext> CreateCustomSearchesDataDbContextFactory(
            IServiceTokenProvider tokenProvider,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            IConfigurationSection values = config.GetSection("Values");

            // Sql Logging
            bool logSqlStatements = false;
            bool.TryParse(values[Startup.LogSqlStatementsSettingName], out logSqlStatements);

            return new Factory<CustomSearchesDataDbContext>(() =>
            {
                DbContextOptionsBuilder<CustomSearchesDataDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomSearchesDataDbContext>()
                    .UseSqlServer(
                    config[Startup.SQLServerDataConnectionStringSettingName],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseNetTopologySuite();
                        sqlOptions.CommandTimeout(30);
                        sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    });

                return new CustomSearchesDataDbContext(optionsBuilder.Options, tokenProvider, null);
            });
        }

        /// <summary>
        /// Creates the gis database context factory.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="config">Configuration settings.</param>
        /// <returns>
        /// The database factory.
        /// </returns>
        private IFactory<GisDbContext> CreateGisDbContextFactory(
            IServiceTokenProvider tokenProvider,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            IConfigurationSection values = config.GetSection("Values");

            // Sql Logging
            bool logSqlStatements = false;
            bool.TryParse(values[Startup.LogSqlStatementsSettingName], out logSqlStatements);

            return new Factory<GisDbContext>(() =>
            {
                DbContextOptionsBuilder<GisDbContext> optionsBuilder = new DbContextOptionsBuilder<GisDbContext>()
                    .UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseNetTopologySuite();
                        sqlOptions.CommandTimeout(30);
                        sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    });

                if (logSqlStatements)
                {
                    optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true);
                }

                return new GisDbContext(optionsBuilder.Options, tokenProvider, null);
            });
        }

        /// <summary>
        /// Creates the worker job database context factory.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="config">Configuration settings.</param>
        /// <returns>
        /// The database factory.
        /// </returns>
        private IFactory<WorkerJobDbContext> CreateWorkerJobDbContextFactory(
            IServiceTokenProvider tokenProvider,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            IConfigurationSection values = config.GetSection("Values");

            // Sql Logging
            bool logSqlStatements = false;
            bool.TryParse(values[Startup.LogSqlStatementsSettingName], out logSqlStatements);

            return new Factory<WorkerJobDbContext>(() =>
            {
                DbContextOptionsBuilder<WorkerJobDbContext> optionsBuilder = new DbContextOptionsBuilder<WorkerJobDbContext>()
                    .UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseNetTopologySuite();
                        sqlOptions.CommandTimeout(30);
                        sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    });

                if (logSqlStatements)
                {
                    optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true);
                }

                return new WorkerJobDbContext(optionsBuilder.Options, tokenProvider, null);
            });
        }
    }
}