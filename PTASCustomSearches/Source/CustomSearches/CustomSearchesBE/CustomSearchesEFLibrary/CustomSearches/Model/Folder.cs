using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Folder
    {
        public Folder()
        {
            Dataset = new HashSet<Dataset>();
            InverseParentFolder = new HashSet<Folder>();
        }

        public int FolderId { get; set; }
        public int? ParentFolderId { get; set; }
        public Guid? UserId { get; set; }
        public string FolderName { get; set; }
        public string FolderType { get; set; }

        public virtual FolderType FolderTypeNavigation { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual Systemuser User { get; set; }
        public virtual ICollection<Dataset> Dataset { get; set; }
        public virtual ICollection<Folder> InverseParentFolder { get; set; }
    }
}
