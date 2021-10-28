using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Dataset
    {
        public Dataset()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
            DatasetPostProcess = new HashSet<DatasetPostProcess>();
            DatasetPostProcessSecondaryDataset = new HashSet<DatasetPostProcessSecondaryDataset>();
            DatasetUserClientState = new HashSet<DatasetUserClientState>();
            InteractiveChart = new HashSet<InteractiveChart>();
            InverseSourceDataset = new HashSet<Dataset>();
            UserProjectDataset = new HashSet<UserProjectDataset>();
        }

        public Guid DatasetId { get; set; }
        public int CustomSearchDefinitionId { get; set; }
        public Guid UserId { get; set; }
        public int? ParentFolderId { get; set; }
        public string DatasetName { get; set; }
        public string ParameterValues { get; set; }
        public string GeneratedTableName { get; set; }
        public int GenerateSchemaElapsedMs { get; set; }
        public int ExecuteStoreProcedureElapsedMs { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public string DatasetClientState { get; set; }
        public string DataSetState { get; set; }
        public int? GenerateIndexesElapsedMs { get; set; }
        public int TotalRows { get; set; }
        public DateTime LastModifiedTimestamp { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastExecutionTimestamp { get; set; }
        public string DataSetPostProcessState { get; set; }
        public int? LockingJobId { get; set; }
        public Guid? SourceDatasetId { get; set; }
        public string DbLockType { get; set; }
        public string Comments { get; set; }
        public Guid? LastExecutedBy { get; set; }
        public bool? IsDataLocked { get; set; }
        public DateTime? DbLockTime { get; set; }

        public virtual Systemuser CreatedByNavigation { get; set; }
        public virtual CustomSearchDefinition CustomSearchDefinition { get; set; }
        public virtual DatasetState DataSetStateNavigation { get; set; }
        public virtual DbLockType DbLockTypeNavigation { get; set; }
        public virtual Systemuser LastExecutedByNavigation { get; set; }
        public virtual Systemuser LastModifiedByNavigation { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual Dataset SourceDataset { get; set; }
        public virtual Systemuser User { get; set; }
        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual ICollection<DatasetPostProcess> DatasetPostProcess { get; set; }
        public virtual ICollection<DatasetPostProcessSecondaryDataset> DatasetPostProcessSecondaryDataset { get; set; }
        public virtual ICollection<DatasetUserClientState> DatasetUserClientState { get; set; }
        public virtual ICollection<InteractiveChart> InteractiveChart { get; set; }
        public virtual ICollection<Dataset> InverseSourceDataset { get; set; }
        public virtual ICollection<UserProjectDataset> UserProjectDataset { get; set; }
    }
}
