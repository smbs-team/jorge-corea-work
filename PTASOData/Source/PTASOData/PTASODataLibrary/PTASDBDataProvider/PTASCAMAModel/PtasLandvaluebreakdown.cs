using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLandvaluebreakdown
    {
        public PtasLandvaluebreakdown()
        {
            InversePtasMasterlandvaluebreakdownidValueNavigation = new HashSet<PtasLandvaluebreakdown>();
        }

        public Guid PtasLandvaluebreakdownid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPercent { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasLandidValue { get; set; }
        public Guid? PtasMasterlandvaluebreakdownidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLand PtasLandidValueNavigation { get; set; }
        public virtual PtasLandvaluebreakdown PtasMasterlandvaluebreakdownidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> InversePtasMasterlandvaluebreakdownidValueNavigation { get; set; }
    }
}
