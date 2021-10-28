namespace PTASMapTileFunctions.Functions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Azure function class that servers vector tiles.
    /// </summary>
    public class GetVectorTile : GetTileBase
    {
        /// <summary>
        /// The proto-buff content type.
        /// </summary>
        private const string ProtoBuffContentType = "application/x-protobuf";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVectorTile" /> class.
        /// </summary>
        /// <param name="cloudStorageConfigurationProvider">The cloud storage configuration provider.</param>
        /// <param name="blobTileConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="tileProviderFactory">The tile provider factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">If cloudStorageConfigurationProvider/blobTileConfigurationProvider/tileProviderFactory parameter is null.</exception>
        public GetVectorTile(
            ICloudStorageConfigurationProvider cloudStorageConfigurationProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            ITileProviderFactory tileProviderFactory,
            IServiceTokenProvider tokenProvider,
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            TelemetryClient telemetryClient = null)
            : base(
                TileOutputType.PBF,
                cloudStorageConfigurationProvider,
                blobTileConfigurationProvider,
                tileProviderFactory,
                tokenProvider,
                dbContextFactory,
                telemetryClient)
        {
        }

        /// <summary>
        /// Runs the GetTile Function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="z">The level of zoom for the tile.</param>
        /// <param name="y">The y coordinate of the tile.</param>
        /// <param name="x">The x coordinate of the tile.</param>
        /// <param name="log">The logger.</param>
        /// <param name="context">The execution context.</param>
        /// <returns>The file with the tile bytes.</returns>
        [FunctionName("GetVectorTile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vectortiles/{layerId}/{z}/{y}/{x}")] HttpRequest req,
            string layerId,
            int z,
            int y,
            int x,
            ILogger log,
            ExecutionContext context)
        {
            bool isPassThrough = this.IsPassThroughLayer(layerId);
            ITileProvider fallbackProvider = isPassThrough ? null : this.GetMapServerTileProvider(context);

            ITileProvider tileProvider = this.GetBlobTileProvider(fallbackProvider, log);

            try
            {
                byte[] tileBytes = await tileProvider.GetTile(x, y, z, layerId);
                tileBytes = tileBytes ?? new byte[0];

                return new FileContentResult(tileBytes, GetVectorTile.ProtoBuffContentType);
            }
            catch (TileProviderException tileProviderException)
            {
                return MapTileFunctionsExceptionHandler.HandleTileProviderException(tileProviderException, req, log);
            }
            catch (System.Exception ex)
            {
                return MapTileFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
