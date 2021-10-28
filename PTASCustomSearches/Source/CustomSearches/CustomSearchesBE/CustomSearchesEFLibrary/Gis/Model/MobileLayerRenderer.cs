using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class MobileLayerRenderer
    {
        public MobileLayerRenderer()
        {
        }

        public Guid MobileLayerRendererId { get; set; }
        public int LayerSourceId { get; set; }
        public string RendererRules { get; set; }
        public string Query { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public bool IsSelected { get; set; }
        public string Role { get; set; }
        public string Categories { get; set; }

        public virtual LayerSource LayerSource { get; set; }
    }
}
