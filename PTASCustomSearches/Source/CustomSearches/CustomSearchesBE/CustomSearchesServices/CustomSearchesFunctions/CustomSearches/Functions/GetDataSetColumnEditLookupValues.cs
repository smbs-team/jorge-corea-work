namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
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
    /// Azure function class for the GetDataSetColumnEditLookupValues service.  The service gets the dataset column edit lookup values.
    /// </summary>
    public class GetDataSetColumnEditLookupValues
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
        /// Initializes a new instance of the <see cref="GetDataSetColumnEditLookupValues"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory/dbContextFactory parameter is null.</exception>
        public GetDataSetColumnEditLookupValues(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the dataset column edit lookup values.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDataSetColumnEditLookupValues/{datasetId}/{columnName}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="columnName"  cref="string" in="path">The column name.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="T:CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches.CustomSearchParameterValueData[]" in="body">Dependent parameters.</param>
        /// <returns>
        /// The dataset column edit lookup values.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("GetDataSetColumnEditLookupValues")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/GetDataSetColumnEditLookupValues/{datasetId}/{columnName}")] HttpRequest req, Guid datasetId, string columnName, ILogger log)
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
                    GetDataSetColumnEditLookupValuesService service = new GetDataSetColumnEditLookupValuesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var response = await service.GetDataSetColumnEditLookupValuesAsync(datasetId, columnName, dependedParameterValues, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
