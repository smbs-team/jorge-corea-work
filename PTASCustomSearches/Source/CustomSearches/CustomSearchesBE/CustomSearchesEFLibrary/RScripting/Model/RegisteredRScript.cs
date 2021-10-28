namespace CustomSearchesEFLibrary.RScripting.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entity for RegisteredRScript table.
    /// </summary>
    [Table("RegisteredRScript", Schema = "dbo")]
    public class RegisteredRScript
    {
        /// <summary>
        /// Gets or sets the RScriptId field.
        /// </summary>
        [Key]
        public int RegisteredRScriptId { get; set; }

        /// <summary>
        /// Gets or sets the RScript field.
        /// </summary>
        public string RScript { get; set; }

        /// <summary>
        /// Gets or sets the OutputType field.
        /// </summary>
        public int OutputType { get; set; }
    }
}
