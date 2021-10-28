// <copyright file="IDirectoryManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;

    /// <summary>Defines a method that gets files from a given folder.</summary>
    public interface IDirectoryManager
    {
        /// <inheritdoc cref="DirectoryManager.GetFolderFilesByDate(string, string, DateTime)"/>
        IOrderedEnumerable<IFileInfo> GetFolderFilesByDate(string folder, string extension, DateTime mostRecentFile);

        /// <inheritdoc cref="DirectoryManager.DeleteFiles(string)"/>
        void DeleteFiles(string folder);
    }
}