// <copyright file="ICloudService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System.Threading.Tasks;

    /// <summary>Defines a methods to upload a file to azure storage.</summary>
    public interface ICloudService
    {
        /// <inheritdoc cref = "CloudService.UploadFile(byte[], string, string)"/>
        Task UploadFile(byte[] file, string containerName, string path);
    }
}