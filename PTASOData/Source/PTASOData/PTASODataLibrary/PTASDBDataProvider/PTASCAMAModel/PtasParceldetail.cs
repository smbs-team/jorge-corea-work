using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasParceldetail
    {
        public PtasParceldetail()
        {
            Contact = new HashSet<Contact>();
            InversePtasMasterparcelidValueNavigation = new HashSet<PtasParceldetail>();
            PtasAbstractdocument = new HashSet<PtasAbstractdocument>();
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectsourceparcel = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAccessorydetail = new HashSet<PtasAccessorydetail>();
            PtasAddresschangehistory = new HashSet<PtasAddresschangehistory>();
            PtasAnnexationparcelreview = new HashSet<PtasAnnexationparcelreview>();
            PtasAptavailablecomparablesale = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptlistedrent = new HashSet<PtasAptlistedrent>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasBookmark = new HashSet<PtasBookmark>();
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasCondocomplexPtasAssociatedparcel2idValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexPtasAssociatedparcel3idValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexPtasAssociatedparcelidValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondocomplexPtasParcelidValueNavigation = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasContaminatedlandreduction = new HashSet<PtasContaminatedlandreduction>();
            PtasCurrentuseapplicationparcelPtasCrossreferenceparcelidValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuseapplicationparcelPtasParcelValueNavigation = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentuselanduse = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentusenote = new HashSet<PtasCurrentusenote>();
            PtasCurrentusepbrsresource = new HashSet<PtasCurrentusepbrsresource>();
            PtasExemption = new HashSet<PtasExemption>();
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasHomeimprovement = new HashSet<PtasHomeimprovement>();
            PtasInspectionhistory = new HashSet<PtasInspectionhistory>();
            PtasLandvaluebreakdown = new HashSet<PtasLandvaluebreakdown>();
            PtasParceleconomicunit = new HashSet<PtasParceleconomicunit>();
            PtasParcelmetadata = new HashSet<PtasParcelmetadata>();
            PtasPaymentreceipt = new HashSet<PtasPaymentreceipt>();
            PtasPermitPtasCondounitidValueNavigation = new HashSet<PtasPermit>();
            PtasPermitPtasParcelidValueNavigation = new HashSet<PtasPermit>();
            PtasPermitinspectionhistory = new HashSet<PtasPermitinspectionhistory>();
            PtasPersonalproperty = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistory = new HashSet<PtasPersonalpropertyhistory>();
            PtasPropertyreview = new HashSet<PtasPropertyreview>();
            PtasRecentparcel = new HashSet<PtasRecentparcel>();
            PtasRefundpetition = new HashSet<PtasRefundpetition>();
            PtasSalesparcel = new HashSet<PtasSalesparcel>();
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
            PtasSeapplication = new HashSet<PtasSeapplication>();
            PtasSeappnote = new HashSet<PtasSeappnote>();
            PtasSefrozenvaluePtasParcel2idValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvaluePtasParcelidValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSketch = new HashSet<PtasSketch>();
            PtasTask = new HashSet<PtasTask>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
            PtasTaxbill = new HashSet<PtasTaxbill>();
            PtasTaxrollcorrection = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionvalue = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasValuehistory = new HashSet<PtasValuehistory>();
        }

        public Guid PtasParceldetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAccessorycount { get; set; }
        public int? PtasAccounttype { get; set; }
        public string PtasAcctnbr { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public string PtasAddress { get; set; }
        public string PtasAlternatekey { get; set; }
        public string PtasApplgroup { get; set; }
        public decimal? PtasBenefitacres { get; set; }
        public int? PtasBldgnbr { get; set; }
        public bool? PtasBothinspected { get; set; }
        public int? PtasBuildingcount { get; set; }
        public string PtasChangesource { get; set; }
        public int? PtasCommarea { get; set; }
        public int? PtasCommercialdistrict { get; set; }
        public int? PtasCommsubarea { get; set; }
        public int? PtasCondocommgridupdated { get; set; }
        public int? PtasCondocount { get; set; }
        public int? PtasCondoresgridupdated { get; set; }
        public int? PtasCurrentuse { get; set; }
        public decimal? PtasDelinquenttaxesowed { get; set; }
        public decimal? PtasDelinquenttaxesowedBase { get; set; }
        public string PtasDirectnavigation { get; set; }
        public string PtasDirsuffix { get; set; }
        public string PtasDistrict { get; set; }
        public string PtasEvncode { get; set; }
        public int? PtasFloatgridupdated { get; set; }
        public int? PtasFloatmobilecount { get; set; }
        public string PtasFolio { get; set; }
        public decimal? PtasForestfireacres { get; set; }
        public int? PtasFormhasloaded { get; set; }
        public int? PtasGeoarea { get; set; }
        public int? PtasGeoneighborhood { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistoricsite { get; set; }
        public int? PtasHistyear { get; set; }
        public int? PtasHoldoutreason { get; set; }
        public int? PtasInspectionreason { get; set; }
        public bool? PtasIsgovernmentowned { get; set; }
        public bool? PtasIslipa { get; set; }
        public bool? PtasIsmfte { get; set; }
        public bool? PtasIstribalowned { get; set; }
        public string PtasLandalternatekey { get; set; }
        public bool? PtasLandinspected { get; set; }
        public DateTimeOffset? PtasLandinspecteddate { get; set; }
        public int? PtasLandtype { get; set; }
        public string PtasLandusecode { get; set; }
        public string PtasLandusedesc { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasLegaldescription { get; set; }
        public string PtasLevycode { get; set; }
        public string PtasLinktorecordersoffice { get; set; }
        public decimal? PtasLotacreage { get; set; }
        public string PtasMajor { get; set; }
        public bool? PtasMarkfordelete { get; set; }
        public string PtasMediaguid { get; set; }
        public int? PtasMediatype { get; set; }
        public string PtasMigrationnote { get; set; }
        public string PtasMinor { get; set; }
        public int? PtasMobilegridupdated { get; set; }
        public string PtasName { get; set; }
        public string PtasNamesonaccount { get; set; }
        public string PtasNbrfraction { get; set; }
        public int? PtasNbrlivingunits { get; set; }
        public bool? PtasNeedtorevisit { get; set; }
        public int? PtasNeighborhood { get; set; }
        public string PtasNewconstrval { get; set; }
        public int? PtasNotecount { get; set; }
        public int? PtasNumberofbuildings { get; set; }
        public string PtasOtherexemptions { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public bool? PtasParcelinspected { get; set; }
        public DateTimeOffset? PtasParcelinspecteddate { get; set; }
        public string PtasParcelsnapshotnote { get; set; }
        public int? PtasParceltype { get; set; }
        public int? PtasPermitcount { get; set; }
        public string PtasPlatblock { get; set; }
        public string PtasPlatlot { get; set; }
        public string PtasPostcardsharepointurl { get; set; }
        public int? PtasProjectcount { get; set; }
        public string PtasPropertyname { get; set; }
        public string PtasProptype { get; set; }
        public int? PtasRegion { get; set; }
        public int? PtasResarea { get; set; }
        public int? PtasResidentialdistrict { get; set; }
        public int? PtasRessubarea { get; set; }
        public string PtasRpaalternatekey { get; set; }
        public int? PtasSalecount { get; set; }
        public int? PtasSalesnotecount { get; set; }
        public bool? PtasSeesenior { get; set; }
        public bool? PtasShowallunits { get; set; }
        public bool? PtasShowappraisal { get; set; }
        public bool? PtasShowbookmarks { get; set; }
        public bool? PtasShowcommercialfields { get; set; }
        public bool? PtasShowcommercialunits { get; set; }
        public bool? PtasShowdetaillist { get; set; }
        public bool? PtasShowestimate { get; set; }
        public bool? PtasShowexemptions { get; set; }
        public bool? PtasShowfloatunits { get; set; }
        public bool? PtasShowinspectionhistory { get; set; }
        public bool? PtasShowmobileunits { get; set; }
        public bool? PtasShowprojecthistory { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public bool? PtasShowresidentialfields { get; set; }
        public bool? PtasShowresidentialunits { get; set; }
        public bool? PtasShowtasks { get; set; }
        public bool? PtasShowtaxroll { get; set; }
        public int? PtasSnapshotassessmentyear { get; set; }
        public int? PtasSnapshotcount { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public string PtasSnapshoterror { get; set; }
        public int? PtasSnapshotprogress { get; set; }
        public int? PtasSnapshottype { get; set; }
        public string PtasSpacenumber { get; set; }
        public int? PtasSpecialtyarea { get; set; }
        public int? PtasSpecialtyneighborhood { get; set; }
        public string PtasSplitcode { get; set; }
        public int? PtasSqftlot { get; set; }
        public string PtasStreetname { get; set; }
        public string PtasStreetnbr { get; set; }
        public string PtasStreettype { get; set; }
        public int? PtasSupergroup { get; set; }
        public int? PtasTaxstatus { get; set; }
        public decimal? PtasTotalaccessoryvalue { get; set; }
        public decimal? PtasTotalaccessoryvalueBase { get; set; }
        public bool? PtasVacantland { get; set; }
        public DateTimeOffset? PtasWhatifsyncdate { get; set; }
        public string PtasZipcode { get; set; }
        public string PtasZoning { get; set; }
        public string PtasZoningdescription { get; set; }
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
        public Guid? PtasAbstractparcelresultidValue { get; set; }
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1StreetnameidValue { get; set; }
        public Guid? PtasAddr1StreettypeidValue { get; set; }
        public Guid? PtasAddr1ZipcodeidValue { get; set; }
        public Guid? PtasAreaidValue { get; set; }
        public Guid? PtasAssignedappraiseridValue { get; set; }
        public Guid? PtasDistrictidValue { get; set; }
        public Guid? PtasEconomicunitValue { get; set; }
        public Guid? PtasGeoareaidValue { get; set; }
        public Guid? PtasGeonbhdidValue { get; set; }
        public Guid? PtasJurisdictionValue { get; set; }
        public Guid? PtasLandidValue { get; set; }
        public Guid? PtasLandinspectedbyidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasMasterparcelidValue { get; set; }
        public Guid? PtasNeighborhoodidValue { get; set; }
        public Guid? PtasParcelinspectedbyidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasQstridValue { get; set; }
        public Guid? PtasResponsibilityidValue { get; set; }
        public Guid? PtasSaleidValue { get; set; }
        public Guid? PtasSpecialtyappraiseridValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSpecialtynbhdidValue { get; set; }
        public Guid? PtasSplitaccount1idValue { get; set; }
        public Guid? PtasSplitaccount2idValue { get; set; }
        public Guid? PtasSubareaidValue { get; set; }
        public Guid? PtasSubmarketidValue { get; set; }
        public Guid? PtasSupergroupdidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAbstractprojectresultparcel PtasAbstractparcelresultidValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1CityidValueNavigation { get; set; }
        public virtual PtasCountry PtasAddr1CountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1StateidValueNavigation { get; set; }
        public virtual PtasStreetname PtasAddr1StreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasAddr1StreettypeidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1ZipcodeidValueNavigation { get; set; }
        public virtual PtasArea PtasAreaidValueNavigation { get; set; }
        public virtual Systemuser PtasAssignedappraiseridValueNavigation { get; set; }
        public virtual PtasDistrict PtasDistrictidValueNavigation { get; set; }
        public virtual PtasEconomicunit PtasEconomicunitValueNavigation { get; set; }
        public virtual PtasGeoarea PtasGeoareaidValueNavigation { get; set; }
        public virtual PtasGeoneighborhood PtasGeonbhdidValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasJurisdictionValueNavigation { get; set; }
        public virtual PtasLand PtasLandidValueNavigation { get; set; }
        public virtual Systemuser PtasLandinspectedbyidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasMasterparcelidValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasNeighborhoodidValueNavigation { get; set; }
        public virtual Systemuser PtasParcelinspectedbyidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasQstr PtasQstridValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityidValueNavigation { get; set; }
        public virtual PtasSales PtasSaleidValueNavigation { get; set; }
        public virtual Systemuser PtasSpecialtyappraiseridValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual PtasSpecialtyneighborhood PtasSpecialtynbhdidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasSplitaccount1idValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasSplitaccount2idValueNavigation { get; set; }
        public virtual PtasSubarea PtasSubareaidValueNavigation { get; set; }
        public virtual PtasSubmarket PtasSubmarketidValueNavigation { get; set; }
        public virtual PtasSupergroup PtasSupergroupdidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<PtasParceldetail> InversePtasMasterparcelidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocument { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcel { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetail { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistory { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreview { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesale { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrent { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmark { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexPtasAssociatedparcel2idValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexPtasAssociatedparcel3idValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexPtasAssociatedparcelidValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplexPtasParcelidValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreduction { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelPtasCrossreferenceparcelidValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcelPtasParcelValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduse { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenote { get; set; }
        public virtual ICollection<PtasCurrentusepbrsresource> PtasCurrentusepbrsresource { get; set; }
        public virtual ICollection<PtasExemption> PtasExemption { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovement { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistory { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdown { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunit { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadata { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceipt { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasCondounitidValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasParcelidValueNavigation { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistory { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalproperty { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistory { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreview { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcel { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetition { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcel { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvaluePtasParcel2idValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvaluePtasParcelidValueNavigation { get; set; }
        public virtual ICollection<PtasSketch> PtasSketch { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbill { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrection { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalue { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistory { get; set; }
    }
}
