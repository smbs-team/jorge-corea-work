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
    /// Azure function class for the SetUserMapCategories service.  The service sets the user map categories.
    /// </summary>
    public class SetUserMapCategories
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the user map categories.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<GisDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetUserMapCategories"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public SetUserMapCategories(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Sets the user map categories.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/GIS/SetUserMapCategories/{userMapId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userMapId">The user map id.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="SetUserMapCategoriesData" in="body">The user map categories data to set.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [FunctionName("SetUserMapCategories")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/GIS/SetUserMapCategories/{userMapId}")] HttpRequest req, int userMapId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    SetUserMapCategoriesData setUserMapCategoriesData = null;
                    try
                    {
                        setUserMapCategoriesData = JsonConvert.DeserializeObject<SetUserMapCategoriesData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    SetUserMapCategoriesService service = new SetUserMapCategoriesService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        await service.SetUserMapCategories(userMapId, setUserMapCategoriesData, dbContext);
                    }

                    return new OkResult();
                },
                req,
                log);
        }
    }
}
