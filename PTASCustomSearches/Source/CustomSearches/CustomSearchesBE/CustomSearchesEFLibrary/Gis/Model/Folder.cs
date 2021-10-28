using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class Folder
    {
        public Folder()
        {
            InverseParentFolder = new HashSet<Folder>();
            UserMap = new HashSet<UserMap>();
        }

        public int FolderId { get; set; }
        public int? ParentFolderId { get; set; }
        public Guid? UserId { get; set; }
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public string FolderItemType { get; set; }

        public virtual FolderItemType FolderItemTypeNavigation { get; set; }
        public virtual FolderType FolderTypeNavigation { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual Systemuser User { get; set; }
        public virtual ICollection<Folder> InverseParentFolder { get; set; }
        public virtual ICollection<UserMap> UserMap { get; set; }
    }
}
