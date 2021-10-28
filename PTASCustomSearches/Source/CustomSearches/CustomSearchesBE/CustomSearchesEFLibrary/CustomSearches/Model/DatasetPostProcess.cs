using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class DatasetPostProcess
    {
        public DatasetPostProcess()
        {
            BackendUpdate = new HashSet<BackendUpdate>();
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
            DatasetPostProcessSecondaryDataset = new HashSet<DatasetPostProcessSecondaryDataset>();
            ExceptionPostProcessRule = new HashSet<ExceptionPostProcessRule>();
            InversePrimaryDatasetPostProcess = new HashSet<DatasetPostProcess>();
        }

        public int DatasetPostProcessId { get; set; }
        public Guid DatasetId { get; set; }
        public int Priority { get; set; }
        public string PostProcessType { get; set; }
        public int? RscriptModelId { get; set; }
        public string PostProcessDefinition { get; set; }
        public string ResultPayload { get; set; }
        public string PostProcessName { get; set; }
        public string PostProcessSubType { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public DateTime? LastExecutionTimestamp { get; set; }
        public bool IsDirty { get; set; }
        public string CalculatedView { get; set; }
        public string ParameterValues { get; set; }
        public string PostProcessRole { get; set; }
        public int ExecutionOrder { get; set; }
        public string TraceEnabledFields { get; set; }
        public int? PrimaryDatasetPostProcessId { get; set; }

        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual Dataset Dataset { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual PostProcessType PostProcessTypeNavigation { get; set; }
        public virtual DatasetPostProcess PrimaryDatasetPostProcess { get; set; }
        public virtual RscriptModel RscriptModel { get; set; }
        public virtual ICollection<BackendUpdate> BackendUpdate { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual ICollection<DatasetPostProcessSecondaryDataset> DatasetPostProcessSecondaryDataset { get; set; }
        public virtual ICollection<ExceptionPostProcessRule> ExceptionPostProcessRule { get; set; }
        public virtual ICollection<DatasetPostProcess> InversePrimaryDatasetPostProcess { get; set; }
    }
}
