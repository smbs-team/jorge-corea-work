// <copyright file="VcaddManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;

    /// <summary>Convert all .VCD file to .XML from a given folder.</summary>
    public class VCADDManager : IVCADDManager
    {
        /// <summary>The date format
        /// used to save to configuration file.</summary>
        private const string DateFormat = "MM dd yyyy h:mm:ss.fffffff tt";

        /// <summary> Wait time for the task, when it reaches the specified time it will kill the task.</summary>
        private const int MaxWaitTime = 2000;

        private readonly ISettingsManager settings;

        private readonly IVCADDEngine engine;

        private readonly IFileSystem fileSystem;

        /// <summary>Initializes a new instance of the <see cref="VCADDManager"/> class.</summary>
        /// <param name="settings">It has the application settings functionality.</param>
        /// <param name="engine">It has the main VCADD functionality.</param>
        /// <param name="directory">It has create directory functionality.</param>
        /// <param name="fileSystem">
        ///   <para>File system wrapper.</para>
        /// </param>
        public VCADDManager(ISettingsManager settings, IVCADDEngine engine, IFileSystem fileSystem)
        {
            this.settings = settings;
            this.engine = engine;
            this.fileSystem = fileSystem;
            this.Counter = 0;
            this.ErrorList = new List<dynamic>();
        }

        /// <summary>Gets or sets the i.</summary>
        /// <value>
        ///   <para>Counter used to count how many VCD files have been exported.</para>
        /// </value>
        public int Counter { get; set; }

        /// <summary>Gets or sets the error list.</summary>
        /// <value>The error list that stores failed .VCD files.</value>
        public List<dynamic> ErrorList { get; set; }

        /// <inheritdoc/>
        public void ExportXML(IEnumerable<IFileInfo> fileArray, string inputFolder, string outputFolder, DateTime newDate, bool saveToRoot = true)
        {
            this.settings.UpdateAppConfig("Flag", "Exporting");
            var directory = this.fileSystem.Directory;
            var fileManager = this.fileSystem.File;

            if (fileArray == null)
            {
                return;
            }

            this.engine.InitVCADD();

            Console.WriteLine("Exporting...");

            try
            {
                foreach (var file in fileArray)
                {
                    this.Counter++;
                    directory.CreateDirectory(outputFolder);
                    var outputPath = outputFolder;
                    var folderName = file.DirectoryName.Split(Path.DirectorySeparatorChar).Last();
                    var inputFolderName = inputFolder.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();
                    if (folderName != inputFolderName && !saveToRoot)
                    {
                        folderName = file.DirectoryName.Substring(file.DirectoryName.IndexOf(inputFolderName) + inputFolderName.Count());
                        directory.CreateDirectory(outputFolder + folderName);

                        outputPath = outputPath + folderName + @"\";
                    }

                    var task = Task.Run(() => this.engine.LoadVCDFromFile(file.FullName));
                    if (!task.Wait(MaxWaitTime))
                    {
                        Log.Error("Error exporting: {0}, the file may be corrupt.", file.Name);
                        this.engine.ClearDrawing();
                        dynamic vcd = new JObject();
                        vcd.Path = file.FullName;
                        this.ErrorList.Add(vcd);
                        continue;
                    }

                    this.engine.ExportXML(outputPath, file.Name);
                    this.engine.ClearDrawing();

                    if (DateTime.Compare(file.CreationTime, newDate) > 0)
                    {
                        newDate = file.CreationTime;
                    }

                    if (this.Counter == 1000)
                    {
                        this.settings.UpdateAppConfig("ExportMostRecentFileDate", newDate.ToString(DateFormat));
                        if (this.ErrorList.Count() > 0)
                        {
                            this.ExportErrorList(this.ErrorList);
                            this.ErrorList.Clear();
                        }

                        this.Counter = 0;
                    }
                }

                if (this.ErrorList.Count() > 0)
                {
                    this.ExportErrorList(this.ErrorList);
                }

                Console.WriteLine("Done.");
                this.engine.TerminateVCADD();
            }
            finally
            {
                this.settings.UpdateAppConfig("ExportMostRecentFileDate", newDate.ToString(DateFormat));
            }
        }

        /// <summary>Exports the error list to a JSON file.</summary>
        /// <param name="list">The error list.</param>
        private void ExportErrorList(dynamic list)
        {
            var fileManager = this.fileSystem.File;
            var result = JsonConvert.SerializeObject(list);
            fileManager.AppendAllText(@".\" + "failedExporting.json", "*** " + DateTime.Now.ToString() + " ***");
            fileManager.AppendAllText(@".\" + "failedExporting.json", result.ToString());
        }
    }
}
