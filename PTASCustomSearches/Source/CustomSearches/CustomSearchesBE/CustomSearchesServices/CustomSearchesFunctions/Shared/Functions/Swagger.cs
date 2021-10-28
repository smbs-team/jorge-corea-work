namespace CustomSearchesFunctions.Shared.Functions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration;
    using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration.Models;
    using Microsoft.OpenApi.Extensions;

    /// <summary>
    /// Swagger function.
    /// </summary>
    public static class Swagger
    {
        /// <summary>
        /// Runs the swagger.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="log">The log.</param>
        /// <param name="executionContext">The execution context.</param>
        /// <returns>
        /// Swagger document.
        /// </returns>
        [FunctionName("Swagger")]
        public static async Task<HttpResponseMessage> RunSwagger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Swagger")] HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {
            string xmlFilePath = Path.Combine(
                executionContext?.FunctionAppDirectory ?? string.Empty,
                @"CustomSearchesFunctions.xml");

            string assemblyPah = Path.Combine(
                executionContext?.FunctionAppDirectory ?? string.Empty,
                @"bin\CustomSearchesFunctions.dll");

            var input = new OpenApiGeneratorConfig(
            annotationXmlDocuments: new List<XDocument>()
            {
                XDocument.Load(xmlFilePath),
            },
            assemblyPaths: new List<string>()
            {
               assemblyPah
            },
            openApiDocumentVersion: "V1",
            filterSetVersion: FilterSetVersion.V1);

            input.OpenApiInfoDescription = "Custom Searches API Definition.";

            var generator = new OpenApiGenerator();
            var openApiDocuments = generator.GenerateDocuments(
                openApiGeneratorConfig: input,
                generationDiagnostic: out GenerationDiagnostic result);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    openApiDocuments.First().Value.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0),
                    Encoding.UTF8,
                    "application/json")
            };
        }
    }
}
