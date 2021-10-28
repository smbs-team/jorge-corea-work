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
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.AspNetCore;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Azure function class that servers sprites for vector tiles.
    /// </summary>
    public class GetSprite : GetTileBase
    {
        /// <summary>
        /// The connection string parse error message.
        /// </summary>
        private const string ConnectionStringParseErrorMessage = "Error parsing storage connection string";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSprite" /> class.
        /// </summary>
        /// <param name="cloudStorageConfigurationProvider">The cloud storage configuration provider.</param>
        /// <param name="blobTileConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="tileProviderFactory">The tile provider factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">If cloudStorageConfigurationProvider/blobTileConfigurationProvider/tileProviderFactory parameter is null.</exception>
        public GetSprite(
            ICloudStorageConfigurationProvider cloudStorageConfigurationProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            ITileProviderFactory tileProviderFactory,
            IServiceTokenProvider tokenProvider,
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            TelemetryClient telemetryClient)
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
        /// Runs the GetSprite Function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="log">The logger.</param>
        /// <param name="context">The execution context.</param>
        /// <returns>
        /// The file with the tile bytes.
        /// </returns>
        [FunctionName("GetSprite")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sprites/{layerId}/{fileName}")] HttpRequest req,
            string layerId,
            string fileName,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                bool isPassThrough = this.IsPassThroughLayer(layerId);
                if (!isPassThrough)
                {
                    throw new System.ArgumentException($"{layerId} is not PassThrough.");
                }

                CloudBlobContainer container = null;
                try
                {
                    container = await this.GetCloudStorageProvider().GetCloudBlobContainer(
                        this.GetBlobTileConfigurationProvider().TileContainerName);
                }
                catch (System.FormatException formatException)
                {
                    string error = string.Format(GetSprite.ConnectionStringParseErrorMessage);
                    throw new TileProviderException(TileProviderExceptionCategory.InvalidConfiguration, this.GetType(), error, formatException);
                }

                var spritePath = $"{layerId}/sprites/{fileName}";

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(spritePath);

                if (await blockBlob.ExistsAsync())
                {
                    System.IO.MemoryStream spriteStream = new MemoryStream();
                    await blockBlob.DownloadToStreamAsync(spriteStream);

                    spriteStream.Position = 0;
                    byte[] spriteBytes = new byte[spriteStream.Length];
                    await spriteStream.ReadAsync(spriteBytes, 0, (int)spriteStream.Length);
                    return new FileContentResult(spriteBytes, ContentTypeProvider.GetMimeType(fileName));
                }
                else
                {
                    return new NotFoundObjectResult($"Sprite '{spritePath}' not found.");
                }
            }
            catch (System.Exception ex)
            {
                return MapTileFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
