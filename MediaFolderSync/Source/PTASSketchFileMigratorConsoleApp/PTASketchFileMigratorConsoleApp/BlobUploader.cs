// <copyright file="BlobUploader.cs" company="King County">
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
    using PTASExportConnector.SDK;
    using PTASketchFileMigratorConsoleApp;
    using PTASketchFileMigratorConsoleApp.Types;

    /// <summary>
    ///   <para>Uploads .XML files to the azure storage container.</para>
    /// </summary>
    public class BlobUploader : IBlobUploader
    {
        private const string DateFormat = "MM dd yyyy h:mm:ss.fffffff tt";
        private readonly ICloudService cloudService;
        private ISettingsManager settings;
        private IDataServices dataServices;

        /// <summary>Initializes a new instance of the <see cref="BlobUploader"/> class.</summary>
        /// <param name="settings">It has the application settings functionality.</param>
        /// <param name="cloudService">Provides the file upload functionality.</param>
        /// <param name="dataServices">Provides data services API requests.</param>
        public BlobUploader(ISettingsManager settings, ICloudService cloudService, IDataServices dataServices)
        {
                this.settings = settings;
                this.cloudService = cloudService;
                this.Counter = 0;
                this.dataServices = dataServices;
        }

        /// <summary>Gets or sets the i.</summary>
        /// <value>
        ///   <para>Counter used to count how many XML files have been exported.</para>
        /// </value>
        public int Counter { get; set; }

        /// <summary>Uploads the XML to the azure storage container.</summary>
        /// <param name="fileArray">  The array of .xml files to upload.</param>
        /// <param name="officialFiles">Array used to check if the file to process is marked as official.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadXML(IEnumerable<IFileInfo> fileArray, List<OfficialFile> officialFiles)
        {
            if (fileArray == null)
            {
                return;
            }

            this.settings.UpdateAppConfig("Flag", "Uploading");
            var mostRecentUploadDate = DateTime.Parse(this.settings.ReadConfig("UploadMostRecentFileDate"));

            try
            {
                Console.WriteLine("Uploading XML files...");
                foreach (IFileInfo file in fileArray)
                {
                    var isOfficial = officialFiles.Find(f => $"{f.SketchName}.xml" == file.Name);
                    if (isOfficial == null)
                    {
                        continue;
                    }

                    this.Counter++;
                    byte[] fileBytes;
                    fileBytes = new byte[file.Length];
                    await file.OpenRead().ReadAsync(fileBytes, 0, (int)file.Length);

                    var path = file.Name;
                    var containerName = this.settings.ReadSetting("BlobContainerName");
                    var outputFolderName = this.settings.ReadSetting("OutputFolder").TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();
                    var folderName = file.DirectoryName.Split(Path.DirectorySeparatorChar).Last();

                    if (folderName != outputFolderName)
                    {
                        folderName = file.DirectoryName.Substring(file.DirectoryName.IndexOf(outputFolderName) + outputFolderName.Count()).TrimStart(Path.DirectorySeparatorChar);
                        path = Path.Combine(folderName, file.Name);
                    }

                    if (DateTime.Compare(file.CreationTime, mostRecentUploadDate) > 0)
                    {
                        mostRecentUploadDate = file.CreationTime;
                    }

                    await this.cloudService.UploadFile(fileBytes, containerName, path);
                    await this.dataServices.SetIsOfficial(isOfficial.EntityResult.GetData());

                    if (this.Counter == 1000)
                    {
                        this.settings.UpdateAppConfig("UploadMostRecentFileDate", mostRecentUploadDate.ToString(DateFormat));
                        this.Counter = 0;
                    }
                }

                Console.WriteLine("Upload Done.");
            }
            finally
            {
                this.settings.UpdateAppConfig("UploadMostRecentFileDate", mostRecentUploadDate.ToString(DateFormat));
            }
        }

        /// <summary>Uploads a file to azure
        /// storage.</summary>
        /// <param name="file">  The file to upload.</param>
        /// <param name="filePath">  The path for the file.</param>
        public void UploadSingleFile(IFileInfo file, string filePath)
        {
            if (file == null)
            {
                return;
            }

            Console.WriteLine("Uploading " + file.Name);
            var containerName = this.settings.ReadSetting("BlobContainerName");

            byte[] fileBytes;
            fileBytes = new byte[file.Length];
            file.OpenRead().Read(fileBytes, 0, (int)file.Length);

            this.cloudService.UploadFile(fileBytes, containerName, filePath);
            Console.WriteLine("Done.");
        }
    }
}
