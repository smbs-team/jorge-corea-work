namespace PTASTileStorageWorkerLibrary.SqlServer.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entity for TileStorageJobType table.
    /// </summary>
    [Table("TileStorageJobType", Schema = "gis")]
    public class TileStorageJobType
    {
        /// <summary>
        /// Gets or sets the JobTypeId field.
        /// </summary>
        [Key]
        public int JobTypeId { get; set; }

        /// <summary>
        /// Gets or sets the SourceLocation field.
        /// </summary>
        public string SourceLocation { get; set; }

        /// <summary>
        /// Gets or sets the JobFormat field.
        /// </summary>
        [Column(TypeName = "int")]
        public StorageConversionType JobFormat { get; set; }

        /// <summary>
        /// Gets or sets the TargetLocation field.
        /// </summary>
        /// <value>
        /// The target location.
        /// </value>
        public string TargetLocation { get; set; }
    }
}
