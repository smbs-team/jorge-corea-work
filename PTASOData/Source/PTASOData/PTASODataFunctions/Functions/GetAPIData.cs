namespace PTASODataFunctions.Functions
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.OData;
    using PTASODataFunctions.Exception;
    using PTASODataLibrary.Api;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetAPIData service.  This service returns data related to CAMA.
    /// </summary>
    public class GetAPIData
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<DbContext> dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAPIData"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="tileFeatureDataProviderFactory">The tile feature data provider factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetAPIData(IFactory<DbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Runs the GetData function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="resource">The resource being requested.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested feature data.
        /// </returns>
        [FunctionName("GetData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/{resource}")] HttpRequest req, string resource, ILogger log)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(resource))
                {
                    throw new System.ArgumentNullException(nameof(resource));
                }

                log.LogInformation(
                    "Processing OData Request for Resource: {0}.  Query String: {1}.  Host: {2}.  Path: {3}",
                    resource,
                    req.QueryString,
                    req.Host.ToString(),
                    req.Path.ToString());

                string encoding = "application/json";
                string toReturn = string.Empty;

                if (resource.ToLower() == "$metadata")
                {
                    toReturn = await PtasODataApi.GetMetaData();
                    encoding = "application/xml";
                }
                else
                {
                    using (var dbContext = this.dbContextFactory.Create())
                    {
                        toReturn = await PtasODataApi.GetData(req, dbContext, resource, log);
                    }
                }

                byte[] encodedResult = Encoding.UTF8.GetBytes(toReturn);
                return new FileContentResult(encodedResult, encoding);
            }
            catch (ODataException oDataEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleODataException(oDataEx, req, log);
            }
            catch (System.ArgumentOutOfRangeException argRangeEx)
            {
                if (argRangeEx.ParamName?.ToLower() == resource.ToLower())
                {
                    return PTASODataFunctionsExceptionHandler.HandleArgumentOutOfRangeException(argRangeEx, req, log);
                }
                else
                {
                    return PTASODataFunctionsExceptionHandler.HandleUntypedException(argRangeEx, req, log);
                }
            }
            catch (System.ArgumentNullException argNullEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleArgumentNullException(argNullEx, req, log);
            }
            catch (SqlException sqlEx)
            {
                return PTASODataFunctionsExceptionHandler.HandleSqlException(sqlEx, req, log);
            }
            catch (System.Exception ex)
            {
                return PTASODataFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
