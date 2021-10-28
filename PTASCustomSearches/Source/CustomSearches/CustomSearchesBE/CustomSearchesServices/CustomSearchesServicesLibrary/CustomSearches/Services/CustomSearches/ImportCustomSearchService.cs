namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports a custom search.
    /// </summary>
    public class ImportCustomSearchService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCustomSearchService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportCustomSearchService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Validates the custom search data.
        /// </summary>
        /// <param name="definition">The custom search data to import.</param>
        /// <param name="expressions">The custom search expressions to validate.</param>
        /// <param name="datasetContext">The dataset context.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="isFakeSearch">Indicate whether this is a fake search.  Certain validations don't apply to fake searches (store procedure does not need to exist).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ValidateCustomSearchDefinitionAsync(
            CustomSearchDefinition definition,
            List<CustomSearchExpression> expressions,
            Dataset datasetContext,
            IServiceContext serviceContext,
            bool isFakeSearch)
        {
            if (definition.CustomSearchValidationRule.Count > 0)
            {
                string exceptionExpressionsMessage = $"Each validation rule must have one role of '{CustomSearchExpressionRoleType.ValidationConditionExpression}'" +
                    $" and one role of '{CustomSearchExpressionRoleType.ValidationErrorExpression}'.";

                foreach (var validationRule in definition.CustomSearchValidationRule)
                {
                    InputValidationHelper.AssertNotEmpty(validationRule.Description, nameof(validationRule.Description));
                    InputValidationHelper.AssertNotEmpty(
                        validationRule.CustomSearchExpression.ToArray(),
                        nameof(CustomSearchValidationRuleData.Expressions),
                        exceptionExpressionsMessage);

                    if (validationRule.CustomSearchExpression.Count != 2)
                    {
                        throw new CustomSearchesRequestBodyException(exceptionExpressionsMessage, innerException: null);
                    }

                    CustomSearchExpressionValidator.AssertHasOneExpression(validationRule.CustomSearchExpression, CustomSearchExpressionRoleType.ValidationConditionExpression, exceptionExpressionsMessage);
                    CustomSearchExpressionValidator.AssertHasOneExpression(validationRule.CustomSearchExpression, CustomSearchExpressionRoleType.ValidationErrorExpression, exceptionExpressionsMessage);
                }
            }

            if (definition.CustomSearchBackendEntity.Count > 0)
            {
                Dictionary<string, CustomSearchBackendEntity> backendEntities = new Dictionary<string, CustomSearchBackendEntity>();

                foreach (var backendEntity in definition.CustomSearchBackendEntity)
                {
                    InputValidationHelper.AssertNotEmpty(backendEntity.BackendEntityName, nameof(backendEntity.BackendEntityName));
                    InputValidationHelper.AssertNotEmpty(backendEntity.BackendEntityKeyFieldName, nameof(backendEntity.BackendEntityKeyFieldName));
                    InputValidationHelper.AssertNotEmpty(backendEntity.CustomSearchKeyFieldName, nameof(backendEntity.CustomSearchKeyFieldName));

                    if (backendEntities.ContainsKey(backendEntity.BackendEntityName))
                    {
                        throw new CustomSearchesRequestBodyException($"BackendEntityName '{backendEntity.BackendEntityName}' already used.", innerException: null);
                    }
                    else
                    {
                        backendEntities.Add(backendEntity.BackendEntityName, backendEntity);
                    }
                }
            }

            if (definition.CustomSearchColumnDefinition.Count > 0)
            {
                foreach (var columnDefinition in definition.CustomSearchColumnDefinition)
                {
                    if (!string.IsNullOrWhiteSpace(columnDefinition.BackendEntityName))
                    {
                        var backendEntity =
                            definition.CustomSearchBackendEntity.
                            FirstOrDefault(be => be.BackendEntityName.Trim().ToLower() == columnDefinition.BackendEntityName.Trim().ToLower());

                        if (backendEntity == null)
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"BackendEntityName '{columnDefinition.BackendEntityName}' in column definition doesn't match any backend entity.",
                                innerException: null);
                        }

                        InputValidationHelper.AssertNotEmpty(
                            columnDefinition.BackendEntityFieldName,
                            nameof(columnDefinition.BackendEntityFieldName),
                            $"A column definition with BackendEntityName '{columnDefinition.BackendEntityName}' requires a BackendEntityFieldName.");
                    }
                }
            }

            if (!isFakeSearch)
            {
                await StoredProcedureHelper.ValidateStoredProcedureExistsAsync(definition.StoredProcedureName, serviceContext);
                await ImportCustomSearchService.ValidateStoredProcedureParametersAsync(definition.StoredProcedureName, definition.CustomSearchParameter, serviceContext);
            }

            // Validates range parameters
            var rangeStartParameters = definition.CustomSearchParameter.Where(p => p.ParameterRangeType.ToLower() == "rangestart").ToList();
            var rangeEndParameters = definition.CustomSearchParameter.Where(p => p.ParameterRangeType.ToLower() == "rangeend").ToList();
            foreach (var rangeStartParameter in rangeStartParameters)
            {
                var rangeEndParameter = rangeEndParameters.FirstOrDefault(p => p.ParameterGroupName.ToLower() == rangeStartParameter.ParameterGroupName.ToLower());
                if (rangeEndParameter == null)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"Parameter group '{rangeStartParameter.ParameterGroupName}' has a 'RangeStart' parameter but does not have a 'RangeEnd' parameter",
                        innerException: null);
                }

                rangeEndParameters.Remove(rangeEndParameter);
            }

            if (rangeEndParameters.Count > 0)
            {
                throw new CustomSearchesRequestBodyException(
                    $"Parameter group '{rangeEndParameters[0].ParameterGroupName}' has a 'RangeEnd' parameter but does not have a 'RangeStart' parameter",
                    innerException: null);
            }

            HashSet<string> dataTypes;
            using (var context = serviceContext.DbContextFactory.Create())
            {
                dataTypes = (await context.DataType.Select(dt => dt.DataType1).ToArrayAsync()).ToHashSet();
            }

            // Validates data type and default values of parameters
            foreach (var parameter in definition.CustomSearchParameter)
            {
                if (!dataTypes.Contains(parameter.ParameterDataType, StringComparer.OrdinalIgnoreCase))
                {
                    string validTypes = string.Join(", ", dataTypes.Select(v => $"'{v}'"));
                    throw new CustomSearchesRequestBodyException(
                        $"Parameter data type '{parameter.ParameterDataType}' is invalid. Valid values are: {validTypes}.",
                        null);
                }

                if (parameter.ParameterDefaultValue == null)
                {
                    continue;
                }

                CustomSearchesValidationHelper.AssertParameterValueIsAssignable(
                    parameter.ParameterName,
                    parameter.ParameterDefaultValue,
                    parameter.ParameterDataType,
                    parameter.AllowMultipleSelection);
            }

            // Validates role and type of parameter expressions
            CustomSearchExpressionValidator.ValidateTypes(
                expressions.Where(e => e.CustomSearchParameter != null),
                validRoles: new CustomSearchExpressionRoleType[] { CustomSearchExpressionRoleType.LookupExpression });

            // Validates role of column definition expressions
            CustomSearchExpressionValidator.ValidateTypes(
                expressions.Where(e => e.CustomSearchColumnDefinition != null),
                validRoles: new CustomSearchExpressionRoleType[] { CustomSearchExpressionRoleType.EditLookupExpression, CustomSearchExpressionRoleType.RangedValuesOverrideExpression });

            // Validate expressions
            await CustomSearchExpressionValidator.ValidateExpressionScriptsAsync(
                expressions,
                serviceContext,
                datasetContext: datasetContext,
                previousPostProcessContext: null,
                postProcessContext: null,
                chartTypeContext: null,
                true);
        }

        /// <summary>
        /// Imports the custom search.
        /// </summary>
        /// <param name="customSearchData">The custom search data to import.</param>
        /// <param name="sPOutputChanged" cref="bool" in="query">Value indicating whether the stored procedure output changed.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The imported object id.</returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomExpressionValidationException">Exception when One or more expressions failed to validate.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task<IdResult> ImportCustomSearchAsync(
            CustomSearchDefinitionData customSearchData,
            bool sPOutputChanged,
            CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportCustomSearch");

            InputValidationHelper.AssertZero(customSearchData.Id, nameof(CustomSearchDefinition), nameof(customSearchData.Id));
            InputValidationHelper.AssertNotEmpty(customSearchData.Name, nameof(customSearchData.Name));

            var existingSearches = await (from cd in dbContext.CustomSearchDefinition
                                          where cd.CustomSearchName.Trim().ToLower() == customSearchData.Name.Trim().ToLower()
                                          select cd).
                                          Include(cd => cd.CustomSearchCategoryDefinition).
                                          OrderByDescending(cd => cd.Version).
                                          ToArrayAsync();

            // Gets the latest version
            var latestVersionSearch = existingSearches.FirstOrDefault();

            List<string> categoriesToAttach = customSearchData.Categories != null ?
                customSearchData.Categories.ToList() : new List<string>();

            // Stored procedures validations do not apply to searches in the fake category.
            bool isFakeSearch = (from c in categoriesToAttach where c.ToLower() == "fake" select c).FirstOrDefault() != null;
            if (isFakeSearch)
            {
                if (categoriesToAttach.Count > 1)
                {
                    throw new CustomSearchValidationException("Fake searches can't belong to any other category than the 'Fake' category", null);
                }
            }
            else
            {
                if (!categoriesToAttach.Contains("All"))
                {
                    categoriesToAttach.Add("All");
                }
            }

            List<CustomSearchExpression> expressions;
            CustomSearchDefinition newDefinition = customSearchData.ToEfModel(out expressions);
            await ImportCustomSearchService.ValidateCustomSearchDefinitionAsync(
                newDefinition,
                expressions.Where(e => e.CustomSearchParameter != null || e.CustomSearchColumnDefinition != null).ToList(),
                datasetContext: null,
                this.ServiceContext,
                isFakeSearch);

            List<CustomSearchCategory> categories = null;

            // Add new Categories
            categories = await
                (from c in dbContext.CustomSearchCategory select c).
                ToListAsync();

            var categoryStrings = (from c in categories select c.CategoryName.ToLower()).ToArray();

            foreach (var category in categoriesToAttach)
            {
                if (!categoryStrings.Contains(category.ToLower()))
                {
                    CustomSearchCategory newCategory = new CustomSearchCategory()
                    {
                        CategoryName = category,
                        CategoryDescription = category,
                    };

                    dbContext.CustomSearchCategory.Add(newCategory);
                    categories.Add(newCategory);
                }
            }

            newDefinition.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            newDefinition.LastModifiedTimestamp = DateTime.UtcNow;

            CustomSearchDefinition definitionToSave = newDefinition;
            if (latestVersionSearch == null)
            {
                newDefinition.CreatedBy = newDefinition.LastModifiedBy;
                newDefinition.CreatedTimestamp = newDefinition.LastModifiedTimestamp;

                // Add new custom search definition if it was not found.
                dbContext.CustomSearchDefinition.Add(newDefinition);
            }
            else
            {
                bool isNewVersion = latestVersionSearch.IsDeleted ||
                    !(await this.CheckVersionCompatibilityAsync(customSearchData, latestVersionSearch, sPOutputChanged, dbContext));

                // The import resets the IsDeleted flag
                if (latestVersionSearch.IsDeleted)
                {
                    foreach (var currentSearch in existingSearches)
                    {
                        currentSearch.IsDeleted = false;
                    }
                }

                // If it is a new version then we add a new custom search definition else we update the latest version.
                if (isNewVersion)
                {
                    newDefinition.CreatedBy = newDefinition.LastModifiedBy;
                    newDefinition.CreatedTimestamp = newDefinition.LastModifiedTimestamp;
                    newDefinition.Version = latestVersionSearch.Version + 1;
                    newDefinition.TableInputParameterDbType = latestVersionSearch.TableInputParameterDbType;
                    newDefinition.TableInputParameterName = latestVersionSearch.TableInputParameterName;

                    // Duplicates relations with chart template.
                    await dbContext.Entry(latestVersionSearch).Collection(csd => csd.CustomSearchChartTemplate).LoadAsync();

                    foreach (var chartTemplate in latestVersionSearch.CustomSearchChartTemplate)
                    {
                        var newChartTemplate = new CustomSearchChartTemplate { ChartTemplateId = chartTemplate.ChartTemplateId, CustomSearchDefinition = newDefinition };
                        newDefinition.CustomSearchChartTemplate.Add(newChartTemplate);
                    }

                    // Updates relations with project type.
                    var projectTypeCustomSearchDefinitions =
                        await dbContext.ProjectTypeCustomSearchDefinition.
                        Where(ptd => ptd.CustomSearchDefinitionId == latestVersionSearch.CustomSearchDefinitionId).
                        ToArrayAsync();

                    foreach (var projectTypeCustomSearchDefinition in projectTypeCustomSearchDefinitions)
                    {
                        projectTypeCustomSearchDefinition.CustomSearchDefinitionId = 0;
                        projectTypeCustomSearchDefinition.CustomSearchDefinition = newDefinition;
                    }

                    // Adds the new version of the custom search definition.
                    dbContext.CustomSearchDefinition.Add(newDefinition);
                }
                else
                {
                    // Updates the latest version of the custom search definition.
                    definitionToSave = latestVersionSearch;
                    var existingCustomSearchQuery =
                        from cd in dbContext.CustomSearchDefinition
                        where cd.CustomSearchDefinitionId == latestVersionSearch.CustomSearchDefinitionId
                        select cd;

                    var existingParametersQuery =
                        from cd in existingCustomSearchQuery
                        join cp in dbContext.CustomSearchParameter
                            on cd.CustomSearchDefinitionId equals cp.CustomSearchDefinitionId
                        select cp;

                    var existingColumnsQuery =
                        from cd in existingCustomSearchQuery
                        join cc in dbContext.CustomSearchColumnDefinition
                            on cd.CustomSearchDefinitionId equals cc.CustomSearchDefinitionId
                        select cc;

                    var existingBackendEntitiesQuery =
                        from cd in existingCustomSearchQuery
                        join cb in dbContext.CustomSearchBackendEntity
                            on cd.CustomSearchDefinitionId equals cb.CustomSearchDefinitionId
                        select cb;

                    var existingValidationRulesQuery =
                        from cd in existingCustomSearchQuery
                        join cv in dbContext.CustomSearchValidationRule
                            on cd.CustomSearchDefinitionId equals cv.CustomSearchDefinitionId
                        select cv;

                    var existingExpressions = await
                        (from cp in existingParametersQuery
                         join cse in dbContext.CustomSearchExpression
                             on cp.CustomSearchParameterId equals cse.CustomSearchParameterId
                         select cse).Union(
                            from cc in existingColumnsQuery
                            join cse in dbContext.CustomSearchExpression
                                on cc.CustomSearchColumnDefinitionId equals cse.CustomSearchColumnDefinitionId
                            select cse).Union(
                                from cv in existingValidationRulesQuery
                                join cse in dbContext.CustomSearchExpression
                                    on cv.CustomSearchValidationRuleId equals cse.CustomSearchValidationRuleId
                                select cse).ToArrayAsync();

                    var existingParameters = await existingParametersQuery.ToArrayAsync();
                    var existingColumns = await existingColumnsQuery.ToArrayAsync();
                    var existingBackendEntities = await existingBackendEntitiesQuery.ToArrayAsync();
                    var existingValidationRules = await existingValidationRulesQuery.ToArrayAsync();

                    // Delete all existing custom search expressions.
                    dbContext.CustomSearchExpression.RemoveRange(existingExpressions);

                    // Delete all existing custom search columns.
                    dbContext.CustomSearchColumnDefinition.RemoveRange(existingColumns);

                    // Delete all existing custom search parameters.
                    dbContext.CustomSearchParameter.RemoveRange(existingParameters);

                    // Delete all existing custom search backend entities.
                    dbContext.CustomSearchBackendEntity.RemoveRange(existingBackendEntities);

                    // Delete all existing custom search backend entities.
                    dbContext.CustomSearchValidationRule.RemoveRange(existingValidationRules);

                    // Add new parameters
                    foreach (var newParameter in newDefinition.CustomSearchParameter)
                    {
                        latestVersionSearch.CustomSearchParameter.Add(newParameter);
                    }

                    // Add new columns
                    foreach (var newColumnDefinition in newDefinition.CustomSearchColumnDefinition)
                    {
                        latestVersionSearch.CustomSearchColumnDefinition.Add(newColumnDefinition);
                    }

                    // Add new backend entities
                    foreach (var newBackendEntity in newDefinition.CustomSearchBackendEntity)
                    {
                        latestVersionSearch.CustomSearchBackendEntity.Add(newBackendEntity);
                    }

                    // Add new validation rules
                    foreach (var newValidationRule in newDefinition.CustomSearchValidationRule)
                    {
                        latestVersionSearch.CustomSearchValidationRule.Add(newValidationRule);
                    }
                }
            }

            // First remove existing category associations
            dbContext.CustomSearchCategoryDefinition.RemoveRange(definitionToSave.CustomSearchCategoryDefinition);
            definitionToSave.CustomSearchCategoryDefinition.Clear();

            // Update categories if needed
            if (customSearchData.Categories != null)
            {
                // Then add new ones.
                foreach (var category in customSearchData.Categories)
                {
                    CustomSearchCategory categoryEntity =
                        (from c in categories where c.CategoryName.Trim().ToLower() == category.Trim().ToLower() select c).FirstOrDefault();

                    InputValidationHelper.AssertEntityExists(categoryEntity, nameof(category), category);

                    var newCategory = new CustomSearchCategoryDefinition()
                    {
                        CustomSearchCategory = categoryEntity,
                        CustomSearchDefinition = definitionToSave
                    };

                    definitionToSave.CustomSearchCategoryDefinition.Add(newCategory);
                }
            }

            definitionToSave.Validated = false;
            await dbContext.ValidateAndSaveChangesAsync();
            return new IdResult(definitionToSave.CustomSearchDefinitionId);
        }

        /// <summary>
        /// Checks if the custom search definition is compatible with the previous one.
        /// </summary>
        /// <param name="customSearchData">The custom search data to import.</param>
        /// <param name="existingSearch">The existing custom search definition.</param>
        /// <param name="sPOutputChanged" cref="bool" in="query">Value indicating whether the stored procedure output changed.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Value indicating whether the custom search definition is compatible with the previous one.</returns>
        public async Task<bool> CheckVersionCompatibilityAsync(
            CustomSearchDefinitionData customSearchData,
            CustomSearchDefinition existingSearch,
            bool sPOutputChanged,
            CustomSearchesDbContext dbContext)
        {
            bool keepVersion = !sPOutputChanged;

            if (keepVersion)
            {
                // Compares name and type of the required stored procedure parameters
                await dbContext.Entry<CustomSearchDefinition>(existingSearch).Collection(csd => csd.CustomSearchParameter).LoadAsync();
                var currentRequiredParameters = existingSearch.CustomSearchParameter
                    .Where(csp => csp.ParameterIsRequired)
                    .Select(csp => new { Name = csp.ParameterName.Trim().ToLower(), Type = csp.ParameterDataType.Trim().ToLower() })
                    .ToHashSet();

                var newRequiredParameters = customSearchData.Parameters
                    .Where(csp => csp.IsRequired)
                    .Select(csp => new { Name = csp.Name.Trim().ToLower(), Type = csp.Type.Trim().ToLower() })
                    .ToHashSet();

                // The old and new required parameters must be the same to keep the version.
                keepVersion = currentRequiredParameters.SetEquals(newRequiredParameters);
            }

            if (keepVersion)
            {
                // Compares name and type of the optional stored procedure parameters.
                var currentOptionalParameters = existingSearch.CustomSearchParameter
                    .Where(csp => !csp.ParameterIsRequired)
                    .Select(csp => new { Name = csp.ParameterName.Trim().ToLower(), Type = csp.ParameterDataType.Trim().ToLower() })
                    .ToHashSet();

                var newOptionalParameters = customSearchData.Parameters
                    .Where(csp => !csp.IsRequired)
                    .Select(csp => new { Name = csp.Name.Trim().ToLower(), Type = csp.Type.Trim().ToLower() })
                    .ToHashSet();

                // The old optional parameters must be a subset of the new ones to keep the version.
                keepVersion = currentOptionalParameters.IsSubsetOf(newOptionalParameters);
            }

            if (keepVersion)
            {
                // Compares name and type of the editable columns.
                var currentEditableColumns =
                    (await DatasetColumnHelper.GetEditableColumnsAsync(existingSearch.CustomSearchDefinitionId, dbContext))
                    .Select(ec => new { Name = ec.ColumnName.Trim().ToLower() })
                    .ToHashSet();

                var newEditableColumns =
                    customSearchData.ColumnDefinitions?.Where(cd => cd.IsEditable)
                    .Select(ec => new { Name = ec.ColumnName.Trim().ToLower() })
                    .ToHashSet();

                // The old and new editable columns must be the same to keep the version.
                if (newEditableColumns == null)
                {
                    keepVersion = currentEditableColumns.Count == 0;
                }
                else
                {
                    keepVersion = currentEditableColumns.SetEquals(newEditableColumns);
                }
            }

            return keepVersion;
        }

        /// <summary>
        /// Validates the definition parameter against the stored procedure exists.
        /// </summary>
        /// <param name="procedureName">The procedure name.</param>
        /// <param name="customSearchParameters">The custom search parameters.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        private static async Task ValidateStoredProcedureParametersAsync(
            string procedureName,
            ICollection<CustomSearchParameter> customSearchParameters,
            IServiceContext serviceContext)
        {
            HashSet<string> sqlParameterNames =
                await StoredProcedureHelper.GetStoredProcedureParametersAsync(procedureName, serviceContext);

            var invalidParameterNames = customSearchParameters.Select(p => p.ParameterName).ToHashSet();
            invalidParameterNames = invalidParameterNames.Except(sqlParameterNames, StringComparer.OrdinalIgnoreCase).ToHashSet();
            if (invalidParameterNames.Count > 0)
            {
                string validParameters = string.Join(", ", sqlParameterNames);
                string invalidParameters = string.Join(", ", invalidParameterNames);
                throw new CustomSearchesRequestBodyException(
                    $"Invalid parameter names: {invalidParameters}. Valid values are: {validParameters}.",
                    null);
            }
        }
    }
}
