using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class FolderItemType
    {
        public FolderItemType()
        {
            Folder = new HashSet<Folder>();
        }

        public string FolderItemType1 { get; set; }

        public virtual ICollection<Folder> Folder { get; set; }
    }
}
