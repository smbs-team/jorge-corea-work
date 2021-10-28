namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// Custom search expression role type.
    /// </summary>
    public enum CustomSearchExpressionRoleType
    {
        /// <summary>
        /// Calculated column.
        /// </summary>
        CalculatedColumn,

        /// <summary>
        /// Calculated column post commit.
        /// </summary>
        CalculatedColumnPostCommit,

        /// <summary>
        /// Calculated column pre commit.
        /// </summary>
        CalculatedColumnPreCommit,

        /// <summary>
        /// Dependent variable.
        /// </summary>
        DependentVariable,

        /// <summary>
        /// Edit lookup expression.
        /// </summary>
        EditLookupExpression,

        /// <summary>
        /// Filter expression.
        /// </summary>
        FilterExpression,

        /// <summary>
        /// Independent variable.
        /// </summary>
        IndependentVariable,

        /// <summary>
        /// Lookup expression.
        /// </summary>
        LookupExpression,

        /// <summary>
        /// RScript parameter.
        /// </summary>
        RScriptParameter,

        /// <summary>
        /// Calculated column pre commit dependent expression role.
        /// </summary>
        CalculatedColumnPreCommit_Dependent,

        /// <summary>
        /// Calculated column pre commit independent expression role.
        /// </summary>
        CalculatedColumnPreCommit_Independent,

        /// <summary>
        /// Group filter expression.
        /// </summary>
        GroupFilterExpression,

        /// <summary>
        /// Validation condition expression.
        /// </summary>
        ValidationConditionExpression,

        /// <summary>
        /// Validation error expression.
        /// </summary>
        ValidationErrorExpression,

        /// <summary>
        /// Group by variable expression.
        /// </summary>
        GroupByVariable,

        /// <summary>
        /// Dynamic evaluator role type.
        /// </summary>
        DynamicEvaluator,

        /// <summary>
        /// Update stored procedure role type.
        /// </summary>
        UpdateStoredProcedure,

        /// <summary>
        /// Ranged values override role type.  Used by GisMapDataWrapper to override value retrieval for ranged value services
        /// (GetDatasetColumnLookupValues, GetDatasetColumnRangeValues, GetDatasetColumnBreaksStdDeviation and GetDatasetColumnBreaksQuantile).
        /// Values overrides only work when not working with post-processes.
        /// </summary>
        RangedValuesOverrideExpression
    }
}