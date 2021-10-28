namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;

    /// <summary>
    /// Model for the response of the GetUserCustomSearchData service.
    /// </summary>
    public class GetUserCustomSearchDataResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCustomSearchDataResponse"/> class.
        /// </summary>
        public GetUserCustomSearchDataResponse()
        {
            this.TotalRows = -1;
            this.TotalUpdatedRows = -1;
            this.TotalExportedRows = -1;
        }

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        public object[] Results { get; set; }

        /// <summary>
        /// Gets or sets the total rows field.
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// Gets or sets the total updated rows field.
        /// </summary>
        public int TotalUpdatedRows { get; set; }

        /// <summary>
        /// Gets or sets the total exported rows field.
        /// </summary>
        public int TotalExportedRows { get; set; }

        /// <summary>
        /// Gets or sets information about what columns have been updated per row.
        /// </summary>
        public RowUpdatedColumnsData[] UpdatedColumns { get; set; }
    }
}
