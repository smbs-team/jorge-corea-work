namespace PTASTileStorageWorkerLibrary.Model
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using PTASTileStorageWorkerLibrary.Processor;

    /// <summary>
    /// Model for layer conversion payloads.
    /// </summary>
    public class LayerConversionPayload
    {
        /// <summary>
        /// Gets or sets the dataset data.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public StorageConversionType ConversionType { get; set; }

        /// <summary>
        /// Gets or sets the source location of the layer to be transformed.
        /// </summary>
        public string SourceLocation { get; set; }

        /// <summary>
        /// Gets or sets the target location of the layer to be transformed.
        /// </summary>
        public string TargetLocation { get; set; }

        /// <summary>
        /// Gets or sets the source SRS of the layer to be transformed.
        /// </summary>
        public string SourceSRS { get; set; }

        /// <summary>
        /// Gets or sets the target SRS of the layer to be transformed.
        /// </summary>
        public string TargetSRS { get; set; }

        /// <summary>
        /// Gets or sets the format for the target layer.
        /// </summary>
        public string TargetFormat { get; set; }
    }
}
