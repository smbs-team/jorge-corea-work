using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingsectionfeature
    {
        public PtasBuildingsectionfeature()
        {
            InversePtasMasterfeatureidValueNavigation = new HashSet<PtasBuildingsectionfeature>();
        }

        public Guid PtasBuildingsectionfeatureid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public string PtasDirectnavigation { get; set; }
        public int? PtasFeaturegrosssqft { get; set; }
        public int? PtasFeaturenetsqft { get; set; }
        public int? PtasFeaturetype { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistyear { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
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
        public Guid? PtasBuildingValue { get; set; }
        public Guid? PtasBuildingsectionuseidValue { get; set; }
        public Guid? PtasMasterfeatureidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingValueNavigation { get; set; }
        public virtual PtasBuildingdetailCommercialuse PtasBuildingsectionuseidValueNavigation { get; set; }
        public virtual PtasBuildingsectionfeature PtasMasterfeatureidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> InversePtasMasterfeatureidValueNavigation { get; set; }
    }
}
