// <copyright file="AutofacConfig.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using AutofacSerilogIntegration;

    using ILinxSoapImport;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

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
        private const string BlobStorageContainer = "BlobStorageContainer";
        private const string FinalizerURL = "FinalizerURL";
        private const string CognitiveEndPoint = "CognitiveEndPoint";
        private const string CognitiveSubscriptionKey = "CognitiveSubscriptionKey";
        private const string DocuSignAccountId = "DocuSignAccountId";
        private const string DocuSignApiUrl = "DocuSignApiUrl";
        private const string DocuSignAuthServer = "DocuSignAuthServer";
        private const string DocusignPrivateKey = "DocuSignPrivateKey";
        private const string DocuSignImpersonatedUserId = "DocuSignImpersonatedUserId";
        private const string DocuSignIntegratorId = "DocuSignIntegratorId";
        private const string DynamicsApiURL = "DynamicsApiURL";
        private const string SketchContainer = "SketchContainer";
        private const string LoadDocusignHtmlFromBlob = "LoadDocusignHtmlFromBlob";
        private const string JSONContainerName = "JSONContainerName";
        private const string ProcessedJSONContainerName = "ProcessedJSONContainerName";
        private const string SautinLicense = "SautinLicense";
        private const string SharepointApiURL = "SharepointApiURL";

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

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.Register(c => GetAppSettings(ConfigurationManager.AppSettings)).As<IConfigParams>();
            builder
                .Register(c => new CloudStorageConfigurationProvider(c.Resolve<IConfigParams>().BlobStorageConnectionString))
                .As<ICloudStorageConfigurationProvider>();

            builder.Register(c => new ImageAnalysisHelper(c.Resolve<IConfigParams>())).As<IImageAnalysisHelper>();
            builder.RegisterType<ContentStoreHelper>().As<IContentStoreHelper>();
            builder.RegisterType<ILinxHelper>().As<IILinxHelper>();
            builder.RegisterType<CloudStorageProvider>().As<ICloudStorageProvider>();
            builder.RegisterType<CacheManager<ConvertFileResults>>().As<ICacheManager<ConvertFileResults>>();
            builder.RegisterType<CacheManager<DocumentResult>>().As<ICacheManager<DocumentResult>>();
            builder.RegisterType<CacheManager<ConvertFileResults>>().As<ICacheManager<ConvertFileResults>>();
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
                BlobStorageContainer = src[BlobStorageContainer],
                FinalizerUrl = src[FinalizerURL],
                CognitiveSubscriptionKey = src[CognitiveSubscriptionKey],
                CognitiveEndPoint = src[CognitiveEndPoint],
                DynamicsApiURL = src[DynamicsApiURL],
                SketchContainer = src[SketchContainer],
                LoadDocusignHtmlFromBlob = src[LoadDocusignHtmlFromBlob]?.ToLower().Equals("true") ?? false,
                JSONContainerName = src[JSONContainerName] ?? "json-store",
                ProcessedJSONContainerName = src[ProcessedJSONContainerName ?? "json-store-processed"],
                SautinLicense = src[SautinLicense] ?? "70040677759,20091458462",
                SharepointApiURL = src[SharepointApiURL] ?? string.Empty,
            };

            if (string.IsNullOrWhiteSpace(toReturn.ActivationId))
            {
                throw new ArgumentNullException(ActivationId);
            }

            if (string.IsNullOrWhiteSpace(toReturn.ApplicationName))
            {
                throw new ArgumentNullException(ApplicationName);
            }

            if (string.IsNullOrWhiteSpace(toReturn.EdmsSoapServicesEndpoint))
            {
                throw new ArgumentNullException(EdmsSoapServicesEndpoint);
            }

            if (string.IsNullOrWhiteSpace(toReturn.Password))
            {
                throw new ArgumentNullException(Password);
            }

            if (string.IsNullOrWhiteSpace(toReturn.UserName))
            {
                throw new ArgumentNullException(UserName);
            }

            if (string.IsNullOrWhiteSpace(toReturn.BlobStorageConnectionString))
            {
                throw new ArgumentNullException(BlobStorage);
            }

            if (string.IsNullOrWhiteSpace(toReturn.FinalizerUrl))
            {
                throw new ArgumentNullException(FinalizerURL);
            }

            if (string.IsNullOrWhiteSpace(toReturn.BlobStorageContainer))
            {
                throw new ArgumentNullException(BlobStorageContainer);
            }

            return toReturn;
        }
    }
}