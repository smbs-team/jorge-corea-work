namespace CustomSearchesFunctions.Auth.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Azure function class to get the current user information.
    /// </summary>
    public class GetCurrentUserInfo
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentUserInfo"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetCurrentUserInfo(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the current user information.
        /// </summary>
        /// <group>Auth</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Auth/GetCurrentUserInfo/</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The user info.
        /// </returns>
        [FunctionName("GetCurrentUserInfo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Auth/GetCurrentUserInfo/")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    return new JsonResult(serviceContext.AuthProvider.UserInfoData);
                },
                req,
                log);
        }
    }
}
