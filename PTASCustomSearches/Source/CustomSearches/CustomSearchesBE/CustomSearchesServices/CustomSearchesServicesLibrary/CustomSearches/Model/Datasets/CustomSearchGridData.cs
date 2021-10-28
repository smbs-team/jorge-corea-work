namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for the data of the custom search grid.
    /// </summary>
    public class CustomSearchGridData
    {
        /// <summary>
        /// Gets or sets the first row requested.
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Gets or sets the first row to "Not Get".
        /// </summary>
        public int EndRow { get; set; }

        /// <summary>
        /// Gets or sets the row group columns.
        /// </summary>
        public CustomSearchGridColumnData[] RowGroupCols { get; set; }

        /// <summary>
        /// Gets or sets the value columns.
        /// </summary>
        public CustomSearchGridColumnData[] ValueCols { get; set; }

        /// <summary>
        /// Gets or sets pivot columns.
        /// </summary>
        public CustomSearchGridColumnData[] PivotCols { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pivot mode is one.
        /// </summary>
        public bool PivotMode { get; set; }

        /// <summary>
        /// Gets or sets what groups the user is viewing.
        /// </summary>
        public List<object> GroupKeys { get; set; }

        /// <summary>
        /// Gets or sets (if filtering) what the filter model is.
        /// </summary>
        public Dictionary<string, CustomSearchGridFilterConditionData> FilterModel { get; set; }

        /// <summary>
        /// Gets or sets (if sorting) what the sort model is.
        /// </summary>
        public CustomSearchGridSortingData[] SortModel { get; set; }
    }
}
