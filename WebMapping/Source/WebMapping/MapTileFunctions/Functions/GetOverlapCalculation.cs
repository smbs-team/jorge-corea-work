namespace PTASMapTileFunctions.Functions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.GeoLocationProvider;
    using PTASMapTileServicesLibrary.OverlapCalutionProvider;
    using PTASMapTileServicesLibrary.OverlapCalutionProvider.Exception;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Azure function class for the GetGeoLocation service.  This service returns feature coordinates based on feature data.
    /// </summary>
    public class GetOverlapCalculation
    {
        /// <summary>
        /// The geo location provider.
        /// </summary>
        private readonly OverlapCalculationProvider overlapCalculationProvider;

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly IServiceTokenProvider tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOverlapCalculation" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetOverlapCalculation(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            IServiceTokenProvider tokenProvider)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            this.dbContextFactory = dbContextFactory;
            this.tokenProvider = tokenProvider;
            this.overlapCalculationProvider = new OverlapCalculationProvider(dbContextFactory, tokenProvider);
        }

        /// <summary>
        /// Runs the PostOverlapCalculation function for the desired layerid, expect a body like {"buffer":10, "pins":['1120069021','1120069019','1120069016','1120069006', '1120069001', '729981TR-D']} and return a json list of OverlapData entities <seealso cref="OverlapData"/>.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="layerid">The parcel identifier.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested list of overlap data. if layerid is valid, else will return a empty list.
        /// </returns>
        [FunctionName("PostOverlapCalculation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "overlapCalculation/{layerid}")] HttpRequest req,
            long layerid,
            ILogger log)
        {
            try
            {
                string requestBody = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                double buffer = 0.0;
                buffer = data?.buffer;
                List<string> pins = new List<string>();
                foreach (var item in data?.pins)
                {
                    pins.Add(item.ToString());
                }

                var result = await this.overlapCalculationProvider.GetOverlapCalculation(pins, layerid, buffer);
                if (result == null)
                {
                    return new NotFoundResult();
                }

                return new JsonResult(result);
            }
            catch (OverlapCalculationProviderException overlapCalculationProviderException)
            {
                return MapTileFunctionsExceptionHandler.HandleOverlapCalculationProviderException(overlapCalculationProviderException, req, log);
            }
            catch (System.Exception ex)
            {
                return MapTileFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
