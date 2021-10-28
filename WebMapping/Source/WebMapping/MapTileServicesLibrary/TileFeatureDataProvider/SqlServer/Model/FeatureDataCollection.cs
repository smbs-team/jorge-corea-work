namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using PTASMapTileServicesLibrary.Geography.Data;

    /// <summary>
    /// Collection of feature data and information of the tile it comes from.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeatureDataCollection
    {
        /// <summary>
        /// Gets or sets the layer identifier.
        /// </summary>
        public TileLocation Location { get; set; }

        /// <summary>
        /// Gets or sets the features data.
        /// </summary>
        public IEnumerable<dynamic> FeaturesData { get; set; }
    }
}
