namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the data of column quantile breaks.
    /// </summary>
    public class ColumnQuantileBreaksData
    {
        /// <summary>
        /// Gets or sets the count of class breaks.
        /// </summary>
        public int ClassBreakCount { get; set; }

        /// <summary>
        /// Gets or sets an expression that filters out empty values from the class breaks.
        /// </summary>
        public CustomSearchExpressionData FilterEmptyValuesExpression { get; set; }
    }
}
