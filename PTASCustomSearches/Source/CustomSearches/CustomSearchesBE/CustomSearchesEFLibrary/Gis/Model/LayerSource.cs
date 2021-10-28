using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class LayerSource
    {
        public LayerSource()
        {
            MapRenderer = new HashSet<MapRenderer>();
            MobileLayerRenderers = new HashSet<MobileLayerRenderer>();
        }

        public int LayerSourceId { get; set; }
        public string LayerSourceName { get; set; }
        public string GisLayerName { get; set; }
        public string Metadata { get; set; }
        public string LayerSourceAlias { get; set; }
        public string Jurisdiction { get; set; }
        public string Organization { get; set; }
        public string Description { get; set; }
        public bool IsParcelSource { get; set; }
        public int DefaultMinZoom { get; set; }
        public int DefaultMaxZoom { get; set; }
        public int DefaultLabelMinZoom { get; set; }
        public int DefaultLabelMaxZoom { get; set; }
        public bool IsVectorLayer { get; set; }
        public string DataSourceUrl { get; set; }
        public int TileSize { get; set; }
        public string DefaultMapboxLayer { get; set; }
        public string NativeMapboxLayers { get; set; }
        public string DefaultLabelMapboxLayer { get; set; }
        public bool HasOfflineSupport { get; set; }
        public bool HasMobileSupport { get; set; }
        public bool HasOverlapSupport { get; set; }
        public bool IsBlobPassThrough { get; set; }
        public string DbTableName { get; set; }
        public string EmbeddedDataFields { get; set; }
        public string OgrLayerData { get; set; }
        public bool ServeFromFileShare { get; set; }

        public virtual ICollection<MapRenderer> MapRenderer { get; set; }
        public virtual ICollection<MobileLayerRenderer> MobileLayerRenderers { get; set; }
    }
}
