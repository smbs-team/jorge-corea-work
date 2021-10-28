namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Gis.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the DeleteFolder service.  The service deletes the folder.
    /// </summary>
    public class DeleteFolder
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the folder data.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<GisDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolder"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public DeleteFolder(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Deletes the user map.
        /// Returns Conflict(409) if the folder could not be deleted.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/GIS/DeleteFolder</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="CreateFolderData" in="body">Folder data.</param>
        /// <returns>
        /// The delete folder result.
        /// </returns>
        [FunctionName("DeleteFolder")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/GIS/DeleteFolder")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    CreateFolderData folderData = null;
                    try
                    {
                        folderData = JsonConvert.DeserializeObject<CreateFolderData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    DeleteFolderService service = new DeleteFolderService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        DeleteFolderResponse response = await service.DeleteFolderAsync(folderData, dbContext);

                        if (!string.IsNullOrWhiteSpace(response.Message))
                        {
                            return new ConflictObjectResult(response);
                        }

                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
