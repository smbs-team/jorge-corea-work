using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class CustomSearchDefinition
    {
        public CustomSearchDefinition()
        {
            CustomSearchBackendEntity = new HashSet<CustomSearchBackendEntity>();
            CustomSearchCategoryDefinition = new HashSet<CustomSearchCategoryDefinition>();
            CustomSearchChartTemplate = new HashSet<CustomSearchChartTemplate>();
            CustomSearchColumnDefinition = new HashSet<CustomSearchColumnDefinition>();
            CustomSearchParameter = new HashSet<CustomSearchParameter>();
            CustomSearchValidationRule = new HashSet<CustomSearchValidationRule>();
            Dataset = new HashSet<Dataset>();
            ProjectTypeCustomSearchDefinition = new HashSet<ProjectTypeCustomSearchDefinition>();
        }

        public int CustomSearchDefinitionId { get; set; }
        public string CustomSearchName { get; set; }
        public string CustomSearchDescription { get; set; }
        public string StoredProcedureName { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public bool? Validated { get; set; }
        public string ExecutionRoles { get; set; }
        public string DatasetEditRoles { get; set; }
        public string RowLevelEditRolesColumn { get; set; }
        public string TableInputParameterDbType { get; set; }
        public string TableInputParameterName { get; set; }
        public int Version { get; set; }

        public virtual ICollection<CustomSearchBackendEntity> CustomSearchBackendEntity { get; set; }
        public virtual ICollection<CustomSearchCategoryDefinition> CustomSearchCategoryDefinition { get; set; }
        public virtual ICollection<CustomSearchChartTemplate> CustomSearchChartTemplate { get; set; }
        public virtual ICollection<CustomSearchColumnDefinition> CustomSearchColumnDefinition { get; set; }
        public virtual ICollection<CustomSearchParameter> CustomSearchParameter { get; set; }
        public virtual ICollection<CustomSearchValidationRule> CustomSearchValidationRule { get; set; }
        public virtual ICollection<Dataset> Dataset { get; set; }
        public virtual ICollection<ProjectTypeCustomSearchDefinition> ProjectTypeCustomSearchDefinition { get; set; }
    }
}
