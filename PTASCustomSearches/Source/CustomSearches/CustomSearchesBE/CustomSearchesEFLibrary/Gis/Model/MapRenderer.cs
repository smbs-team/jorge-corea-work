using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MapRenderer
    {
        public MapRenderer()
        {
            MapRendererCategoryMapRenderer = new HashSet<MapRendererCategoryMapRenderer>();
            MapRendererUserSelection = new HashSet<MapRendererUserSelection>();
        }

        public int MapRendererId { get; set; }
        public string MapRendererName { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public bool HasLabelRenderer { get; set; }
        public Guid? DatasetId { get; set; }
        public string MapRendererType { get; set; }
        public string MapRendererLogicType { get; set; }
        public string RendererRules { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public int UserMapId { get; set; }
        public int LayerSourceId { get; set; }

        public virtual LayerSource LayerSource { get; set; }

        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual MapRendererLogicType MapRendererLogicTypeNavigation { get; set; }
        public virtual MapRendererType MapRendererTypeNavigation { get; set; }
        public virtual UserMap UserMap { get; set; }
        public virtual ICollection<MapRendererCategoryMapRenderer> MapRendererCategoryMapRenderer { get; set; }
        public virtual ICollection<MapRendererUserSelection> MapRendererUserSelection { get; set; }
    }
}
