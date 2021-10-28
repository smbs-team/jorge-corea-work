namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System;

    /// <summary>
    /// Model for the data of the setup annual update adjustments.
    /// </summary>
    public class SetupAnnualUpdateAdjustmentsData
    {
        /// <summary>
        /// Gets or sets the secondary datasets.
        /// </summary>
        public Guid[] SecondaryDatasets { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the post process is a custom modeling step.
        /// </summary>
        public bool IsCustomModelingStepPostProcess { get; set; }
    }
}
