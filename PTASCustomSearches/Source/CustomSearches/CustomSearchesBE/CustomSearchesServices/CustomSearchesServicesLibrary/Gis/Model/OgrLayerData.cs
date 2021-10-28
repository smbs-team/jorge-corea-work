namespace CustomSearchesServicesLibrary.Gis.Model
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Model for the data of the layer source.
    /// </summary>
    public class OgrLayerData
    {
        /// <summary>
        /// Gets or sets the layer name.
        /// </summary>
        public string LayerName { get; set; }

        /// <summary>
        /// Gets or sets the ogr layer name.
        /// </summary>
        public string OgrLayerName { get; set; }

        /// <summary>
        /// Gets or sets the layer file path.
        /// </summary>
        public string LayerConnectionPath { get; set; }

        /// <summary>
        /// Gets or sets the layer projection.
        /// </summary>
        public string LayerProjection { get; set; }

        /// <summary>
        /// Gets or sets the OGR Type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public OgrLayerType OgrLayerType { get; set; }
    }
}
