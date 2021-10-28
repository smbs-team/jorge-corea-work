namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using NetTopologySuite.Geometries;

    /// <summary>
    /// Represents a parcel feature.
    /// </summary>
    [ExcludeFromCodeCoverage, Table("PARCEL_GEOM_AREA", Schema = "gis")]
    public class ParcelFeature
    {
        /// <summary>
        /// Gets or sets the Major field.
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the Minor field.
        /// </summary>
        public string Minor { get; set; }

        /// <summary>
        /// Gets or sets the PIN field.
        /// </summary>
        [Key]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the geometry field.
        /// </summary>
        [Column(TypeName = "geometry")]
        public Geometry Shape { get; set; }
    }
}
