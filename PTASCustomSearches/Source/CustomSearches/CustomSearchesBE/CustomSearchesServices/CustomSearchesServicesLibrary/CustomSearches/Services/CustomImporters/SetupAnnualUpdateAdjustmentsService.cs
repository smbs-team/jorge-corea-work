namespace CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.ProjectBusinessLogic;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service that setups the annual update adjustments creating the required post processes.
    /// </summary>
    public class SetupAnnualUpdateAdjustmentsService : BaseService
    {
        /// <summary>
        /// The apply annual update adjustments post process name.
        /// </summary>
        public const string ApplyAnnualUpdateAdjustmentsPostProcessName = "Apply annual update adjustments";

        /// <summary>
        /// The annual update adjustments 'Land' category.
        /// </summary>
        public const string LandCategory = "Land";

        /// <summary>
        /// The annual update adjustments 'Multiple Imps' category.
        /// </summary>
        public const string MultipleImpsCategory = "Multiple Imps";

        /// <summary>
        /// The annual update adjustments 'Mobile Homes' category.
        /// </summary>
        public const string MobileHomesCategory = "Mobile Homes";

        /// <summary>
        /// The annual update adjustments 'Accesory only' category.
        /// </summary>
        public const string AccessoryOnlyCategory = "Accessory only";

        /// <summary>
        /// The annual update adjustments 'Imp no char' category.
        /// </summary>
        public const string ImpNoCharCategory = "Imp no char";

        /// <summary>
        /// The annual update adjustments 'Building' category.
        /// </summary>
        public const string BuildingCategory = "Building";

        /// <summary>
        /// The annual update adjustments 'Land' category filter.
        /// </summary>
        public const string LandCategoryFilter = "1 = 1";

        /// <summary>
        /// The annual update adjustments 'Multiple Imps' category filter.
        /// </summary>
        public const string MultipleImpsCategoryFilter = "1 = 1";

        /// <summary>
        /// The annual update adjustments 'Mobile Homes' category filter.
        /// </summary>
        public const string MobileHomesCategoryFilter = "1 = 1";

        /// <summary>
        /// The annual update adjustments 'Accesory only' category filter.
        /// </summary>
        public const string AccessoryOnlyCategoryFilter = "1 = 1";

        /// <summary>
        /// The annual update adjustments 'Imp no char' category filter.
        /// </summary>
        public const string ImpNoCharCategoryFilter = "1 = 1";

        /// <summary>
        /// The annual update adjustments 'Building' category filter.
        /// </summary>
        public const string BuildingCategoryFilter = "1 = 1";

        /// <summary>
        /// The land factor name.
        /// </summary>
        public const string LandFactorName = "Apply to Land Value";

        /// <summary>
        /// The model factor name.
        /// </summary>
        public const string ModelFactorName = "Multiple imps use model";

        /// <summary>
        /// The total factor name.
        /// </summary>
        public const string TotalFactorName = "Apply to Total Value";

        /// <summary>
        /// The improvements factor name.
        /// </summary>
        public const string ImprovementsFactorName = "Apply to Imp Value";

        /// <summary>
        /// The annual update adjustments post process categories.
        /// </summary>
        public static readonly string[] Categories = { LandCategory, MultipleImpsCategory, MobileHomesCategory, AccessoryOnlyCategory, ImpNoCharCategory, BuildingCategory };

        /// <summary>
        /// The annual update adjustments post process category filters.
        /// </summary>
        public static readonly string[] CategoryFilters =
        {
            LandCategoryFilter,
            MultipleImpsCategoryFilter,
            MobileHomesCategoryFilter,
            AccessoryOnlyCategoryFilter,
            ImpNoCharCategoryFilter,
            BuildingCategoryFilter
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAnnualUpdateAdjustmentsService"/> class.
        /// </summary>
        /// <param name="setupAnnualUpdateAdjustmentsData">The setup annual update adjustments data.</param>
        /// <param name="serviceContext">The service context.</param>
        public SetupAnnualUpdateAdjustmentsService(SetupAnnualUpdateAdjustmentsData setupAnnualUpdateAdjustmentsData, IServiceContext serviceContext)
            : base(serviceContext)
        {
            this.SetupAnnualUpdateAdjustmentsData = setupAnnualUpdateAdjustmentsData;
        }

        /// <summary>
        /// Gets or sets the setup annual update adjustments data.
        /// </summary>
        public SetupAnnualUpdateAdjustmentsData SetupAnnualUpdateAdjustmentsData { get; set; }

        /// <summary>
        /// Creates the default annual update adjustments exception rule.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="extensions">The extensions data.</param>
        public static void CreateDefaultAnnualExceptionRule(
            DatasetPostProcess datasetPostProcess,
            string columnName,
            AnnualUpdateAdjustmentsExtensionsData extensions)
        {
            string calculatedColumnScript = extensions.ApplyFactorTo.Trim().ToLower() == ModelFactorName.ToLower() ?
                "[EMVAdjustment]" :
                $"{extensions.Factor}";

            string traceMessage = $"Applied a factor of {calculatedColumnScript} to {extensions.ApplyFactorTo} for the category {extensions.Category} " +
                $"where the user filter is {extensions.UserFilter} and the minimun land value is {extensions.MinimumLandValueToFactor}";

            extensions.TraceMessage = $"'{datasetPostProcess.PostProcessRole}: {traceMessage} => '" + "{ColumnValue}";

            SetupAnnualUpdateAdjustmentsService.AddExceptionRule(
                datasetPostProcess,
                exceptionRuleDescription: $"Default rule for {columnName}",
                columnName,
                filterScript: SetupAnnualUpdateAdjustmentsService.GetScriptFilter(extensions),
                calculatedColumnScript,
                isAutoGenerated: false,
                extensions);
        }

        /// <summary>
        /// Adds exception rule.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="exceptionRuleDescription">The exception rule description.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="filterScript">The filter script.</param>
        /// <param name="calculatedColumnScript">The calculated column script.</param>
        /// <param name="isAutoGenerated">Value indicating whether the expression is auto generated.</param>
        /// <param name="extensions">The extensions.</param>
        /// <returns>The exception post process rule.</returns>
        public static ExceptionPostProcessRule AddExceptionRule(
            DatasetPostProcess datasetPostProcess,
            string exceptionRuleDescription,
            string columnName,
            string filterScript,
            string calculatedColumnScript,
            bool isAutoGenerated = true,
            AnnualUpdateAdjustmentsExtensionsData extensions = null)
        {
            ExceptionPostProcessRule exceptionPostProcessRule = new ExceptionPostProcessRule()
            {
                DatasetPostProcess = datasetPostProcess,
                Description = exceptionRuleDescription,
                ExecutionOrder = datasetPostProcess.ExceptionPostProcessRule.Count,
            };

            datasetPostProcess.ExceptionPostProcessRule.Add(exceptionPostProcessRule);
            CustomSearchExpression filterExpression = new CustomSearchExpression()
            {
                ColumnName = columnName,
                ExceptionPostProcessRule = exceptionPostProcessRule,
                ExecutionOrder = 0,
                ExpressionExtensions = JsonHelper.SerializeObject(extensions),
                ExpressionRole = CustomSearchExpressionRoleType.FilterExpression.ToString(),
                ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                IsAutoGenerated = isAutoGenerated,
                OwnerType = CustomSearchExpressionOwnerType.ExceptionPostProcessRule.ToString(),
                Script = filterScript
            };

            exceptionPostProcessRule.CustomSearchExpression.Add(filterExpression);

            CustomSearchExpression calculatedColumnExpression = new CustomSearchExpression()
            {
                ColumnName = columnName,
                ExceptionPostProcessRule = exceptionPostProcessRule,
                ExecutionOrder = 1,
                ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                ExpressionType = CustomSearchExpressionType.TSQL.ToString(),
                IsAutoGenerated = isAutoGenerated,
                OwnerType = CustomSearchExpressionOwnerType.ExceptionPostProcessRule.ToString(),
                Script = calculatedColumnScript
            };

            exceptionPostProcessRule.CustomSearchExpression.Add(calculatedColumnExpression);

            datasetPostProcess.ExceptionPostProcessRule.Add(exceptionPostProcessRule);

            return exceptionPostProcessRule;
        }

        /// <summary>
        /// Adds exception rule.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="exceptionRuleDescription">The exception rule description.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="filterScript">The filter script.</param>
        /// <param name="calculatedColumnScript">The calculated column script.</param>
        /// <param name="extensions">The extensions.</param>
        public static void AddApplyExceptionRules(
            DatasetPostProcess datasetPostProcess,
            string exceptionRuleDescription,
            string columnName,
            string filterScript,
            string calculatedColumnScript,
            AnnualUpdateAdjustmentsExtensionsData extensions = null)
        {
            SetupAnnualUpdateAdjustmentsService.AddExceptionRule(
                datasetPostProcess,
                exceptionRuleDescription,
                columnName,
                filterScript,
                calculatedColumnScript,
                isAutoGenerated: true,
                extensions);

            SetupAnnualUpdateAdjustmentsService.AddExceptionRule(
                datasetPostProcess,
                $"Trace: {exceptionRuleDescription}",
                $"{columnName}_RulesTrace",
                filterScript,
                $"'Annual Update Adjustments ({exceptionRuleDescription}). LandFactor: ' + [LandFactor_RulesTrace] + ' NonLandFactor: ' + [NonLandFactor_RulesTrace]");
        }

        /// <summary>
        /// Adds exception rule.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="landImprovementScript">The land and improvement adjustments calculated column script.</param>
        /// <param name="landTotalScript">The land and total adjustments calculated column script.</param>
        /// <param name="landModelScript">The land and model adjustments calculated column script.</param>
        /// <param name="landScript">The land adjustments calculated column script.</param>
        /// <param name="improvementScript">The improvement adjustments calculated column script.</param>
        /// <param name="defaultScript">The default adjustments calculated column script.</param>
        public static void AddExceptionRuleSet(
            DatasetPostProcess datasetPostProcess,
            string columnName,
            string landImprovementScript,
            string landTotalScript,
            string landModelScript,
            string landScript,
            string improvementScript,
            string defaultScript)
        {
            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Land and improvement adjustments",
                columnName: columnName,
                filterScript: $"(([LandFactor] != 1) AND ([NonLandFactor] != 1) AND ([ApplyNonLandFactorTo] = 'Apply to Imp Value'))",
                calculatedColumnScript: landImprovementScript);

            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Land and total adjustments",
                columnName: columnName,
                filterScript: "(([NonLandFactor] != 1) AND ([ApplyNonLandFactorTo] = 'Apply to Total Value'))",
                calculatedColumnScript: landTotalScript);

            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Land and model adjustments",
                columnName: columnName,
                filterScript: "(([EMVAdjustment] != 1) AND ([ApplyNonLandFactorTo] = 'Multiple imps use model'))",
                calculatedColumnScript: landModelScript);

            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Land adjustments",
                columnName: columnName,
                filterScript: "([LandFactor] != 1)",
                calculatedColumnScript: landScript);

            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Improvement adjustments",
                columnName: columnName,
                filterScript: "(([NonLandFactor] != 1) AND ([ApplyNonLandFactorTo] = 'Apply to Imp Value'))",
                calculatedColumnScript: improvementScript);

            SetupAnnualUpdateAdjustmentsService.AddApplyExceptionRules(
                datasetPostProcess,
                exceptionRuleDescription: "Default adjustments",
                columnName: columnName,
                filterScript: "default",
                calculatedColumnScript: defaultScript);
        }

        /// <summary>
        /// Creates and adds the default annual update adjustments exception rule to the dataset post process.
        /// </summary>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="extensions">The extensions data.</param>
        public static void AddDefaultAnnualExceptionRule(
            DatasetPostProcess datasetPostProcess,
            AnnualUpdateAdjustmentsExtensionsData extensions)
        {
            if (extensions.ApplyFactorTo.Trim().ToLower() == SetupAnnualUpdateAdjustmentsService.LandFactorName.ToLower())
            {
                SetupAnnualUpdateAdjustmentsService.CreateDefaultAnnualExceptionRule(datasetPostProcess, columnName: "LandFactor", extensions);
            }
            else
            {
                SetupAnnualUpdateAdjustmentsService.CreateDefaultAnnualExceptionRule(datasetPostProcess, columnName: "NonLandFactor", extensions);

                SetupAnnualUpdateAdjustmentsService.AddExceptionRule(
                    datasetPostProcess,
                    exceptionRuleDescription: "Default rule for ApplyNonLandFactorTo",
                    columnName: "ApplyNonLandFactorTo",
                    SetupAnnualUpdateAdjustmentsService.GetScriptFilter(extensions),
                    calculatedColumnScript: $"'{extensions.ApplyFactorTo.Trim().ToLower()}'");
            }
        }

        /// <summary>
        /// Setups the annual update adjustments creating the required post processes.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>
        /// The annual update adjustments post processes.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<SetupAnnualUpdateAdjustmentsResponse> SetupAnnualUpdateAdjustmentsAsync(
            Guid datasetId,
            CustomSearchesDbContext dbContext,
            ILogger logger)
        {
            var response = new SetupAnnualUpdateAdjustmentsResponse();

            List<Guid> datasetIds = new List<Guid>();
            datasetIds.Add(datasetId);
            if (this.SetupAnnualUpdateAdjustmentsData.SecondaryDatasets?.Length > 0)
            {
                datasetIds.AddRange(this.SetupAnnualUpdateAdjustmentsData.SecondaryDatasets);
            }

            var datasets = await dbContext.Dataset
                .Where(d => datasetIds.Contains(d.DatasetId))
                .Include(d => d.DatasetPostProcess)
                .ToListAsync();

            var primaryDataset = datasets.FirstOrDefault(d => d.DatasetId == datasetId);
            InputValidationHelper.AssertEntityExists(primaryDataset, nameof(Dataset), datasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(primaryDataset, dbContext);
            InputValidationHelper.AssertEntityExists(userProject, nameof(userProject), entityId: null, $"Dataset should belong to an user project.");
            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(
                primaryDataset.UserId,
                primaryDataset.ParentFolder,
                primaryDataset.IsLocked,
                userProject,
                "SetupAnnualUpdateAdjustments");

            ProjectBusinessLogicFactory projectBusinessLogicFactory = new ProjectBusinessLogicFactory();
            var projectBusinessLogic = projectBusinessLogicFactory.CreateProjectBusinessLogic(userProject, dbContext);
            projectBusinessLogic.ValidateImportPostProcess(new DatasetPostProcessData() { PostProcessRole = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole }, datasets, bypassPostProcessBundleCheck: true);

            // Starting at the primary dataset
            datasets.Remove(primaryDataset);
            datasets.Insert(0, primaryDataset);

            foreach (var currentDataset in datasets)
            {
                if (this.SetupAnnualUpdateAdjustmentsData.IsCustomModelingStepPostProcess)
                {
                    this.SetupCustomModelingStepPostProcess(primaryDataset, currentDataset);
                }
                else
                {
                    this.SetupAnnualUpdateAdjustments(primaryDataset, currentDataset);
                    this.SetupApplyAnnualUpdateAdjustments(primaryDataset, currentDataset);
                }
            }

            await dbContext.SaveChangesAsync();

            var datasetPostProcessDataList = new List<DatasetPostProcessData>();
            foreach (var datasetPostProcess in primaryDataset.DatasetPostProcess)
            {
                if (datasetPostProcess.PostProcessRole == AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole ||
                    datasetPostProcess.PostProcessRole == AnnualUpdateProjectBusinessLogic.ApplyAnnualUpdateAdjustmentsPostProcessRole)
                {
                    datasetPostProcessDataList.Add(new DatasetPostProcessData(datasetPostProcess, ModelInitializationType.FullObjectWithDepedendencies, userDetails: null));
                }
            }

            response.PostProcesses = datasetPostProcessDataList.ToArray();

            var lastPostProcessRole = this.SetupAnnualUpdateAdjustmentsData.IsCustomModelingStepPostProcess ?
                AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole :
                AnnualUpdateProjectBusinessLogic.ApplyAnnualUpdateAdjustmentsPostProcessRole;

            ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
            response.JobId = (int)(await service.QueueExecuteDatasetPostProcessAsync(
                datasetId,
                primaryDataset.DatasetPostProcess.First(dpp => dpp.PostProcessRole == lastPostProcessRole).DatasetPostProcessId,
                major: null,
                minor: null,
                parameters: null,
                dataStream: null,
                dbContext)).Id;

            return response;
        }

        /// <summary>
        /// Setups the custom modeling step post process.
        /// </summary>
        /// <param name="primaryDataset">The primary dataset.</param>
        /// <param name="dataset">The dataset.</param>
        public void SetupCustomModelingStepPostProcess(Dataset primaryDataset, Dataset dataset)
        {
            DatasetPostProcess datasetPostProcess =
                dataset.DatasetPostProcess.FirstOrDefault(dp => dp.PostProcessRole == AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole);

            if (datasetPostProcess == null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                datasetPostProcess = new DatasetPostProcess()
                {
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    Dataset = dataset,
                    IsDirty = true,
                    LastModifiedBy = userId,
                    PostProcessDefinition = "Annual update adjustments",
                    PostProcessName = "Annual update adjustments",
                    PostProcessRole = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole,
                    LastModifiedTimestamp = DateTime.UtcNow,
                    PostProcessType = DatasetPostProcessType.CustomModelingStepPostProcess.ToString(),
                    Priority = 2100,
                    TraceEnabledFields = "[\"NewLandValue\", \"NewImprovementsValue\", \"NewTotalValue\"]",
                };

                dataset.DatasetPostProcess.Add(datasetPostProcess);

                CustomSearchExpression newLandValueExpression = new CustomSearchExpression()
                {
                    ColumnName = "NewLandValue",
                    ExecutionOrder = 1,
                    ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                    ExpressionType = CustomSearchExpressionType.Imported.ToString(),
                    IsAutoGenerated = true,
                    OwnerType = CustomSearchExpressionOwnerType.DatasetPostProcess.ToString(),
                    Script = "1 = 1"
                };

                datasetPostProcess.CustomSearchExpression.Add(newLandValueExpression);

                CustomSearchExpression newImprovementsValueExpression = new CustomSearchExpression()
                {
                    ColumnName = "NewImprovementsValue",
                    ExecutionOrder = 1,
                    ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                    ExpressionType = CustomSearchExpressionType.Imported.ToString(),
                    IsAutoGenerated = true,
                    OwnerType = CustomSearchExpressionOwnerType.DatasetPostProcess.ToString(),
                    Script = "1 = 1"
                };

                datasetPostProcess.CustomSearchExpression.Add(newImprovementsValueExpression);

                CustomSearchExpression newTotalValueExpression = new CustomSearchExpression()
                {
                    ColumnName = "NewTotalValue",
                    ExecutionOrder = 1,
                    ExpressionRole = CustomSearchExpressionRoleType.CalculatedColumn.ToString(),
                    ExpressionType = CustomSearchExpressionType.Imported.ToString(),
                    IsAutoGenerated = true,
                    OwnerType = CustomSearchExpressionOwnerType.DatasetPostProcess.ToString(),
                    Script = "1 = 1"
                };

                datasetPostProcess.CustomSearchExpression.Add(newTotalValueExpression);
            }

            this.AssignPrimaryDatasetPostProcess(primaryDataset, dataset, datasetPostProcess);
        }

        /// <summary>
        /// Setups the annual update adjustments post process.
        /// </summary>
        /// <param name="primaryDataset">The primary dataset.</param>
        /// <param name="dataset">The dataset.</param>
        public void SetupAnnualUpdateAdjustments(Dataset primaryDataset, Dataset dataset)
        {
            DatasetPostProcess datasetPostProcess =
                dataset.DatasetPostProcess.FirstOrDefault(dp => dp.PostProcessRole == AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole);

            if (datasetPostProcess == null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                datasetPostProcess = new DatasetPostProcess()
                {
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    Dataset = dataset,
                    IsDirty = true,
                    LastModifiedBy = userId,
                    PostProcessDefinition = "Annual update adjustments",
                    PostProcessName = "Annual update adjustments",
                    PostProcessRole = AnnualUpdateProjectBusinessLogic.AnnualUpdateAdjustmentsPostProcessRole,
                    LastModifiedTimestamp = DateTime.UtcNow,
                    PostProcessSubType = ExceptionMethodType.UniqueConditionSelector.ToString(),
                    PostProcessType = DatasetPostProcessType.ExceptionPostProcess.ToString(),
                    Priority = 2100,
                    TraceEnabledFields = "[\"LandFactor\", \"NonLandFactor\", \"ApplyNonLandFactorTo\"]",
                };

                dataset.DatasetPostProcess.Add(datasetPostProcess);

                AnnualUpdateAdjustmentsExtensionsData landExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.LandCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.LandCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.LandFactorName,
                };

                AnnualUpdateAdjustmentsExtensionsData multipleImpsExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.MultipleImpsCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.MultipleImpsCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.ModelFactorName,
                };

                AnnualUpdateAdjustmentsExtensionsData mobileHomesImpsExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.MobileHomesCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.MobileHomesCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.TotalFactorName,
                };

                AnnualUpdateAdjustmentsExtensionsData accessoryOnlyExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.AccessoryOnlyCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.AccessoryOnlyCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.TotalFactorName,
                };

                AnnualUpdateAdjustmentsExtensionsData impNoCharExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.ImpNoCharCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.ImpNoCharCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.ImprovementsFactorName,
                };

                AnnualUpdateAdjustmentsExtensionsData buildingExtension = new AnnualUpdateAdjustmentsExtensionsData
                {
                    Category = SetupAnnualUpdateAdjustmentsService.BuildingCategory,
                    CategoryFilter = SetupAnnualUpdateAdjustmentsService.BuildingCategoryFilter,
                    Factor = 1,
                    ApplyFactorTo = SetupAnnualUpdateAdjustmentsService.TotalFactorName,
                };

                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, buildingExtension);
                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, impNoCharExtension);
                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, accessoryOnlyExtension);
                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, mobileHomesImpsExtension);
                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, multipleImpsExtension);
                SetupAnnualUpdateAdjustmentsService.AddDefaultAnnualExceptionRule(datasetPostProcess, landExtension);
            }

            this.AssignPrimaryDatasetPostProcess(primaryDataset, dataset, datasetPostProcess);
        }

        /// <summary>
        /// Setups the apply annual update adjustments post process.
        /// </summary>
        /// <param name="primaryDataset">The primary dataset.</param>
        /// <param name="dataset">The dataset.</param>
        public void SetupApplyAnnualUpdateAdjustments(Dataset primaryDataset, Dataset dataset)
        {
            DatasetPostProcess datasetPostProcess =
                dataset.DatasetPostProcess.FirstOrDefault(dp => dp.PostProcessRole == AnnualUpdateProjectBusinessLogic.ApplyAnnualUpdateAdjustmentsPostProcessRole);

            if (datasetPostProcess == null)
            {
                Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
                datasetPostProcess = new DatasetPostProcess()
                {
                    CreatedBy = userId,
                    CreatedTimestamp = DateTime.UtcNow,
                    Dataset = dataset,
                    IsDirty = true,
                    LastModifiedBy = userId,
                    PostProcessDefinition = SetupAnnualUpdateAdjustmentsService.ApplyAnnualUpdateAdjustmentsPostProcessName,
                    PostProcessName = SetupAnnualUpdateAdjustmentsService.ApplyAnnualUpdateAdjustmentsPostProcessName,
                    PostProcessRole = AnnualUpdateProjectBusinessLogic.ApplyAnnualUpdateAdjustmentsPostProcessRole,
                    LastModifiedTimestamp = DateTime.UtcNow,
                    PostProcessSubType = ExceptionMethodType.UniqueConditionSelector.ToString(),
                    PostProcessType = DatasetPostProcessType.ExceptionPostProcess.ToString(),
                    Priority = 2500,
                };

                dataset.DatasetPostProcess.Add(datasetPostProcess);

                string commonLandValueScript = "([RollLand] * [LandFactor])";

                SetupAnnualUpdateAdjustmentsService.AddExceptionRuleSet(
                    datasetPostProcess,
                    columnName: "NewLandValue",
                    landImprovementScript: commonLandValueScript,
                    landTotalScript: commonLandValueScript,
                    landModelScript: commonLandValueScript,
                    landScript: commonLandValueScript,
                    improvementScript: "([RollTot] - ([NonLandFactor] * [RollImp]))",
                    defaultScript: "[RollLand]");

                string commonImprovementsValueScript = "(([RollTot] * [NonLandFactor]) - ([RollLand] * [LandFactor]))";
                string factorImprovementsValueScript = "([RollImp] * [NonLandFactor])";

                SetupAnnualUpdateAdjustmentsService.AddExceptionRuleSet(
                    datasetPostProcess,
                    columnName: "NewImprovementsValue",
                    landImprovementScript: factorImprovementsValueScript,
                    landTotalScript: commonImprovementsValueScript,
                    landModelScript: commonImprovementsValueScript,
                    landScript: commonImprovementsValueScript,
                    improvementScript: factorImprovementsValueScript,
                    defaultScript: "[RollImp]");

                string commonTotalValueScript = "([RollLand] * [NonLandFactor])";

                SetupAnnualUpdateAdjustmentsService.AddExceptionRuleSet(
                    datasetPostProcess,
                    columnName: "NewTotalValue",
                    landImprovementScript: "([RollLand] * [LandFactor]) + ([NonLandFactor] * [RollImp])",
                    landTotalScript: commonTotalValueScript,
                    landModelScript: commonTotalValueScript,
                    landScript: commonTotalValueScript,
                    improvementScript: commonTotalValueScript,
                    defaultScript: "[RollTot]");
            }

            this.AssignPrimaryDatasetPostProcess(primaryDataset, dataset, datasetPostProcess);
        }

        /// <summary>
        /// Gets the script filter.
        /// </summary>
        /// <param name="extensions">The extensions data.</param>
        /// <returns>The script filter.</returns>
        private static string GetScriptFilter(AnnualUpdateAdjustmentsExtensionsData extensions)
        {
            string filterScript = $"{extensions.CategoryFilter} AND RollTot >= {extensions.MinimumLandValueToFactor}";
            if (!string.IsNullOrWhiteSpace(extensions.UserFilter))
            {
                filterScript += $" AND ({extensions.UserFilter})";
            }

            return filterScript;
        }

        /// <summary>
        /// Assigns the primary dataset post process.
        /// </summary>
        /// <param name="primaryDataset">The primary dataset.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        private void AssignPrimaryDatasetPostProcess(Dataset primaryDataset, Dataset dataset, DatasetPostProcess datasetPostProcess)
        {
            datasetPostProcess.PrimaryDatasetPostProcess =
                primaryDataset == dataset ?
                null :
                primaryDataset.DatasetPostProcess.FirstOrDefault(dpp => dpp.PostProcessRole == datasetPostProcess.PostProcessRole);
        }
    }
}
