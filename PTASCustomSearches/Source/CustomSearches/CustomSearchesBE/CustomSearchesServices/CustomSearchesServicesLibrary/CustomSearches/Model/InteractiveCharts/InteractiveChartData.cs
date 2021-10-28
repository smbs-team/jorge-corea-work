namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the interactive chart data.
    /// </summary>
    public class InteractiveChartData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveChartData"/> class.
        /// </summary>
        public InteractiveChartData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveChartData"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public InteractiveChartData(InteractiveChart chart, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.InteractiveChartId = chart.InteractiveChartId;
            this.DatasetId = chart.DatasetId;
            this.ChartType = chart.ChartType;
            this.ChartTitle = chart.ChartTitle;
            this.CreatedBy = chart.CreatedBy;
            this.LastModifiedBy = chart.LastModifiedBy;
            this.CreatedTimestamp = chart.CreatedTimestamp;
            this.LastModifiedTimestamp = chart.LastModifiedTimestamp;

            this.ChartExtensions = JsonHelper.DeserializeObject(chart.ChartExtensions);

            UserDetailsHelper.GatherUserDetails(chart.CreatedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(chart.LastModifiedByNavigation, userDetails);

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if (chart.CustomSearchExpression != null && chart.CustomSearchExpression.Count > 0)
                {
                    var expressions = new List<CustomSearchExpressionData>();
                    foreach (var expression in chart.CustomSearchExpression)
                    {
                        expressions.Add(new CustomSearchExpressionData(expression, initializationType));
                    }

                    this.ChartExpressions = expressions.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets the interactive chart identifier.
        /// </summary>
        public int InteractiveChartId { get; set; }

        /// <summary>
        /// Gets or sets the dataset identifier.
        /// </summary>
        public Guid DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the type of the chart.
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// Gets or sets the chart title.
        /// </summary>
        public string ChartTitle { get; set; }

        /// <summary>
        /// Gets or sets the created by field.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by field.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created time stamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified time stamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the chart extensions.
        /// </summary>
        public object ChartExtensions { get; set; }

        /// <summary>
        /// Gets or sets the chart expressions.
        /// </summary>
        public CustomSearchExpressionData[] ChartExpressions { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>The entity framework model.</returns>
        public InteractiveChart ToEfModel()
        {
            var toReturn = new InteractiveChart()
            {
                InteractiveChartId = this.InteractiveChartId,
                DatasetId = this.DatasetId,
                ChartType = this.ChartType?.Trim(),
                ChartTitle = this.ChartTitle?.Trim(),
                CreatedBy = this.CreatedBy,
                LastModifiedBy = this.LastModifiedBy,
                CreatedTimestamp = this.CreatedTimestamp,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
            };

            toReturn.ChartExtensions = JsonHelper.SerializeObject(this.ChartExtensions);

            if (this.ChartExpressions != null)
            {
                toReturn.CustomSearchExpression = new List<CustomSearchExpression>();
                foreach (var expression in this.ChartExpressions)
                {
                    CustomSearchExpression newExpression = expression.ToEfModel();
                    newExpression.DatasetChartId = toReturn.InteractiveChartId;
                    newExpression.DatasetChart = toReturn;
                    newExpression.OwnerType = CustomSearchExpressionOwnerType.InteractiveChart.ToString();
                    toReturn.CustomSearchExpression.Add(newExpression);
                }
            }

            return toReturn;
        }
    }
}
