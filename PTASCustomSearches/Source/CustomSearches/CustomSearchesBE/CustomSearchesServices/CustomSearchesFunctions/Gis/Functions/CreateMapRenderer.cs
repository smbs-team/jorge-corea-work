namespace CustomSearchesFunctions.Gis.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Gis.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the CreateMapRenderer service.  The service creates the map renderer.
    /// </summary>
    public class CreateMapRenderer
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the map renderer.";

        /// <summary>
        /// The SQL Server custom searches db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> customSearchesDbContextFactory;

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<GisDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMapRenderer"/> class.
        /// </summary>
        /// <param name="customSearchesDbContextFactory">The SQL Server custom searches db context factory.</param>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public CreateMapRenderer(IFactory<CustomSearchesDbContext> customSearchesDbContextFactory, IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
        {
            if (customSearchesDbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(customSearchesDbContextFactory));
            }

            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.customSearchesDbContextFactory = customSearchesDbContextFactory;
            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Creates the map renderer.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/GIS/CreateMapRenderer</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="MapRendererData" in="body">Custom search parameters.</param>
        /// <returns>
        /// The map renderer id.
        /// </returns>
        [FunctionName("CreateMapRenderer")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/GIS/CreateMapRenderer")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    MapRendererData mapRendererData = null;
                    try
                    {
                        mapRendererData = JsonConvert.DeserializeObject<MapRendererData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    CreateMapRendererService service = new CreateMapRendererService(serviceContext);
                    using (CustomSearchesDbContext customSearchesDbContext = this.customSearchesDbContextFactory.Create())
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        int renderMapId = await service.CreateMapRenderer(customSearchesDbContext, dbContext, mapRendererData);
                        return new OkObjectResult(renderMapId);
                    }
                },
                req,
                log);
        }
    }
}
