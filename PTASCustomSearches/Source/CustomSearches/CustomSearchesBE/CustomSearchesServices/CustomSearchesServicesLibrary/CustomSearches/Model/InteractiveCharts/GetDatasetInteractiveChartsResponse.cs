namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    /// <summary>
    /// Model for the response of the GetDatasetInteractiveCharts service.
    /// </summary>
    public class GetDatasetInteractiveChartsResponse
    {
        /// <summary>
        /// Gets or sets the interactive charts.
        /// </summary>
        public InteractiveChartData[] InteractiveCharts { get; set; }
    }
}
