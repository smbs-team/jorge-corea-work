namespace PTAS.MagicLinkService.Models
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Entity that represents the information about a MagicLinkLogin.
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class QrTokenEvidenceEntity : TableEntity
    {
        /// <summary>
        /// The table name for this entity.
        /// </summary>
        public const string TableName = "QrTokenEvidence";

        /// <summary>
        /// The constant partition key for this entity.
        /// </summary>
        public const string QrTokenEvidencePartitionKey = "0";

        /// <summary>
        /// Initializes a new instance of the <see cref="QrTokenEvidenceEntity"/> class.
        /// </summary>
        public QrTokenEvidenceEntity()
        {
        }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }
    }
}
