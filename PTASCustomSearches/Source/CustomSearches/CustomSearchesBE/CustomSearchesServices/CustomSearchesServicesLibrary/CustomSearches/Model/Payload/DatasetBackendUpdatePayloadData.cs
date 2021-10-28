namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using System;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;

    /// <summary>
    /// Model for dataset backend update payload.
    /// </summary>
    public class DatasetBackendUpdatePayloadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetBackendUpdatePayloadData"/> class.
        /// </summary>
        public DatasetBackendUpdatePayloadData()
        {
            this.SingleRowExecutionData = new SingleRowExecutionData();
        }

        /// <summary>
        /// Gets or sets the identifier of the dataset.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the dataset post process.
        /// </summary>
        public int DatasetPostProcessId { get; set; }

        /// <summary>
        /// Gets or sets the single row execution data.
        /// </summary>
        public SingleRowExecutionData SingleRowExecutionData { get; set; }

        /// <summary>
        /// Checks if the payloads are equal.
        /// </summary>
        /// <param name="payload">The payload to compare.</param>
        /// <returns>True if the payloads are equal, otherwise false.</returns>
        public bool Equals(DatasetBackendUpdatePayloadData payload)
        {
            return (this.DatasetId == payload.DatasetId) && (this.DatasetPostProcessId == payload.DatasetPostProcessId);
        }
    }
}
