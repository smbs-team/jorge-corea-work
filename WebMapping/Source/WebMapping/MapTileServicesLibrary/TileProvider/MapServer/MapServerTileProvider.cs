namespace PTASMapTileServicesLibrary.TileProvider.MapServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.ApplicationInsights;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.Telemetry;

    /// <summary>
    /// Provides tiles served from a web service.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.Providers.ITileProvider" />
    public class MapServerTileProvider : ITileProvider
    {
        /// <summary>
        /// Parameter used to select all layers from a map in MapServer.
        /// </summary>
        private const string AllLayersParametervalue = "all";

        /// <summary>
        /// Provides information about layer configuration in MapServer that is used to build MapServer URLs.
        /// </summary>
        private readonly IMapServerLayerConfigurationProvider mapServerLayerConfigurationProvider;

        /// <summary>
        /// Type of the output for the generated tile.
        /// </summary>
        private readonly TileOutputType outputType;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// The HTTP message handler used to instantiate HttpClient.  Null by default (uses standard httpMessageHandler from .NET).
        /// </summary>
        private readonly HttpMessageHandler httpMessageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapServerTileProvider" /> class.
        /// </summary>
        /// <param name="mapServerLayerConfigurationProvider">The map server layer configuration provider.</param>
        /// <param name="outputType">Type of the output for the generated tile.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="httpMessageHandler">The HTTP message handler.  Null by default.</param>
        /// <exception cref="ArgumentNullException">When mapServerLayerConfigurationProvider is null.</exception>
        public MapServerTileProvider(
            IMapServerLayerConfigurationProvider mapServerLayerConfigurationProvider,
            TileOutputType outputType,
            TelemetryClient telemetryClient = null,
            HttpMessageHandler httpMessageHandler = null)
        {
            if (mapServerLayerConfigurationProvider == null)
            {
                throw new ArgumentNullException(nameof(mapServerLayerConfigurationProvider));
            }

            this.outputType = outputType;
            this.mapServerLayerConfigurationProvider = mapServerLayerConfigurationProvider;
            this.httpMessageHandler = httpMessageHandler;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Get the bytes for a tile from Map Server.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>An array of bytes with the specified tile.  NULL if the tile was not found.</returns>
        public async Task<byte[]> GetTile(int x, int y, int z, string layer)
        {
            string tileUrl = await this.ResolveTileUrlAsync(x, y, z, layer);

            try
            {
                var httpMessageHandler = this.httpMessageHandler ?? new HttpClientHandler();

                byte[] tileBytes = null;

                await TelemetryHelper.TrackPerformanceAsync(
                    "MapServerTileProvider_GetTile",
                    async () =>
                    {
                        var metrics = new Dictionary<string, double>();

                        using (HttpClient client = new HttpClient(httpMessageHandler))
                        using (HttpResponseMessage response = await client.GetAsync(tileUrl))
                        using (HttpContent content = response.Content)
                        {
                            bool contentIsErrorMessage = false;
                            if (content != null)
                            {
                                // If Maps Server sends us a text/html reply, it is an error.
                                contentIsErrorMessage = content.Headers.ContentType?.ToString() == "text/html";
                            }

                            if ((response.StatusCode == System.Net.HttpStatusCode.OK) && !contentIsErrorMessage)
                            {
                                Stream tileStream = await content.ReadAsStreamAsync();
                                tileStream.Position = 0;
                                tileBytes = new byte[tileStream.Length];

                                // tileStream.Length can be safely cast to int since tile streams are always going to be small enough
                                await tileStream.ReadAsync(tileBytes, 0, (int)tileStream.Length);
                                metrics.Add("MapServerTileProvider_GetTile:Length", (double)tileBytes.Length);
                            }
                            else
                            {
                                string responseContent = await content.ReadAsStringAsync();

                                string error = string.Format("Server error trying to retrieve the tile from: {0}. Status Code: {1}. Response Content: {2}", tileUrl, response.StatusCode, responseContent);
                                throw new TileProviderException(TileProviderExceptionCategory.ServerError, this.GetType(), error, null);
                            }
                        }

                        return metrics;
                    },
                    this.telemetryClient);

                return tileBytes;
            }
            catch (HttpRequestException requestException)
            {
                string error = string.Format("Error trying to retrieve tile from MapServer.  Can't connect to the URL: {0}", tileUrl);
                throw new TileProviderException(TileProviderExceptionCategory.HttpRequestSendError, this.GetType(), error, requestException);
            }
        }

        /// <summary>
        /// Resolves the tile URL.
        /// </summary>
        /// <param name="x">The x tile coordinate.</param>
        /// <param name="y">The y tile coordinate.</param>
        /// <param name="z">The z tile coordinate.</param>
        /// <param name="layerId">The layer id.</param>
        /// <returns>The URL where the tile can be retrieved.</returns>
        private async Task<string> ResolveTileUrlAsync(int x, int y, int z, string layerId)
        {
            string mapserverOutputType = this.outputType.ToString();
            if (this.outputType == TileOutputType.PBF)
            {
                mapserverOutputType = "MVT";
            }

            var resolvedMapParameters = this.mapServerLayerConfigurationProvider.GetMapFileForLayerId(layerId);
            if (resolvedMapParameters.mapFile == null)
            {
                return null;
            }

            string layer = layerId;
            if (this.mapServerLayerConfigurationProvider.IsRasterLayerProvider)
            {
                layer = MapServerTileProvider.AllLayersParametervalue;
            }

            return string.Format(
                this.mapServerLayerConfigurationProvider.MapServerUrl,
                HttpUtility.UrlEncode(resolvedMapParameters.mapFile),
                x,
                y,
                z,
                "all",
                mapserverOutputType);
        }
    }
}
