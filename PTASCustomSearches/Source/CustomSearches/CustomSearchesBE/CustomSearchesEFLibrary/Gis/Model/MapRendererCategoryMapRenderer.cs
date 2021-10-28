using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRendererCategoryMapRenderer
    {
        public int MapRendererCategoryId { get; set; }
        public int MapRendererId { get; set; }

        public virtual MapRenderer MapRenderer { get; set; }
        public virtual MapRendererCategory MapRendererCategory { get; set; }
    }
}
