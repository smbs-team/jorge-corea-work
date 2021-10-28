namespace PTASServicesCommon.CloudStorage.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.File;
    using PTASServicesCommon.AspNetCore;

    /// <summary>
    /// Extensions for cloud file.
    /// </summary>
    public static class CloudFileExtensions
    {
        /// <summary>
        /// Uploads a file and sets the content type based on the file name.
        /// </summary>
        /// <param name="cloudFile">The cloud file.</param>
        /// <param name="source">The source.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An async task.</returns>
        public static async Task UploadFromStreamAsync(this CloudFile cloudFile, Stream source, string fileName)
        {
            await cloudFile.UploadFromStreamAsync(source);
            cloudFile.Properties.ContentType = ContentTypeProvider.GetMimeType(fileName);
            await cloudFile.SetPropertiesAsync();
        }
    }
}
