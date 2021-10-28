namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes an Interactive Chart.
    /// </summary>
    public class DeleteInteractiveChartService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteInteractiveChartService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteInteractiveChartService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Soft deletes an interactive chart.
        /// </summary>
        /// <param name="interactiveChartId">The interactive chart id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Chart was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteInteractiveChartAsync(int interactiveChartId, CustomSearchesDbContext dbContext)
        {
            InteractiveChart interactiveChart = await
                (from ic in dbContext.InteractiveChart
                where ic.InteractiveChartId == interactiveChartId
                select ic).
                Include(ic => ic.CustomSearchExpression).
                Include(ic => ic.Dataset).
                    ThenInclude(d => d.ParentFolder).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(interactiveChart, "Interactive chart", interactiveChartId);

            Dataset dataset = interactiveChart.Dataset;

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "DeleteInteractiveChart");

            dbContext.CustomSearchExpression.RemoveRange(interactiveChart.CustomSearchExpression);
            dbContext.InteractiveChart.Remove(interactiveChart);
            await dbContext.SaveChangesAsync();
        }
    }
}
