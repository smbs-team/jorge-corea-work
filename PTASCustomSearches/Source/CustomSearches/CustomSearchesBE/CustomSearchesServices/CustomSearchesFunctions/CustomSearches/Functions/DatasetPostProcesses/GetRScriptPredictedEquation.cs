namespace CustomSearchesFunctions.CustomSearches.Functions.DatasetPostProcesses
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetRScriptPredictedEquation service.
    /// The service gets the predicted equation for the RScript post process.
    /// </summary>
    public class GetRScriptPredictedEquation
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRScriptPredictedEquation"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetRScriptPredictedEquation(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the predicted equation for the RScript post process.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetRScriptPredictedEquation/{rScriptPostProcessId}</url>
        /// <param name="req">The request.</param>
        /// <param name="rScriptPostProcessId" cref="int" in="path">The RScript post process id.</param>
        /// <param name="log">The log.</param>
        /// <param name="precision" cref="int" in="query">The precision (number of decimals) of the predicted values.</param>
        /// <returns>
        /// The predicted equation for the RScript post process.
        /// </returns>
        [FunctionName("GetRScriptPredictedEquation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/CustomSearches/GetRScriptPredictedEquation/{rScriptPostProcessId}")]
            HttpRequest req,
            int rScriptPostProcessId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    int precision = -1;
                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("precision") == true)
                        {
                            precision = int.Parse(req.Query["precision"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetRScriptPredictedEquationService service = new GetRScriptPredictedEquationService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetRScriptPredictedEquationResponse response =
                            await service.GetRScriptPredictedEquationAsync(rScriptPostProcessId, precision, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}