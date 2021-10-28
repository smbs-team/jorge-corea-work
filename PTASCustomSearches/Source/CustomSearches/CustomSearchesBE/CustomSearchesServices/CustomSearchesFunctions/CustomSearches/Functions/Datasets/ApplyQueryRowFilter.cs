namespace CustomSearchesFunctions.CustomSearches.Functions.Datasets
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
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
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ApplyQueryRowFilter service.  The service applies
    /// the query row filter in the dataset data.
    /// </summary>
    public class ApplyQueryRowFilter
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the custom search grid data.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyQueryRowFilter"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ApplyQueryRowFilter(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Applies the query row filter in the dataset data.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ApplyQueryRowFilter/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="QueryRowFilterData" in="body">The query row filter.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("ApplyQueryRowFilter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ApplyQueryRowFilter/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool usePostProcess = true;
                    int? postProcessId = null;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("usePostProcess") == true)
                        {
                            usePostProcess = bool.Parse(req.Query["usePostProcess"].ToString());
                        }

                        if (req.Query.ContainsKey("postProcessId") == true)
                        {
                            postProcessId = int.Parse(req.Query["postProcessId"].ToString());
                        }
                    }

                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    QueryRowFilterData queryRowFilterData;
                    try
                    {
                        queryRowFilterData = JsonConvert.DeserializeObject<QueryRowFilterData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ApplyQueryRowFilterService service = new ApplyQueryRowFilterService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        await service.ApplyQueryRowFilterAsync(
                            datasetId,
                            usePostProcess,
                            postProcessId,
                            queryRowFilterData,
                            dbContext);

                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
