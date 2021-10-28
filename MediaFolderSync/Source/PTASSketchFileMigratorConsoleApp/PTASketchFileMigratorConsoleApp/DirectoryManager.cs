// <copyright file="DirectoryManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Serilog;

    /// <summary>Provides a custom methods to retrieve, copy and delete files.</summary>
    public class DirectoryManager : IDirectoryManager
    {
        private readonly ISettingsManager settings;
        private readonly IFileSystem fileSystem;

        /// <summary>Initializes a new instance of the <see cref="DirectoryManager"/> class.</summary>
        /// <param name="settings">Application configurations methods.</param>
        /// <param name="fileSystem">
        ///   <para>File system wrapper.</para>
        /// </param>
        public DirectoryManager(ISettingsManager settings, IFileSystem fileSystem)
        {
            this.settings = settings;
            this.fileSystem = fileSystem;
        }

        /// <summary>Gets all folder files by date.</summary>
        /// <param name="folder">  Folder where the files are located.</param>
        /// <param name="extension">  The file extension to look for.</param>
        /// <param name="mostRecentFile">  Date of the most recent file, used to return the unprocessed files.</param>
        /// <returns>Array of files of the given extension.</returns>
        public IOrderedEnumerable<IFileInfo> GetFolderFilesByDate(string folder, string extension, DateTime mostRecentFile)
        {
            Console.WriteLine("Getting " + folder + " files...");
            var directory = this.fileSystem.Directory;
            directory.CreateDirectory(folder);

            var directoryInfo = this.fileSystem.DirectoryInfo.FromDirectoryName(folder);

            var files = directoryInfo.GetFiles(extension, SearchOption.AllDirectories).Where(x => DateTime.Compare(x.CreationTime, mostRecentFile) > 0).OrderBy(x => x.CreationTime);
            if (files.Count() == 0)
                {
                    Log.Information("No new files found. Latest file date: {0}", mostRecentFile.ToString());
                    return null;
                }
                else
                {
                    Console.WriteLine(files.Count() + " files found.");
                    Console.WriteLine("Done.");
                    return files;
                }
         }

        /// <summary>Deletes the files and folder from a given path.</summary>
        /// <param name="folder">The folder path to be deleted.</param>
        public void DeleteFiles(string folder)
        {
            this.settings.UpdateAppConfig("Flag", "Delete");
            var directory = this.fileSystem.Directory;

            var ogFolder = this.settings.ReadSetting("InputFolder");
            if (folder == ogFolder)
            {
                return;
            }

            Console.WriteLine("Deleting: " + folder + " contents.");

            var files = directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                DeleteFile(file);
            }

            var directories = directory.EnumerateDirectories(folder);
            foreach (string dir in directories)
            {
               directory.Delete(dir);
            }

            this.settings.UpdateAppConfig("Flag", "Exporting");
            Console.WriteLine("Done.");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFile(string lpFileName);
    }
}
