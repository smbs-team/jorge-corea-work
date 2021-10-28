namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the custom search parameter.
    /// </summary>
    public class CustomSearchParameterData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchParameterData"/> class.
        /// </summary>
        public CustomSearchParameterData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchParameterData"/> class.
        /// </summary>
        /// <param name="customSearchParameter">The custom search parameter.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public CustomSearchParameterData(CustomSearchParameter customSearchParameter, ModelInitializationType initializationType)
        {
            this.Id = customSearchParameter.CustomSearchParameterId;
            this.Name = customSearchParameter.ParameterName?.Trim();
            this.Description = customSearchParameter.ParameterDescription?.Trim();
            this.Type = customSearchParameter.ParameterDataType?.Trim();
            this.TypeLength = customSearchParameter.ParameterTypeLength;
            this.DefaultValue = customSearchParameter.ParameterDefaultValue == null ? null : customSearchParameter.ParameterDefaultValue?.Trim();
            this.IsRequired = customSearchParameter.ParameterIsRequired;
            this.ParameterRangeType = customSearchParameter.ParameterRangeType?.Trim();
            this.ParameterGroupName = customSearchParameter.ParameterGroupName?.Trim();
            this.DisplayName = customSearchParameter.DisplayName?.Trim();
            this.ForceEditLookupExpression = customSearchParameter.ForceEditLookupExpression;
            this.AllowMultipleSelection = customSearchParameter.AllowMultipleSelection;
            this.ParameterExtensions = JsonHelper.DeserializeObject(customSearchParameter.ParameterExtensions);

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (customSearchParameter.CustomSearchExpression != null && customSearchParameter.CustomSearchExpression.Count > 0)
                {
                    var expressionsData = new List<CustomSearchExpressionData>();

                    foreach (var expression in customSearchParameter.CustomSearchExpression)
                    {
                        expressionsData.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.Expressions = expressionsData.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the custom search parameter.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the custom search parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the custom search parameter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the parameter type length.
        /// </summary>
        public int TypeLength { get; set; }

        /// <summary>
        /// Gets or sets the parameter default value.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the parameter range type.
        /// </summary>
        public string ParameterRangeType { get; set; }

        /// <summary>
        /// Gets or sets the parameter group name.
        /// </summary>
        public string ParameterGroupName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter has edit lookup.
        /// </summary>
        public bool HasEditLookupExpression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter can be used as lookup.
        /// </summary>
        public bool ForceEditLookupExpression { get; set; }

        /// <summary>
        /// Gets or sets the expressions for this parameter.
        /// </summary>
        public CustomSearchExpressionData[] Expressions { get; set; }

        /// <summary>
        /// Gets or sets the lookup values.
        /// </summary>
        public object[] LookupValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter allows for multiple selection.
        /// </summary>
        public bool AllowMultipleSelection { get; set; }

        /// <summary>
        /// Gets or sets the parameter extensions.
        /// </summary>
        public object ParameterExtensions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <param name="expressions">The expressions in the parameter.</param>
        /// <returns>The entity framework model and related expressions.</returns>
        public CustomSearchParameter ToEfModel(out List<CustomSearchExpression> expressions)
        {
            var toReturn = new CustomSearchParameter()
            {
                CustomSearchParameterId = this.Id,
                ParameterName = this.Name?.Trim(),
                ParameterDescription = this.Description?.Trim(),
                ParameterDataType = this.Type?.Trim(),
                ParameterTypeLength = this.TypeLength,
                ParameterDefaultValue = this.DefaultValue?.Trim(),
                ParameterIsRequired = this.IsRequired,
                ParameterRangeType = this.ParameterRangeType?.Trim(),
                ParameterGroupName = this.ParameterGroupName?.Trim(),
                DisplayName = this.DisplayName?.Trim(),
                ForceEditLookupExpression = this.ForceEditLookupExpression,
                AllowMultipleSelection = this.AllowMultipleSelection,
                ParameterExtensions = JsonHelper.SerializeObject(this.ParameterExtensions),
            };

            expressions = new List<CustomSearchExpression>();

            if (this.Expressions != null)
            {
                toReturn.CustomSearchExpression = new List<CustomSearchExpression>();
                foreach (var expression in this.Expressions)
                {
                    CustomSearchExpression newExpression = expression.ToEfModel();
                    newExpression.CustomSearchParameterId = toReturn.CustomSearchParameterId;
                    newExpression.CustomSearchParameter = toReturn;
                    newExpression.OwnerType = CustomSearchExpressionOwnerType.CustomSearchParameter.ToString();
                    toReturn.CustomSearchExpression.Add(newExpression);
                    expressions.Add(newExpression);
                }
            }

            return toReturn;
        }
    }
}
