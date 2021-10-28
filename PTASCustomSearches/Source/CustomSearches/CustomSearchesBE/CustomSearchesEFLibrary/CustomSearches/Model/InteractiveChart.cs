using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class InteractiveChart
    {
        public InteractiveChart()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
        }

        public int InteractiveChartId { get; set; }
        public Guid DatasetId { get; set; }
        public string ChartType { get; set; }
        public string ChartTitle { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public string ChartExtensions { get; set; }

        public virtual ChartType ChartTypeNavigation { get; set; }
        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual Dataset Dataset { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
    }
}
