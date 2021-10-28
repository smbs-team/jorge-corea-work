using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class FolderType
    {
        public FolderType()
        {
            Folder = new HashSet<Folder>();
        }

        public string FolderType1 { get; set; }

        public virtual ICollection<Folder> Folder { get; set; }
    }
}
