namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters
{
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the schedule land extensions.
    /// </summary>
    public class ScheduleLandExtensionsData
    {
        /// <summary>
        /// Gets or sets the schedule step.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public float ScheduleStep { get; set; }

        /// <summary>
        /// Gets or sets the step value.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public float StepValue { get; set; }

        /// <summary>
        /// Gets or sets the trace message.
        /// </summary>
        public string TraceMessage { get; set; }

        /// <summary>
        /// Gets or sets the step filter.
        /// </summary>
        public string StepFilter { get; set; }
    }
}
