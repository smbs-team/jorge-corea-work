namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Gets the GIS layer download URL.
    /// </summary>
    public class GetGisLayerDownloadUrlService : BaseService
    {
        /// <summary>
        /// Name of the blob container where cached tiles are stored.
        /// </summary>
        private const string TileCacheContainerName = "tilecachecontainer";

        /// <summary>
        /// The layer file share name.
        /// </summary>
        private const string LayerFileShareName = "layers";

        /// <summary>
        /// The path where processed layers are found in the layers share.
        /// </summary>
        private const string ShareProcessedLayersPath = "mapserver/processed-layers/";

        /// <summary>
        /// Name of the blob container where cached tiles are stored.
        /// </summary>
        private const string ErrorMessage = "Layer file not found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGisLayerDownloadUrlService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetGisLayerDownloadUrlService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the GIS layer download URL response.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="layerSourceId">The layer source identifier.</param>
        /// <returns>
        /// The GIS layer download URL response.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity should not be null.</exception>
        public async Task<GetGisLayerDownloadUrlResponse> GetGisLayerDownloadUrlAsync(GisDbContext dbContext, int layerSourceId)
        {
            var layerSource = await dbContext.LayerSource.Where(ls => ls.LayerSourceId == layerSourceId).FirstOrDefaultAsync();
            InputValidationHelper.AssertEntityExists(layerSource, nameof(layerSource), layerSourceId);

            return layerSource.IsBlobPassThrough ?
                await this.GetGisLayerDownloadUrlFromBlobAsync(layerSource) :
                await this.GetGisLayerDownloadUrlFromFileShareAsync(layerSource);
        }

        /// <summary>
        /// Gets the GIS layer download URL response from blob storage.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <returns>
        /// The GIS layer download URL response.
        /// </returns>
        private async Task<GetGisLayerDownloadUrlResponse> GetGisLayerDownloadUrlFromBlobAsync(LayerSource layerSource)
        {
            GetGisLayerDownloadUrlResponse response = new GetGisLayerDownloadUrlResponse();

            var blobContainer = await this.ServiceContext.PremiumCloudStorageProvider.GetCloudBlobContainer(
                GetGisLayerDownloadUrlService.TileCacheContainerName,
                this.ServiceContext.AppCredential);

            if ((await blobContainer.ExistsAsync()) == false)
            {
                var errorDetails = $" The blob container '{GetGisLayerDownloadUrlService.TileCacheContainerName}' was not found in the server.";
                throw new CustomSearchesEntityNotFoundException(GetGisLayerDownloadUrlService.ErrorMessage + errorDetails, innerException: null);
            }

            string blobName = $"{layerSource.GisLayerName}/{layerSource.GisLayerName}.zip";
            var cloudBlob = blobContainer.GetBlockBlobReference(blobName);

            if ((await cloudBlob.ExistsAsync()) == false)
            {
                var errorDetails = $" The blob resource '{blobName}' was not found in the server.";
                throw new CustomSearchesEntityNotFoundException(GetGisLayerDownloadUrlService.ErrorMessage + errorDetails, innerException: null);
            }

            BlobSharedSignatureProvider signatureProvider = new BlobSharedSignatureProvider(this.ServiceContext.PremiumCloudStorageProvider);
            string sharedSignature = await signatureProvider.GetSharedSignature(GetGisLayerDownloadUrlService.TileCacheContainerName, blobName);

            response.FileSize = cloudBlob.Properties.Length;
            response.Url = $"{cloudBlob.Uri}{sharedSignature}";

            return response;
        }

        /// <summary>
        /// Gets the GIS layer download URL response from file share storage.
        /// </summary>
        /// <param name="layerSource">The layer source.</param>
        /// <returns>
        /// The GIS layer download URL response.
        /// </returns>
        private async Task<GetGisLayerDownloadUrlResponse> GetGisLayerDownloadUrlFromFileShareAsync(LayerSource layerSource)
        {
            GetGisLayerDownloadUrlResponse response = new GetGisLayerDownloadUrlResponse();

            var ogrLayerDataArray = JsonHelper.DeserializeObject<OgrLayerData[]>(layerSource.OgrLayerData);
            var ogrLayerData = ogrLayerDataArray?.Where(d => !string.IsNullOrWhiteSpace(d.LayerConnectionPath)).FirstOrDefault();
            if (ogrLayerData == null)
            {
                var errorDetails = $" {nameof(layerSource.OgrLayerData)} does not contain a {nameof(ogrLayerData.LayerConnectionPath)}.";
                throw new CustomSearchesEntityNotFoundException(GetGisLayerDownloadUrlService.ErrorMessage + errorDetails, innerException: null);
            }

            var cloudFileShare = await this.ServiceContext.CloudStorageProvider.GetCloudFileContainer(GetGisLayerDownloadUrlService.LayerFileShareName);
            if ((await cloudFileShare.ExistsAsync()) == false)
            {
                var errorDetails = $" The file container '{GetGisLayerDownloadUrlService.LayerFileShareName}' was not found in the server.";
                throw new CustomSearchesEntityNotFoundException(GetGisLayerDownloadUrlService.ErrorMessage + errorDetails, innerException: null);
            }

            string fileName = $"{GetGisLayerDownloadUrlService.ShareProcessedLayersPath}{ogrLayerData.LayerConnectionPath}";
            var cloudFile = cloudFileShare.GetRootDirectoryReference().GetFileReference(fileName);

            if ((await cloudFile.ExistsAsync()) == false)
            {
                var errorDetails = $" The file resource '{fileName}' was not found in the server.";
                throw new CustomSearchesEntityNotFoundException(GetGisLayerDownloadUrlService.ErrorMessage + errorDetails, innerException: null);
            }

            BlobSharedSignatureProvider signatureProvider = new BlobSharedSignatureProvider(this.ServiceContext.CloudStorageProvider);
            string sharedSignature = await signatureProvider.GetSharedFileSignature(GetGisLayerDownloadUrlService.LayerFileShareName, fileName);

            response.FileSize = cloudFile.Properties.Length;
            response.Url = $"{cloudFile.Uri}{sharedSignature}";

            return response;
        }
    }
}
