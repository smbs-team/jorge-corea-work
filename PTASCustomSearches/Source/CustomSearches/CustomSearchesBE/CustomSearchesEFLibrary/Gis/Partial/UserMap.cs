namespace CustomSearchesEFLibrary.Gis.Model
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CustomSearchesEFLibrary;

    public partial class UserMap : IFolderEntity<Folder>
    {
        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        [NotMapped]
        public int? FolderId
        {
            get => this.ParentFolderId; set => this.ParentFolderId = value ?? 0;
        }
    }
}
