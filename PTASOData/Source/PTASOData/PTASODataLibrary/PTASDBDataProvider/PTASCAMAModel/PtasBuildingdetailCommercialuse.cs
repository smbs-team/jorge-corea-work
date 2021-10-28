using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingdetailCommercialuse
    {
        public PtasBuildingdetailCommercialuse()
        {
            InversePtasMastersectionuseidValueNavigation = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasAptcommercialincomeexpense = new HashSet<PtasAptcommercialincomeexpense>();
            PtasBuildingsectionfeature = new HashSet<PtasBuildingsectionfeature>();
        }

        public Guid PtasBuildingdetailCommercialuseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAnchorstore { get; set; }
        public string PtasDescription { get; set; }
        public string PtasFloornumbertxt { get; set; }
        public int? PtasGrosssqft { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistyear { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasName { get; set; }
        public int? PtasNetsqft { get; set; }
        public int? PtasNumberofstories { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasStoryheight { get; set; }
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
        public Guid? PtasBuildingdetailidValue { get; set; }
        public Guid? PtasBuildingsectionuseidValue { get; set; }
        public Guid? PtasMastersectionuseidValue { get; set; }
        public Guid? PtasProjectidValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSpecialtynbhdidValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingdetailidValueNavigation { get; set; }
        public virtual PtasBuildingsectionuse PtasBuildingsectionuseidValueNavigation { get; set; }
        public virtual PtasBuildingdetailCommercialuse PtasMastersectionuseidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasProjectidValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual PtasSpecialtyneighborhood PtasSpecialtynbhdidValueNavigation { get; set; }
        public virtual PtasCondounit PtasUnitidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> InversePtasMastersectionuseidValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpense { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeature { get; set; }
    }
}
