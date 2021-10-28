using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRendererLogicType
    {
        public MapRendererLogicType()
        {
            MapRenderer = new HashSet<MapRenderer>();
        }

        public string MapRendererLogicType1 { get; set; }

        public virtual ICollection<MapRenderer> MapRenderer { get; set; }
    }
}
