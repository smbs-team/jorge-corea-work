namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the custom search validation rule.
    /// </summary>
    public class CustomSearchValidationRuleData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchValidationRuleData"/> class.
        /// </summary>
        public CustomSearchValidationRuleData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchValidationRuleData"/> class.
        /// </summary>
        /// <param name="validationRule">The validation rule entity from EF.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public CustomSearchValidationRuleData(
            CustomSearchValidationRule validationRule,
            ModelInitializationType initializationType)
        {
            this.CustomSearchValidationRuleId = validationRule.CustomSearchValidationRuleId;
            this.CustomSearchDefinitionId = validationRule.CustomSearchDefinitionId;
            this.Description = validationRule.Description;
            this.ExecutionOrder = validationRule.ExecutionOrder;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (validationRule.CustomSearchExpression != null && validationRule.CustomSearchExpression.Count > 0)
                {
                    var expressionsData = new List<CustomSearchExpressionData>();

                    foreach (var expression in validationRule.CustomSearchExpression)
                    {
                        expressionsData.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.Expressions = expressionsData.ToArray();
                }
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }
        }

        /// <summary>
        /// Gets or sets the custom search validation rule id.
        /// </summary>
        public int CustomSearchValidationRuleId { get; set; }

        /// <summary>
        /// Gets or sets the custom search definition id.
        /// </summary>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the custom search validation rule description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        public int ExecutionOrder { get; set; }

        /// <summary>
        /// Gets or sets the custom search expressions.
        /// </summary>
        public CustomSearchExpressionData[] Expressions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <param name="expressions">The expressions in the validation rule.</param>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public CustomSearchValidationRule ToEfModel(out List<CustomSearchExpression> expressions)
        {
            var toReturn = new CustomSearchValidationRule()
            {
                CustomSearchValidationRuleId = this.CustomSearchValidationRuleId,
                CustomSearchDefinitionId = this.CustomSearchDefinitionId,
                Description = this.Description,
                ExecutionOrder = this.ExecutionOrder
            };

            expressions = new List<CustomSearchExpression>();

            if (this.Expressions != null)
            {
                toReturn.CustomSearchExpression = new List<CustomSearchExpression>();
                foreach (var expression in this.Expressions)
                {
                    CustomSearchExpression newExpression = expression.ToEfModel();
                    newExpression.CustomSearchValidationRuleId = this.CustomSearchValidationRuleId;
                    newExpression.CustomSearchValidationRule = toReturn;
                    newExpression.OwnerType = CustomSearchExpressionOwnerType.CustomSearchValidationRule.ToString();
                    toReturn.CustomSearchExpression.Add(newExpression);
                    expressions.Add(newExpression);
                }
            }

            return toReturn;
        }
    }
}
