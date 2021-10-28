using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPersonalproperty
    {
        public PtasPersonalproperty()
        {
            InversePtasMasterpersonalpropertyidValueNavigation = new HashSet<PtasPersonalproperty>();
            InversePtasParentaccountidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasAssessmentrollcorrection = new HashSet<PtasAssessmentrollcorrection>();
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasOmit = new HashSet<PtasOmit>();
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyhistoryPtasParentpersonalpropertyidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertyhistoryPtasPersonalpropertyidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertynote = new HashSet<PtasPersonalpropertynote>();
            PtasQuickcollectPtasPersonalpropertyidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasQuickcollectPtasQuickcollectPersonalpropertyidValueNavigation = new HashSet<PtasQuickcollect>();
            PtasTask = new HashSet<PtasTask>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
        }

        public Guid PtasPersonalpropertyid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public DateTimeOffset? PtasAccounstatusupdatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAccountstatus { get; set; }
        public string PtasAddr1Business { get; set; }
        public string PtasAddr1BusinessLine2 { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public string PtasAddr1Preparer { get; set; }
        public string PtasAddr1PreparerLine2 { get; set; }
        public int? PtasAlternatekey { get; set; }
        public int? PtasAppealauditstatus { get; set; }
        public decimal? PtasAssetestimate { get; set; }
        public decimal? PtasAssetestimateBase { get; set; }
        public DateTimeOffset? PtasAuditdate { get; set; }
        public string PtasBusinessApt { get; set; }
        public string PtasBusinessCompositeAddress { get; set; }
        public string PtasBusinessCompositeaddressOneline { get; set; }
        public string PtasBusinessHousenumber { get; set; }
        public int? PtasBusinessHousenumberdetail { get; set; }
        public int? PtasBusinessLitigationpending { get; set; }
        public string PtasBusinessQuicknotes { get; set; }
        public int? PtasBusinessStreetdetailPostfix { get; set; }
        public int? PtasBusinessStreetdetailPrefix { get; set; }
        public string PtasBusinessname { get; set; }
        public bool? PtasCheckvaluechange { get; set; }
        public bool? PtasConfirmednotpartofachain { get; set; }
        public int? PtasConfirmedopen { get; set; }
        public bool? PtasConfirmedopeninperson { get; set; }
        public bool? PtasConfirmedopenviacurrentyelpreview { get; set; }
        public bool? PtasConfirmedopenviaphone { get; set; }
        public int? PtasContactavailable { get; set; }
        public bool? PtasContactemailavailable { get; set; }
        public bool? PtasContactphoneavailable { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasDiscovered { get; set; }
        public DateTimeOffset? PtasDiscoverydate { get; set; }
        public decimal? PtasEditedav { get; set; }
        public decimal? PtasEditedavBase { get; set; }
        public decimal? PtasEditedavsupplies { get; set; }
        public decimal? PtasEditedavsuppliesBase { get; set; }
        public decimal? PtasEditedcost { get; set; }
        public decimal? PtasEditedcostBase { get; set; }
        public decimal? PtasEditedsupplies { get; set; }
        public decimal? PtasEditedsuppliesBase { get; set; }
        public string PtasElistingaccesscode { get; set; }
        public DateTimeOffset? PtasElistinginfosent { get; set; }
        public string PtasElistingurl { get; set; }
        public bool? PtasFarmexemption { get; set; }
        public int? PtasFieldaudit { get; set; }
        public DateTimeOffset? PtasFilingdate { get; set; }
        public int? PtasFilingmethod { get; set; }
        public DateTimeOffset? PtasFilingrecevied { get; set; }
        public bool? PtasHof { get; set; }
        public int? PtasInhouseauditstatus { get; set; }
        public int? PtasInhousediscovery { get; set; }
        public decimal? PtasLeaseholdimprovementscost { get; set; }
        public decimal? PtasLeaseholdimprovementscostBase { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegalentity { get; set; }
        public bool? PtasLetter { get; set; }
        public decimal? PtasLevyrate { get; set; }
        public decimal? PtasLhiestimate { get; set; }
        public decimal? PtasLhiestimateBase { get; set; }
        public bool? PtasLitigationpending { get; set; }
        public string PtasNaicsdescription { get; set; }
        public string PtasName { get; set; }
        public bool? PtasNeedtorevisit { get; set; }
        public bool? PtasNonprofit { get; set; }
        public bool? PtasNotaduplicateinpersprop { get; set; }
        public bool? PtasNotaresidentialaddresss { get; set; }
        public int? PtasNumofomityears { get; set; }
        public bool? PtasOktopostvalue { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentassessedvalue { get; set; }
        public DateTimeOffset? PtasPosted { get; set; }
        public decimal? PtasPostedav { get; set; }
        public decimal? PtasPostedavBase { get; set; }
        public decimal? PtasPostedavsupplies { get; set; }
        public decimal? PtasPostedavsuppliesBase { get; set; }
        public decimal? PtasPostedcost { get; set; }
        public decimal? PtasPostedcostBase { get; set; }
        public decimal? PtasPostedsupplies { get; set; }
        public decimal? PtasPostedsuppliesBase { get; set; }
        public decimal? PtasPostedtotalassessedvalue { get; set; }
        public decimal? PtasPostedtotalassessedvalueBase { get; set; }
        public string PtasPreparerApt { get; set; }
        public string PtasPreparerAttention { get; set; }
        public string PtasPreparerCellphone1 { get; set; }
        public string PtasPreparerCellphone2 { get; set; }
        public string PtasPreparerCompositeAddress { get; set; }
        public string PtasPreparerCompositeaddressOneline { get; set; }
        public string PtasPreparerEmail1 { get; set; }
        public string PtasPreparerEmail2 { get; set; }
        public string PtasPreparerEmail3 { get; set; }
        public string PtasPreparerEmail4 { get; set; }
        public string PtasPreparerEmail5 { get; set; }
        public string PtasPreparerFax { get; set; }
        public int? PtasPreparerHousenbrdetail { get; set; }
        public string PtasPreparerHousenumber { get; set; }
        public int? PtasPreparerStreetdetailPostfix { get; set; }
        public int? PtasPreparerStreetdetailPrefix { get; set; }
        public string PtasPreparerWorkphone1 { get; set; }
        public string PtasPreparerWorkphone1ext { get; set; }
        public string PtasPreparerWorkphone2 { get; set; }
        public string PtasPreparerWorkphone2ext { get; set; }
        public string PtasPreparername { get; set; }
        public decimal? PtasPrioryearav { get; set; }
        public decimal? PtasPrioryearavBase { get; set; }
        public decimal? PtasPrioryearavsupplies { get; set; }
        public decimal? PtasPrioryearavsuppliesBase { get; set; }
        public decimal? PtasPrioryearcost { get; set; }
        public decimal? PtasPrioryearcostBase { get; set; }
        public decimal? PtasPrioryearsupplies { get; set; }
        public decimal? PtasPrioryearsuppliesBase { get; set; }
        public int? PtasProcessstatus { get; set; }
        public DateTimeOffset? PtasProcessstatusupdatedon { get; set; }
        public int? PtasPropertytype { get; set; }
        public bool? PtasQuickcollect { get; set; }
        public string PtasQuicknotes2 { get; set; }
        public bool? PtasShowdiscoverychecklist { get; set; }
        public bool? PtasShowpreparersection { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public string PtasTaxpayerApt { get; set; }
        public string PtasTaxpayerAttention { get; set; }
        public string PtasTaxpayerCellphone1 { get; set; }
        public string PtasTaxpayerCellphone2 { get; set; }
        public string PtasTaxpayerCompositeAddress { get; set; }
        public string PtasTaxpayerCompositeaddressOneline { get; set; }
        public string PtasTaxpayerEmail1 { get; set; }
        public string PtasTaxpayerEmail2 { get; set; }
        public string PtasTaxpayerEmail3 { get; set; }
        public string PtasTaxpayerEmail4 { get; set; }
        public string PtasTaxpayerEmail5 { get; set; }
        public string PtasTaxpayerFax { get; set; }
        public string PtasTaxpayerHousenumber { get; set; }
        public int? PtasTaxpayerHousenumberdetail { get; set; }
        public int? PtasTaxpayerStreetdetailPostfix { get; set; }
        public int? PtasTaxpayerStreetdetailPrefix { get; set; }
        public string PtasTaxpayerWorkphone1 { get; set; }
        public string PtasTaxpayerWorkphone1ext { get; set; }
        public string PtasTaxpayerWorkphone2 { get; set; }
        public string PtasTaxpayerWorkphone2ext { get; set; }
        public bool? PtasTaxpayerispreparer { get; set; }
        public string PtasTaxpayername { get; set; }
        public string PtasUbi { get; set; }
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
        public Guid? PtasAccountcreatedbyidValue { get; set; }
        public Guid? PtasAccountmanageridValue { get; set; }
        public Guid? PtasAddr1BusinessCityidValue { get; set; }
        public Guid? PtasAddr1BusinessStateidValue { get; set; }
        public Guid? PtasAddr1BusinessZipcodeidValue { get; set; }
        public Guid? PtasAddr1PreparerCityidValue { get; set; }
        public Guid? PtasAddr1PreparerStateidValue { get; set; }
        public Guid? PtasAddr1PreparerZipcodeidValue { get; set; }
        public Guid? PtasAddr1TaxpayerCityidValue { get; set; }
        public Guid? PtasAddr1TaxpayerStateidValue { get; set; }
        public Guid? PtasAddr1TaxpayerZipcodeidValue { get; set; }
        public Guid? PtasAuditedbyidValue { get; set; }
        public Guid? PtasBusinessStreetnameidValue { get; set; }
        public Guid? PtasBusinessStreettypeidValue { get; set; }
        public Guid? PtasCreatedondateyearidValue { get; set; }
        public Guid? PtasDiscoveredbyidValue { get; set; }
        public Guid? PtasDiscoveryauditbyidValue { get; set; }
        public Guid? PtasDiscoverydateyearidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasMasterpersonalpropertyidValue { get; set; }
        public Guid? PtasNaicscodeidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasParentaccountidValue { get; set; }
        public Guid? PtasPreparerStreetnameidValue { get; set; }
        public Guid? PtasPreparerStreettypeidValue { get; set; }
        public Guid? PtasPreparercountryidValue { get; set; }
        public Guid? PtasStateincorporatedidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxpayerStreetnameidValue { get; set; }
        public Guid? PtasTaxpayerStreettypeidValue { get; set; }
        public Guid? PtasTaxpayercountryidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Systemuser PtasAccountcreatedbyidValueNavigation { get; set; }
        public virtual Systemuser PtasAccountmanageridValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1BusinessCityidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1BusinessStateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1BusinessZipcodeidValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1PreparerCityidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1PreparerStateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1PreparerZipcodeidValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1TaxpayerCityidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1TaxpayerStateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1TaxpayerZipcodeidValueNavigation { get; set; }
        public virtual Systemuser PtasAuditedbyidValueNavigation { get; set; }
        public virtual PtasStreetname PtasBusinessStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasBusinessStreettypeidValueNavigation { get; set; }
        public virtual PtasYear PtasCreatedondateyearidValueNavigation { get; set; }
        public virtual Systemuser PtasDiscoveredbyidValueNavigation { get; set; }
        public virtual Systemuser PtasDiscoveryauditbyidValueNavigation { get; set; }
        public virtual PtasYear PtasDiscoverydateyearidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasMasterpersonalpropertyidValueNavigation { get; set; }
        public virtual PtasNaicscode PtasNaicscodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasParentaccountidValueNavigation { get; set; }
        public virtual PtasStreetname PtasPreparerStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasPreparerStreettypeidValueNavigation { get; set; }
        public virtual PtasCountry PtasPreparercountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasStateincorporatedidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasStreetname PtasTaxpayerStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasTaxpayerStreettypeidValueNavigation { get; set; }
        public virtual PtasCountry PtasTaxpayercountryidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> InversePtasMasterpersonalpropertyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> InversePtasParentaccountidValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrection { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasOmit> PtasOmit { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasParentpersonalpropertyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistoryPtasPersonalpropertyidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynote { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasPersonalpropertyidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollectPtasQuickcollectPersonalpropertyidValueNavigation { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
    }
}
