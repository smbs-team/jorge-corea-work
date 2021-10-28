namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;

    /// <summary>
    /// Service that imports a new layer source.
    /// </summary>
    /// <seealso cref="CustomSearchesServicesLibrary.ServiceFramework.BaseService" />
    public class ImportLayerSourceService : BaseService
    {
        /// <summary>
        /// Default tile size.
        /// </summary>
        private const int DefaultTileSize = 256;

        /// <summary>
        /// The layer file share name.
        /// </summary>
        private const string LayerFileShareName = "layers";

        /// <summary>
        /// Mapserver path in the layers share.
        /// </summary>
        private const string MapServerSharePath = "mapserver";

        /// <summary>
        /// The path where map definitions are stored within the layers share.
        /// </summary>
        private const string MapDefinitionsSharePath = "mapserver/mapserver-map-definitions";

        /// <summary>
        /// The path where processed layers are found in the layers share.
        /// </summary>
        private const string ShareProcessedLayersPath = "/mapserver/map-share/mapserver/processed-layers/";

        /// <summary>
        /// The path where processed layers are found in the map cache.
        /// </summary>
        private const string CacheProcessedLayersPath = "/mapserver/map-cache/processed-layers/";

        /// <summary>
        /// Name of the blob container where cached tiles are stored.
        /// </summary>
        private const string TileCacheContainerName = "tilecachecontainer";

        /// <summary>
        /// The file name for mapbox layer file in tile cache container (for passthrough vector tiles).
        /// </summary>
        private const string DefaultMapLayersFileName = "defaultMapLayers.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLayerSourceService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportLayerSourceService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports a layer source.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="layerSourceData">User map data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<int> ImportLayerSource(GisDbContext dbContext, LayerSourceData layerSourceData)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            layerSourceData.LayerSourceName = layerSourceData.LayerSourceName.Trim();
            InputValidationHelper.AssertZero(layerSourceData.LayerSourceId, nameof(LayerSource), nameof(layerSourceData.LayerSourceId));
            InputValidationHelper.AssertNotEmpty(layerSourceData.LayerSourceName, nameof(layerSourceData.LayerSourceName));
            InputValidationHelper.AssertNotEmpty(layerSourceData.GisLayerName, nameof(layerSourceData.GisLayerName));

            string errorMessage = string.Empty;
            if (!layerSourceData.ValidateOgrData(out errorMessage))
            {
                throw new CustomSearchesRequestBodyException(errorMessage, null);
            }

            if (string.IsNullOrWhiteSpace(layerSourceData.DataSourceUrl))
            {
                string baseEndpoint = layerSourceData.IsVectorLayer ? "vectortiles" : "rastertiles";
                string tileCoords = "{z}/{y}/{x}";
                layerSourceData.DataSourceUrl = $"v1.0/{baseEndpoint}/{layerSourceData.GisLayerName}/{tileCoords}";
            }

            if (layerSourceData.TileSize == 0)
            {
                layerSourceData.TileSize = ImportLayerSourceService.DefaultTileSize;
            }

            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportLayerSource");

            LayerSource existingLayerSource =
                await (from ls in dbContext.LayerSource
                       where ls.LayerSourceName.ToLower() == layerSourceData.LayerSourceName.ToLower()
                       select ls).FirstOrDefaultAsync();

            LayerSource savedLayerSource = null;
            if (existingLayerSource == null)
            {
                savedLayerSource = layerSourceData.ToEfModel();
                dbContext.LayerSource.Add(savedLayerSource);
            }
            else
            {
                savedLayerSource = existingLayerSource;
                layerSourceData.UpdateEFModel(existingLayerSource);
            }

            if (savedLayerSource.IsBlobPassThrough)
            {
                await this.LoadNativeRenderersFromBlob(savedLayerSource);
            }
            else
            {
                var mapFileContent = await this.GenerateMapFileContentAsync(layerSourceData, false);
                var mapfileContentInShare = await this.GenerateMapFileContentAsync(layerSourceData, true);
                await this.CreateMapDefinitionInShareAsync(mapFileContent, mapfileContentInShare, layerSourceData);
            }

            await dbContext.ValidateAndSaveChangesAsync();

            return savedLayerSource.LayerSourceId;
        }

        /// <summary>
        /// Creates the map definition in the file share.
        /// </summary>
        /// <param name="mapfileContent">Content of the mapfile (for cached access).</param>
        /// <param name="mapfileContentInShare">The mapfile content in share (for file share access).</param>
        /// <param name="layerSourceData">The layer source data.</param>
        private async Task CreateMapDefinitionInShareAsync(
            string mapfileContent,
            string mapfileContentInShare,
            LayerSourceData layerSourceData)
        {
            var cachedMapDefinitionFileName = $"{layerSourceData.LayerSourceName}.map";
            var shareMapDefinitionFileName = $"{layerSourceData.LayerSourceName}_Share.map";

            CloudFileClient fileClient = await this.ServiceContext.CloudStorageProvider.GetCloudFileClient();
            CloudFileShare fileShare = fileClient.GetShareReference(ImportLayerSourceService.LayerFileShareName);
            CloudFileDirectory rootDirectory = fileShare.GetRootDirectoryReference();

            CloudFileDirectory mapServerDirectory =
                rootDirectory.GetDirectoryReference(ImportLayerSourceService.MapServerSharePath);

            await mapServerDirectory.CreateIfNotExistsAsync();

            CloudFileDirectory mapDefinitionsDirectory =
                rootDirectory.GetDirectoryReference(ImportLayerSourceService.MapDefinitionsSharePath);

            await mapDefinitionsDirectory.CreateIfNotExistsAsync();

            CloudFile cachedfileInfo = mapDefinitionsDirectory.GetFileReference(cachedMapDefinitionFileName);
            await cachedfileInfo.UploadTextAsync(mapfileContent);

            CloudFile sharefileInfo = mapDefinitionsDirectory.GetFileReference(shareMapDefinitionFileName);
            await sharefileInfo.UploadTextAsync(mapfileContentInShare);
        }

        /// <summary>
        /// Generates the map file string.
        /// </summary>
        /// <param name="layerSourceData">The layer source data.</param>
        /// <param name="fileShareAccess">if true generate a map file that references layers in the file share,
        /// otherwise they are references in the cache.</param>
        /// <returns>
        /// A string with the contents of the map file.
        /// </returns>
        private async Task<string> GenerateMapFileContentAsync(LayerSourceData layerSourceData, bool fileShareAccess)
        {
            var mapTemplate = await this.GetTemplateFileAsync("MapTemplate.map");
            mapTemplate = mapTemplate.Replace("{LayerName}", layerSourceData.GisLayerName);

            var layersContent = await this.GenerateMapLayersContentAsync(layerSourceData.OgrLayerData, fileShareAccess);
            mapTemplate = mapTemplate.Replace("{Layers}", layersContent);
            return mapTemplate;
        }

        /// <summary>
        /// Generates the map file string.
        /// </summary>
        /// <param name="layerData">The layer source data.</param>
        /// <returns>A string with the contents of the map file.</returns>
        private async Task<string> GenerateMapLayersContentAsync(OgrLayerData[] layerData, bool fileShareAccess)
        {
            var vectorLayerTemplate = await this.GetTemplateFileAsync("VectorLayerTemplate.map");
            var rasterLayerTemplate = await this.GetTemplateFileAsync("RasterLayerTemplate.map");

            StringBuilder layersStringBuilder = new StringBuilder();
            foreach (var layer in layerData)
            {
                string newLayer = string.Empty;
                if (layer.OgrLayerType == OgrLayerType.Raster)
                {
                    newLayer += rasterLayerTemplate;
                }
                else
                {
                    newLayer += vectorLayerTemplate;
                }

                string layerConnectionPath = fileShareAccess ?
                    ImportLayerSourceService.ShareProcessedLayersPath :
                    ImportLayerSourceService.CacheProcessedLayersPath;

                layerConnectionPath += layer.LayerConnectionPath;

                newLayer = newLayer.Replace("{LayerProjection}", layer.LayerProjection);
                newLayer = newLayer.Replace("{LayerName}", layer.LayerName);
                newLayer = newLayer.Replace("{LayerConnectionPath}", layerConnectionPath);

                newLayer = newLayer.Replace("{OgrLayerName}", layer.OgrLayerName);
                newLayer = newLayer.Replace("{VectorType}", layer.OgrLayerType.ToString());

                layersStringBuilder.Append(newLayer);
                layersStringBuilder.Append(Environment.NewLine);
            }

            return layersStringBuilder.ToString();
        }

        /// <summary>
        /// Gets the template file.
        /// </summary>
        /// <param name="templateFileName">Name of the template file.</param>
        /// <returns>The template file.</returns>
        private async Task<string> GetTemplateFileAsync(string templateFileName)
        {
            var fileName = $"CustomSearchesServicesLibrary.Gis.Templates.{templateFileName}";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(fileName);

            if (stream == null)
            {
                throw new FileNotFoundException("Cannot find mapserver map template.", templateFileName);
            }

            var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Loads the native renderers from BLOB.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        private async Task LoadNativeRenderersFromBlob(LayerSource layerSource)
        {
            var blobContainer = await this.ServiceContext.PremiumCloudStorageProvider.GetCloudBlobContainer(
                ImportLayerSourceService.TileCacheContainerName,
                this.ServiceContext.AppCredential);

            CloudBlobDirectory directory = blobContainer.GetDirectoryReference(
                $"{layerSource.GisLayerName}/renderers");

            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(ImportLayerSourceService.DefaultMapLayersFileName);

            if (await blockBlob.ExistsAsync())
            {
                string fileContents = await blockBlob.DownloadTextAsync();
                layerSource.NativeMapboxLayers = fileContents;
            }
        }
    }
}