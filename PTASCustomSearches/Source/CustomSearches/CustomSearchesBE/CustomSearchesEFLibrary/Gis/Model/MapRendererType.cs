using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRendererType
    {
        public MapRendererType()
        {
            MapRenderer = new HashSet<MapRenderer>();
        }

        public string MapRendererType1 { get; set; }

        public virtual ICollection<MapRenderer> MapRenderer { get; set; }
    }
}
