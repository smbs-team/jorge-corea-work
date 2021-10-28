namespace CustomSearchesServicesLibrary.CustomSearches.Model.Payload
{
    using System;
    using System.Linq;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the data of the dataset generation payload.
    /// </summary>
    public class DatasetGenerationPayloadData : DatasetPayloadData
    {
        /// <summary>
        /// Gets or sets the custom search definition id.
        /// </summary>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the custom search parameters.
        /// </summary>
        public CustomSearchParameterValueData[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dataset generation corresponds to a duplication.
        /// </summary>
        public Guid SourceDatasetId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row filter should be used to define the rows to duplicate.
        /// </summary>
        public bool ApplyRowFilterFromSourceDataset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user selection should be used to define the rows to duplicate.
        /// </summary>
        public bool ApplyUserSelectionFromSourceDataset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the related post processes should be executed.
        /// </summary>
        public bool NeedsPostProcessExecution { get; set; }

        /// <summary>
        /// Gets or sets the dataset execution mode.
        /// </summary>
        public DatasetExecutionMode ExecutionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the custom search should be validated.
        /// </summary>
        public bool Validate { get; set; }

        /// <summary>
        /// Gets or sets the script to insert into the SQL user-defined table type parameter used in the custom search stored procedure.
        /// </summary>
        public string TableTypeInputParameterScript { get; set; }

        /// <summary>
        /// Gets a value indicating whether the payload is used in a single row execution mode.
        /// </summary>
        public bool IsSingleRowExecutionMode
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(this.RowMajor) && !string.IsNullOrWhiteSpace(this.RowMinor)) || (!string.IsNullOrWhiteSpace(this.IsRecalculation));
            }
        }

        /// <summary>
        /// Gets the major value of the row when the payload is used in a single row execution mode.
        /// </summary>
        public string RowMajor
        {
            get
            {
                return this.Parameters?.FirstOrDefault(p => p.Name?.ToLower() == "major")?.Value;
            }
        }

        /// <summary>
        /// Gets the minor value of the row when the payload is used in a single row execution mode.
        /// </summary>
        public string RowMinor
        {
            get
            {
                return this.Parameters?.FirstOrDefault(p => p.Name?.ToLower() == "minor")?.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the payload is used in a single row execution mode.
        /// </summary>
        public string IsRecalculation
        {
            get
            {
                return this.Parameters?.FirstOrDefault(p => p.Name?.ToLower() == "isrecalculation")?.Value;
            }
        }
    }
}
