using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class UserMap
    {
        public UserMap()
        {
            MapRenderer = new HashSet<MapRenderer>();
            UserMapCategoryUserMap = new HashSet<UserMapCategoryUserMap>();
            UserMapSelection = new HashSet<UserMapSelection>();
        }

        public int UserMapId { get; set; }
        public string UserMapName { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public int ParentFolderId { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }

        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual ICollection<MapRenderer> MapRenderer { get; set; }
        public virtual ICollection<UserMapCategoryUserMap> UserMapCategoryUserMap { get; set; }
        public virtual ICollection<UserMapSelection> UserMapSelection { get; set; }
    }
}
