namespace CustomSearchesWorker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BaseWorkerLibrary;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesWorkerLibrary.DatasetProcessor;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesWorkerLibrary.RScript;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PTASCRMHelpers;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;
    using RDotNet;
    using Serilog;
    using Serilog.Extensions.Logging;
    using CustomSearchesEFLibrary.Gis;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using SignalRManager;
    using PTASTileStorageWorkerLibrary.Processor;
    using PTASTileStorageWorkerLibrary.SystemProcess;
    using Microsoft.Extensions.Logging;
    using CustomSearchesEFLibrary.WorkerJob;
    using Microsoft.ApplicationInsights;
    using PTASServicesCommon.Telemetry;
    using PTASImportWorkerLibrary.Processor;
    using PTASExportWorkerLibrary.Processor;
    using PTASDynamicsSynchronizationToSQL.Processor;
    using PTASDynamicsTranferWorkerLibrary.Processor;
    using System.Linq;
    using SyncDatabaseWorkerLibrary.Processor;

    internal class Program
    {
        private const int GlobalQueuePollTime = 30000;
        private static readonly string LibrariesPath = "C:/RScriptWorker/Libraries";
        private static readonly string PandocPath = "C:/Program Files/Pandoc";
        private static readonly string ExecutionPath = "C:\\RScriptWorker\\Execution";

        private static void Main(string[] args)
        {
            Microsoft.Extensions.Logging.ILogger seriLogLogger = null;
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("app.settings.json", false, true)
                    .AddJsonFile("local.settings.json", true, true)
                    .Build();

                IConfigurationSection values = config.GetSection("Values");

                // Configure active telemetry channels
                var activeChannels = values["ActiveTelemetryChannels"]?.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
                TelemetryHelper.SetActiveTelemetryChannels(activeChannels);

                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();

                telemetryConfiguration.InstrumentationKey = values["APPINSIGHTS_INSTRUMENTATIONKEY"];

                telemetryConfiguration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());
                telemetryConfiguration.TelemetryInitializers.Add(new CloudRoleNameTelemetryInitializer("CustomSearchesWorker"));

                using (DependencyTrackingTelemetryModule depModule = new DependencyTrackingTelemetryModule())
                {
                    depModule.Initialize(telemetryConfiguration);
                }

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Trace()
                    .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
                    .CreateLogger();

                SerilogLoggerProvider serilogLoggerProvider = new SerilogLoggerProvider(Log.Logger);
                seriLogLogger = serilogLoggerProvider.CreateLogger("SerilogLogger");

                Directory.CreateDirectory(LibrariesPath);

                // Dependencies are only installed for workers, not for job containers
                if (args.Length == 0)
                {
                    string installDependenciesScript = LoadScriptFromResourceFile("InstallDependencies.r");
                    RScriptHelper.Evaluate(installDependenciesScript);
                }

                // Configure sql statement logging in DB
                bool sqlLoggingInDbEnabled = false;
                bool.TryParse(values["LogDynamicSqlStatementsInDb"], out sqlLoggingInDbEnabled);
                DbTransientRetryPolicy.SetDynamicSqlLoggingInDbEnabled(sqlLoggingInDbEnabled);


                WorkerJobCoordinator.AllowApplicationExit = values["AllowCoordinatorToExit"]?.ToLower() == "true";

                IServiceTokenProvider tokenProvider = new AzureTokenProvider();

                IFactory<WorkerJobDbContext> workerJobDbContextFactory = new Factory<WorkerJobDbContext>(() =>
                {
                    DbContextOptionsBuilder<WorkerJobDbContext> optionsBuilder = new DbContextOptionsBuilder<WorkerJobDbContext>()
                        .UseSqlServer(
                        values["SQLServerConnectionString"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.CommandTimeout(30);
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        });

                    ClientCredential principalCredentials = new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]);
                    return new WorkerJobDbContext(optionsBuilder.Options, tokenProvider, principalCredentials);
                });

                IFactory<CustomSearchesDbContext> customSearchesDbContextFactory = new Factory<CustomSearchesDbContext>(() =>
                {
                    DbContextOptionsBuilder<CustomSearchesDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomSearchesDbContext>()
                        .UseSqlServer(
                        values["SQLServerConnectionString"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.CommandTimeout(30);
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        });

                    ClientCredential principalCredentials = new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]);
                    return new CustomSearchesDbContext(optionsBuilder.Options, tokenProvider, principalCredentials);
                });

                IFactory<GisDbContext> gisDbContextFactory = new Factory<GisDbContext>(() =>
                {
                    DbContextOptionsBuilder<GisDbContext> optionsBuilder = new DbContextOptionsBuilder<GisDbContext>()
                        .UseSqlServer(
                        values["SQLServerConnectionString"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.CommandTimeout(30);
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        });

                    ClientCredential principalCredentials = new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]);
                    return new GisDbContext(optionsBuilder.Options, tokenProvider, principalCredentials);
                });

                IFactory<CustomSearchesDataDbContext> customSearchesDataDbContextFactory = new Factory<CustomSearchesDataDbContext>(() =>
                {
                    DbContextOptionsBuilder<CustomSearchesDataDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomSearchesDataDbContext>()
                        .UseSqlServer(
                        values["SQLServerDataConnectionString"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.UseNetTopologySuite();
                            sqlOptions.CommandTimeout(30);
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        });

                    ClientCredential principalCredentials = new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]);
                    return new CustomSearchesDataDbContext(optionsBuilder.Options, tokenProvider, principalCredentials);
                });

                IFactory<GenericDynamicsHelper> dynamicsODataHelperFactory = new Factory<GenericDynamicsHelper>(() =>
                {
                    return new GenericDynamicsHelper(
                        values["DynamicsApiRoute"],
                        values["DynamicsODataUri"],
                        values["DynamicsAuthUri"],
                        values["DynamicsClientId"],
                        values["DynamicsClientSecret"]);
                });

                ICloudStorageConfigurationProvider storageConfigurationProvider =
                    new CloudStorageConfigurationProvider(values["StorageConnectionString"]);

                ICloudStorageProvider cloudStorageProvider = new CloudStorageProvider(
                    storageConfigurationProvider,
                    tokenProvider);

                ICloudStorageConfigurationProvider premiumStorageConfigurationProvider =
                    new CloudStorageConfigurationProvider(values["PremiumStorageConnectionString"]);

                ICloudStorageProvider premiumCloudStorageProvider = new CloudStorageProvider(
                    premiumStorageConfigurationProvider,
                    tokenProvider);

                Random random = new Random();

                /* Define worker queues */
                WorkerQueueDefinition[] queueDefinitions = new WorkerQueueDefinition[] { 
                    new WorkerQueueDefinition()
                    {
                        QueueName = "RScript",
                        MaxParallelJobs = 1,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "DatasetGeneration",
                        MaxParallelJobs = 4,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "DatasetPostProcessExecution",
                        MaxParallelJobs = 4,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "DatasetBackendUpdate",
                        MaxParallelJobs = 4,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "LayerConversion",
                        MaxParallelJobs = 1,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "FastQueue",
                        MaxParallelJobs = 10,
                        PollIntervalMs = GlobalQueuePollTime
                    },
                    new WorkerQueueDefinition()
                    {
                        QueueName = "SyncMobile",
                        MaxParallelJobs = 2,
                        PollIntervalMs = GlobalQueuePollTime
                    }
                };

                IFactory<WorkerJobProcessor> rScriptProcessorFactory = new Factory<WorkerJobProcessor>(() =>
                {
                    return new RScriptProcessor(
                        cloudStorageProvider,
                        customSearchesDbContextFactory,
                        seriLogLogger,
                        LibrariesPath,
                        ExecutionPath);
                });

                IFactory<WorkerJobProcessor> datasetGenerationProcessorFactory = new Factory<WorkerJobProcessor>(() =>
                {
                    return new DatasetGenerationProcessor(customSearchesDbContextFactory, dynamicsODataHelperFactory, seriLogLogger);
                });

                IFactory<WorkerJobProcessor> datasetPostProcessExecutionProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new DatasetPostProcessExecutionProcessor(customSearchesDbContextFactory, seriLogLogger);
                });

                IFactory<WorkerJobProcessor> datasetBackendUpdateProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new DatasetBackendUpdateProcessor(
                        customSearchesDbContextFactory,
                        dynamicsODataHelperFactory,
                        seriLogLogger);
                });

                IFactory<WorkerJobProcessor> datasetDataUpdateProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new UpdateDatasetDataFromFileProcessor(
                        customSearchesDbContextFactory,
                        seriLogLogger);
                });

                IFactory<WorkerJobProcessor> applyLandRegressionToScheduleProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new ApplyLandRegressionToScheduleProcessor(customSearchesDbContextFactory, seriLogLogger);
                });

                IFactory<WorkerJobProcessor> layerConversionProcessor = new Factory<WorkerJobProcessor>(() =>
               {
                   return new LayerConversionProcessor(new ProcessFactory(), config, seriLogLogger);
               });

                IFactory<WorkerJobProcessor> importProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new ImportProcessor(seriLogLogger);
                });

                IFactory<WorkerJobProcessor> unsaveDatasetDeletionProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new UnsavedDatasetDeletionProcessor(seriLogLogger);
                });

                IFactory<WorkerJobProcessor> applyModelProcessorFactory = new Factory<WorkerJobProcessor>(() =>
                {
                    return new ApplyModelProcessor(customSearchesDbContextFactory, dynamicsODataHelperFactory, seriLogLogger);
                });

                IFactory<WorkerJobProcessor> exportProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new ExportProcessor(seriLogLogger, values["DynamicsApiRoute"], values["DynamicsODataUri"], values["DynamicsAuthUri"], values["DynamicsClientId"], values["DynamicsClientSecret"]);
                });

                IFactory<WorkerJobProcessor> transferProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new TransferProcessor(seriLogLogger, values["DynamicsApiRoute"], values["DynamicsODataUri"], values["DynamicsAuthUri"], values["DynamicsClientId"], values["DynamicsClientSecret"], cloudStorageProvider);
                });

                IFactory<WorkerJobProcessor> dynamicsSyncWorker = new Factory<WorkerJobProcessor>(() =>
                {
                    ClientCredential principalCredentials = new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]);
                    return new DynamicsSyncWorker(seriLogLogger, values["OrganizationId"], values["OrganizationName"], values["DynamicsODataUri"], values["DynamicsAuthUri"], values["DynamicsClientId"], values["SQLServerConnectionString"], values["DynamicsClientSecret"], principalCredentials
                        );
                });

                IFactory<WorkerJobProcessor> userProjectGenerationProcessorFactory = new Factory<WorkerJobProcessor>(() =>
                {
                    return new UserProjectGenerationProcessor(customSearchesDbContextFactory, dynamicsODataHelperFactory, seriLogLogger);
                });

                IFactory<WorkerJobProcessor> syncDBProcessor = new Factory<WorkerJobProcessor>(() =>
                {
                    return new SyncDBProcessor(seriLogLogger, values["DynamicsApiRoute"], values["DynamicsODataUri"], values["DynamicsAuthUri"], values["DynamicsClientId"], values["DynamicsClientSecret"], config, cloudStorageProvider);
                });

                IFactory<WorkerJobProcessor>[] processors = new IFactory<WorkerJobProcessor>[] {
                    rScriptProcessorFactory,
                    datasetGenerationProcessorFactory,
                    datasetPostProcessExecutionProcessor,
                    datasetBackendUpdateProcessor,
                    datasetDataUpdateProcessor,
                    applyLandRegressionToScheduleProcessor,
                    layerConversionProcessor,
                    unsaveDatasetDeletionProcessor,
                    importProcessor,
                    applyModelProcessorFactory,
                    exportProcessor,
                    dynamicsSyncWorker,
                    userProjectGenerationProcessorFactory,
                    transferProcessor,
                    syncDBProcessor,
                };

                IFactory<IAuthProvider> authTokenProviderFactory = new Factory<IAuthProvider>(() =>
                {
                    return new AuthProvider(customSearchesDbContextFactory, values["AdminRoles"]);
                });

                IMessagePublisherFactory messagePublisherFactory = new MessagePublisherFactory(values["AzureSignalRConnectionString"]);

                ServiceContextFactory serviceContextFactory = new ServiceContextFactory(
                    authTokenProviderFactory,
                    customSearchesDbContextFactory,
                    gisDbContextFactory,
                    customSearchesDataDbContextFactory,
                    workerJobDbContextFactory,
                    messagePublisherFactory,
                    cloudStorageProvider,
                    premiumCloudStorageProvider,
                    new ClientCredential(values["DynamicsClientId"], values["DynamicsClientSecret"]),
                    new TelemetryClient(telemetryConfiguration),
                    ExecutionPath,
                    values["JobQueueSuffix"],
                    values["SignalRHubEndpoint"],
                    seriLogLogger);

                Worker worker = new Worker(
                    workerJobDbContextFactory,
                    serviceContextFactory,
                    queueDefinitions,
                    processors,
                    values["SignalRHubEndpoint"],
                    values["MapServerStorageSharePath"],
                    seriLogLogger);

                if (args.Length > 0)
                {
                    string jobType = args[0];
                    int timeoutInSeconds = int.Parse(args[1]);
                    string jobPayload = null;

                    if (args.Length == 3)
                    {
                        jobPayload = args[2];
                    }

                    worker.RunJob(jobType, timeoutInSeconds, jobPayload);
                }
                else
                {
                    worker.Listen();
                }
            }
            catch (EvaluationException ex)
            {
                string errorMessage = $"Error evaluating exception: '{ex.GetBaseException().Message}'";
                if (seriLogLogger != null)
                {
                    seriLogLogger.LogError(ex, errorMessage);
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }

        private static string LoadScriptFromResourceFile(string scriptFileName, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = GetBaseParameters();
            }

            string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            string toReturn = File.ReadAllText(currentPath + "\\RScripts\\" + scriptFileName);

            foreach (string parameterName in parameters.Keys)
            {
                toReturn = toReturn.Replace("{" + parameterName + "}", parameters[parameterName]);
            }

            return toReturn;
        }

        private static Dictionary<string, string> GetBaseParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "LibrariesPath", LibrariesPath },
                { "PandocPath", PandocPath }
            };
            return parameters;
        }
    }
}