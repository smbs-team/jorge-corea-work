namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PTASTileStorageWorkerLibrary.SqlServer.Model;

    /// <summary>
    /// Database context for tile storage jobs.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class TileStorageJobDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileStorageJobDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public TileStorageJobDbContext(DbContextOptions<TileStorageJobDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the TileStorageJobQueue table.
        /// </summary>
        public virtual DbSet<TileStorageJobQueue> TileStorageJobQueue { get; set; }

        /// <summary>
        /// Gets or sets the TileStorageJobType table.
        /// </summary>
        public virtual DbSet<TileStorageJobType> TileStorageJobType { get; set; }

        /// <summary>
        /// Pops the next storage job.
        /// </summary>
        /// <returns>The next pending storage job if there is any.</returns>
        public virtual TileStorageJobQueue PopNextStorageJob()
        {
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@returnVal", SqlDbType.Int) { Direction = ParameterDirection.Output },
            };

            this.Database.ExecuteSqlCommand("exec @returnVal=PopNextStorageJob", sqlParams);

            int returnValue = (int)sqlParams[0].Value;
            if (returnValue > 0)
            {
                return (from e in this.TileStorageJobQueue.Include(job => job.JobType) where e.JobId == returnValue select e).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Switches staging and final tables.
        /// </summary>
        /// <param name="tableName">Name of the table to switch.</param>
        public virtual void SwitchStagingTable(string tableName)
        {
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@TableName", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = tableName },
            };

            this.Database.ExecuteSqlCommand("exec SwitchStagingTable @TableName", sqlParams);
        }
    }
}