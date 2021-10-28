namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    /// <summary>
    /// Model for the data of the custom search grid column.
    /// </summary>
    public class CustomSearchGridColumnData
    {
        /// <summary>
        /// Gets or sets the identifier of the column.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the column.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the field of the column.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the aggregation function to the column to populate the group row with values.
        /// </summary>
        public string AggFunc { get; set; }
    }
}