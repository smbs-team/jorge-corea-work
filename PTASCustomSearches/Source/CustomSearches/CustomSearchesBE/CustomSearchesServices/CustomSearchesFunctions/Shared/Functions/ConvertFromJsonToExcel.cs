namespace CustomSearchesFunctions.Shared.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Misc;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Azure function class for the ConvertFromJsonToExcel service.  The service converts from json object to excel file.
    /// </summary>
    public class ConvertFromJsonToExcel
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the json object.";

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertFromJsonToExcel" /> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public ConvertFromJsonToExcel(
            IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Converts from json object to excel file.
        /// </summary>
        /// <group>Shared</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Shared/ConvertFromJsonToExcel</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="JObject" in="body">The json object.</param>
        /// <returns>
        /// The file content result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        [FunctionName("ConvertFromJsonToExcel")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/Shared/ConvertFromJsonToExcel")] HttpRequest req,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    JObject jsonObject = null;
                    try
                    {
                        jsonObject = JsonConvert.DeserializeObject<JObject>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ConvertFromJsonToExcelService service = new ConvertFromJsonToExcelService(serviceContext);

                    Ref<string> fileName = new Ref<string>();
                    var fileBytes = await service.ConvertFromJsonToExcelAsync(
                        jsonObject,
                        fileName);

                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return new FileContentResult(fileBytes, contentType)
                    {
                        FileDownloadName = fileName.Value
                    };
                },
                req,
                log);
        }
    }
}
