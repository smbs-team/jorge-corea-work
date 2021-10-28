namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    /// <summary>
    /// Model for the response of the GetChartTemplates service.
    /// </summary>
    public class GetChartTemplatesResponse
    {
        /// <summary>
        /// Gets or sets the chart templates.
        /// </summary>
        public ChartTemplateData[] ChartTemplates { get; set; }
    }
}
