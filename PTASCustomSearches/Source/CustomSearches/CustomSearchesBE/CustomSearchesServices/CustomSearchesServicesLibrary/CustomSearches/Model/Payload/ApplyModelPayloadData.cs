namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;

    /// <summary>
    /// Model for the data of the apply model payload.
    /// </summary>
    public class ApplyModelPayloadData : DatasetGenerationPayloadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyModelPayloadData"/> class.
        /// </summary>
        public ApplyModelPayloadData()
        {
            this.SingleRowExecutionData = new SingleRowExecutionData();
        }

        /// <summary>
        /// Gets or sets the user project id.
        /// </summary>
        public int UserProjectId { get; set; }

        /// <summary>
        /// Gets or sets the single row execution data.
        /// </summary>
        public SingleRowExecutionData SingleRowExecutionData { get; set; }
    }
}
