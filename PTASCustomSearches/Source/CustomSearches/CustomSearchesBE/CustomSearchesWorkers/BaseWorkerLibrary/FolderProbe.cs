// <copyright file="FolderProbe.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace BaseWorkerLibrary
{
    using System;
    using System.IO;

    using CustomSearchesWorkerLibrary.RScript;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that is used to check the folder access calling probes in time intervals.
    /// </summary>
    public class FolderProbe
    {
        /// <summary>
        /// The time interval between probes in seconds.
        /// </summary>
        private const int ProbeInterval = 300;

        /// <summary>
        /// The folder path.
        /// </summary>
        private readonly string folderPath;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The next probe time.
        /// </summary>
        private DateTime nextProbeTime = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderProbe" /> class.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContext/logger parameter is null.</exception>
        public FolderProbe(string folderPath, ILogger logger)
        {
            this.folderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Check the folder access calling probes in time intervals.
        /// </summary>
        public void CheckFolderAccess()
        {
            if (this.nextProbeTime > DateTime.UtcNow)
            {
                return;
            }

            this.nextProbeTime = DateTime.UtcNow.AddSeconds(FolderProbe.ProbeInterval);

            try
            {
                var probeFileName = $"ShareProbe[{Guid.NewGuid()}].txt";
                var probeText = $"Test: {probeFileName}";
                var probeFolderPath = Path.Combine(this.folderPath, "probe");
                var probePath = Path.Combine(probeFolderPath, probeFileName);

                Directory.CreateDirectory(probeFolderPath);

                // Create a file to write to.
                using (StreamWriter streamWriter = File.CreateText(probePath))
                {
                    streamWriter.WriteLine(probeText);
                }

                // Open the file to read from.
                using (StreamReader sr = File.OpenText(probePath))
                {
                    string text = sr.ReadLine();
                    if (text != probeText)
                    {
                        throw new Exception("The written text does not match the read text.");
                    }
                }

                File.Delete(probePath);

                WorkerJobCoordinator.LostAccessToShareFolderCount = 0;
                this.logger.LogInformation($"Probe to folder '{this.folderPath}' was successful.");
            }
            catch (Exception ex)
            {
                WorkerJobCoordinator.LostAccessToShareFolderCount++;

                this.logger.LogError(ex, $"Lost access to share folder '{this.folderPath}'.");
            }
        }
    }
}