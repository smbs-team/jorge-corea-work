using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class ProjectType
    {
        public ProjectType()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
            ProjectTypeCustomSearchDefinition = new HashSet<ProjectTypeCustomSearchDefinition>();
            UserProject = new HashSet<UserProject>();
        }

        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public string BulkUpdateProcedureName { get; set; }
        public string ApplyModelUserFilterColumnName { get; set; }
        public string EffectiveLotSizeColumnName { get; set; }
        public string DryLotSizeColumnName { get; set; }
        public string WaterFrontLotSizeColumnName { get; set; }

        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual ICollection<ProjectTypeCustomSearchDefinition> ProjectTypeCustomSearchDefinition { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
    }
}
