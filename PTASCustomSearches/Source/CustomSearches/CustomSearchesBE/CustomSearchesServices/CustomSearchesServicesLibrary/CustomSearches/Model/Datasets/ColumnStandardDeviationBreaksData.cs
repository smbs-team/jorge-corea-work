namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the data of column standard deviation breaks.
    /// </summary>
    public class ColumnStandardDeviationBreaksData
    {
        /// <summary>
        /// Gets or sets the interval for the standard deviation breaks.  The interval is expressed as a fraction of the standard deviation (e.g 0.25*STD).
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// Gets or sets Standard deviation function to be used.  If omitted, STDEV will be used.
        /// </summary>
        public string StandardDeviationFunction { get; set; }

        /// <summary>
        /// Gets or sets an expression that filters out empty values from the class breaks.
        /// </summary>
        public CustomSearchExpressionData FilterEmptyValuesExpression { get; set; }
    }
}
