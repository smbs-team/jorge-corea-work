namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the data of the custom search grid filter condition.
    /// </summary>
    public class CustomSearchGridFilterConditionData
    {
        /// <summary>
        /// Gets or sets the filter type (i.e. 'text', 'number').
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// Gets or sets the type of the filter condition (i.e. 'contains', 'greaterThan').
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the filter to apply.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the the filter to apply, range filter has two values (from and to).
        /// </summary>
        public string FilterTo { get; set; }
    }
}
