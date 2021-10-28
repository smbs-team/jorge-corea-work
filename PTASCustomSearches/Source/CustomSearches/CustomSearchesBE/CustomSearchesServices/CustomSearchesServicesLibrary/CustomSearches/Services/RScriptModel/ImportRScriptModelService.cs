namespace CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel
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
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScriptModel;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that imports a rscript model.
    /// </summary>
    public class ImportRScriptModelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRScriptModelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportRScriptModelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Validate the rscript expressions.
        /// </summary>
        /// <param name="expressions">Validate expressions.</param>
        public static void ValidateExpressions(ICollection<CustomSearchExpression> expressions)
        {
            var calculatedColumns = expressions
                .Where(c => c.ExpressionRole.Trim().ToLower() == CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower() &&
                            c.ExpressionType.Trim().ToLower() == CustomSearchExpressionType.RScript.ToString().ToLower());

            var calculatedColumnNames = calculatedColumns.Select(e => e.ColumnName.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (calculatedColumns.Count() != calculatedColumnNames.Count())
            {
                throw new CustomSearchesRequestBodyException(
                    $"Calculated columns have repeated names.",
                    innerException: null);
            }

            var calculatedColumnScripts = calculatedColumns.Select(e => e.Script.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (calculatedColumns.Count() != calculatedColumnScripts.Count())
            {
                throw new CustomSearchesRequestBodyException(
                    $"Calculated columns have repeated scripts.",
                    innerException: null);
            }

            CustomSearchExpressionRoleType[] validRoles =
            {
                CustomSearchExpressionRoleType.RScriptParameter,
                CustomSearchExpressionRoleType.CalculatedColumn,
                CustomSearchExpressionRoleType.CalculatedColumnPreCommit,
                CustomSearchExpressionRoleType.CalculatedColumnPostCommit,
                CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Dependent,
                CustomSearchExpressionRoleType.CalculatedColumnPreCommit_Independent,
                CustomSearchExpressionRoleType.DynamicEvaluator
            };

            CustomSearchExpressionValidator.ValidateTypes(
                expressions,
                validRoles);
        }

        /// <summary>
        /// Imports the rscript model.
        /// </summary>
        /// <param name="rscriptModelData">The rscript model to import.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The id of the imported project.</returns>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<IdResult> ImportRScriptModelAsync(
            RScriptModelData rscriptModelData,
            CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportRScriptModel");

            InputValidationHelper.AssertZero(rscriptModelData.RscriptModelId, nameof(RscriptModel), nameof(rscriptModelData.RscriptModelId));
            InputValidationHelper.AssertNotEmpty(rscriptModelData.RscriptModelName, nameof(rscriptModelData.RscriptModelName));

            RscriptModel rscriptModel = await dbContext.RscriptModel
                         .Where(r => r.RscriptModelName == rscriptModelData.RscriptModelName)
                         .Include(r => r.CustomSearchExpression)
                         .Include(r => r.CustomSearchParameter).ThenInclude(p => p.CustomSearchExpression)
                         .FirstOrDefaultAsync();

            if (rscriptModel != null)
            {
                RscriptModel updatedModel = rscriptModelData.ToEfModel();

                // Updatable fields.
                rscriptModel.Rscript = updatedModel.Rscript;
                rscriptModel.Description = updatedModel.Description;
                rscriptModel.RscriptFolderName = updatedModel.RscriptFolderName;
                rscriptModel.RscriptFileName = updatedModel.RscriptFileName;
                rscriptModel.RscriptModelRole = updatedModel.RscriptModelRole;
                rscriptModel.RscriptResultsDefinition = updatedModel.RscriptResultsDefinition;
                rscriptModel.PredictedTsqlExpression = updatedModel.PredictedTsqlExpression;
                rscriptModel.RscriptDisplayName = updatedModel.RscriptDisplayName;
                rscriptModel.LockPrecommitExpressions = updatedModel.LockPrecommitExpressions;

                dbContext.CustomSearchExpression.RemoveRange(rscriptModel.CustomSearchExpression);
                rscriptModel.CustomSearchExpression.Clear();
                foreach (var parameter in rscriptModel.CustomSearchParameter)
                {
                    dbContext.CustomSearchExpression.RemoveRange(parameter.CustomSearchExpression);
                }

                dbContext.CustomSearchParameter.RemoveRange(rscriptModel.CustomSearchParameter);
                rscriptModel.CustomSearchParameter.Clear();

                dbContext.RscriptModel.Update(rscriptModel);
            }
            else
            {
                rscriptModelData.RscriptModelId = 0;
                rscriptModel = rscriptModelData.ToEfModel();
                dbContext.RscriptModel.Add(rscriptModel);
            }

            InputValidationHelper.AssertNotEmpty(rscriptModelData.Expressions, nameof(rscriptModelData.Expressions));

            rscriptModel.CustomSearchExpression = new List<CustomSearchExpression>();

            int executionOrder = 0;
            foreach (var customSearchExpressionData in rscriptModelData.Expressions)
            {
                CustomSearchExpression newExpression = customSearchExpressionData.ToEfModel();
                newExpression.RscriptModel = rscriptModel;
                newExpression.OwnerType = CustomSearchExpressionOwnerType.RScriptModel.ToString();
                newExpression.ExecutionOrder = executionOrder;
                rscriptModel.CustomSearchExpression.Add(newExpression);
                newExpression.RscriptModel = rscriptModel;
                executionOrder++;
            }

            ImportRScriptModelService.ValidateExpressions(rscriptModel.CustomSearchExpression);

            if (rscriptModelData.Parameters != null)
            {
                rscriptModel.CustomSearchParameter = new List<CustomSearchParameter>();

                int displayOrder = 0;
                foreach (var parameter in rscriptModelData.Parameters)
                {
                    CustomSearchParameter newParameter = parameter.ToEfModel(out _);
                    newParameter.RscriptModel = rscriptModel;
                    newParameter.OwnerType = CustomSearchExpressionOwnerType.RScriptModel.ToString();
                    newParameter.DisplayOrder = displayOrder;
                    rscriptModel.CustomSearchParameter.Add(newParameter);
                    displayOrder++;
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();

            return new IdResult(rscriptModel.RscriptModelId);
        }
    }
}
