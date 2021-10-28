// <copyright file="ISettingsManager.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    /// <summary>Defines methods to read, add and update the application settings variables.</summary>
    public interface ISettingsManager
    {
        /// <inheritdoc cref="SettingsManager.UpdateAppConfig(string, string)"/>
        void UpdateAppConfig(string key, string value);

        /// <inheritdoc cref="SettingsManager.ReadSetting(string)"/>
        string ReadSetting(string key);

        /// <inheritdoc cref="SettingsManager.ReadConfig(string)"/>
        string ReadConfig(string key);
    }
}