using System;

namespace CustomSearchesEFLibrary
{
    /// <summary>
    /// The folder item interface.
    /// </summary>
    /// <typeparam name="TFolder">The type of each folder.</typeparam>
    public interface IFolderEntity<TFolder>
         where TFolder : class, IFolder<TFolder>, new()
    {
        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        int? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        TFolder ParentFolder { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last modified this map renderer.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }
    }
}
