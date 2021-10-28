namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for the data query row filter.
    /// </summary>
    public class QueryRowFilterData
    {
        /// <summary>
        /// Gets or sets (if filtering) what the filter model is.
        /// </summary>
        public Dictionary<string, CustomSearchGridFilterConditionData> FilterModel { get; set; }
    }
}
