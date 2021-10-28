using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class UserMapSelection
    {
        public Guid UserId { get; set; }
        public int UserMapId { get; set; }

        public virtual UserMap UserMap { get; set; }
    }
}
