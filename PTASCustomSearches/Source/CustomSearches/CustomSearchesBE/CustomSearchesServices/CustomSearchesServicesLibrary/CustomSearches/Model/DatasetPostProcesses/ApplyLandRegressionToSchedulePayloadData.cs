namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using System;

    /// <summary>
    /// Model for the data of the dataset post process execution payload.
    /// </summary>
    public class ApplyLandRegressionToSchedulePayloadData
    {
        /// <summary>
        /// Gets or sets the identifier of the dataset .
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the rscript post process.
        /// </summary>
        public int RScriptPostProcessId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the exception post process.
        /// </summary>
        public int ExceptionPostProcessId { get; set; }
    }
}
