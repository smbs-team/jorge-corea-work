// <copyright file="MediaStorageHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASFunctionMediaInfo
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MediaInfo;

    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;

    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.CloudStorage.Extensions;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Base to encapsulate common stuff.
    /// </summary>
    public static class MediaStorageHelper
    {
        /// <summary>
        /// The media container name.
        /// </summary>
        public const string MediaContainerName = "media";

        /// <summary>
        /// Get blob reference.
        /// </summary>
        /// <param name="storageConnectionString">Connection string to the storage.</param>
        /// <param name="entityStr">Entity.</param>
        /// <returns>Reference to the blob.</returns>
        public static async Task<CloudBlockBlob> GetBlobReference(string storageConnectionString, string entityStr)
        {
            var blobName = GetName(entityStr);
            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(storageConnectionString);
            ICloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider, new AzureTokenProvider());
            var container = await storageProvider.GetCloudBlobContainer(MediaContainerName);
            var refr = container.GetBlockBlobReference(blobName);
            return refr;
        }

        /// <summary>
        /// Get blob reference.
        /// </summary>
        /// <param name="storageConnectionString">Connection string to the storage.</param>
        /// <param name="entityStr">Entity.</param>
        /// <returns>Reference to the blob.</returns>
        public static async Task<CloudFile> GetFileReference(string storageConnectionString, string entityStr)
        {
            var blobName = GetName(entityStr);
            CloudFileShare shareReference = await GetFileShare(storageConnectionString);
            var rootDirectory = shareReference.GetRootDirectoryReference();
            CloudFile file = rootDirectory.GetFileReference(blobName);
            return file;
        }

        /// <summary>
        /// Gets the file share.
        /// </summary>
        /// <param name="storageConnectionString">Storage connections string.</param>
        /// <returns>File share.</returns>
        public static async Task<CloudFileShare> GetFileShare(string storageConnectionString)
        {
            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(storageConnectionString);
            ICloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider, new AzureTokenProvider());
            var client = await storageProvider.GetCloudFileClient();
            CloudFileShare shareReference = client.GetShareReference(MediaContainerName);
            if (!await shareReference.ExistsAsync())
            {
                throw new FileNotFoundException();
            }

            return shareReference;
        }

        /// <summary>
        /// Gets a file name.
        /// </summary>
        /// <param name="entityStr">Entity to get.</param>
        /// <returns>File name.</returns>
        public static string GetName(string entityStr)
        {
            var nameParts = entityStr.Split('.');
            if (nameParts.Length != 2)
            {
                throw new PostMediaException("Must be file name and extension.", null);
            }

            (string fileName, string extension) = (nameParts[0], nameParts[1]);

            string outName = MediaStorageHelper.GetUrl(fileName, extension);
            return outName;
        }

        /// <summary>
        /// Gets a url based on the file name.
        /// </summary>
        /// <param name="fileName">File to get the url for.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Formatted file url.</returns>
        public static string GetUrl(string fileName, string extension)
        {
            if (fileName.Length < 4)
            {
                throw new ArgumentException($"Invalid length: {fileName} should be at least 4 characters long.");
            }

            var p1 = fileName.Substring(0, 1);
            var p2 = fileName.Substring(1, 1);
            var p3 = fileName.Substring(2, 1);
            var p4 = fileName.Substring(3, 1);
            return $@"{p1}/{p2}/{p3}/{p4}/{fileName}.{extension}";
        }

        /// <summary>
        /// Saves a new file.
        /// </summary>
        /// <param name="storageConnectionString">Storage connection.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="stream">Input stream.</param>
        /// <returns>Nothing.</returns>
        internal static async Task<string> SaveNewFile(string storageConnectionString, string fileName, Stream stream)
        {
            stream.Position = 0;
            CloudFileShare t = await GetFileShare(storageConnectionString);
            CloudFileDirectory currDir = t.GetRootDirectoryReference();
            var fullFilename = GetName(fileName);
            var parts = fullFilename.Split("/");
            var file = parts.Last();
            var butLast = parts.Take(parts.Count() - 1).ToArray();
            for (int i = 0; i < butLast.Length; i++)
            {
                var itm = butLast[i];
                CloudFileDirectory refr = currDir.GetDirectoryReference(itm);
                await refr.CreateIfNotExistsAsync();
                currDir = refr;
            }

            var fileref = currDir.GetFileReference(file);
            await fileref.UploadFromStreamAsync(stream, fileName);
            return fullFilename;
        }
    }
}