namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using NetTopologySuite.Geometries;

    /// <summary>
    /// Represents a parcel feature.
    /// </summary>
    [ExcludeFromCodeCoverage, Table("LayerSource", Schema = "gis")]
    public partial class LayerSource
    {
        /// <summary>
        /// Gets or sets the layer source identifier.
        /// </summary>
        [Key]
        public int LayerSourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the layer source.
        /// </summary>
        public string LayerSourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the gis layer.
        /// </summary>
        public string GisLayerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is parcel source.
        /// </summary>
        public bool IsParcelSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is vector layer.
        /// </summary>
        public bool IsVectorLayer { get; set; }

        /// <summary>
        /// Gets or sets the name of the database table.
        /// </summary>
        public string DbTableName { get; set; }

        /// <summary>
        /// Gets or sets the ogr layer data.
        /// </summary>
        public string OgrLayerData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the layer is served from the share.
        /// </summary>
        public bool ServeFromFileShare { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the layer is a blob pass through.
        /// </summary>
        public bool IsBlobPassThrough { get; set; }
    }
}
