namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using System;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;

    /// <summary>
    /// Model for the data of the dataset post process execution payload.
    /// </summary>
    public class DatasetPostProcessExecutionPayloadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetPostProcessExecutionPayloadData"/> class.
        /// </summary>
        public DatasetPostProcessExecutionPayloadData()
        {
            this.SingleRowExecutionData = new SingleRowExecutionData();
        }

        /// <summary>
        /// Gets or sets the identifier of the dataset .
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the dataset post process.
        /// </summary>
        public int DatasetPostProcessId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the execution should only generate the view.
        /// </summary>
        public bool OnlyView { get; set; }

        /// <summary>
        /// Gets or sets the custom search parameters.
        /// </summary>
        public CustomSearchParameterValueData[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets the single row execution data.
        /// </summary>
        public SingleRowExecutionData SingleRowExecutionData { get; set; }
    }
}
