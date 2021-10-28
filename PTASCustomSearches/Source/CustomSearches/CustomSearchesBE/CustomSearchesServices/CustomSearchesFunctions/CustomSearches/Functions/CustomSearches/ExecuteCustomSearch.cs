namespace CustomSearchesFunctions.CustomSearches.Functions.CustomSearches
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ExecuteCustomSearch service.  The service executes a custom search
    /// and returns an id that can be used to retrieve the data.
    /// </summary>
    public class ExecuteCustomSearch
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the custom search parameters.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteCustomSearch"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ExecuteCustomSearch(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Executes a custom search.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ExecuteCustomSearch/{customSearchDefinitionId}</url>
        /// <param name="req">The request.</param>
        /// <param name="customSearchDefinitionId" cref="int" in="path">The custom search definition to execute.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="ExecuteCustomSearchData" in="body">Custom search data.</param>
        /// <param name="validate" cref="bool" in="query">Value indicating whether the custom search should be validated.</param>
        /// <returns>
        /// A unique Id that can be used to get the database data.
        /// </returns>
        /// <response code="200" cref="ExecuteCustomSearchResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("ExecuteCustomSearch")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ExecuteCustomSearch/{customSearchDefinitionId}")] HttpRequest req, int customSearchDefinitionId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    ExecuteCustomSearchData customSearchData = null;
                    try
                    {
                        customSearchData = JsonConvert.DeserializeObject<ExecuteCustomSearchData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    bool validate = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("validate") == true)
                        {
                            validate = bool.Parse(req.Query["validate"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ExecuteCustomSearchService service = new ExecuteCustomSearchService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        ExecuteCustomSearchResponse response = await service.QueueExecuteCustomSearchAsync(customSearchDefinitionId, customSearchData, validate, dbContext);

                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
