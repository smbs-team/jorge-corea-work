namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions.ValidationGroups
{
    /// <summary>
    /// Custom search expression role type.
    /// </summary>
    public enum ExpressionValidationGroupType
    {
        /// <summary>
        /// Group containing a single expression where each expression can be validated separately.
        /// </summary>
        UnitValidationGroup,

        /// <summary>
        /// Group containing tsql pre/post-commit expressions that need to be validated together.
        /// </summary>
        TSqlPrePostCommitGroup,

        /// <summary>
        /// Chart expressions that need to be validated together.
        /// </summary>
        TSqlChartExpressionGroup,

        /// <summary>
        /// Chart expressions that need to be validated together when no group by should be allowed (e.g. plot)
        /// </summary>
        TSqlChartExpressionGroupNoGroupBy,

        /// <summary>
        /// A group that chains the selection of several columns (or a single column several times).
        /// These are grouped because columns defined later may use replacements ({}) with columns
        /// defined before.
        /// </summary>
        TSqlSelectColumnChainGroup
    }
}
