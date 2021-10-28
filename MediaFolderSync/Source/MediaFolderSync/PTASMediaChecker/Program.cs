namespace PTASMediaChecker
{
    using System;
    using System.Configuration;
    using System.IO;
    using PTASMediaHelperClasses;

    /// <summary>
    /// Main program to run.
    /// </summary>
    internal class Program
    {
        private const int RecordCount = 10;
        private const string OutputPath = @"c:\data\";
        private const string ConnectionName = "ptasDB";
        private static readonly ILogger Logger = new EventLogLogger();

        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args">System arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    throw new Exception("Please addd the table name to the params");
                }

                var tableName = args[0];
                Logger.OptionalOutput($"Start on table: {tableName}");
                var azureWrapper = new AzureStorageHelper(FromConfig("StorageConnectionString"));
                TableChecker tableChecker = new TableChecker(
                    tableName, GetDBConnectionString(ConnectionName));
                var progressManager = new ProgressManager(OutputPath, Logger);
                int lastId = progressManager.GetLastProcessedRecord(tableName);
                bool mustContinue = true;
                while (mustContinue)
                {
                    var startTime = DateTime.Now;
                    mustContinue = tableChecker.Check(
                      lastId,
                      out lastId,
                      (Guid g, string fileExtension) =>
                    {
                        var guidStr = g.ToString();

                        // first four digits with slashes and then the full guid with the file extension.
                        var relativePath = $"{guidStr[0]}/{guidStr[1]}/{guidStr[2]}/{guidStr[3]}/{g}.{fileExtension}";
                        var exists = azureWrapper.FileExists("media", relativePath).Result;
                        Logger.OptionalOutput($"{relativePath} {exists}");
                        if (!exists)
                        {
                            SaveNotExists(tableName, relativePath);
                        }
                    },
                      recordCount: RecordCount);
                    var ellapsedSeconds = DateTime.Now.Subtract(startTime).TotalSeconds;
                    Logger.OptionalOutput($"{lastId} seconds: {ellapsedSeconds} for {RecordCount}. {ellapsedSeconds / RecordCount}");
                    progressManager.SaveLastRecordProcessed(tableName, lastId);
                }
            }
            catch (Exception ex)
            {
                var errorStr = $"{ex.Message} {ex.InnerException?.Message}";
                PTASEventManagerLibrary.ErrorLogger.LogEvent(errorStr, System.Diagnostics.EventLogEntryType.Error);
                Console.Error.WriteLine(errorStr);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Retrieve a string from the configuration.
        /// </summary>
        /// <param name="key">Key for the value.</param>
        /// <returns>Found string or null.</returns>
        private static string FromConfig(string key) => ConfigurationManager.AppSettings[key];

        /// <summary>
        ///  Loads connection string from configuration.
        /// </summary>
        /// <param name="connectionName">Name of the connection to fetch.</param>
        /// <returns>The connection string.</returns>
        private static string GetDBConnectionString(string connectionName) => ConfigurationManager.ConnectionStrings[
          connectionName].ConnectionString;

        private static void SaveNotExists(string tableName, string relativePath)
        {
            Logger.OptionalOutput(relativePath);
            var sourceFile = (FromConfig("file-root") + relativePath).Replace(@"/", @"\");
            if (File.Exists(sourceFile))
            {
                var outputFile = $"{OutputPath}report_{tableName}.txt";
                File.AppendAllLines(outputFile, new string[] { $"{relativePath},{sourceFile}" });
                Logger.OptionalOutput($"Table: {tableName}. File {sourceFile} exists and was not copied.");
            }
            else
            {
                string warning = $"File {relativePath} ({sourceFile}) does not exist on source";
                Logger.WriteWarning(warning);
                Logger.OptionalOutput(warning);
            }
        }
    }
}
