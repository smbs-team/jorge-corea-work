namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the custom search column definition.
    /// </summary>
    public class CustomSearchColumnDefinitionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchColumnDefinitionData"/> class.
        /// </summary>
        public CustomSearchColumnDefinitionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchColumnDefinitionData"/> class.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public CustomSearchColumnDefinitionData(
            CustomSearchColumnDefinition columnDefinition,
            ModelInitializationType initializationType)
        {
            this.CustomSearchDefinitionId = columnDefinition.CustomSearchDefinitionId;
            this.ColumnName = columnDefinition.ColumnName;
            this.BackendEntityName = columnDefinition.BackendEntityName;
            this.BackendEntityFieldName = columnDefinition.BackendEntityFieldName;
            this.ColumnTypeLength = columnDefinition.ColumnTypeLength;
            this.CanBeUsedAsLookup = columnDefinition.CanBeUsedAsLookup;
            this.ColumnCategory = columnDefinition.ColumnCategory;
            this.IsEditable = columnDefinition.IsEditable;
            this.ForceEditLookupExpression = columnDefinition.ForceEditLookupExpression;
            this.ColumDefinitionExtensions = JsonHelper.DeserializeObject(columnDefinition.ColumDefinitionExtensions);
            this.DependsOnColumn = columnDefinition.DependsOnColumn;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (columnDefinition.CustomSearchExpression != null && columnDefinition.CustomSearchExpression.Count > 0)
                {
                    var expressionsData = new List<CustomSearchExpressionData>();

                    foreach (var expression in columnDefinition.CustomSearchExpression)
                    {
                        expressionsData.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.Expressions = expressionsData.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom search definition id.
        /// </summary>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// Gets or sets the column type length.
        /// </summary>
        public int ColumnTypeLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column can be used as lookup.
        /// </summary>
        public bool? CanBeUsedAsLookup { get; set; }

        /// <summary>
        /// Gets or sets the column category.
        /// </summary>
        public string ColumnCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column can be edited.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column has edit lookup.
        /// </summary>
        public bool HasEditLookupExpression { get; set; }

        /// <summary>
        /// Gets or sets the backend entity name.
        /// </summary>
        public string BackendEntityName { get; set; }

        /// <summary>
        /// Gets or sets the backend entity field name.
        /// </summary>
        public string BackendEntityFieldName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column can be used as lookup.
        /// </summary>
        public bool ForceEditLookupExpression { get; set; }

        /// <summary>
        /// Gets or sets the column definition extensions.
        /// </summary>
        public object ColumDefinitionExtensions { get; set; }

        /// <summary>
        /// Gets or sets the depends on column.
        /// </summary>
        public string DependsOnColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is indexed.
        /// </summary>
        public bool IsIndexed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is calculated.
        /// </summary>
        public bool IsCalculatedColumn { get; set; }

        /// <summary>
        /// Gets or sets the expressions for this parameter.
        /// </summary>
        public CustomSearchExpressionData[] Expressions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <param name="expressions">The expressions in the column definition.</param>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public CustomSearchColumnDefinition ToEfModel(out List<CustomSearchExpression> expressions)
        {
            var toReturn = new CustomSearchColumnDefinition()
            {
                CustomSearchDefinitionId = this.CustomSearchDefinitionId,
                ColumnName = this.ColumnName,
                BackendEntityName = this.BackendEntityName,
                BackendEntityFieldName = this.BackendEntityFieldName,
                ColumnTypeLength = this.ColumnTypeLength,
                CanBeUsedAsLookup = this.CanBeUsedAsLookup,
                ColumnCategory = this.ColumnCategory,
                IsEditable = this.IsEditable,
                ForceEditLookupExpression = this.ForceEditLookupExpression,
                ColumDefinitionExtensions = JsonHelper.SerializeObject(this.ColumDefinitionExtensions),
                DependsOnColumn = this.DependsOnColumn,
            };

            expressions = new List<CustomSearchExpression>();

            if (this.Expressions != null)
            {
                toReturn.CustomSearchExpression = new List<CustomSearchExpression>();
                foreach (var expression in this.Expressions)
                {
                    CustomSearchExpression newExpression = expression.ToEfModel();
                    newExpression.CustomSearchColumnDefinitionId = toReturn.CustomSearchColumnDefinitionId;
                    newExpression.CustomSearchColumnDefinition = toReturn;
                    newExpression.OwnerType = CustomSearchExpressionOwnerType.CustomSearchColumnDefinition.ToString();
                    toReturn.CustomSearchExpression.Add(newExpression);
                    expressions.Add(newExpression);
                }
            }

            return toReturn;
        }
    }
}
