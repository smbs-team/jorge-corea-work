namespace CustomSearchesServicesLibrary.CustomSearches.Model.RScriptModel
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the rscript model data.
    /// </summary>
    public class RScriptModelData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RScriptModelData"/> class.
        /// </summary>
        public RScriptModelData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RScriptModelData"/> class.
        /// </summary>
        /// <param name="model">The rscript model.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public RScriptModelData(RscriptModel model, ModelInitializationType initializationType)
        {
            this.RscriptModelId = model.RscriptModelId;
            this.Rscript = model.Rscript;
            this.Description = model.Description;
            this.RscriptFileName = model.RscriptFileName;
            this.RscriptFolderName = model.RscriptFolderName;
            this.RscriptModelName = model.RscriptModelName;
            this.RscriptDisplayName = model.RscriptDisplayName;
            this.RscriptModelRole = model.RscriptModelRole;
            this.PredictedTSqlExpression = model.PredictedTsqlExpression;
            this.LockPrecommitExpressions = model.LockPrecommitExpressions;
            this.IsDeleted = model.IsDeleted;

            this.ResultsDefinitions = JsonHelper.DeserializeObject<CustomSearchParameterData[]>(model.RscriptResultsDefinition);

            if (initializationType == ModelInitializationType.FullObject)
            {
                if (model.CustomSearchParameter.Count > 0)
                {
                    List<CustomSearchParameterData> parameters = new List<CustomSearchParameterData>();
                    var customSearchParameters = model.CustomSearchParameter.OrderBy(p => p.DisplayOrder);
                    foreach (var customSearchParameter in customSearchParameters)
                    {
                        parameters.Add(new CustomSearchParameterData(customSearchParameter, initializationType));
                    }

                    this.Parameters = parameters.ToArray();
                }

                if (model.CustomSearchExpression.Count > 0)
                {
                    List<CustomSearchExpressionData> expressions = new List<CustomSearchExpressionData>();
                    foreach (var expression in model.CustomSearchExpression)
                    {
                        expressions.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.Expressions = expressions.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rscript model id.
        /// </summary>
        public int RscriptModelId { get; set; }

        /// <summary>
        /// Gets or sets the rscript.
        /// </summary>
        public string Rscript { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rscript file name.
        /// </summary>
        public string RscriptFileName { get; set; }

        /// <summary>
        /// Gets or sets the rscript folder name.
        /// </summary>
        public string RscriptFolderName { get; set; }

        /// <summary>
        /// Gets or sets the rscript model name.
        /// </summary>
        public string RscriptModelName { get; set; }

        /// <summary>
        /// Gets or sets the rscript display name.
        /// </summary>
        public string RscriptDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the rscript model role.
        /// </summary>
        public string RscriptModelRole { get; set; }

        /// <summary>
        /// Gets or sets the TSQL expression for the Predicted variable.
        /// </summary>
        public string PredictedTSqlExpression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pre-commit expressions are locked.
        /// </summary>
        public bool LockPrecommitExpressions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pre-commit expressions are locked.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the dataset post processes.
        /// </summary>
        public DatasetPostProcessData[] DatasetPostProcesses { get; set; }

        /// <summary>
        /// Gets or sets the rscript model parameters.
        /// </summary>
        public CustomSearchParameterData[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the expressions for this parameter.
        /// </summary>
        public CustomSearchExpressionData[] Expressions { get; set; }

        /// <summary>
        /// Gets or sets the rscript model result definitions.
        /// </summary>
        public CustomSearchParameterData[] ResultsDefinitions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public RscriptModel ToEfModel()
        {
            var toReturn = new RscriptModel()
            {
                RscriptModelId = this.RscriptModelId,
                Rscript = this.Rscript,
                Description = this.Description,
                RscriptFileName = this.RscriptFileName,
                RscriptFolderName = this.RscriptFolderName,
                RscriptModelRole = this.RscriptModelRole,
                RscriptModelName = this.RscriptModelName,
                RscriptDisplayName = this.RscriptDisplayName,
                PredictedTsqlExpression = this.PredictedTSqlExpression,
                LockPrecommitExpressions = this.LockPrecommitExpressions,
                IsDeleted = this.IsDeleted
            };

            toReturn.RscriptResultsDefinition = JsonHelper.SerializeObject(this.ResultsDefinitions);

            return toReturn;
        }
    }
}
