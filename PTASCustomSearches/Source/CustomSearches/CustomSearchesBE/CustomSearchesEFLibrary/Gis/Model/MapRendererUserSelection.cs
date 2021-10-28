using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRendererUserSelection
    {
        public Guid UserId { get; set; }
        public int MapRendererId { get; set; }

        public virtual MapRenderer MapRenderer { get; set; }
    }
}
