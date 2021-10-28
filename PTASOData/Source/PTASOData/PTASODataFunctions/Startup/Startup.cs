using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(PTASODataFunctions.Startup.Startup))]

namespace PTASODataFunctions.Startup
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using PTASODataLibrary.Helper;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Startup class.
    /// </summary>
    /// <seealso cref="Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup" />
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// The SQL server connection string setting name.
        /// </summary>
        private const string SQLServerConnectionStringSettingName = "SQLServerConnectionString";

        /// <summary>
        /// The db context type setting name.
        /// </summary>
        private const string DbContextTypeSettingName = "DbContextType";

        /// <summary>
        /// The allow OData updates setting name.
        /// </summary>
        private const string AllowODataUpdatesSettingName = "AllowODataUpdates";

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

            // Token provider singleton
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();
            builder.Services.AddSingleton<IServiceTokenProvider>(tokenProvider);

            // Token provider singleton
            IFactory<DbContext> dbContextFactory = new Factory<DbContext>(() =>
            {
                if (config[Startup.DbContextTypeSettingName].ToLower() == "ptascamadbcontext")
                {
                    DbContextOptionsBuilder<PTASCAMADbContext> optionsBuilder = new DbContextOptionsBuilder<PTASCAMADbContext>()
                        .UseSqlServer(
                            config[Startup.SQLServerConnectionStringSettingName],
                            x => x.UseNetTopologySuite().CommandTimeout(30));

                    return new PTASCAMADbContext(optionsBuilder.Options, tokenProvider);
                }
                else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptastreasurycontext")
                {
                    DbContextOptionsBuilder<PtasTreasuryContext> optionsBuilder = new DbContextOptionsBuilder<PtasTreasuryContext>()
                        .UseSqlServer(
                            config[Startup.SQLServerConnectionStringSettingName],
                            x => x.UseNetTopologySuite().CommandTimeout(30));

                    return new PtasTreasuryContext(optionsBuilder.Options, tokenProvider);
                }
                else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptascamaandhistoricaldbcontext")
                {
                    DbContextOptionsBuilder<PTASCamaAndHistoricalDbContext> optionsBuilder = new DbContextOptionsBuilder<PTASCamaAndHistoricalDbContext>()
                        .UseSqlServer(
                            config[Startup.SQLServerConnectionStringSettingName],
                            x => x.UseNetTopologySuite().CommandTimeout(30));

                    return new PTASCamaAndHistoricalDbContext(optionsBuilder.Options, tokenProvider);
                }
                else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptashistoricaldbcontext")
                {
                    DbContextOptionsBuilder<PtasHistoricalDbContext> optionsBuilder = new DbContextOptionsBuilder<PtasHistoricalDbContext>()
                        .UseSqlServer(
                            config[Startup.SQLServerConnectionStringSettingName],
                            x => x.UseNetTopologySuite().CommandTimeout(30));

                    return new PtasHistoricalDbContext(optionsBuilder.Options, tokenProvider);
                }

                return null;
            });

            builder.Services.AddSingleton<IFactory<DbContext>>(dbContextFactory);

            // Add data db context
            builder.Services.AddDbContext<PTASCAMADbContext>(opts =>
            {
                opts.UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));
            });

            // Add data db context
            builder.Services.AddDbContext<PtasTreasuryContext>(opts =>
            {
                opts.UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));
            });

            // Add data db context
            builder.Services.AddDbContext<PtasHistoricalDbContext>(opts =>
            {
                opts.UseSqlServer(
                    config[Startup.SQLServerConnectionStringSettingName],
                    x => x.UseNetTopologySuite().CommandTimeout(30));
            });

            if (config[Startup.AllowODataUpdatesSettingName]?.ToLower() == "true")
            {
                ODataExtensions.AllowODataUpdates = true;
            }

            if (config[Startup.DbContextTypeSettingName].ToLower() == "ptascamadbcontext")
            {
                DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));
            }
            else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptastreasurycontext")
            {
                DbContextExtensions.RegisterDbContext(typeof(PtasTreasuryContext));
            }
            else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptascamaandhistoricaldbcontext")
            {
                DbContextExtensions.RegisterDbContext(typeof(PTASCamaAndHistoricalDbContext));
            }
            else if (config[Startup.DbContextTypeSettingName].ToLower() == "ptashistoricaldbcontext")
            {
                DbContextExtensions.RegisterDbContext(typeof(PtasHistoricalDbContext));
            }
        }
    }
}