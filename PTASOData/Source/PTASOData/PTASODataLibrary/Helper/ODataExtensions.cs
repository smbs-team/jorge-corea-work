namespace PTASODataLibrary.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNet.OData.Formatter.Serialization;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNet.OData.Query.Validators;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Net.Http.Headers;
    using Microsoft.OData;
    using Microsoft.OData.Edm;
    using Microsoft.OData.Edm.Csdl;
    using Microsoft.OData.Edm.Validation;
    using Microsoft.OData.Json;
    using Microsoft.OData.UriParser;
    using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

    /// <summary>
    /// Extensions to allow executing of oData queries in Azure funcitons.
    /// </summary>
    public static class ODataExtensions
    {
        private static readonly object InitializationLock = new object();

        /// <summary>
        /// Fake service provider.
        /// </summary>
        private static IServiceProvider provider = null;

        /// <summary>
        /// Fake route builder.
        /// </summary>
        private static IRouteBuilder routeBuilder = null;

        /// <summary>
        /// The OData convention model builder.
        /// </summary>
        private static ODataConventionModelBuilder modelBuilder = null;

        /// <summary>
        /// The EDM model.
        /// </summary>
        private static IEdmModel model = null;

        /// <summary>
        /// Gets or sets a value indicating whether the OData updates are allowed.
        /// </summary>
        public static bool AllowODataUpdates { get; set; }

        /// <summary>
        /// Resets the state of the extensions to starting point.  (Used for test re-entrance).
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public static void Reset()
        {
            ODataExtensions.provider = null;
            ODataExtensions.routeBuilder = null;
            ODataExtensions.modelBuilder = null;
            ODataExtensions.model = null;
        }

        /// <summary>
        /// Registers the entity into the OData entity model.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public static void RegisterEntity<TEntity>()
            where TEntity : class
        {
            RegisterEntity(typeof(TEntity));
        }

        /// <summary>
        /// Registers the entity into the OData entity model.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public static void RegisterEntity(Type entityType)
        {
            ODataConventionModelBuilder modelBuilder = ODataExtensions.GetModelBuilder();
            EntityTypeConfiguration entityConfiguration = modelBuilder.AddEntityType(entityType);
            modelBuilder.AddEntitySet(entityType.Name, entityConfiguration);
        }

        /// <summary>
        /// Applies OData query from the request to an existing query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="internalRequest">The internal/simulated request.</param>
        /// <param name="query">The query.</param>
        /// <returns>A new query with the OData filtering.</returns>
        /// <exception cref="ArgumentNullException"> Request or internalRequest or query.</exception>
        public static IQueryable ApplyTo<TEntity>(this HttpRequest request, HttpRequest internalRequest, IQueryable<TEntity> query)
            where TEntity : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (internalRequest == null)
            {
                throw new ArgumentNullException(nameof(internalRequest));
            }

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            IServiceProvider provider = ODataExtensions.BuildMvcService();

            // Simulate an HTTP request as if it comes from ASP.NET Core
            var edmModel = ODataExtensions.GetEdmModel();

            var oDataQueryContext = new ODataQueryContext(edmModel, typeof(TEntity), new Microsoft.AspNet.OData.Routing.ODataPath());

            var odataQuery = new ODataQueryOptions<TEntity>(oDataQueryContext, internalRequest);

            // Applies OData filter to the query
            return odataQuery.ApplyTo(query.AsQueryable());
        }

        /// <summary>
        /// Gets the service meta-data.
        /// </summary>
        /// <returns>The Service meta-data.</returns>
        public static async Task<string> GetMetadata()
        {
            IEnumerable<EdmError> errors;
            var edmModel = ODataExtensions.GetEdmModel();
            MemoryStream stream = new MemoryStream(1024 * 1014 * 100);
            ODataMessageWriterSettings writterSettings = new ODataMessageWriterSettings();
            writterSettings.EnableMessageStreamDisposal = false;

            XmlWriter xmlWriter = ODataSerializationHelper.CreateXmlWriter(stream, writterSettings, Encoding.UTF8);
            if (!CsdlWriter.TryWriteCsdl(edmModel, xmlWriter, CsdlTarget.OData, out errors))
            {
                throw new InvalidOperationException("Error generating meta-data");
            }

            xmlWriter.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            byte[] binaryMetadata = new byte[stream.Length];
            await stream.ReadAsync(binaryMetadata, 0, (int)stream.Length);
            string metadata = Encoding.UTF8.GetString(binaryMetadata);

            return metadata;
        }

        /// <summary>
        /// Gets the OData formatted response for a given query.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="internalRequest">The internal/simulated request.</param>
        /// <param name="entities">List of entities to be serialized.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>Query results in OData format.</returns>
        public static async Task<string> SerializeIntoODataResponse(this HttpRequest request, HttpRequest internalRequest, IList entities, Type entityType)
        {
            var httpContext = internalRequest.HttpContext;
            Type objectType = entities.GetType();
            MediaTypeHeaderValue contentType = GetContentType(request.Headers[HeaderNames.ContentType].FirstOrDefault());

            IServiceProvider serviceProvider = ODataExtensions.provider;
            ActionContextAccessor actionContextAccessor = serviceProvider.GetService<ActionContextAccessor>();
            actionContextAccessor.ActionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData()
            };

            string result = string.Empty;

            using (Stream memoryStream = new MemoryStream())
            {
                ODataMessageWrapper messageWrapper = new ODataMessageWrapper(
                    memoryStream,
                    request.Headers.ToDictionary(kvp => kvp.Key, kvp => string.Join(";", kvp.Value)));
                messageWrapper.Container = serviceProvider;

                ODataSerializationHelper.WriteToStream(
                    entityType,
                    (object)entities,
                    ODataExtensions.GetEdmModel(),
                    ODataVersion.V4,
                    contentType,
                    internalRequest,
                    messageWrapper);

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (StreamReader readStream = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    result = await readStream.ReadToEndAsync();
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the internal HTTP context.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A fake context with the same info as the request parameter.
        /// </returns>
        public static HttpContext CreateInternalHttpContext(this HttpRequest request)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = provider
            };

            httpContext.Request.Method = "GET";
            httpContext.Request.Scheme = request.Scheme;
            httpContext.Request.Host = request.Host;
            httpContext.Request.Path = request.Path;
            httpContext.Request.QueryString = request.QueryString;

            return httpContext;
        }

        /// <summary>
        /// Gets the Edm model.
        /// </summary>
        /// <returns>The Edm model.</returns>
        public static IEdmModel GetEdmModel()
        {
            if (ODataExtensions.model == null)
            {
                var modelBuilder = ODataExtensions.GetModelBuilder();

                lock (ODataExtensions.InitializationLock)
                {
                    if (ODataExtensions.model == null)
                    {
                        ODataExtensions.model = modelBuilder.GetEdmModel();
                    }
                }
            }

            return ODataExtensions.model;
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="contentTypeValue">The content type value.</param>
        private static MediaTypeHeaderValue GetContentType(string contentTypeValue)
        {
            MediaTypeHeaderValue contentType = null;
            if (!string.IsNullOrEmpty(contentTypeValue))
            {
                MediaTypeHeaderValue.TryParse(contentTypeValue, out contentType);
            }

            return contentType;
        }

        /// <summary>
        /// Builds the MVC service.
        /// </summary>
        private static IServiceProvider BuildMvcService()
        {
            if (ODataExtensions.provider == null)
            {
                lock (ODataExtensions.InitializationLock)
                {
                    if (provider == null)
                    {
                        // Register service components.
                        IServiceCollection collection = new ServiceCollection()
                            .AddMvcCore().Services
                            .AddOData().Services
                            .AddTransient<ODataUriResolver>()
                            .AddTransient<ODataQueryValidator>()
                            .AddTransient<TopQueryValidator>()
                            .AddTransient<FilterQueryValidator>()
                            .AddTransient<SkipQueryValidator>()
                            .AddTransient<OrderByQueryValidator>()
                            .AddTransient<ActionContextAccessor>()
                            .AddTransient<ODataResourceSetSerializer>()
                            .AddTransient<ODataCollectionSerializer>()
                            .AddTransient<ODataResourceSerializer>()
                            .AddTransient<ODataRawValueSerializer>()
                            .AddTransient<ODataPrimitiveSerializer>()
                            .AddTransient<ODataMediaTypeResolver>()
                            .AddTransient<ODataMessageInfo>()
                            .AddTransient<ODataPayloadValueConverter>()
                            .AddTransient<ODataSimplifiedOptions>()
                            .AddTransient<ODataUriParserSettings>()
                            .AddTransient<UriPathParser>()
                            .AddTransient<IJsonWriterFactory, DefaultJsonWriterFactory>()
                            .AddTransient<ODataSerializerProvider, DefaultODataSerializerProvider>()
                            .AddTransient<IEdmModel>((serviceProvider) => { return ODataExtensions.model; })
                            .AddTransient<ODataMessageWriterSettings>((serviceProvider) =>
                            {
                                return new ODataMessageWriterSettings
                                {
                                    EnableMessageStreamDisposal = false,
                                    MessageQuotas = new ODataMessageQuotas { MaxReceivedMessageSize = long.MaxValue },
                                };
                            });

                        ODataExtensions.provider = collection.BuildServiceProvider();
                        ODataExtensions.routeBuilder = new RouteBuilder(new ApplicationBuilder(provider));
                        ODataExtensions.routeBuilder.EnableDependencyInjection();
                    }
                }
            }

            return ODataExtensions.provider;
        }

        /// <summary>
        /// Gets the model builder.
        /// </summary>
        private static ODataConventionModelBuilder GetModelBuilder()
        {
            var provider = ODataExtensions.BuildMvcService();

            if (ODataExtensions.modelBuilder == null)
            {
                lock (ODataExtensions.InitializationLock)
                {
                    if (ODataExtensions.modelBuilder == null)
                    {
                        ODataExtensions.modelBuilder = new ODataConventionModelBuilder(provider);
                    }
                }
            }

            return ODataExtensions.modelBuilder;
        }
    }
}
