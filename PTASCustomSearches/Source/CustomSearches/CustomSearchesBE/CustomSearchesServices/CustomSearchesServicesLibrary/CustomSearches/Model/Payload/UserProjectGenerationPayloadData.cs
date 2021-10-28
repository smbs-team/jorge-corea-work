namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using System;

    /// <summary>
    /// Model for the data of the dataset generation payload.
    /// </summary>
    public class UserProjectGenerationPayloadData
    {
        /// <summary>
        /// Gets or sets the user project id.
        /// </summary>
        public int UserProjectId { get; set; }

        /// <summary>
        /// Gets or sets the source dataset ids.
        /// </summary>
        public Guid[] SourceDatasets { get; set; }

        /// <summary>
        /// Gets or sets the new dataset ids.
        /// </summary>
        public Guid[] NewDatasets { get; set; }
    }
}
