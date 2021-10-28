// <copyright file="VCADDEngine.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System.IO;
    using PTASVCADDAbstractionLibrary;

    /// <summary>Provides methods to initialize and terminate automation engine class.</summary>
    public class VCADDEngine : IVCADDEngine
    {
        private readonly IAutomationEngine vcadd;

        /// <summary>Initializes a new instance of the <see cref="VCADDEngine"/> class.</summary>
        /// <param name="vcadd"> It has the necessary VCADD functionality.</param>
        public VCADDEngine(IAutomationEngine vcadd)
        {
            this.vcadd = vcadd;
        }

        /// <summary>Initializes an instance of AutomationEngine and creates a space for the drawings.</summary>
        public void InitVCADD()
        {
            this.vcadd.Init();
            var world = this.vcadd.NewWorld(90000);
            this.vcadd.SetCurrWorld(world);
            this.vcadd.SetDisplayHiddenLayersMessage(0);
        }

        /// <summary>Terminates a VCADD instance and frees the drawing area space (world).</summary>
        /// <param name="vcaddInstance">  An instance of automation engine class.</param>
        public void TerminateVCADD()
        {
            this.vcadd.DestroyWorld(this.vcadd.GetCurrWorld());
            this.vcadd.Terminate();
        }

        /// <summary>Loads the VCD from file.</summary>
        /// <param name="fullName">  The full path to the file to load.</param>
        public void LoadVCDFromFile(string fullName)
        {
            this.vcadd.LoadVCDFromFile(fullName);
        }

        /// <summary>Exports the VCD to XML.</summary>
        /// <param name="outputPath">The output path.</param>
        /// <param name="fileName">Name of the file.</param>
        public void ExportXML(string outputPath, string fileName)
        {
            this.vcadd.ExportXML(this.vcadd.GetXMLStruct(), outputPath + Path.GetFileNameWithoutExtension(fileName) + ".xml", 0);
        }

        /// <summary>Clears the drawing from the current world or drawing space.</summary>
        public void ClearDrawing()
        {
            this.vcadd.ClearDrawingNoPrompt(this.vcadd.GetCurrWorld());
        }
    }
}
