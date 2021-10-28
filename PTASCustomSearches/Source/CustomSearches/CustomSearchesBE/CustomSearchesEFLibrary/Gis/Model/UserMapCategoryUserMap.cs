using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class UserMapCategoryUserMap
    {
        public int UserMapCategoryId { get; set; }
        public int UserMapId { get; set; }

        public virtual UserMap UserMap { get; set; }
        public virtual UserMapCategory UserMapCategory { get; set; }
    }
}
