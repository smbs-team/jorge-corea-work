namespace CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class to manage the annual update project business logic.
    /// </summary>
    public class AnnualUpdateProjectBusinessLogic : DefaultProjectBusinessLogic
    {
        /// <summary>
        /// The land factor column name.
        /// </summary>
        public const string LandFactorColumnName = "LandFactor";

        /// <summary>
        /// The 'non land factor' column name.
        /// </summary>
        public const string NonLandFactorColumnName = "NonLandFactor";

        /// <summary>
        /// The 'apply non land factor to' column name.
        /// </summary>
        public const string ApplyNonLandFactorToColumnName = "ApplyNonLandFactorTo";

        /// <summary>
        /// The new trended price column name.
        /// </summary>
        public const string NewTrendedPriceColumnName = "NewTrendedPrice";

        /// <summary>
        /// The EMV adjustment column name.
        /// </summary>
        public const string EMVAdjustmentColumnName = "EMVAdjustment";

        /// <summary>
        /// The new land value column name.
        /// </summary>
        public const string NewLandValueColumnName = "NewLandValue";

        /// <summary>
        /// The new improvements value column name.
        /// </summary>
        public const string NewImprovementsValueColumnName = "NewImprovementsValue";

        /// <summary>
        /// The new total value column name.
        /// </summary>
        public const string NewTotalValueColumnName = "NewTotalValue";

        /// <summary>
        /// The time trend regression post process role.
        /// </summary>
        public const string TimeTrendRegressionPostProcessRole = "TimeTrendRegression";

        /// <summary>
        /// The time trend regression post process role.
        /// </summary>
        public const string MultipleRegressionPostProcessRole = "MultipleRegression";

        /// <summary>
        /// The annual update adjustments post process role.
        /// </summary>
        public const string AnnualUpdateAdjustmentsPostProcessRole = "AnnualUpdateAdjustments";

        /// <summary>
        /// The apply annual update adjustments post process role.
        /// </summary>
        public const string ApplyAnnualUpdateAdjustmentsPostProcessRole = "ApplyAnnualUpdateAdjustments";

        /// <summary>
        /// The annual update adjustments post process role order.
        /// </summary>
        public static readonly List<string> AnnualUpdateAdjustmentsPostProcessRoleOrder = new List<string>
            {
                AnnualUpdateAdjustmentsPostProcessRole.ToLower(), ApplyAnnualUpdateAdjustmentsPostProcessRole.ToLower()
            };

        /// <summary>
        /// The post process role order.
        /// </summary>
        public static readonly List<string> PostProcessRoleOrder = new List<string>
            {
                TimeTrendRegressionPostProcessRole.ToLower(),
                MultipleRegressionPostProcessRole.ToLower(),
                AnnualUpdateAdjustmentsPostProcessRole.ToLower(),
                ApplyAnnualUpdateAdjustmentsPostProcessRole.ToLower()
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnualUpdateProjectBusinessLogic"/> class.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        public AnnualUpdateProjectBusinessLogic(
            UserProject userProject,
            CustomSearchesDbContext dbContext)
            : base(dbContext)
        {
            this.UserProject = userProject;
        }

        /// <summary>
        /// Gets a value indicating whether the project business logic supports the pivot dataset.
        /// </summary>
        protected override bool SupportsPivotDataset
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Validates the import post process operation.
        /// </summary>
        /// <param name="datasetPostProcessData">The post process data.</param>
        /// <param name="datasets">The datasets.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        public override void ValidateImportPostProcess(DatasetPostProcessData datasetPostProcessData, List<Dataset> datasets, bool bypassPostProcessBundleCheck)
        {
            string postProcessRoleLower = datasetPostProcessData.PostProcessRole?.Trim().ToLower();
            int roleIndex = AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRoleLower);
            if (roleIndex < 0)
            {
                return;
            }

            var pivotDataset =
                datasets.FirstOrDefault(d => d.UserProjectDataset.FirstOrDefault(
                    upd => upd.OwnsDataset && upd.DatasetRole?.Trim().ToLower() == this.PivotDatasetRole.ToLower()) != null);

            if (pivotDataset == null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"The import should have a {this.PivotDatasetRole} dataset as primary or secondary.",
                    innerException: null);
            }

            if (roleIndex > 0)
            {
                int landModelRoleIndex = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder.IndexOf(postProcessRoleLower);

                string firstAnnualUpdateAdjustmentsPostProcessRole = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder[0];
                string previousRole = landModelRoleIndex > 0 ?
                    firstAnnualUpdateAdjustmentsPostProcessRole :
                    AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder[roleIndex - 1];

                foreach (var currentDataset in datasets)
                {
                    if (currentDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == previousRole) == null)
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"The dataset '{currentDataset.DatasetId}' should first create a post process with role '{previousRole}'.",
                            innerException: null);
                    }

                    if ((!bypassPostProcessBundleCheck) &&
                        (landModelRoleIndex >= 0) &&
                        (currentDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == firstAnnualUpdateAdjustmentsPostProcessRole) == null))
                    {
                        throw new CustomSearchesRequestBodyException(
                            $"The dataset '{currentDataset.DatasetId}' should first use the SetupAnnualUpdateAdjustments service to import the annual update adjustment post process.",
                            innerException: null);
                    }
                }
            }

            if (bypassPostProcessBundleCheck)
            {
                return;
            }

            // Validates required column name.
            HashSet<string> columnNames = null;
            string postProcessRole = datasetPostProcessData.PostProcessRole?.Trim().ToLower();

            if (postProcessRole == AnnualUpdateProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower())
            {
                columnNames = new HashSet<string> { AnnualUpdateProjectBusinessLogic.NewTrendedPriceColumnName };
            }
            else if (postProcessRole == AnnualUpdateProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower())
            {
                columnNames = new HashSet<string> { AnnualUpdateProjectBusinessLogic.EMVAdjustmentColumnName };
            }
            else if (postProcessRole == AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole.ToLower())
            {
                columnNames = new HashSet<string>
                {
                    AnnualUpdateProjectBusinessLogic.LandFactorColumnName,
                    AnnualUpdateProjectBusinessLogic.NonLandFactorColumnName,
                    AnnualUpdateProjectBusinessLogic.ApplyNonLandFactorToColumnName
                };
            }

            if (columnNames != null)
            {
                List<CustomSearchExpressionData> calculatedColumnExpressions = new List<CustomSearchExpressionData>();

                var exceptionCalculatedColumnExpressions =
                    datasetPostProcessData.ExceptionPostProcessRules?.
                    Select(r => r.CustomSearchExpressions.
                        FirstOrDefault(cse => cse.ExpressionRole.Trim().ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower())));

                if (exceptionCalculatedColumnExpressions != null && exceptionCalculatedColumnExpressions.Count() > 0)
                {
                    calculatedColumnExpressions.AddRange(exceptionCalculatedColumnExpressions);
                }

                var postProcessCalculatedColumnExpressions = datasetPostProcessData.CustomSearchExpressions?.
                    Where(cse => cse.ExpressionRole.Trim().ToLower().StartsWith(CustomSearchExpressionRoleType.CalculatedColumn.ToString().ToLower()));

                if (postProcessCalculatedColumnExpressions != null && postProcessCalculatedColumnExpressions.Count() > 0)
                {
                    calculatedColumnExpressions.AddRange(postProcessCalculatedColumnExpressions);
                }

                if (!columnNames.Select(c => c.ToLower()).ToHashSet().IsSubsetOf(calculatedColumnExpressions.Select(cse => cse.ColumnName.Trim().ToLower())))
                {
                    string columnNamesText = string.Join(" and ", columnNames.Select(v => $"'{v}'"));
                    throw new CustomSearchesRequestBodyException(
                        $"The {nameof(datasetPostProcessData)} requires expression(s) with a '{CustomSearchExpressionRoleType.CalculatedColumn}' role " +
                        $"and column name(s): {columnNamesText}.",
                        innerException: null);
                }
            }
        }

        /// <summary>
        /// Validates the import custom modeling step post process operation.
        /// </summary>
        /// <param name="datasetPostProcessData">The post process data.</param>
        /// <param name="datasets">The datasets.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        public override void ValidateImportCustomModelingStepPostProcess(DatasetPostProcessData datasetPostProcessData, List<Dataset> datasets, bool bypassPostProcessBundleCheck)
        {
            string postProcessRole = datasetPostProcessData.PostProcessRole?.Trim().ToLower();
            if (postProcessRole != AnnualUpdateProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower() &&
                postProcessRole != AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole.ToLower() &&
                postProcessRole != AnnualUpdateProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower())
            {
                throw new CustomSearchesRequestBodyException(
                    $"The {nameof(datasetPostProcessData)} requires an expression with one of the following roles: '{AnnualUpdateProjectBusinessLogic.TimeTrendRegressionPostProcessRole}'," +
                    $" '{AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole}', '{AnnualUpdateProjectBusinessLogic.MultipleRegressionPostProcessRole}'.",
                    innerException: null);
            }

            this.ValidateImportPostProcess(datasetPostProcessData, datasets, bypassPostProcessBundleCheck);
        }

        /// <summary>
        /// Validates the execute post process operation.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        public override void ValidateExecutePostProcess(DatasetPostProcess datasetPostProcess)
        {
            if (AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(datasetPostProcess.PostProcessRole?.Trim().ToLower()) >= 0)
            {
                if (datasetPostProcess.PrimaryDatasetPostProcessId != null)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"The execute should be over the primary dataset post process.",
                        innerException: null);
                }
            }
        }

        /// <summary>
        /// Validates the delete post process operation.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="bypassPostProcessBundleCheck">
        /// Value indicating whether the post process bundle check should be bypassed.
        /// Note: Some post-processed are bundled together (like land model) and operations like delete are not allowed on an individual post-process.
        /// </param>
        /// <param name="checkPostProcessStackOnPivot">Value indicating whether the stack check is over the current dataset or should be over the pivot.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task ValidateDeletePostProcessAsync(DatasetPostProcess datasetPostProcess, bool bypassPostProcessBundleCheck, bool checkPostProcessStackOnPivot)
        {
            var postProcessRole = datasetPostProcess.PostProcessRole?.Trim().ToLower();

            int annualUpdateRoleIndex = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder.IndexOf(postProcessRole);

            if (!bypassPostProcessBundleCheck)
            {
                if (annualUpdateRoleIndex >= 0)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"To delete the dataset post process with role '{datasetPostProcess.PostProcessRole}' " +
                        $"should use the DeleteLandModel service.",
                        innerException: null);
                }
            }

            int roleIndex = annualUpdateRoleIndex >= 0 ?
                AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRoleOrder.Last()) :
                AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRole);

            if (roleIndex < 0)
            {
                return;
            }

            Dataset dataset = null;
            if (checkPostProcessStackOnPivot)
            {
                dataset = await this.LoadPivotDatasetAsync(datasetId: null);
            }
            else
            {
                dataset = await this.DbContext.Dataset.
                    Where(d => d.DatasetId == datasetPostProcess.DatasetId).
                    Include(d => d.DatasetPostProcess).
                    FirstOrDefaultAsync();
            }

            List<string> nextExistingRoles = new List<string>();
            var postProcesses = dataset.DatasetPostProcess.OrderBy(dpp => dpp.Priority);
            foreach (var currentPostProcess in postProcesses)
            {
                int currentRoleIndex = AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(currentPostProcess.PostProcessRole?.Trim().ToLower());
                if (currentRoleIndex > roleIndex)
                {
                    nextExistingRoles.Add(currentPostProcess.PostProcessRole);
                }
            }

            if (nextExistingRoles.Count > 0)
            {
                string nextExistingRolesText = string.Join(", ", nextExistingRoles);
                throw new CustomSearchesRequestBodyException(
                    $"To delete the dataset post process with role '{datasetPostProcess.PostProcessRole}' " +
                    $"should first delete the post processes with roles: {nextExistingRolesText}.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Indicates whether can execute secondary post processes for the specified role.
        /// </summary>
        /// <param name="postProcessRole">The post process role.</param>
        /// <returns>
        /// A value to indicates whether can execute secondary post processes for the specified role.
        /// </returns>
        public override bool UseMultiDatasetPipeline(string postProcessRole)
        {
            return AnnualUpdateProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRole?.Trim().ToLower()) >= 0 ? true : false;
        }

        /// <summary>
        /// Validates the apply model operation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task ValidateApplyModelAsync()
        {
            await this.LoadPivotDatasetAsync(datasetId: null);
            if ((this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == AnnualUpdateProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower()) == null) ||
                (this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole.ToLower()) == null) ||
                (this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == AnnualUpdateProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower()) == null))
            {
                throw new CustomSearchesRequestBodyException(
                    $"The apply model service needs post processes with the following roles: '{AnnualUpdateProjectBusinessLogic.TimeTrendRegressionPostProcessRole}', " +
                    $"'{AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole}', '{AnnualUpdateProjectBusinessLogic.MultipleRegressionPostProcessRole}'.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Gets the use table type input parameter name used in the apply model operation.
        /// </summary>
        /// <returns>
        /// The use table type input parameter name.
        /// </returns>
        public override string GetApplyModelUseTableTypeInputParameterName()
        {
            return "UseAnnualUpdateModel";
        }

        /// <summary>
        /// Gets the table type input parameter names used in the apply model operation.
        /// </summary>
        /// <returns>
        /// The table type input parameter names.
        /// </returns>
        public override string[] GetApplyModelTableTypeInputParameterNames()
        {
            return new string[]
            {
                AnnualUpdateProjectBusinessLogic.NewTrendedPriceColumnName,
                AnnualUpdateProjectBusinessLogic.EMVAdjustmentColumnName,
                AnnualUpdateProjectBusinessLogic.NewLandValueColumnName,
                AnnualUpdateProjectBusinessLogic.NewImprovementsValueColumnName,
                AnnualUpdateProjectBusinessLogic.NewTotalValueColumnName
            };
        }
    }
}
