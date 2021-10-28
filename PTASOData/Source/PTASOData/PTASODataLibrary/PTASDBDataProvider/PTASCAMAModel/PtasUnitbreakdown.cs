using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasUnitbreakdown
    {
        public PtasUnitbreakdown()
        {
            InversePtasMasterunitbreakdownidValueNavigation = new HashSet<PtasUnitbreakdown>();
        }

        public Guid PtasUnitbreakdownid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public decimal? PtasCityparcel { get; set; }
        public string PtasDescription { get; set; }
        public string PtasDirectnavigation { get; set; }
        public decimal? PtasDnrparcel { get; set; }
        public bool? PtasFurnished { get; set; }
        public int? PtasGrosssqft { get; set; }
        public bool? PtasKitchen { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLinearft { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNbrofbedrooms { get; set; }
        public int? PtasNetsqft { get; set; }
        public decimal? PtasNumberofbathrooms { get; set; }
        public int? PtasNumberofbeds { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasQuantity { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public int? PtasSlipGrade { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasSquarefootage { get; set; }
        public decimal? PtasSubjectparcel { get; set; }
        public int? PtasTempcontrol { get; set; }
        public int? PtasUnitbreakdownroomtype { get; set; }
        public int? PtasWidth { get; set; }
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
        public Guid? PtasBuildingidValue { get; set; }
        public Guid? PtasCondocomplexidValue { get; set; }
        public Guid? PtasFloatinghomeValue { get; set; }
        public Guid? PtasMasterunitbreakdownidValue { get; set; }
        public Guid? PtasUnitbreakdowntypeidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasCondocomplexidValueNavigation { get; set; }
        public virtual PtasCondounit PtasFloatinghomeValueNavigation { get; set; }
        public virtual PtasUnitbreakdown PtasMasterunitbreakdownidValueNavigation { get; set; }
        public virtual PtasUnitbreakdowntype PtasUnitbreakdowntypeidValueNavigation { get; set; }
        public virtual ICollection<PtasUnitbreakdown> InversePtasMasterunitbreakdownidValueNavigation { get; set; }
    }
}
