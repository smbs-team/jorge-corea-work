namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    /// <summary>
    /// Model for the histogram chart data.
    /// </summary>
    public class HistogramChartData
    {
        /// <summary>
        /// Gets or sets the bin range start.
        /// </summary>
        public double BinRangeStart { get; set; }

        /// <summary>
        /// Gets or sets the bin range end.
        /// </summary>
        public double BinRangeEnd { get; set; }

        /// <summary>
        /// Gets or sets the bin observations.
        /// </summary>
        public int BinObservations { get; set; }

        /// <summary>
        /// Gets or sets the bin category.
        /// </summary>
        public string BinCategory { get; set; }
    }
}
