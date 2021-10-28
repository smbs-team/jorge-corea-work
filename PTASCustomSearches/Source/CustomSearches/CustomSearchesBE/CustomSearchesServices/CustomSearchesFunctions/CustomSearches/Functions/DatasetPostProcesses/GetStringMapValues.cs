namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure function class for the GetStringMapValues service.  The service gets the string map values.
    /// </summary>
    public class GetStringMapValues
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the string map properties data.";

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStringMapValues"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public GetStringMapValues(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the string map values.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetStringMapValues</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="GetStringMapValuesData" in="body">The string map properties to look for.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200" cref="GetStringMapValuesResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        [FunctionName("GetStringMapValues")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/GetStringMapValues")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    GetStringMapValuesData getStringMapValuesData;
                    try
                    {
                        getStringMapValuesData = JsonConvert.DeserializeObject<GetStringMapValuesData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetStringMapValuesService service = new GetStringMapValuesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = serviceContext.DbContextFactory.Create())
                    {
                        var response = await service.GetStringMapValuesAsync(getStringMapValuesData, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
