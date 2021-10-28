namespace CustomSearchesFunctions.CustomSearches.Functions.CustomSearches
{
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
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
    /// Azure function class for the GetCustomSearchParameterLookupValues service.  The service gets lookup values for a custom search parameter.
    /// </summary>
    public class GetCustomSearchParameterLookupValues
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
        /// Initializes a new instance of the <see cref="GetCustomSearchParameterLookupValues"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetCustomSearchParameterLookupValues(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the custom search parameter lookup values.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetCustomSearchParameterLookupValues/{customSearchId}/{parameterName}</url>
        /// <param name="req">The request.</param>
        /// <param name="customSearchId" cref="int" in="path">The custom search definition id.</param>
        /// <param name="parameterName"  cref="string" in="path">The parameter id.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="T:CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches.CustomSearchParameterValueData[]" in="body">Dependent parameters.</param>
        /// <returns>
        /// The dataset column edit lookup values.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetCustomSearchParameterLookupValues")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/GetCustomSearchParameterLookupValues/{customSearchId}/{parameterName}")] HttpRequest req, int customSearchId, string parameterName, ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            CustomSearchParameterValueData[] dependedParameterValues = null;
            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    dependedParameterValues = JsonConvert.DeserializeObject<CustomSearchParameterValueData[]>(body);
                }
                catch (Newtonsoft.Json.JsonException jsonSerializationException)
                {
                    throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                }
            }

            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetCustomSearchParameterLookupValuesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var response = await service.GetCustomSearchParameterLookupValuesAsync(customSearchId, parameterName, dependedParameterValues, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
