namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Executor;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the predicted equation for the RScript post process.
    /// </summary>
    public class GetRScriptPredictedEquationService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRScriptPredictedEquationService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetRScriptPredictedEquationService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the predicted equation for the RScript post process.
        /// </summary>
        /// <param name="rScriptPostProcessId">The RScript post process id.</param>
        /// <param name="predictedPrecision">The precision (number of decimals) of the predicted values.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The predicted equation for the RScript post process.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Expressions for rscript model were not found.</exception>
        public async Task<GetRScriptPredictedEquationResponse> GetRScriptPredictedEquationAsync(
            int rScriptPostProcessId,
            int predictedPrecision,
            CustomSearchesDbContext dbContext)
        {
            GetRScriptPredictedEquationResponse response = new GetRScriptPredictedEquationResponse();

            var rScriptPostProcess = await dbContext.DatasetPostProcess
                .Where(dpp => dpp.DatasetPostProcessId == rScriptPostProcessId)
                .Include(dpp => dpp.Dataset)
                .Include(dpp => dpp.RscriptModel)
                .Include(dpp => dpp.CustomSearchExpression)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(rScriptPostProcess, nameof(rScriptPostProcess), rScriptPostProcessId);

            if (string.IsNullOrWhiteSpace(rScriptPostProcess.ResultPayload))
            {
                throw new CustomSearchesConflictException("The RScriptPostProcess should be executed.", innerException: null);
            }

            Dictionary<string, string> replacementDictionary = new Dictionary<string, string>();

            await RScriptDatasetPostProcessExecutor.AddRScriptResultsAsReplacementsAsync(
                rScriptPostProcess.Dataset,
                rScriptPostProcess,
                dbContext,
                this.ServiceContext,
                replacementDictionary,
                predictedPrecision);

            if (replacementDictionary.ContainsKey(RScriptDatasetPostProcessExecutor.PredictedVariableName))
            {
                response.PredictedEquation = replacementDictionary[RScriptDatasetPostProcessExecutor.PredictedVariableName];
            }

            return response;
        }
    }
}