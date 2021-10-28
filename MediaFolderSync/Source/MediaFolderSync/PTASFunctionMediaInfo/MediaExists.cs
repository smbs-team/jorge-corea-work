namespace PTASFunctionMediaInfo
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using MediaInfo;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Checks if media exists in storage.
    /// </summary>
    public static class MediaExists
    {
        /// <summary>
        /// Exported function.
        /// </summary>
        /// <param name="req">Request.</param>
        /// <param name="log">Log.</param>
        /// <returns>Result.</returns>
        [FunctionName("MediaExists")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string entityStr = req.Query["entityId"];
                string storageConnectionString = Environment.GetEnvironmentVariable("BlobStorage");
                string media_url = Environment.GetEnvironmentVariable("media-url");

                if (entityStr == null)
                {
                    throw new PostMediaException("Entity is required in the query.", null);
                }

                var refr = await MediaStorageHelper.GetFileReference(storageConnectionString, entityStr);
                return !await refr.ExistsAsync()
                    ? new StatusCodeResult(StatusCodes.Status404NotFound)
                    : (ActionResult)new OkObjectResult($"{refr.Name}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
