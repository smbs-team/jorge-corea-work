namespace CustomSearchesServicesLibrary.CustomSearches.Model.Datasets
{
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;

    /// <summary>
    /// Model for the dataset dependencies.
    /// </summary>
    public class DatasetDependenciesData
    {
        /// <summary>
        /// Gets or sets the dataset post processes.
        /// </summary>
        public DatasetPostProcessData[] PostProcesses { get; set; }

        /// <summary>
        /// Gets or sets the dataset interactive charts.
        /// </summary>
        public InteractiveChartData[] InteractiveCharts { get; set; }
    }
}
