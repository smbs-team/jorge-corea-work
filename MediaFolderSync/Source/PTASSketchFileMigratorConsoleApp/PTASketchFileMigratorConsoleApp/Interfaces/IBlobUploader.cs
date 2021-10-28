// <copyright file="IBlobUploader.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Threading.Tasks;
    using PTASketchFileMigratorConsoleApp.Types;

    /// <summary>Defines the method to upload a XML file to the blob container.</summary>
    public interface IBlobUploader
    {
        /// <inheritdoc cref = "BlobUploader.UploadXML(IEnumerable{IFileInfo})"/>
        Task UploadXML(IEnumerable<IFileInfo> fileArray, List<OfficialFile> officialFiles);

        /// <inheritdoc cref = "BlobUploader.UploadSingleFile(IFileInfo, string)"/>
        void UploadSingleFile(IFileInfo file, string filePath);
    }
}