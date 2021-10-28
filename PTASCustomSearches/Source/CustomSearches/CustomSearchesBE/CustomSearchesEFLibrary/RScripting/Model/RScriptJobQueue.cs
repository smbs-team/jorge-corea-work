namespace CustomSearchesEFLibrary.RScripting.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entity for TileStorageJobQueue table.
    /// </summary>
    [Table("RScriptJobQueue", Schema = "dbo")]
    public class RScriptJobQueue
    {
        /// <summary>
        /// Gets or sets the JobId field.
        /// </summary>
        [Key]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the RegisteredRScriptId field.
        /// </summary>
        public int RegisteredRScriptId { get; set; }

        /// <summary>
        /// Gets or sets the ExecutionTime field.
        /// </summary>
        public double? ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the JobResult field.
        /// </summary>
        public string JobResult { get; set; }

        /// <summary>
        /// Gets or sets the RegisteredRScript field.
        /// </summary>
        public RegisteredRScript RegisteredRScript { get; set; }
    }
}
