namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using System;

    /// <summary>
    /// Base model for dataset payloads.
    /// </summary>
    public class DatasetPayloadData
    {
        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }
    }
}
