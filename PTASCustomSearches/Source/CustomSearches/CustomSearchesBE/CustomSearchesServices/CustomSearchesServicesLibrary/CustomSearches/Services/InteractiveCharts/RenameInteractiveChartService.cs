namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that renames an Interactive Chart.
    /// </summary>
    public class RenameInteractiveChartService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameInteractiveChartService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RenameInteractiveChartService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Validates if the new chart title is not used by other dataset charts.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="chartId">The chart id.</param>
        /// <param name="newChartTitle">The new chart title.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public static async Task ValidateNewChartTitle(CustomSearchesDbContext dbContext, Guid datasetId, int chartId, string newChartTitle)
        {
            var repeatedChartsCount = await dbContext.InteractiveChart
                .CountAsync(c =>
                    c.DatasetId == datasetId &&
                    c.InteractiveChartId != chartId &&
                    c.ChartTitle.Trim().ToLower() == newChartTitle.Trim().ToLower());

            if (repeatedChartsCount > 0)
            {
                throw new CustomSearchesConflictException("A chart with this title already exists.", innerException: null);
            }
        }

        /// <summary>
        /// Renames an interactive chart..
        /// </summary>
        /// <param name="interactiveChartId">The interactive chart id.</param>
        /// <param name="renameInteractiveChartData">The interactive chart data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Chart was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task RenameInteractiveChartAsync(int interactiveChartId, RenameInteractiveChartData renameInteractiveChartData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(renameInteractiveChartData.NewName, nameof(renameInteractiveChartData.NewName));

            InteractiveChart interactiveChart = await
                (from ic in dbContext.InteractiveChart
                 where ic.InteractiveChartId == interactiveChartId
                 select ic).
                Include(ic => ic.Dataset).
                   ThenInclude(d => d.ParentFolder).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(interactiveChart, "Interactive chart", interactiveChartId);

            Dataset dataset = interactiveChart.Dataset;

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "RenameInteractiveChart");

            if (renameInteractiveChartData.NewName != interactiveChart.ChartTitle)
            {
                await ValidateNewChartTitle(dbContext, dataset.DatasetId, interactiveChartId, renameInteractiveChartData.NewName);
                interactiveChart.ChartTitle = renameInteractiveChartData.NewName.Trim();
                await dbContext.ValidateAndSaveChangesAsync();
            }
        }
    }
}
