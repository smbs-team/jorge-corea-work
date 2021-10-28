namespace CustomSearchesServicesLibrary.CustomSearches.Executor
{
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Executor for exception dataset post process.
    /// </summary>
    public class FilterDatasetPostProcessExecutor : DatasetPostProcessExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDatasetPostProcessExecutor"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="datasetPostProcess">The dataset post process.</param>
        /// <param name="previousViewScript">The previousViewScript.</param>
        /// <param name="singleRowExecutionData">The single row execution data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <param name="serviceContext">The service context.</param>
        public FilterDatasetPostProcessExecutor(
            Dataset dataset,
            DatasetPostProcess datasetPostProcess,
            string previousViewScript,
            SingleRowExecutionData singleRowExecutionData,
            CustomSearchesDbContext dbContext,
            IServiceContext serviceContext)
            : base(dataset, datasetPostProcess, previousViewScript, singleRowExecutionData, dbContext, serviceContext)
        {
        }

        /// <summary>
        /// Calculates the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CalculateViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (postProcessViewPhase == PostProcessViewPhase.PreCommit)
            {
                return;
            }

            string viewName = CustomSearchesDataDbContext.GetDatasetViewFullName(this.Dataset, !string.IsNullOrWhiteSpace(this.ViewScript) /*usePostProcess*/);
            string previousViewScript = string.IsNullOrWhiteSpace(this.ViewScript) ? ("SELECT * FROM " + viewName) : this.ViewScript;

            DatasetPostProcess datasetPostProcess =
                await this.DbContext.DatasetPostProcess
                .Where(d => d.DatasetPostProcessId == this.DatasetPostProcess.DatasetPostProcessId)
                .Include(d => d.CustomSearchExpression)
                .FirstOrDefaultAsync();

            string whereScript = string.Empty;
            foreach (var filterExpression in datasetPostProcess.CustomSearchExpression)
            {
                string filterScript = await CustomSearchExpressionEvaluator.ReplaceVariablesAsync(
                    filterExpression.Script,
                    replacementDictionary: null,
                    keyName: null,
                    this.ServiceContext,
                    this.Dataset,
                    throwOnFail: true);

                if (string.IsNullOrWhiteSpace(whereScript))
                {
                    whereScript = filterScript;
                }
                else
                {
                    whereScript += " AND " + filterScript;
                }
            }

            string script = "SELECT *\n";
            script += "FROM (" + previousViewScript + ") a\n";
            script += "WHERE " + whereScript + "\n";
            this.ViewScript = script;
        }

        /// <summary>
        /// Commits the view.
        /// </summary>
        /// <param name="postProcessViewPhase">The post process view phase.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task CommitViewAsync(PostProcessViewPhase postProcessViewPhase)
        {
            if (postProcessViewPhase == PostProcessViewPhase.PreCommit)
            {
                return;
            }

            await base.CommitViewAsync(postProcessViewPhase);
        }
    }
}
