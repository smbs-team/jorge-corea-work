namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Dataset : IFolderEntity<Folder>
    {
        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        [NotMapped]
        public int? FolderId
        {
            get => this.ParentFolderId; set => this.ParentFolderId = value;
        }
    }
}
