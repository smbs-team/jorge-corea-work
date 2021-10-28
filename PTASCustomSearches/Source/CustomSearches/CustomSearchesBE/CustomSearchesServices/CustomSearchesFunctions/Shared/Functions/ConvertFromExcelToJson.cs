namespace CustomSearchesFunctions.Shared.Functions
{
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Azure function class for the ConvertFromExcelToJson service.  The service converts excel file to json object.
    /// </summary>
    public class ConvertFromExcelToJson
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertFromExcelToJson"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory is null.</exception>
        public ConvertFromExcelToJson(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Converts excel file to json object.
        /// </summary>
        /// <group>Shared</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Shared/ConvertFromExcelToJson</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="file" cref="string" format="binary" in="body">The excel file.</param>
        /// <param name="hasHeader" cref="bool" in="query">Value indicating whether the excel contains headers.</param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        [FunctionName("ConvertFromExcelToJson")]
        public async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/Shared/ConvertFromExcelToJson")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool hasHeader = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("hasHeader") == true)
                        {
                            hasHeader = bool.Parse(req.Query["hasHeader"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ConvertFromExcelToJsonService service = new ConvertFromExcelToJsonService(serviceContext);
                    object result = await service.ConvertFromExcelToJsonAsync(req.Body, hasHeader);
                    return new OkObjectResult(result);
                },
                req,
                log);
        }
    }
}
