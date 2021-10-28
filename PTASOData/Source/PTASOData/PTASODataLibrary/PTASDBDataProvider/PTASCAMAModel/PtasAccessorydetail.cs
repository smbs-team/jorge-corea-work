using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAccessorydetail
    {
        public PtasAccessorydetail()
        {
            InversePtasMasteraccessoryidValueNavigation = new HashSet<PtasAccessorydetail>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasSalesaccessory = new HashSet<PtasSalesaccessory>();
            PtasSketch = new HashSet<PtasSketch>();
        }

        public Guid PtasAccessorydetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAccessoryvalue { get; set; }
        public decimal? PtasAccessoryvalueBase { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAlternatekey { get; set; }
        public int? PtasBuildinggrade { get; set; }
        public int? PtasBuildingquality { get; set; }
        public int? PtasCommaccessorytype { get; set; }
        public int? PtasConditionlevel { get; set; }
        public DateTimeOffset? PtasDatevalued { get; set; }
        public string PtasDescription { get; set; }
        public string PtasDirectnavigation { get; set; }
        public int? PtasEffectiveyear { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public int? PtasGaragearea { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistyear { get; set; }
        public int? PtasInteriorfinish { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public bool? PtasLoft { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNetconditionvalue { get; set; }
        public decimal? PtasNetconditionvalueBase { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentnetcondition { get; set; }
        public int? PtasQuality { get; set; }
        public int? PtasQualitylevel { get; set; }
        public int? PtasQuantity { get; set; }
        public int? PtasResaccessorytype { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public string PtasSitusaddress { get; set; }
        public int? PtasSize { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasUnitofmeasure { get; set; }
        public int? PtasWalltype { get; set; }
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
        public Guid? PtasMasteraccessoryidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasProjectidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasSketchidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingdetailidValueNavigation { get; set; }
        public virtual PtasAccessorydetail PtasMasteraccessoryidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasProjectidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasSketch PtasSketchidValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> InversePtasMasteraccessoryidValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessory { get; set; }
        public virtual ICollection<PtasSketch> PtasSketch { get; set; }
    }
}
