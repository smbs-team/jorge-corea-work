namespace PTASMapTileServicesLibrary.TileProvider.MapServer
{
    using System;
    using System.Collections.Generic;
    using System.IO.Enumeration;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Class that provides configuration information about map server layers.
    /// </summary>
    public class MapServerLayerConfigurationProvider : IMapServerLayerConfigurationProvider
    {
        /// <summary>
        /// The path where map definitions are stored within the layers share.
        /// </summary>
        private const string MapDefinitionsSharePath = "/mapserver/map-share/mapserver/mapserver-map-definitions";

        /// <summary>
        /// The path where map definitions are stored within the layers share.
        /// </summary>
        private const string MapDefinitionsCachePath = "/mapserver/map-cache/mapserver-map-definitions";

        /// <summary>
        /// The database context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// IP address of the MapServer server.
        /// </summary>
        private readonly string mapServerIp;

        /// <summary>
        /// Map server Url.
        /// </summary>
        private readonly string mapServerUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapServerLayerConfigurationProvider" /> class.
        /// </summary>
        /// <param name="mapServerUrl">The map server URL (mask).</param>
        /// <param name="mapServerIp">The map server ip.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="isRasterLayerProvider">if set to <c>true</c> indicates that this is a raster layer provider.</param>
        /// <exception cref="ArgumentNullException">If either mapsServerURL/layers/dbContext parameter is null.</exception>
        public MapServerLayerConfigurationProvider(
            string mapServerUrl,
            string mapServerIp,
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            bool isRasterLayerProvider)
        {
            if (string.IsNullOrEmpty(mapServerUrl))
            {
                throw new ArgumentNullException(nameof(mapServerUrl));
            }

            if (string.IsNullOrEmpty(mapServerIp))
            {
                throw new ArgumentNullException(nameof(mapServerIp));
            }

            if (dbContextFactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextFactory));
            }

            this.mapServerIp = mapServerIp;
            this.mapServerUrl = mapServerUrl;
            this.IsRasterLayerProvider = isRasterLayerProvider;
            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Gets the URL where map server tiles will be looked up.
        /// </summary>
        /// <remarks>
        /// This should be a mask containing the tile parameters:  tile={1}+{2}+{3}&layers={4} where {1}=x, {2}=y, {3}=z, {4}=layerId.
        /// </remarks>
        public string MapServerUrl
        {
            get
            {
                return this.mapServerUrl.Replace("{MapServerIp}", this.mapServerIp);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this configuration is for a raster layer provider.
        /// </summary>
        public bool IsRasterLayerProvider { get; set; }

        /// <summary>
        /// Gets the MapServer map file for a layer identifier and the corresponding layers query
        /// that needs to be submitted to mapserver.
        /// </summary>
        /// <param name="layerId">The layer identifier.</param>
        /// <returns>The map name and the layers query.  Returns null if the layer is not valid.</returns>
        public (string layersQuery, string mapFile) GetMapFileForLayerId(string layerId)
        {
            using (var dbContext = this.dbContextFactory.Create())
            {
                var layerSource =
                    (from ls in dbContext.LayerSource
                        where ls.GisLayerName.ToLower() == layerId.ToLower()
                        select ls)
                    .FirstOrDefault();

                if (layerSource != null)
                {
                    string mapBasePath = layerSource.ServeFromFileShare ?
                        MapServerLayerConfigurationProvider.MapDefinitionsSharePath :
                        MapServerLayerConfigurationProvider.MapDefinitionsCachePath;

                    var fileName = layerSource.ServeFromFileShare ?
                        $"{layerSource.LayerSourceName}_Share.map" :
                        $"{layerSource.LayerSourceName}.map";

                    return ("all", $"{mapBasePath}/{fileName}");
                }
            }

            return (null, null);
        }
    }
}
