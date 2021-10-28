// < copyright file = "SwashBuckleStartup.cs" company = "PlaceholderCompany" >
//   Copyright(c) King County .All rights reserved.
// </copyright>

////namespace MediaInfo
////{
////    using System.Net.Http;
////    using System.Threading.Tasks;

////    using AzureFunctions.Extensions.Swashbuckle;
////    using AzureFunctions.Extensions.Swashbuckle.Attribute;

////    using Microsoft.Azure.WebJobs;
////    using Microsoft.Azure.WebJobs.Extensions.Http;

////    /// <summary>
////    /// Swagger Functions.
////    /// </summary>
////    public static class SwaggerFunctions
////    {
////        /// <summary>
////        /// Swagger.
////        /// </summary>
////        /// <param name="req">Request.</param>
////        /// <param name="swashBuckleClient">Client.</param>
////        /// <returns>Nothing.</returns>
////        [SwaggerIgnore]
////        [FunctionName("Swagger")]
////        public static Task<HttpResponseMessage> Swagger(
////            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/json")]
////            HttpRequestMessage req,
////            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
////        {
////            return Task.FromResult(swashBuckleClient.CreateSwaggerDocumentResponse(req));
////        }

////        /// <summary>
////        /// Swagger UI.
////        /// </summary>
////        /// <param name="req">Request.</param>
////        /// <param name="swashBuckleClient">Client.</param>
////        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
////        [SwaggerIgnore]
////        [FunctionName("SwaggerUi")]
////        public static Task<HttpResponseMessage> SwaggerUi(
////            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/ui")]
////            HttpRequestMessage req,
////            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
////        {
////            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
////        }
////    }
////}
