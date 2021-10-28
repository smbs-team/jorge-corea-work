namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    /// <summary>
    /// Model for the response of the GetChartData service.
    /// </summary>
    public class GetChartDataResponse
    {
        /// <summary>
        /// Gets or sets the chart title.
        /// </summary>
        public string ChartTitle { get; set; }

        /// <summary>
        /// Gets or sets the chart type.
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// Gets or sets the continuation token.
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// Gets or sets the dataset client state.
        /// </summary>
        public ChartPayloadData[] Results { get; set; }
    }
}
