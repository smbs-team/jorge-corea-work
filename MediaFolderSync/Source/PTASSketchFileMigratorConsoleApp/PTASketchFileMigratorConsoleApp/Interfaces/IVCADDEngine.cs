// <copyright file="IVCADDEngine.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    /// <summary>Provides methods to initialize and terminate automation engine class.</summary>
    public interface IVCADDEngine
    {
        /// <inheritdoc cref = "VCADDEngine.ExportXML(string, string)"/>
        void ExportXML(string outputPath, string fileName);

        /// <inheritdoc cref = "VCADDEngine.InitVCADD"/>
        void InitVCADD();

        /// <inheritdoc cref = "VCADDEngine.LoadVCDFromFile(string)"/>
        void LoadVCDFromFile(string fullName);

        /// <inheritdoc cref = "VCADDEngine.TerminateVCADD"/>
        void TerminateVCADD();

        /// <inheritdoc cref = "VCADDEngine.ClearDrawing"/>
        void ClearDrawing();
    }
}