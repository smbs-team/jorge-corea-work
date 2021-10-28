using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class ExceptionPostProcessRule
    {
        public ExceptionPostProcessRule()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
        }

        public int ExceptionPostProcessRuleId { get; set; }
        public int DatasetPostProcessId { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public int ExecutionOrder { get; set; }

        public virtual DatasetPostProcess DatasetPostProcess { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
    }
}
