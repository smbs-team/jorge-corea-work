namespace CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the response of the GetRScriptModelDefaultExpressions service.
    /// </summary>
    public class GetRScriptModelDefaultExpressionsResponse
    {
        /// <summary>
        /// Gets or sets the custom expressions for the RScriptModel.
        /// </summary>
        public CustomSearchExpressionData[] Expressions { get; set; }
    }
}
