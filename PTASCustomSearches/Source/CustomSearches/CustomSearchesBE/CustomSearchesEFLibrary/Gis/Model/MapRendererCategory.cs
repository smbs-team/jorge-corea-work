using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRendererCategory
    {
        public MapRendererCategory()
        {
            MapRendererCategoryMapRenderer = new HashSet<MapRendererCategoryMapRenderer>();
        }

        public int MapRendererCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<MapRendererCategoryMapRenderer> MapRendererCategoryMapRenderer { get; set; }
    }
}
