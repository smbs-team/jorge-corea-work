namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Response object for the GetFeatures request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeatureDataResponse
    {
        /// <summary>
        /// Gets or sets the layer identifier.
        /// </summary>
        public string LayerId { get; set; }

        /// <summary>
        /// Gets or sets the features data.
        /// </summary>
        public FeatureDataCollection[] FeaturesDataCollections { get; set; }
    }
}
