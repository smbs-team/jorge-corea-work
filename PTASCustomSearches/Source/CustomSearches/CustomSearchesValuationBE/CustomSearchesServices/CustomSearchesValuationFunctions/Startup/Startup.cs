using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CustomSearchesValuationFunctions.Startup.Startup))]

namespace CustomSearchesValuationFunctions.Startup
{
    using System;
    using CustomSearchesValuationEFLibrary;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
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
        /// Startup configuration.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>()?.Value;
            var appDirectory = executionContextOptions?.AppDirectory;

            // Token provider singleton
            IServiceTokenProvider tokenProvider = new AzureTokenProvider();
            builder.Services.AddSingleton<IServiceTokenProvider>(tokenProvider);

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();

            if (!string.IsNullOrWhiteSpace(appDirectory))
            {
                configBuilder = configBuilder.SetBasePath(appDirectory);
            }

            var config = configBuilder.AddJsonFile("app.settings.json", false)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();

            // Custom searches db context factory singleton
            IFactory<ValuationDbContext> valuationDbContextFactory = new Factory<ValuationDbContext>(() =>
            {
                DbContextOptionsBuilder<ValuationDbContext> optionsBuilder = new DbContextOptionsBuilder<ValuationDbContext>()
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

                return new ValuationDbContext(optionsBuilder.Options, tokenProvider, principalCredentials: null);
            });

            builder.Services.AddSingleton<IFactory<ValuationDbContext>>(valuationDbContextFactory);
        }
    }
}