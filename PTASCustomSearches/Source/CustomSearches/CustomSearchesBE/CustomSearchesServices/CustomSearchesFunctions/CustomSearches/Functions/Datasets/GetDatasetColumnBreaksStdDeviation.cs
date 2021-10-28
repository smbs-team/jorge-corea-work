namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
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
    /// Azure function class for the GetDatasetColumnBreaksStdDeviation service.  The service gets class break for the specified column using the
    /// StandardDeviation algorithm.
    /// </summary>
    public class GetDatasetColumnBreaksStdDeviation
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the StandardDeviationClassBreakData.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnBreaksStdDeviation"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetDatasetColumnBreaksStdDeviation(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets class break for the specified column using the Standard Deviation algorithm.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetColumnBreaksStdDeviation/{datasetId}/{columnName}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="columnName"  cref="string" in="path">The column name.</param>
        /// <param name="log">The log.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <param name="postBody" cref="ColumnQuantileBreaksData" in="body">Quantile break parameters.</param>
        /// <returns>
        /// Standard Deviation breaks.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("GetDatasetColumnBreaksStdDeviation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/GetDatasetColumnBreaksStdDeviation/{datasetId}/{columnName}")] HttpRequest req, Guid datasetId, string columnName, ILogger log)
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

                    ColumnStandardDeviationBreaksData standardDeviationParameters;
                    try
                    {
                        standardDeviationParameters = JsonConvert.DeserializeObject<ColumnStandardDeviationBreaksData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    if (standardDeviationParameters.Interval <= 0)
                    {
                        throw new CustomSearchesRequestBodyException("Interval should be positive and non-zero", null);
                    }

                    if (standardDeviationParameters.FilterEmptyValuesExpression != null)
                    {
                        if (standardDeviationParameters.FilterEmptyValuesExpression.ExpressionRole.ToLower() !=
                             CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower())
                        {
                            throw new CustomSearchesRequestBodyException("The ExpressionRole of the filter expression must be FilterExpression.", null);
                        }

                        if (string.IsNullOrWhiteSpace(standardDeviationParameters.FilterEmptyValuesExpression.Script))
                        {
                            throw new ArgumentNullException("FilterEmptyValuesExpression.Script");
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetDatasetColumnBreaksStdDeviationService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserCustomSearchDataResponse response = await service.GetDatasetColumnBreaksStdDeviationAsync(
                            datasetId,
                            usePostProcess,
                            postProcessId,
                            columnName,
                            standardDeviationParameters,
                            dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
