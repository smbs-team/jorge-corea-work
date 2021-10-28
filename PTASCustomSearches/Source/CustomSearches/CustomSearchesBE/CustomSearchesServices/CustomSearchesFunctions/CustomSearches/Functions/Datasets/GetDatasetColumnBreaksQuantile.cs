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
    /// Azure function class for the GetDatasetColumnBreakQuantile service.  The service gets class break for the specified column using the
    /// Quantile algorithm.
    /// </summary>
    public class GetDatasetColumnBreaksQuantile
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the ColumnQuantileBreaksData.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnBreaksQuantile"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetDatasetColumnBreaksQuantile(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets class break for the specified column using the Quantile algorithm.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetColumnBreaksQuantile/{datasetId}/{columnName}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="columnName"  cref="string" in="path">The column name.</param>
        /// <param name="log">The log.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <param name="postBody" cref="ColumnQuantileBreaksData" in="body">Quantile break parameters.</param>
        /// <returns>
        /// Quantile breaks.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("GetDatasetColumnBreaksQuantile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/GetDatasetColumnBreaksQuantile/{datasetId}/{columnName}")] HttpRequest req, Guid datasetId, string columnName, ILogger log)
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

                    ColumnQuantileBreaksData quantileParameters;
                    try
                    {
                        quantileParameters = JsonConvert.DeserializeObject<ColumnQuantileBreaksData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    if (quantileParameters.ClassBreakCount <= 0)
                    {
                        throw new CustomSearchesRequestBodyException("ClassBreakCount should be positive and non-zero", null);
                    }

                    if (quantileParameters.FilterEmptyValuesExpression != null)
                    {
                        if (quantileParameters.FilterEmptyValuesExpression.ExpressionRole.ToLower() !=
                             CustomSearchExpressionRoleType.FilterExpression.ToString().ToLower())
                        {
                            throw new CustomSearchesRequestBodyException("The ExpressionRole of the filter expression must be FilterExpression.", null);
                        }

                        if (string.IsNullOrWhiteSpace(quantileParameters.FilterEmptyValuesExpression.Script))
                        {
                            throw new ArgumentNullException("FilterEmptyValuesExpression.Script");
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetDatasetColumnBreaksQuantileService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserCustomSearchDataResponse response = await service.GetDatasetColumnBreaksQuantileAsync(
                            datasetId,
                            usePostProcess,
                            postProcessId,
                            columnName,
                            quantileParameters,
                            dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
