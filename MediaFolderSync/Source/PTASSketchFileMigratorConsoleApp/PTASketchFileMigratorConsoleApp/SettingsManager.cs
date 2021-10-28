// <copyright file="SettingsManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System.Configuration;
    using System.IO.Abstractions;
    using Newtonsoft.Json.Linq;

    /// <summary>Encapsulates common configuration settings usages methods.</summary>
    public class SettingsManager : ISettingsManager
    {
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        /// <param name="fileSystem">
        ///   <para>File system wrapper.</para>
        /// </param>
        public SettingsManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <summary>  Gets the value from the given key.</summary>
        /// <param name="key">  Key used to get its value.</param>
        /// <returns>Value of the given key.</returns>
        public string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var result = appSettings[key] ?? string.Empty;
            return result;
        }

        /// <summary>  Update an existing value.</summary>
        /// <param name="key">  Key used to locate an existing value or the name of the new one.</param>
        /// <param name="value">  The value to modify.</param>
        public void UpdateAppConfig(string key, string value)
        {
            var fileManager = this.fileSystem.File;
            dynamic config = JObject.Parse(fileManager.ReadAllText("./configurations.json"));
            switch (key)
            {
                case "Flag":
                    config.Flag = value;
                    break;
                case "ExportMostRecentFileDate":
                    config.ExportMostRecentFileDate = value;
                    break;
                case "UploadMostRecentFileDate":
                    config.UploadMostRecentFileDate = value;
                    break;
                default:
                    break;
            }

            fileManager.WriteAllText("./configurations.json", config.ToString());
        }

        /// <summary>  Gets the value from the given key.</summary>
        /// <param name="key">  Key used to get its value.</param>
        /// <returns>Value of the given key.</returns>
        public string ReadConfig(string key)
        {
            var fileManager = this.fileSystem.File;
            dynamic config = JObject.Parse(fileManager.ReadAllText("./configurations.json"));

            return config.GetValue(key);
        }
    }
}
