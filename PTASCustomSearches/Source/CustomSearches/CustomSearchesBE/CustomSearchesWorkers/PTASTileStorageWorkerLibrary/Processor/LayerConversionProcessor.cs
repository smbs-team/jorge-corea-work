namespace PTASTileStorageWorkerLibrary.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BaseWorkerLibrary;

    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using PTASServicesCommon.Telemetry;

    using PTASTileStorageWorkerLibrary.Model;
    using PTASTileStorageWorkerLibrary.SystemProcess;

    /// <summary>
    /// Class that executes layer migration jobs.
    /// </summary>
    public class LayerConversionProcessor : WorkerJobProcessor
    {
        private const string VectorConversionFileDestScript = "C:\\CustomSearchesWorker\\Script\\Ogr2ogrFileDest.bat";
        private const string VectorConversionNoFileDestScript = "C:\\CustomSearchesWorker\\Script\\Ogr2ogrNoFileDest.bat";
        private const string FilePassthrough = "C:\\CustomSearchesWorker\\Script\\FileCopy.bat";
        private const string Espg2926Name = "EPSG:2926";
        private const string Espg3857Name = "EPSG:3857";
        private const string Espg4326Name = "EPSG:4326";
        private const string GpkgFileFormat = "GPKG";
        private const string MSSQLSpatialFormat = "MSSQLSpatial";
        private const string ProcessedLayersPath = "mapserver\\processed-layers";
        private const string TileCacheContainerName = "tilecachecontainer";

        /// <summary>
        /// Global configuration.
        /// </summary>
        private IConfiguration configuration;

        /// <summary>
        /// The process factory.
        /// </summary>
        private IProcessFactory processFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConversionProcessor" /> class.
        /// </summary>
        /// <param name="processFactory">The process factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the tileStorageDbContext/configuration/logger parameter is null.</exception>
        public LayerConversionProcessor(
            IProcessFactory processFactory,
            IConfiguration configuration,
            ILogger logger)
            : base(logger)
        {
            if (processFactory == null)
            {
                throw new ArgumentNullException(nameof(processFactory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.processFactory = processFactory;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType
        {
            get
            {
                return "LayerConversionProcessor";
            }
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob">Worker job to process.</param>
        /// <returns>True if the job completed successfully.</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            var payload = JsonConvert.DeserializeObject<LayerConversionPayload>(workerJob.JobPayload);

            this.LogMessage(workerJob, $"Starting job for layer conversion.  JobId: {workerJob.JobId}, Payload: {workerJob.JobPayload}.");

            // if is a ArcGis pretile first stile the sprite if any and then create the renderer json definition.
            if (payload.ConversionType == StorageConversionType.ArcGisPreTiledLayerPassthrough)
            {
                // get paths for sprite tiling
                IConfigurationSection values = this.configuration.GetSection("Values");
                var sourcePathInfo = LayerConversionProcessor.GetSharePathInfo(
                    values["MapServerStorageSharePath"],
                    string.Empty,
                    payload.SourceLocation,
                    false);
                var sourceDir = sourcePathInfo.fullPath;
                var targetDir = payload.TargetLocation; // target folder inside of the container.
                var jsonPath = Path.Combine(sourceDir, "p12\\resources\\sprites\\sprite@2x.json");
                var spritePath = Path.Combine(sourceDir, "p12\\resources\\sprites\\sprite@2x.png");
                var targetSpritePath = targetDir + "/sprites";
                var stylePath = Path.Combine(sourceDir, "p12\\resources\\styles\\root.json");
                var targetRendererPath = targetDir + "/renderers";

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer blobContainer =
                    await this.ServiceContext.PremiumCloudStorageProvider.GetCloudBlobContainer(
                        LayerConversionProcessor.TileCacheContainerName,
                        this.ServiceContext.AppCredential);

                var uploader = new Utilities.BlobUploader(blobContainer);

                await uploader.UploadFolder(Path.Combine(sourceDir, "p12\\tile"), payload.TargetLocation);
                var splitter = new Utilities.SpriteSplitter(blobContainer);
                var splittedMap = await splitter.SplitToFolder(jsonPath, spritePath, targetSpritePath, targetDir);
                var styleConverter = new Utilities.MapboxStyleConverter(blobContainer);
                await styleConverter.ConvertStyle(stylePath, splittedMap, targetRendererPath, targetDir);
            }
            else
            {
                var executionParameters = this.GetJobExecutionParameters(payload);
                if (!string.IsNullOrWhiteSpace(executionParameters.scriptName) && executionParameters.scriptParameters != null)
                {
                    if (!string.IsNullOrWhiteSpace(executionParameters.targetFolder))
                    {
                        Directory.CreateDirectory(executionParameters.targetFolder);
                    }

                    var tmpFileName = string.Empty;
                    if (!string.IsNullOrWhiteSpace(executionParameters.targetFileName))
                    {
                        tmpFileName = Path.GetFileNameWithoutExtension(executionParameters.targetFileName) +
                            Guid.NewGuid().ToString() +
                            Path.GetExtension(executionParameters.targetFileName);
                    }

                    string arguments = this.GetArgumentsString(executionParameters.scriptParameters, executionParameters.targetFileName, tmpFileName);

                    // Delete table in case this writes a table to SQL.  This is a workaround because sometimes the worker gets hung for an unknown reason if the
                    // table exists.
                    if ((payload.ConversionType == StorageConversionType.SqlServerToSqlServer) || (payload.ConversionType == StorageConversionType.FileToSqlServer))
                    {
                        this.DeleteTables(payload);
                    }

                    using (var process = this.processFactory.CreateProcess(
                        new ProcessStartInfo
                        {
                            FileName = executionParameters.scriptName,
                            Arguments = arguments,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }))
                    {
                        this.LogMessage(workerJob, $"Running script {executionParameters.scriptName} with arguments: {arguments}");
                        process.Start();

                        await TelemetryHelper.TrackPerformanceAsync(
                            "LayerConversionProcessor_ExecuteProcess",
                            async () =>
                            {
                                while (!process.Process.HasExited)
                                {
                                    await Task.Delay(1000);
                                }

                                return null;
                            },
                            this.ServiceContext.TelemetryClient,
                            new Dictionary<string, string>() { { "source", payload.SourceLocation } });

                        if (process.ExitCode != 0)
                        {
                            string outputFileName = executionParameters.scriptParameters.Last();
                            string output = string.Empty;

                            try
                            {
                                if (File.Exists(outputFileName))
                                {
                                    output = File.ReadAllText(outputFileName);
                                    File.Delete(outputFileName);
                                }
                            }
                            catch
                            {
                            }

                            int outputLength = Math.Min(1024 * 128, output.Length);

                            // Output is truncated to 128K (it may be too big to serialize in the db).
                            throw new Exception($"Conversion script failed.  Process exited with code: {process.ExitCode}. Output: {output.Substring(0, outputLength)}. Error: {output}");
                        }
                    }

                    if ((payload.ConversionType == StorageConversionType.SqlServerToSqlServer) || (payload.ConversionType == StorageConversionType.FileToSqlServer))
                    {
                        this.SwapTables(payload);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(executionParameters.targetFileName))
                        {
                            this.SwapFiles(executionParameters.targetFolder, executionParameters.targetFileName, tmpFileName);
                        }
                    }

                    this.LogMessage(workerJob, $"Job {workerJob.JobId} finished!");
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job payload.</param>
        /// <returns>
        /// The payload.
        /// </returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            LayerConversionPayload payload = JsonConvert.DeserializeObject<LayerConversionPayload>(workerJob.JobPayload);
            return payload;
        }

        /// <summary>
        /// Gets detailed information about a path.
        /// </summary>
        /// <param name="sharePath">The share path.</param>
        /// <param name="shareSubPath">A sub-folder within the share.</param>
        /// <param name="relativePath">Relative path within the sub-folder.</param>
        /// <param name="throwIfNotFile">if set to <c>true</c> throws an exception if the path doesn't reference a file.</param>
        /// <returns>
        /// Full path, file name, and a boolean indicating if this is a path for a file.
        /// </returns>
        private static (string fullPath, string folderName, string fileName, string extension, bool isFile) GetSharePathInfo(
            string sharePath,
            string shareSubPath,
            string relativePath,
            bool throwIfNotFile)
        {
            string fullPath = Path.Combine(sharePath, shareSubPath, relativePath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);

            if (throwIfNotFile && (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(extension)))
            {
                throw new ArgumentException($"{relativePath} doesn't contain a file name.");
            }

            return (
                fullPath,
                Path.GetDirectoryName(fullPath),
                fileName,
                extension,
                !string.IsNullOrWhiteSpace(extension));
        }

        /// <summary>
        /// Executes any post-process that might be required for the job.
        /// </summary>
        /// <param name="payload">The job.</param>
        private void SwapTables(LayerConversionPayload payload)
        {
            // Switch from staging to final table.
            GisDbContext dbContext = this.ServiceContext.GisDbContextFactory.Create();
            dbContext.SwitchStagingTable(payload.TargetLocation);
        }

        /// <summary>
        /// Executes any post-process that might be required for the job.
        /// </summary>
        /// <param name="payload">The job.</param>
        private void DeleteTables(LayerConversionPayload payload)
        {
            // Switch from staging to final table.
            GisDbContext dbContext = this.ServiceContext.GisDbContextFactory.Create();
            dbContext.DeleteTable(payload.TargetLocation);
            dbContext.DeleteTable(payload.TargetLocation + "_Staging");
        }

        /// <summary>
        /// Executes any post-process that might be required for the job.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="tmpFileName">Name of the temporary file.</param>
        private void SwapFiles(string targetFolder, string fileName, string tmpFileName)
        {
            var targetPath = Path.Combine(targetFolder, fileName);
            var targetTmpPath = Path.Combine(targetFolder, tmpFileName);
            var targetOldPath = Path.Combine(targetFolder, fileName + Guid.NewGuid().ToString());

            bool moved = false;
            if (File.Exists(targetPath))
            {
                File.Move(targetPath, targetOldPath);
                moved = true;
            }

            File.Move(targetTmpPath, targetPath);

            if (moved)
            {
                File.Delete(targetOldPath);
            }
        }

        /// <summary>
        /// Transforms a list of script arguments into a single line string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A single line with the concatenated parameters.</returns>
        private string GetArgumentsString(string[] parameters, string targetFileName, string tmpFileName)
        {
            StringBuilder toReturn = new StringBuilder();
            foreach (var parameter in parameters)
            {
                string toAppend = parameter;
                if (!string.IsNullOrWhiteSpace(targetFileName) && !string.IsNullOrWhiteSpace(tmpFileName))
                {
                    toAppend = toAppend.Replace(targetFileName, tmpFileName);
                }

                toReturn.Append($" {toAppend}");
            }

            return toReturn.ToString();
        }

        /// <summary>
        /// Gets the script name and parameters that will fulfill a specific job.
        /// </summary>
        /// <param name="payLoad">The job.</param>
        /// <returns>Information necessary to execute the job.</returns>
        private (string scriptName, string[] scriptParameters, string targetFolder, string targetFileName) GetJobExecutionParameters(LayerConversionPayload payLoad)
        {
            string outputFileName = $"Output{Guid.NewGuid()}.txt";
            switch (payLoad.ConversionType)
            {
                case StorageConversionType.FileToFile:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        var targetPathInfo = LayerConversionProcessor.GetSharePathInfo(
                            values["MapServerStorageSharePath"],
                            LayerConversionProcessor.ProcessedLayersPath,
                            payLoad.TargetLocation,
                            true);

                        var sourcePathInfo = LayerConversionProcessor.GetSharePathInfo(
                            values["MapServerStorageSharePath"],
                            string.Empty,
                            payLoad.SourceLocation,
                            true);

                        return (
                            LayerConversionProcessor.VectorConversionFileDestScript,
                            new string[]
                            {
                                string.IsNullOrWhiteSpace(payLoad.TargetFormat) ? LayerConversionProcessor.GpkgFileFormat : payLoad.TargetFormat,
                                $"\"{targetPathInfo.fullPath}\"",
                                $"\"{sourcePathInfo.fullPath}\"",
                                string.IsNullOrWhiteSpace(payLoad.SourceSRS) ? $"\"{LayerConversionProcessor.Espg2926Name}\"" : payLoad.SourceSRS,
                                string.IsNullOrWhiteSpace(payLoad.TargetSRS) ? $"\"{LayerConversionProcessor.Espg3857Name}\"" : payLoad.TargetSRS,
                                outputFileName
                            },
                            Path.GetDirectoryName(targetPathInfo.fullPath),
                            targetPathInfo.fileName + targetPathInfo.extension);
                    }

                case StorageConversionType.SqlServerToFile:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        var targetPathInfo = LayerConversionProcessor.GetSharePathInfo(
                          values["MapServerStorageSharePath"],
                          LayerConversionProcessor.ProcessedLayersPath,
                          payLoad.TargetLocation,
                          true);

                        string sourceConnectionString = values["OgrConnectionString"] + $";tables={payLoad.SourceLocation}";

                        return (
                            LayerConversionProcessor.VectorConversionFileDestScript,
                            new string[]
                            {
                                string.IsNullOrWhiteSpace(payLoad.TargetFormat) ? LayerConversionProcessor.GpkgFileFormat : payLoad.TargetFormat,
                                $"\"{targetPathInfo.fullPath}\"",
                                $"\"{sourceConnectionString}\"",
                                string.IsNullOrWhiteSpace(payLoad.SourceSRS) ? $"\"{LayerConversionProcessor.Espg2926Name}\"" : payLoad.SourceSRS,
                                string.IsNullOrWhiteSpace(payLoad.TargetSRS) ? $"\"{LayerConversionProcessor.Espg3857Name}\"" : payLoad.TargetSRS,
                                outputFileName
                            },
                            Path.GetDirectoryName(targetPathInfo.fullPath),
                            targetPathInfo.fileName + targetPathInfo.extension);
                    }

                case StorageConversionType.SqlServerToSqlServer:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        string stagingTableName = $"{payLoad.TargetLocation}_Staging";
                        string sourceConnectionString = $"\"{values["OgrConnectionString"]};tables={payLoad.SourceLocation}\"";
                        string targetConnectionString = $"\"{values["OgrConnectionString"]};tables={stagingTableName}\"";

                        return (
                            LayerConversionProcessor.VectorConversionNoFileDestScript,
                            new string[]
                            {
                                LayerConversionProcessor.MSSQLSpatialFormat,
                                targetConnectionString,
                                sourceConnectionString,
                                "\"" + LayerConversionProcessor.Espg2926Name + "\"",
                                "\"" + LayerConversionProcessor.Espg4326Name + "\"",
                                stagingTableName,
                                outputFileName
                            },
                            null,
                            null);
                    }

                case StorageConversionType.FileToSqlServer:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        var sourcePathInfo = LayerConversionProcessor.GetSharePathInfo(
                            values["MapServerStorageSharePath"],
                            string.Empty,
                            payLoad.SourceLocation,
                            true);

                        string stagingTableName = $"{payLoad.TargetLocation}_Staging";
                        string targetConnectionString = $"\"{values["OgrConnectionString"]};tables=gis.{stagingTableName}(ogr_geometry)\"";

                        return (
                            LayerConversionProcessor.VectorConversionNoFileDestScript,
                            new string[]
                            {
                                LayerConversionProcessor.MSSQLSpatialFormat,
                                targetConnectionString,
                                $"\"{sourcePathInfo.fullPath}\"",
                                string.IsNullOrWhiteSpace(payLoad.SourceSRS) ? $"\"{LayerConversionProcessor.Espg2926Name}\"" : payLoad.SourceSRS,
                                string.IsNullOrWhiteSpace(payLoad.TargetSRS) ? $"\"{LayerConversionProcessor.Espg4326Name}\"" : payLoad.TargetSRS,
                                stagingTableName,
                                outputFileName
                            },
                            null,
                            null);
                    }

                case StorageConversionType.FilePassthrough:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        var (fullPath, _, fileName, extension, isFile) = LayerConversionProcessor.GetSharePathInfo(
                            values["MapServerStorageSharePath"],
                            LayerConversionProcessor.ProcessedLayersPath,
                            payLoad.TargetLocation,
                            false);

                        var sourcePathInfo = LayerConversionProcessor.GetSharePathInfo(
                            values["MapServerStorageSharePath"],
                            string.Empty,
                            payLoad.SourceLocation,
                            true);

                        var destPath = fullPath;

                        if (!isFile)
                        {
                            destPath = Path.Combine(destPath, sourcePathInfo.fileName + sourcePathInfo.extension);
                        }

                        var destFolder = Path.GetDirectoryName(destPath);

                        return (
                           LayerConversionProcessor.FilePassthrough,
                           new string[]
                           {
                            $"\"{sourcePathInfo.fullPath}\"",
                            $"\"{destPath}\"",
                           },
                           destFolder,
                           fileName + extension);
                    }

                default:
                    throw new NotImplementedException("Conversion Type not implemented.");
            }
        }
    }
}