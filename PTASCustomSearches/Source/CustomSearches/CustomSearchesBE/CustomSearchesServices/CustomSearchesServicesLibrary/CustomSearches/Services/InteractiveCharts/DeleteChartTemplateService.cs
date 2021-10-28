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
    /// Service that deletes a chart template.
    /// </summary>
    public class DeleteChartTemplateService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteChartTemplateService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteChartTemplateService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Soft deletes a charrt template.
        /// </summary>
        /// <param name="chartTemplateId">The chart template id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Chart was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        public async Task DeleteChartTemplateAsync(int chartTemplateId, CustomSearchesDbContext dbContext)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("DeleteChartTemplate");

            ChartTemplate chartTemplate = await
                (from ic in dbContext.ChartTemplate
                 where ic.ChartTemplateId == chartTemplateId
                 select ic).
                Include(ic => ic.CustomSearchExpression).
                Include(ic => ic.CustomSearchChartTemplate).
                FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(chartTemplate, nameof(chartTemplate), chartTemplateId);

            dbContext.CustomSearchChartTemplate.RemoveRange(chartTemplate.CustomSearchChartTemplate);
            dbContext.CustomSearchExpression.RemoveRange(chartTemplate.CustomSearchExpression);
            dbContext.ChartTemplate.Remove(chartTemplate);
            await dbContext.SaveChangesAsync();
        }
    }
}
