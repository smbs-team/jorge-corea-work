// <copyright file="IVcaddManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;

    /// <summary>Defines exporting XML files methods.</summary>
    public interface IVCADDManager
    {
        /// <summary>Exports all .VCD files from the given folder to .XML.</summary>
        /// <param name="fileArray">The .VCD file array from the input folder.</param>
        /// <param name="inputFolder">The input folder where the  .VCD files are located.</param>
        /// <param name="outputFolder">The output folder where the .XML files are going to be exported.</param>
        /// <param name="newDate">Latest exported file date. Used to only convert the latest file.</param>
        /// <param name="saveToRoot">Ignores the folder name when setting the path to save the file.</param>
        void ExportXML(IEnumerable<IFileInfo> fileArray, string inputFolder, string outputFolder, DateTime newDate, bool saveToRoot);
    }
}