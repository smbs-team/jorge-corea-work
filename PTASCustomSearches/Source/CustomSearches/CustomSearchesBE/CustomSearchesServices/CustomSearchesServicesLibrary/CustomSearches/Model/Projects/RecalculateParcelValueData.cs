namespace CustomSearchesServicesLibrary.CustomSearches.Model.Projects
{
    /// <summary>
    /// Model for recalculate parcel value data.
    /// </summary>
    public class RecalculateParcelValueData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateParcelValueData"/> class.
        /// </summary>
        public RecalculateParcelValueData()
        {
        }

        /// <summary>
        /// Gets or sets the parcel major.
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the parcel minor.
        /// </summary>
        public string Minor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the recalculate should execute over the what-if or adjustments project.
        /// </summary>
        public bool IsWhatIf { get; set; }

        /// <summary>
        /// Gets or sets the selected years.
        /// </summary>
        public int[] SelectedYears { get; set; }
    }
}
