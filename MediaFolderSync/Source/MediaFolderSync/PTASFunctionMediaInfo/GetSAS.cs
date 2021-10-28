namespace MediaInfo
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Class helps find the url for the media files.
    /// </summary>
    public static class GetSAS
    {
        /// <summary>
        /// The media container name.
        /// </summary>
        private const string MediaContainerName = "media";

        /// <summary>
        /// Main function run.
        /// </summary>
        /// <param name= "request">Request information.</param>
        /// <param name= "log">Event log to write to.</param>
        /// <returns>A task that resolves to string with the SAS.</returns>
        [FunctionName("GetSAS")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            try
            {
                var re = await GetSas(new AzureTokenProvider());
                return (ActionResult)new OkObjectResult(re);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Gets the sas token to access the blob container.
        /// </summary>
        /// <returns>A SAS to access blob files.</returns>
        private static async Task<string> GetSas(IServiceTokenProvider tokenProvider)
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("BlobStorage");
            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(storageConnectionString);
            ICloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider, tokenProvider);
            ICloudStorageSharedSignatureProvider sharedSignatureProvider = new BlobSharedSignatureProvider(storageProvider);
            string sharedSignature = await sharedSignatureProvider.GetSharedFileSignature(MediaContainerName, null);
            return sharedSignature;
        }
    }
}
