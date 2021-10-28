// <copyright file="SketchFileMigrator.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using PTASketchFileMigratorConsoleApp;
    using Serilog;

    /// <summary>Allow the application to run and execute all its functionalities.</summary>
    public class SketchFileMigrator
    {
        private readonly IBlobUploader blobUploader;
        private readonly ISettingsManager settings;
        private readonly IDirectoryManager directory;
        private readonly IVCADDManager vcadd;
        private readonly IUtility utility;

        /// <summary>Initializes a new instance of the <see cref="SketchFileMigrator"/> class.</summary>
        /// <param name="blobUploader">It has the upload functionality.</param>
        /// <param name="settings">It has the application settings functionality.</param>
        /// <param name="directory">It has get the files folder functionality.</param>
        /// <param name="utility">Utility methods.</param>
        /// <param name="vcadd">
        ///   <para>It has the file conversion functionality.</para>
        /// </param>
        /// <param name="fileSystem">
        ///   <para>File system wrapper.</para>
        /// </param>
        public SketchFileMigrator(
            IBlobUploader blobUploader,
            ISettingsManager settings,
            IVCADDManager vcadd,
            IDirectoryManager directory,
            IUtility utility)
        {
            this.blobUploader = blobUploader;
            this.settings = settings;
            this.vcadd = vcadd;
            this.directory = directory;
            this.utility = utility;
        }

        /// <summary>Runs this instance.</summary>
        public void Run()
        {
            var flag = this.settings.ReadConfig("Flag");
            const string InputFileExtension = "*.vcd";
            const string OutputFileExtension = "*.xml";
            const string ErrorLogPath = @".\";
            const string ErrorLogFileName = "failedExporting.json";
            const string ContainerErrorFolder = @"ErrorLog\";
            var inputFolder = this.settings.ReadSetting("InputFolder");
            var outputFolder = this.settings.ReadSetting("OutputFolder");
            var defaultDate = DateTime.Parse("12/01/1900 12:00:00 AM");
            var isOfficialToken = this.settings.ReadSetting("isOfficialToken");

            var mostRecentUpload = DateTime.Parse(this.settings.ReadConfig("UploadMostRecentFileDate"));
            var mostRecentExport = DateTime.Parse(this.settings.ReadConfig("ExportMostRecentFileDate"));

            if (isOfficialToken.IsNullOrEmpty())
            {
                Console.WriteLine("Token for the is official API request is empty, check PTASketchFileMigratorConsoleApp.dll.config.\nPress any key to exit.");
                Console.ReadKey();
                return;
            }

            if (string.IsNullOrEmpty(flag))
            {
                flag = "Exporting";
            }

            switch (flag)
            {
                case "Exporting":
                    var filesToExport = this.directory.GetFolderFilesByDate(inputFolder, InputFileExtension, mostRecentExport);
                    if (filesToExport == null)
                    {
                        return;
                    }

                    var filteredFiles = this.utility.FilterByValidFiles(filesToExport);
                    try
                    {
                        this.vcadd.ExportXML(filteredFiles, inputFolder, outputFolder, mostRecentExport, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"[VCADD][Export XML] {ex.Message}");
                        var mostRecent = DateTime.Parse(this.settings.ReadConfig("ExportMostRecentFileDate"));
                        this.vcadd.ExportXML(filteredFiles, inputFolder, outputFolder, mostRecent, true);
                    }

                    var errorLog = this.directory.GetFolderFilesByDate(ErrorLogPath, ErrorLogFileName, defaultDate);
                    if (errorLog != null)
                    {
                        var file = errorLog.FirstOrDefault();
                        this.blobUploader.UploadSingleFile(file, Path.Combine(ContainerErrorFolder, file.Name));
                    }

                    goto case "Uploading";
                case "Uploading":
                    var xmlFiles = this.directory.GetFolderFilesByDate(outputFolder, OutputFileExtension, mostRecentUpload);
                    Task.Run(async () =>
                    {
                        await this.utility.RenameFiles(xmlFiles);
                        await this.blobUploader.UploadXML(xmlFiles, this.utility.OfficialFiles);
                    }).Wait();
                    goto case "Delete";
                case "Delete":
                    this.directory.DeleteFiles(outputFolder);
                    break;
                default:
                    break;
            }
        }
    }
}
