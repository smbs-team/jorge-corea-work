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
    /// Class to manage the physical inspection project business logic.
    /// </summary>
    public class PhysicalInspectionProjectBusinessLogic : DefaultProjectBusinessLogic
    {
        /// <summary>
        /// The new trended price column name.
        /// </summary>
        public const string NewTrendedPriceColumnName = "NewTrendedPrice";

        /// <summary>
        /// The time adjustment column name.
        /// </summary>
        public const string TimeAdjustmentColumnName = "TimeAdjustment";

        /// <summary>
        /// The new EMV column name.
        /// </summary>
        public const string NewEMVColumnName = "NewEMV";

        /// <summary>
        /// The new land value column name.
        /// </summary>
        public const string NewLandValueColumnName = "NewLandValue";

        /// <summary>
        /// The supplemental column name.
        /// </summary>
        public const string SupplementalColumnName = "Supplemental";

        /// <summary>
        /// The supplemental formula column name.
        /// </summary>
        public const string SupplementalFormulaColumnName = "SupplementalFormula";

        /// <summary>
        /// The time trend regression post process role.
        /// </summary>
        public const string TimeTrendRegressionPostProcessRole = "TimeTrendRegression";

        /// <summary>
        /// The land schedule post process role.
        /// </summary>
        public const string LandSchedulePostProcessRole = "LandSchedule";

        /// <summary>
        /// The non waterfront expressions post process role.
        /// </summary>
        public const string NonWaterfrontExpressionsPostProcessRole = "NonWaterfrontExpressions";

        /// <summary>
        /// The waterfront schedule post process role.
        /// </summary>
        public const string WaterfrontSchedulePostProcessRole = "WaterfrontSchedule";

        /// <summary>
        /// The waterfront expressions post process role.
        /// </summary>
        public const string WaterfrontExpressionsPostProcessRole = "WaterfrontExpressions";

        /// <summary>
        /// The 'add waterfront value to land' post process role.
        /// </summary>
        public const string AddWaterfrontValueToLandValuePostProcessRole = "AddWaterfrontValueToLandValue";

        /// <summary>
        /// The land adjustment post process role.
        /// </summary>
        public const string LandAdjustmentPostProcessRole = "LandAdjustment";

        /// <summary>
        /// The truncate land value post process role.
        /// </summary>
        public const string TruncateLandValuePostProcessRole = "TruncateLandValue";

        /// <summary>
        /// The multiple regression post process role.
        /// </summary>
        public const string MultipleRegressionPostProcessRole = "MultipleRegression";

        /// <summary>
        /// The supplemental and exception post process role.
        /// </summary>
        public const string SupplementalAndExceptionPostProcessRole = "SupplementalAndException";

        /// <summary>
        /// The land schedule post process role order.
        /// </summary>
        public static readonly List<string> LandSchedulePostProcessRoleOrder = new List<string>
        {
            LandSchedulePostProcessRole.ToLower(),
            NonWaterfrontExpressionsPostProcessRole.ToLower(),
            WaterfrontSchedulePostProcessRole.ToLower(),
            WaterfrontExpressionsPostProcessRole.ToLower(),
            AddWaterfrontValueToLandValuePostProcessRole.ToLower(),
            LandAdjustmentPostProcessRole.ToLower(),
            TruncateLandValuePostProcessRole.ToLower(),
        };

        /// <summary>
        /// The post process role order.
        /// </summary>
        public static readonly List<string> PostProcessRoleOrder = new List<string>
        {
            TimeTrendRegressionPostProcessRole.ToLower(),
            LandSchedulePostProcessRole.ToLower(),
            NonWaterfrontExpressionsPostProcessRole.ToLower(),
            WaterfrontSchedulePostProcessRole.ToLower(),
            WaterfrontExpressionsPostProcessRole.ToLower(),
            AddWaterfrontValueToLandValuePostProcessRole.ToLower(),
            LandAdjustmentPostProcessRole.ToLower(),
            TruncateLandValuePostProcessRole.ToLower(),
            MultipleRegressionPostProcessRole.ToLower(),
            SupplementalAndExceptionPostProcessRole.ToLower()
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalInspectionProjectBusinessLogic"/> class.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="dbContext">The database context.</param>
        public PhysicalInspectionProjectBusinessLogic(
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
            int roleIndex = PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRoleLower);
            if (roleIndex < 0)
            {
                return;
            }

            var populationDataset =
                datasets.FirstOrDefault(d => d.UserProjectDataset.FirstOrDefault(upd => upd.OwnsDataset && upd.DatasetRole?.Trim().ToLower() == this.PivotDatasetRole.ToLower()) != null);

            if (populationDataset == null)
            {
                throw new CustomSearchesRequestBodyException(
                    $"The import should have a {this.PivotDatasetRole} dataset as primary or secondary.",
                    innerException: null);
            }

            if (roleIndex > 0)
            {
                // We exclude 'LandSchedule' because it can be imported without a 'TimeTrend'.
                if (postProcessRoleLower != PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRole.ToLower())
                {
                    int landModelRoleIndex = PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRoleOrder.IndexOf(postProcessRoleLower);
                    string firstSchedulePostProcessRole = PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRoleOrder[0];

                    string previousRole = landModelRoleIndex > 0 ?
                        firstSchedulePostProcessRole :
                        PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder[roleIndex - 1];

                    // 'MultipleRegression' also requires 'TimeTrend'.
                    bool isTimeTrendRequired = postProcessRoleLower == PhysicalInspectionProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower();

                    foreach (var currentDataset in datasets)
                    {
                        if (currentDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == previousRole) == null)
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"The dataset '{currentDataset.DatasetId}' should first create a post process with role '{previousRole}'.",
                                innerException: null);
                        }

                        if (isTimeTrendRequired &&
                            currentDataset.DatasetPostProcess
                                .FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == PhysicalInspectionProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower()) == null)
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"The dataset '{currentDataset.DatasetId}' should first create a post process with role '{PhysicalInspectionProjectBusinessLogic.TimeTrendRegressionPostProcessRole}'.",
                                innerException: null);
                        }

                        if ((!bypassPostProcessBundleCheck) &&
                            (landModelRoleIndex >= 0) &&
                            (currentDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == firstSchedulePostProcessRole) == null))
                        {
                            throw new CustomSearchesRequestBodyException(
                                $"The dataset '{currentDataset.DatasetId}' should first use the SetupLandModel service to import the base land schedule post processes.",
                                innerException: null);
                        }
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

            if (postProcessRole == PhysicalInspectionProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower())
            {
                columnNames = new HashSet<string>
                {
                    PhysicalInspectionProjectBusinessLogic.NewTrendedPriceColumnName,
                    PhysicalInspectionProjectBusinessLogic.TimeAdjustmentColumnName
                };
            }
            else if (postProcessRole == PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRole.ToLower())
            {
                columnNames = new HashSet<string> { PhysicalInspectionProjectBusinessLogic.NewLandValueColumnName };
            }
            else if (postProcessRole == PhysicalInspectionProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower())
            {
                columnNames = new HashSet<string> { PhysicalInspectionProjectBusinessLogic.NewEMVColumnName };
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
            if (postProcessRole != PhysicalInspectionProjectBusinessLogic.TimeTrendRegressionPostProcessRole.ToLower() &&
                postProcessRole != PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRole.ToLower() &&
                postProcessRole != PhysicalInspectionProjectBusinessLogic.MultipleRegressionPostProcessRole.ToLower() &&
                postProcessRole != PhysicalInspectionProjectBusinessLogic.SupplementalAndExceptionPostProcessRole.ToLower())
            {
                throw new CustomSearchesRequestBodyException(
                    $"The {nameof(datasetPostProcessData)} requires an expression with one of the following roles:" +
                    $" '{PhysicalInspectionProjectBusinessLogic.TimeTrendRegressionPostProcessRole}', '{PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRole}'," +
                    $" '{PhysicalInspectionProjectBusinessLogic.MultipleRegressionPostProcessRole}', '{PhysicalInspectionProjectBusinessLogic.SupplementalAndExceptionPostProcessRole}'.",
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
            if (PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(datasetPostProcess.PostProcessRole?.Trim().ToLower()) >= 0)
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

            int landModelBundleRoleIndex = PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRoleOrder.IndexOf(postProcessRole);

            if (!bypassPostProcessBundleCheck)
            {
                if ((landModelBundleRoleIndex >= 0) && (postProcessRole != "nonwaterfrontexpressions") && (postProcessRole != "waterfrontexpressions"))
                {
                    throw new CustomSearchesRequestBodyException(
                        $"To delete the dataset post process with role '{datasetPostProcess.PostProcessRole}' " +
                        $"should use the DeleteLandModel service.",
                        innerException: null);
                }
            }

            int roleIndex = landModelBundleRoleIndex >= 0 ?
                PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(PhysicalInspectionProjectBusinessLogic.LandSchedulePostProcessRoleOrder.Last()) :
                PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRole);

            if ((roleIndex < 0) || (postProcessRole == "nonwaterfrontexpressions") || (postProcessRole == "waterfrontexpressions"))
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
                int currentRoleIndex = PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(currentPostProcess.PostProcessRole?.Trim().ToLower());
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
            return PhysicalInspectionProjectBusinessLogic.PostProcessRoleOrder.IndexOf(postProcessRole?.Trim().ToLower()) >= 0 ? true : false;
        }

        /// <summary>
        /// Validates the apply model operation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task ValidateApplyModelAsync()
        {
            await this.LoadPivotDatasetAsync(datasetId: null);
            if ((this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == "timetrendregression") == null) ||
                (this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == "landschedule") == null) ||
                (this.PivotDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole?.Trim().ToLower() == "multipleregression") == null))
            {
                throw new CustomSearchesRequestBodyException(
                    $"The apply model service needs post processes with the following roles: 'timetrendregression', 'landschedule', 'multipleregression'.",
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
            return "UsePhysicalInspectionModel";
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
                PhysicalInspectionProjectBusinessLogic.NewTrendedPriceColumnName,
                PhysicalInspectionProjectBusinessLogic.TimeAdjustmentColumnName,
                PhysicalInspectionProjectBusinessLogic.NewLandValueColumnName,
                PhysicalInspectionProjectBusinessLogic.NewEMVColumnName,
                PhysicalInspectionProjectBusinessLogic.SupplementalColumnName,
                PhysicalInspectionProjectBusinessLogic.SupplementalFormulaColumnName,
            };
        }
    }
}
