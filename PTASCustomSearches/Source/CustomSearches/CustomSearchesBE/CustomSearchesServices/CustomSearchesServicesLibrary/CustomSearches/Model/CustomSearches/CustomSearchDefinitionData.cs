namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the custom search definition.
    /// </summary>
    public class CustomSearchDefinitionData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchDefinitionData"/> class.
        /// </summary>
        public CustomSearchDefinitionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchDefinitionData"/> class.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public CustomSearchDefinitionData(CustomSearchDefinition customSearchDefinition, ModelInitializationType initializationType)
        {
            this.Id = customSearchDefinition.CustomSearchDefinitionId;
            this.CreatedBy = customSearchDefinition.CreatedBy;
            this.LastModifiedBy = customSearchDefinition.LastModifiedBy;
            this.CreatedTimestamp = customSearchDefinition.CreatedTimestamp;
            this.LastModifiedTimestamp = customSearchDefinition.LastModifiedTimestamp;
            this.Description = customSearchDefinition.CustomSearchDescription;
            this.Name = customSearchDefinition.CustomSearchName;
            this.Version = customSearchDefinition.Version;

            if (customSearchDefinition.CustomSearchCategoryDefinition != null &&
                customSearchDefinition.CustomSearchCategoryDefinition.Count > 0)
            {
                List<string> categoryList = new List<string>();
                foreach (var categoryDefinition in customSearchDefinition.CustomSearchCategoryDefinition)
                {
                    if (categoryDefinition.CustomSearchCategory != null)
                    {
                        categoryList.Add(categoryDefinition.CustomSearchCategory.CategoryName);
                    }
                }

                this.Categories = categoryList.ToArray();
            }

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                this.StoredProcedureName = customSearchDefinition.StoredProcedureName;
                this.TableInputParameterName = customSearchDefinition.TableInputParameterName;
                this.TableInputParameterDbType = customSearchDefinition.TableInputParameterDbType;
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (customSearchDefinition.CustomSearchParameter != null && customSearchDefinition.CustomSearchParameter.Count > 0)
                {
                    List<CustomSearchParameterData> parameters = new List<CustomSearchParameterData>();
                    var customSearchParameters = customSearchDefinition.CustomSearchParameter.OrderBy(p => p.DisplayOrder);
                    foreach (var customSearchParameter in customSearchParameters)
                    {
                        parameters.Add(new CustomSearchParameterData(customSearchParameter, initializationType));
                    }

                    this.Parameters = parameters.ToArray();
                }

                if (customSearchDefinition.CustomSearchColumnDefinition != null &&
                    customSearchDefinition.CustomSearchColumnDefinition.Count > 0)
                {
                    List<CustomSearchColumnDefinitionData> columns = new List<CustomSearchColumnDefinitionData>();
                    foreach (var column in customSearchDefinition.CustomSearchColumnDefinition)
                    {
                        columns.Add(new CustomSearchColumnDefinitionData(column, initializationType));
                    }

                    this.ColumnDefinitions = columns.ToArray();
                }

                if (customSearchDefinition.CustomSearchBackendEntity != null &&
                    customSearchDefinition.CustomSearchBackendEntity.Count > 0)
                {
                    List<CustomSearchBackendEntityData> backendEntities = new List<CustomSearchBackendEntityData>();
                    foreach (var backendEntity in customSearchDefinition.CustomSearchBackendEntity)
                    {
                        backendEntities.Add(new CustomSearchBackendEntityData(backendEntity, initializationType));
                    }

                    this.BackendEntities = backendEntities.ToArray();
                }

                if (customSearchDefinition.CustomSearchValidationRule != null &&
                    customSearchDefinition.CustomSearchValidationRule.Count > 0)
                {
                    List<CustomSearchValidationRuleData> validationRules = new List<CustomSearchValidationRuleData>();
                    foreach (var validationRule in customSearchDefinition.CustomSearchValidationRule)
                    {
                        validationRules.Add(new CustomSearchValidationRuleData(validationRule, initializationType));
                    }

                    this.ValidationRules = validationRules.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the custom search definition.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the custom search definition.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the custom search.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// Gets or sets the description of the custom search definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the description of the custom search definition.
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// Gets or sets the created by field.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by field.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created time stamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified time stamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the SQL user-defined table type parameter used in the custom search stored procedure.
        /// </summary>
        public string TableInputParameterName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SQL user-defined table type parameter used in the custom search stored procedure.
        /// </summary>
        public string TableInputParameterDbType { get; set; }

        /// <summary>
        /// Gets or sets the version of the custom search definition.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the custom search parameters.
        /// </summary>
        public CustomSearchParameterData[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the custom search column definition.
        /// </summary>
        public CustomSearchColumnDefinitionData[] ColumnDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the custom search backend entities.
        /// </summary>
        public CustomSearchBackendEntityData[] BackendEntities { get; set; }

        /// <summary>
        /// Gets or sets the custom search validation rules.
        /// </summary>
        public CustomSearchValidationRuleData[] ValidationRules { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <param name="expressions">The expressions in the Custom Search Definition and child objects.</param>
        /// <returns>
        /// The entity framework model and the list of generated expressions.
        /// </returns>
        public CustomSearchDefinition ToEfModel(out List<CustomSearchExpression> expressions)
        {
            var toReturn = new CustomSearchDefinition()
            {
                CustomSearchDefinitionId = this.Id,
                CustomSearchDescription = this.Description,
                CustomSearchName = this.Name,
                StoredProcedureName = this.StoredProcedureName,
                TableInputParameterName = this.TableInputParameterName,
                TableInputParameterDbType = this.TableInputParameterDbType,
                Version = this.Version,
            };

            expressions = new List<CustomSearchExpression>();

            if (this.Parameters != null)
            {
                toReturn.CustomSearchParameter = new List<CustomSearchParameter>();

                int displayOrder = 0;
                foreach (var parameter in this.Parameters)
                {
                    List<CustomSearchExpression> parameterExpressions;
                    CustomSearchParameter newParameter = parameter.ToEfModel(out parameterExpressions);
                    newParameter.CustomSearchDefinitionId = this.Id;
                    newParameter.CustomSearchDefinition = toReturn;
                    newParameter.OwnerType = CustomSearchExpressionOwnerType.CustomSearchDefinition.ToString();
                    newParameter.DisplayOrder = displayOrder;
                    toReturn.CustomSearchParameter.Add(newParameter);
                    expressions.AddRange(parameterExpressions);
                    displayOrder++;
                }
            }

            if (this.ColumnDefinitions != null)
            {
                toReturn.CustomSearchColumnDefinition = new List<CustomSearchColumnDefinition>();
                foreach (var columnDefinition in this.ColumnDefinitions)
                {
                    List<CustomSearchExpression> columnExpressions;
                    CustomSearchColumnDefinition newColumnDefinition = columnDefinition.ToEfModel(out columnExpressions);
                    newColumnDefinition.CustomSearchDefinitionId = this.Id;
                    newColumnDefinition.CustomSearchDefinition = toReturn;
                    toReturn.CustomSearchColumnDefinition.Add(newColumnDefinition);
                    expressions.AddRange(columnExpressions);
                }
            }

            if (this.BackendEntities != null)
            {
                toReturn.CustomSearchBackendEntity = new List<CustomSearchBackendEntity>();
                foreach (var backendEntity in this.BackendEntities)
                {
                    CustomSearchBackendEntity newBackendEntity = backendEntity.ToEfModel();
                    newBackendEntity.CustomSearchDefinitionId = this.Id;
                    newBackendEntity.CustomSearchDefinition = toReturn;
                    toReturn.CustomSearchBackendEntity.Add(newBackendEntity);
                }
            }

            if (this.ValidationRules != null)
            {
                int executionOrder = 0;
                toReturn.CustomSearchValidationRule = new List<CustomSearchValidationRule>();
                foreach (var validationRule in this.ValidationRules)
                {
                    List<CustomSearchExpression> validationRuleExpressions;
                    CustomSearchValidationRule newValidationRule = validationRule.ToEfModel(out validationRuleExpressions);
                    newValidationRule.CustomSearchDefinitionId = this.Id;
                    newValidationRule.CustomSearchDefinition = toReturn;
                    newValidationRule.ExecutionOrder = executionOrder;
                    toReturn.CustomSearchValidationRule.Add(newValidationRule);
                    expressions.AddRange(validationRuleExpressions);
                    executionOrder++;
                }
            }

            return toReturn;
        }
    }
}
