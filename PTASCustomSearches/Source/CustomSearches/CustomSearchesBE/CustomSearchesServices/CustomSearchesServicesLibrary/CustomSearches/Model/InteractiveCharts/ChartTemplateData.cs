namespace CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;

    /// <summary>
    /// Model for the chart template data.
    /// </summary>
    public class ChartTemplateData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTemplateData"/> class.
        /// </summary>
        public ChartTemplateData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTemplateData"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public ChartTemplateData(ChartTemplate chart, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.ChartTemplateId = chart.ChartTemplateId;
            this.ChartType = chart.ChartType;
            this.ChartTitle = chart.ChartTitle;
            this.CreatedBy = chart.CreatedBy;
            this.LastModifiedBy = chart.LastModifiedBy;
            this.CreatedTimestamp = chart.CreatedTimestamp;
            this.LastModifiedTimestamp = chart.LastModifiedTimestamp;

            if (chart.CustomSearchChartTemplate != null &&
                chart.CustomSearchChartTemplate.Count > 0)
            {
                List<string> customSearchList = new List<string>();
                foreach (var customSearch in chart.CustomSearchChartTemplate)
                {
                    if (customSearch.CustomSearchDefinition != null)
                    {
                        customSearchList.Add(customSearch.CustomSearchDefinition.CustomSearchName);
                    }
                }

                this.CustomSearches = customSearchList.ToArray();
            }

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
        /// Gets or sets the chart template identifier.
        /// </summary>
        public int ChartTemplateId { get; set; }

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
        /// Gets or sets the chart expressions.
        /// </summary>
        public CustomSearchExpressionData[] ChartExpressions { get; set; }

        /// <summary>
        /// Gets or sets the custom searches.
        /// </summary>
        public string[] CustomSearches { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>The entity framework model.</returns>
        public ChartTemplate ToEfModel()
        {
            var toReturn = new ChartTemplate()
            {
                ChartTemplateId = this.ChartTemplateId,
                ChartType = this.ChartType?.Trim(),
                ChartTitle = this.ChartTitle?.Trim(),
                CreatedBy = this.CreatedBy,
                LastModifiedBy = this.LastModifiedBy,
                CreatedTimestamp = this.CreatedTimestamp,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
            };

            if (this.ChartExpressions != null)
            {
                toReturn.CustomSearchExpression = new List<CustomSearchExpression>();
                foreach (var expression in this.ChartExpressions)
                {
                    CustomSearchExpression newExpression = expression.ToEfModel();
                    newExpression.ChartTemplateId = toReturn.ChartTemplateId;
                    newExpression.ChartTemplate = toReturn;
                    newExpression.OwnerType = CustomSearchExpressionOwnerType.ChartTemplate.ToString();
                    toReturn.CustomSearchExpression.Add(newExpression);
                }
            }

            return toReturn;
        }
    }
}
