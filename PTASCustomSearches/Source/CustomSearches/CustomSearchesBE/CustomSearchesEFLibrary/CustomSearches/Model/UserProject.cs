using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class UserProject
    {
        public UserProject()
        {
            InverseRootVersionUserProject = new HashSet<UserProject>();
            UserProjectDataset = new HashSet<UserProjectDataset>();
        }

        public int UserProjectId { get; set; }
        public int VersionNumber { get; set; }
        public string ProjectName { get; set; }
        public string Comments { get; set; }
        public int AssessmentYear { get; set; }
        public DateTime AssessmentDateFrom { get; set; }
        public DateTime AssessmentDateTo { get; set; }
        public string SelectedAreas { get; set; }
        public int? RootVersionUserProjectId { get; set; }
        public Guid UserId { get; set; }
        public int ProjectTypeId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public bool IsLocked { get; set; }
        public string SplitModelProperty { get; set; }
        public string SplitModelValue { get; set; }
        public int ModelArea { get; set; }
        public bool IsFrozen { get; set; }
        public string VersionType { get; set; }

        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        public virtual UserProject RootVersionUserProject { get; set; }
        public virtual Systemuser User { get; set; }
        public virtual ProjectVersionType VersionTypeNavigation { get; set; }
        public virtual ICollection<UserProject> InverseRootVersionUserProject { get; set; }
        public virtual ICollection<UserProjectDataset> UserProjectDataset { get; set; }
    }
}
