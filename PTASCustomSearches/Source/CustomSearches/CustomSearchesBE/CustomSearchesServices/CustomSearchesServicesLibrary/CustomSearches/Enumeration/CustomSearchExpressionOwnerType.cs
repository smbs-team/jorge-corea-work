namespace CustomSearchesServicesLibrary.CustomSearches.Enumeration
{
    /// <summary>
    /// Custom search expression role type.
    /// </summary>
    public enum CustomSearchExpressionOwnerType
    {
        /// <summary>
        /// No Owner Type.
        /// </summary>
        NoOwnerType,

        /// <summary>
        /// Custom search column definition.
        /// </summary>
        CustomSearchColumnDefinition,

        /// <summary>
        /// Custom search definition.
        /// </summary>
        CustomSearchDefinition,

        /// <summary>
        /// Custom search parameter.
        /// </summary>
        CustomSearchParameter,

        /// <summary>
        /// Dataset.
        /// </summary>
        Dataset,

        /// <summary>
        /// Dataset post process.
        /// </summary>
        DatasetPostProcess,

        /// <summary>
        /// Filter expression.
        /// </summary>
        ExceptionPostProcessRule,

        /// <summary>
        /// Interactive chart.
        /// </summary>
        InteractiveChart,

        /// <summary>
        /// Rscript model.
        /// </summary>
        RScriptModel,

        /// <summary>
        /// Custom search validation rule.
        /// </summary>
        CustomSearchValidationRule,

        /// <summary>
        /// Chart template.
        /// </summary>
        ChartTemplate,

        /// <summary>
        /// Map renderer.
        /// </summary>
        MapRenderer
    }
}
