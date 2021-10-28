namespace CustomSearchesFunctions.Auth.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Azure function class to get an user information.
    /// </summary>
    public class GetUserInfo
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserInfo"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetUserInfo(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets an user information.
        /// </summary>
        /// <group>Auth</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Auth/GetUserInfo/{userId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userId" cref="Guid" in="path">The user id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The user info.
        /// </returns>
        [FunctionName("GetUserInfo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Auth/GetUserInfo/{userId}")] HttpRequest req, Guid userId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromUserIdAsync(userId, runningInJobContext: false);
                    UserInfoData userInfoData = new UserInfoData()
                    {
                        Id = serviceContext.AuthProvider.UserInfoData.Id,
                        FullName = serviceContext.AuthProvider.UserInfoData.FullName,
                        Email = serviceContext.AuthProvider.UserInfoData.Email
                    };

                    return new JsonResult(userInfoData);
                },
                req,
                log);
        }
    }
}
