namespace PTASMapTileFunctions.Functions
{
    using System.IO;
    using System.Linq;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASTileStorageWorkerLibrary.SqlServer.Model;

    /// <summary>
    /// Function that gets called when a blob file is updated in the tilesource folder.
    /// </summary>
    public class OnBlobTileSourceUpdated
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private TileStorageJobDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnBlobTileSourceUpdated" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="System.ArgumentNullException">When dbContext is null.</exception>
        public OnBlobTileSourceUpdated(TileStorageJobDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new System.ArgumentNullException(nameof(dbContext));
            }

            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates an entry in the tile storage queue.
        /// </summary>
        /// <param name="blobStream">Blob contents.</param>
        /// <param name="blobFileName">The name of the file.</param>
        /// <param name="log">The log.</param>
        [FunctionName("OnBlobTileSourceUpdated")]
        public void Run(
            [BlobTrigger("tilesource/{blobFileName}", Connection = "AzureWebJobsStorage")]Stream blobStream,
            string blobFileName,
            ILogger log)
        {
            log.LogTrace($"C# Blob trigger function Processed blob\n Name:{blobFileName} \n Size: {blobStream.Length} Bytes");
            int jobTypeId = this.GetJobTypeId(blobFileName);
            if (jobTypeId >= 0)
            {
                this.dbContext.TileStorageJobQueue.Add(new TileStorageJobQueue { JobTypeId = jobTypeId });
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the job type identifier for a blob file.
        /// </summary>
        /// <param name="blobFileName">Name of the BLOB file.</param>
        /// <returns>The job type id for the blob file.</returns>
        public int GetJobTypeId(string blobFileName)
        {
            TileStorageJobType matchingJobType =
                (from jt in this.dbContext.TileStorageJobType
                 where jt.SourceLocation.ToLower() == blobFileName.ToLower()
                 select jt).FirstOrDefault();

            // MapServer provider configuration
            if (matchingJobType != null)
            {
                return matchingJobType.JobTypeId;
            }

            return -1;
        }
    }
}