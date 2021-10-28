namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    /// <summary>
    /// Model for the histogram chart extension data.
    /// </summary>
    public class HistogramChartExtensionData
    {
        /// <summary>
        /// Gets or sets a value indicating whether the chart should autogenerate the number of bins.
        /// </summary>
        public bool AutoBins { get; set; }

        /// <summary>
        /// Gets or sets the number of bins.
        /// </summary>
        public int NumBins { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bins should use discrete values.
        /// </summary>
        public bool UseDiscreteBins { get; set; }
    }
}
