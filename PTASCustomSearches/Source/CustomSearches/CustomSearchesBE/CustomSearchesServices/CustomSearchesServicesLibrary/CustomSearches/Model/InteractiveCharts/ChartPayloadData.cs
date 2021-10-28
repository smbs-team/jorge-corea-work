namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Model for the interactive chart data.
    /// </summary>
    public class ChartPayloadData
    {
        /// <summary>
        /// Gets or sets the independent variable name.
        /// </summary>
        public string IndependentVariable { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public object[] Values { get; set; }
    }
}
