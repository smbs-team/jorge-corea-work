namespace CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the interactive charts for a dataset.
    /// </summary>
    public class GetDatasetInteractiveChartsService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetInteractiveChartsService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetInteractiveChartsService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the interactive charts for a dataset.
        /// </summary>
        /// <param name="datasetId">Dataset id.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The interactive charts for the dataset.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        public async Task<GetDatasetInteractiveChartsResponse> GetDatasetInteractiveChartsAsync(
            Guid datasetId,
            CustomSearchesDbContext dbContext)
        {
            GetDatasetInteractiveChartsResponse response = new GetDatasetInteractiveChartsResponse();

            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.InteractiveChart)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            if (dataset.InteractiveChart.Count > 0)
            {
                var interactiveCharts = dataset.InteractiveChart.ToArray();
                response.InteractiveCharts = new InteractiveChartData[interactiveCharts.Length];
                for (int i = 0; i < interactiveCharts.Length; i++)
                {
                    var item = interactiveCharts[i];
                    response.InteractiveCharts[i] = new InteractiveChartData(item, ModelInitializationType.Summary, userDetails: null);
                }
            }

            return response;
        }
    }
}
