namespace DeploymentHealthService
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using AutofacSerilogIntegration;
    using DeploymentHealthService.Model;
    using ILinxSoapImport;
    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Represent Autofac configuration.
    /// </summary>
    public class AutofacConfig
    {
        private const string ActivationId = "ActivationId";
        private const string ApplicationName = "ApplicationName";
        private const string EdmsSoapServicesEndpoint = "EdmsSoapServicesEndpoint";
        private const string Password = "IlinxPassword";
        private const string UserName = "IlinxUserName";
        private const string BlobStorage = "BlobStorage";
        private const string PremiumBlobStorage = "PremiumBlobStorage";
        private const string SqlServerConnectionString = "SqlServerConnectionString";
        private const string BlobStorageContainer = "BlobStorageContainer";
        private const string FinalizerURL = "FinalizerURL";
        private const string CognitiveEndPoint = "CognitiveEndPoint";
        private const string CognitiveSubscriptionKey = "CognitiveSubscriptionKey";
        private const string DynamicsApiURL = "DynamicsApiURL";

        private const string CRMUri = "CRMUri";
        private const string AuthUri = "AuthUri";
        private const string ClientId = "ClientId";
        private const string ClientSecret = "ClientSecret";
        private const string MBToken = "MBToken";
        private const string MapboxUri = "MapboxUri";

        /// <summary>
        /// Gets or sets configured instance of <see cref="IContainer"/>.
        /// <remarks><see cref="Configure"/> must be called before trying to get Container instance.</remarks>
        /// </summary>
        public static IContainer Container { get; set; }

        /// <summary>
        /// Initializes and configures instance of <see cref="IContainer"/>.
        /// </summary>
        /// <param name="config">Http configuration.</param>
        public static void Configure(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterLogger();
            builder.RegisterInstance(AutoMapperConfig.Configure(config));

            RegisterServices(builder);

            Container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

#pragma warning disable RECS0154 // Parameter is never used
        private static void RegisterServices(ContainerBuilder builder)
#pragma warning restore RECS0154 // Parameter is never used
        {
            // TODO: Register additional services for injection.
            builder.Register(c => GetAppSettings(ConfigurationManager.AppSettings)).As<IConfigParams>();
            builder.Register(c => GetDynamicsAppSettings(ConfigurationManager.AppSettings)).As<IDynamicsConfigurationParams>();
            builder.RegisterType<ILinxHelper>().As<IILinxHelper>();
        }

        private static ConfigParams GetAppSettings(NameValueCollection src)
        {
            if (src == null || src.Count == 0)
            {
                throw new ArgumentNullException(nameof(src));
            }

            ConfigParams toReturn = new ConfigParams
            {
                ActivationId = src[ActivationId],
                ApplicationName = src[ApplicationName],
                EdmsSoapServicesEndpoint = src[EdmsSoapServicesEndpoint],
                Password = src[Password],
                UserName = src[UserName],
                BlobStorageConnectionString = src[BlobStorage],
                PremiumBlobStorageConnectionString = src[PremiumBlobStorage],
                SqlServerConnectionString = src[SqlServerConnectionString],
                BlobStorageContainer = src[BlobStorageContainer],
                FinalizerUrl = src[FinalizerURL],
                CognitiveSubscriptionKey = src[CognitiveSubscriptionKey],
                CognitiveEndPoint = src[CognitiveEndPoint],
                DynamicsApiURL = src[DynamicsApiURL],
            };

            return toReturn;
        }

        private static DynamicsConfigParams GetDynamicsAppSettings(NameValueCollection src)
        {
            if (src == null || src.Count == 0)
            {
                throw new ArgumentNullException(nameof(src));
            }

            DynamicsConfigParams toReturn = new DynamicsConfigParams
            {
                ClientId = src[ClientId],
                ClientSecret = src[ClientSecret],
                CRMUri = src[CRMUri],
                AuthUri = src[AuthUri],
                MBToken = src[MBToken],
                MapboxUri = src[MapboxUri]
            };

            return toReturn;
        }
    }
}