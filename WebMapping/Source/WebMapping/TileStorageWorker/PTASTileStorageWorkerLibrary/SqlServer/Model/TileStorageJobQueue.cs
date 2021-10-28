namespace PTASTileStorageWorkerLibrary.SqlServer.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entity for TileStorageJobQueue table.
    /// </summary>
    [Table("TileStorageJobQueue", Schema = "gis")]
    public class TileStorageJobQueue
    {
        /// <summary>
        /// Gets or sets the JobId field.
        /// </summary>
        [Key]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the JobTypeId field.
        /// </summary>
        public int JobTypeId { get; set; }

        /// <summary>
        /// Gets or sets the JobType field.
        /// </summary>
        public TileStorageJobType JobType { get; set; }
    }
}
