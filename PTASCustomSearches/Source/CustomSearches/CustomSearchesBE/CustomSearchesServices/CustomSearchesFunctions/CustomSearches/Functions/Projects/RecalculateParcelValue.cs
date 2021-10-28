namespace CustomSearchesFunctions.CustomSearches.Functions.Projects
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Projects;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Projects;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure function class for the ApplyModel service.  The service recalculates the parcel value.
    /// </summary>
    public class RecalculateParcelValue
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the recalculate parcel value data.";

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateParcelValue"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public RecalculateParcelValue(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Recalculates the parcel value.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/RecalculateParcelValue/{projectId}/{major}/{minor}</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="RecalculateParcelValueData" in="body">Colum data.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200" cref="ExecuteCustomSearchResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="500" cref="ErrorResultModel">Internal server error.</response>
        [FunctionName("RecalculateParcelValue")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/RecalculateParcelValue/")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    RecalculateParcelValueData recalculateParcelValueData;
                    try
                    {
                        recalculateParcelValueData = JsonConvert.DeserializeObject<RecalculateParcelValueData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    RecalculateParcelValueService service = new RecalculateParcelValueService(serviceContext);
                    using (CustomSearchesDbContext dbContext = serviceContext.DbContextFactory.Create())
                    {
                        CloudBlobContainer blobContainer = await serviceContext.CloudStorageProvider.GetCloudBlobContainer(GetDatasetFile.RScriptBlobContainerName, serviceContext.AppCredential);
                        var result = await service.RecalculateParcelValueAsync(recalculateParcelValueData, dbContext);

                        if (string.IsNullOrWhiteSpace(result))
                        {
                            return new OkResult();
                        }

                        ErrorResultModel error = new ErrorResultModel { Message = result };
                        ObjectResult objectResult = new ObjectResult(error);
                        objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return objectResult;
                    }
                },
                req,
                log);
        }
    }
}
