using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary
{
    /// <summary>
    /// The folder interface.
    /// </summary>
    /// <typeparam name="TFolder">The type of each folder.</typeparam>
    public interface IFolder<TFolder>
        where TFolder : class, IFolder<TFolder>, new()
    {
        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        int FolderId { get; set; }

        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        int? ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        TFolder ParentFolder { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the folder type.
        /// </summary>
        string FolderType { get; set; }

        /// <summary>
        /// Gets or sets the folder name.
        /// </summary>
        string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public ICollection<TFolder> InverseParentFolder { get; set; }
    }
}
