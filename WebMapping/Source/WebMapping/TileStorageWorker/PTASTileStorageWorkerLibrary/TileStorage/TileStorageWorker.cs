namespace PTASTileStorageWorkerLibrary.TileStorage
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASServicesCommon.CloudStorage;
    using PTASTileStorageWorkerLibrary.SqlServer.Model;
    using PTASTileStorageWorkerLibrary.SystemProcess;

    /// <summary>
    /// Class that executes storage jobs.
    /// </summary>
    public class TileStorageWorker
    {
        private const string VectorConversionFileDestScript = "Script\\Ogr2ogrFileDest.bat";
        private const string VectorConversionNoFileDestScript = "Script\\Ogr2ogrNoFileDest.bat";
        private const string BlobFilePassthrough = "Script\\AzCopy.bat";
        private const string SqlServerRetrieveJobErrorMessage = "Error retrieving storage job from SQl Server.";
        private const string Espg2926Name = "EPSG:2926";
        private const string Espg3857Name = "EPSG:3857";
        private const string Espg4326Name = "EPSG:4326";
        private const string GpkgFileFormat = "GPKG";
        private const string MSSQLSpatialFormat = "MSSQLSpatial";
        private const string TileSourceContainerName = "tilesource";

        /// <summary>
        /// The tile storage database context.
        /// </summary>
        private TileStorageJobDbContext tileStorageDbContext;

        /// <summary>
        /// Global configuration.
        /// </summary>
        private IConfiguration configuration;

        /// <summary>
        /// The Shared Access Signature provider for blob storage.
        /// </summary>
        private ICloudStorageSharedSignatureProvider blobSasProvider;

        /// <summary>
        /// The cloud storage provider.
        /// </summary>
        private ICloudStorageProvider storageProvider;

        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The process factory.
        /// </summary>
        private IProcessFactory processFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileStorageWorker" /> class.
        /// </summary>
        /// <param name="tileStorageDbContext">The tile storage database context.</param>
        /// <param name="storageProvider">The cloud storage provider.</param>
        /// <param name="blobSasProvider">The Shared Access Signature provider for blob storage.</param>
        /// <param name="processFactory">The process factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the tileStorageDbContext/configuration/logger parameter is null.</exception>
        public TileStorageWorker(
            TileStorageJobDbContext tileStorageDbContext,
            ICloudStorageProvider storageProvider,
            ICloudStorageSharedSignatureProvider blobSasProvider,
            IProcessFactory processFactory,
            IConfiguration configuration,
            ILogger logger)
        {
            if (tileStorageDbContext == null)
            {
                throw new ArgumentNullException(nameof(tileStorageDbContext));
            }

            if (storageProvider == null)
            {
                throw new ArgumentNullException(nameof(storageProvider));
            }

            if (blobSasProvider == null)
            {
                throw new ArgumentNullException(nameof(blobSasProvider));
            }

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

            this.tileStorageDbContext = tileStorageDbContext;
            this.storageProvider = storageProvider;
            this.blobSasProvider = blobSasProvider;
            this.processFactory = processFactory;
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the next pending job in the queue.
        /// </summary>
        /// <returns>True if a job was executed.  False if there was no job.</returns>
        public bool ExecuteNextJob()
        {
            TileStorageJobQueue nextJob = null;

            try
            {
                nextJob = this.tileStorageDbContext.PopNextStorageJob();
            }
            catch (SqlException ex)
            {
                this.logger.LogError(ex, TileStorageWorker.SqlServerRetrieveJobErrorMessage);
            }

            if (nextJob != null)
            {
                this.logger.LogInformation($"Processing job {nextJob.JobId}...");
                string scriptName;
                string[] parameters;
                this.GetScriptForJob(nextJob, out scriptName, out parameters);
                if (!string.IsNullOrEmpty(scriptName) && parameters != null)
                {
                    string arguments = this.GetArgumentsString(parameters);
                    var process = this.processFactory.CreateProcess(
                        new ProcessStartInfo
                        {
                            FileName = scriptName,
                            Arguments = arguments,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = false,
                        });

                    this.logger.LogInformation($"Running script {scriptName} with arguments: {arguments}");
                    process.Start();
                    process.WaitForExit();

                    this.PostProcess(nextJob);

                    this.tileStorageDbContext.TileStorageJobQueue.Remove(nextJob);
                    this.tileStorageDbContext.SaveChanges();
                    this.logger.LogInformation($"Job {nextJob.JobId} finished!");
                }
            }

            return nextJob != null;
        }

        /// <summary>
        /// Executes any post-process that might be required for the job.
        /// </summary>
        /// <param name="job">The job.</param>
        private void PostProcess(TileStorageJobQueue job)
        {
            if (job.JobType.JobFormat == StorageConversionType.SqlServerToSqlServer)
            {
                // Switch from staging to final table.
                this.tileStorageDbContext.SwitchStagingTable(job.JobType.TargetLocation);
            }
        }

        /// <summary>
        /// Transforms a list of script arguments into a single line string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A single line with the concatenated parameters.</returns>
        private string GetArgumentsString(string[] parameters)
        {
            StringBuilder toReturn = new StringBuilder();
            foreach (var parameter in parameters)
            {
                toReturn.Append(" ");
                toReturn.Append(parameter);
            }

            return toReturn.ToString();
        }

        /// <summary>
        /// Gets the script name and parameters that will fulfill a specific job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="scriptName">Name of the script.</param>
        /// <param name="parameters">The parameters.</param>
        private void GetScriptForJob(TileStorageJobQueue job, out string scriptName, out string[] parameters)
        {
            scriptName = null;
            parameters = null;

            StorageConversionType conversionType = (StorageConversionType)job.JobType.JobFormat;

            switch (conversionType)
            {
                case StorageConversionType.SqlServerToGpkg:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        string destPath = Path.Combine(values["StorageSharePath"], job.JobType.TargetLocation);
                        string destFolder = $"\"{Path.GetDirectoryName(destPath)}\"";
                        destPath = $"\"{destPath}\"";

                        string sourceConnectionString = "\"" + values["OgrConnectionString"] + $";tables={job.JobType.SourceLocation}\"";

                        scriptName = TileStorageWorker.VectorConversionFileDestScript;
                        parameters =
                            new string[]
                            {
                                TileStorageWorker.GpkgFileFormat,
                                destPath,
                                destFolder,
                                sourceConnectionString,
                                "\"" + TileStorageWorker.Espg2926Name + "\"",
                                "\"" + TileStorageWorker.Espg3857Name + "\"",
                            };

                        break;
                    }

                case StorageConversionType.SqlServerToSqlServer:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");

                        string stagingTableName = $"{job.JobType.TargetLocation}_Staging";
                        string sourceConnectionString = "\"" + values["OgrConnectionString"] + $";tables={job.JobType.SourceLocation}\"";
                        string targetConnectionString = "\"" + values["OgrConnectionString"] + $";tables={stagingTableName}\"";

                        scriptName = TileStorageWorker.VectorConversionNoFileDestScript;
                        parameters =
                            new string[]
                            {
                                TileStorageWorker.MSSQLSpatialFormat,
                                targetConnectionString,
                                sourceConnectionString,
                                "\"" + TileStorageWorker.Espg2926Name + "\"",
                                "\"" + TileStorageWorker.Espg4326Name + "\"",
                                stagingTableName,
                            };

                        break;
                    }

                case StorageConversionType.BlobFilePassthrough:
                    {
                        IConfigurationSection values = this.configuration.GetSection("Values");
                        string targetFileName = Path.GetFileName(job.JobType.TargetLocation);
                        string targetFolder = Path.GetDirectoryName(job.JobType.TargetLocation);
                        targetFolder = Path.Combine(values["StorageSharePath"], targetFolder);
                        var storageAccount = this.storageProvider.GetCloudStorageAccount();
                        string sasSignature = Task.Run(async () =>
                        {
                            return await this.blobSasProvider.GetSharedSignature(TileStorageWorker.TileSourceContainerName, null);
                        }).Result;

                        string sourceLocation = $"{storageAccount.BlobEndpoint}{TileStorageWorker.TileSourceContainerName}/{job.JobType.SourceLocation}{sasSignature}";

                        scriptName = TileStorageWorker.BlobFilePassthrough;
                        parameters =
                            new string[]
                            {
                                "\"" + sourceLocation + "\"",
                                targetFileName,
                                targetFolder
                            };

                        break;
                    }
            }
        }
    }
}
