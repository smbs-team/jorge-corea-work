// <copyright file="IUtility.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Threading.Tasks;
    using PTASketchFileMigratorConsoleApp.Types;

    /// <summary>Defines utility methods.</summary>
    public interface IUtility
    {
        /// <summary>Gets or sets the files marked as official.</summary>
        /// <value>The official files.</value>
        List<OfficialFile> OfficialFiles { get; set; }

        /// <summary>Renames the given XML files.</summary>
        /// <param name="fileArray">The file array.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RenameFiles(IEnumerable<IFileInfo> fileArray);

        /// <summary>Filters the file array by valid files.</summary>
        /// <param name="arrayToFilter">The array to filter.</param>
        /// <returns>Filtered array.</returns>
        IEnumerable<IFileInfo> FilterByValidFiles(IEnumerable<IFileInfo> arrayToFilter);
    }
}