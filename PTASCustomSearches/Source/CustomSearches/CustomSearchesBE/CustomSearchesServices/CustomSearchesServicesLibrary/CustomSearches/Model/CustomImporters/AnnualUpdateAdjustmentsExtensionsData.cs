namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters
{
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the annual update adjustments extensions.
    /// </summary>
    public class AnnualUpdateAdjustmentsExtensionsData
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the category filter.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CategoryFilter { get; set; }

        /// <summary>
        /// Gets or sets the user filter.
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string UserFilter { get; set; }

        /// <summary>
        /// Gets or sets the factor.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public float Factor { get; set; }

        /// <summary>
        /// Gets or sets the target of the factor.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ApplyFactorTo { get; set; }

        /// <summary>
        /// Gets or sets the minimum land value to factor.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MinimumLandValueToFactor { get; set; }

        /// <summary>
        /// Gets or sets the trace message.
        /// </summary>
        public string TraceMessage { get; set; }
    }
}
