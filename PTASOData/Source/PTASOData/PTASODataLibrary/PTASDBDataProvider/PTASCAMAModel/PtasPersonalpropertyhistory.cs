using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPersonalpropertyhistory
    {
        public PtasPersonalpropertyhistory()
        {
            InversePtasOriginalppassessmenthistoryidValueNavigation = new HashSet<PtasPersonalpropertyhistory>();
            PtasAssessmentrollcorrectionNavigation = new HashSet<PtasAssessmentrollcorrection>();
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
            PtasQuickcollect = new HashSet<PtasQuickcollect>();
        }

        public Guid PtasPersonalpropertyhistoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAccountstatus { get; set; }
        public DateTimeOffset? PtasAccountstatusupdatedon { get; set; }
        public int? PtasAlternatekeyint { get; set; }
        public string PtasAlternatekeystr { get; set; }
        public int? PtasAppealauditstatus { get; set; }
        public bool? PtasAssessmentrollcorrection { get; set; }
        public string PtasAttention { get; set; }
        public DateTimeOffset? PtasAuditdate { get; set; }
        public DateTimeOffset? PtasAuditeddate { get; set; }
        public int? PtasAuditstatus { get; set; }
        public bool? PtasAutovalue { get; set; }
        public string PtasBusinessCompositeaddress { get; set; }
        public string PtasBusinessCompositeaddressOneline { get; set; }
        public int? PtasBusinessStreetdetailPostfix { get; set; }
        public int? PtasBusinessStreetdetailPrefix { get; set; }
        public string PtasBusinessdescription { get; set; }
        public string PtasBusinesshousenumber { get; set; }
        public int? PtasBusinesshousenumberdetail { get; set; }
        public int? PtasBusinesslegalentity { get; set; }
        public int? PtasBusinesslitigationpending { get; set; }
        public string PtasBusinessname { get; set; }
        public string PtasBusinessnameapt { get; set; }
        public string PtasBusinessquicknotes { get; set; }
        public string PtasBusinessubi { get; set; }
        public bool? PtasBypassminimumav { get; set; }
        public bool? PtasCalcsupplies { get; set; }
        public int? PtasDiscovered { get; set; }
        public DateTimeOffset? PtasDiscovereddate { get; set; }
        public DateTimeOffset? PtasDiscoverydate { get; set; }
        public decimal? PtasEditedav { get; set; }
        public decimal? PtasEditedavBase { get; set; }
        public decimal? PtasEditedavsupplies { get; set; }
        public decimal? PtasEditedavsuppliesBase { get; set; }
        public decimal? PtasEditedcost { get; set; }
        public decimal? PtasEditedcostBase { get; set; }
        public decimal? PtasEditedsupplies { get; set; }
        public decimal? PtasEditedsuppliesBase { get; set; }
        public bool? PtasFarmexemption { get; set; }
        public int? PtasFieldaudit { get; set; }
        public DateTimeOffset? PtasFilingdate { get; set; }
        public int? PtasFilingmethod { get; set; }
        public DateTimeOffset? PtasFilingrecevied { get; set; }
        public bool? PtasHof { get; set; }
        public string PtasHousenumber { get; set; }
        public int? PtasInhouseauditstatus { get; set; }
        public int? PtasLegacyhistid { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public decimal? PtasLevyrate { get; set; }
        public string PtasNaicsdescription { get; set; }
        public string PtasName { get; set; }
        public bool? PtasNonprofit { get; set; }
        public int? PtasPenalty { get; set; }
        public DateTimeOffset? PtasPosted { get; set; }
        public decimal? PtasPostedav { get; set; }
        public decimal? PtasPostedavBase { get; set; }
        public decimal? PtasPostedavsupplies { get; set; }
        public decimal? PtasPostedavsuppliesBase { get; set; }
        public decimal? PtasPostedcost { get; set; }
        public decimal? PtasPostedcostBase { get; set; }
        public decimal? PtasPostedsupplies { get; set; }
        public decimal? PtasPostedsuppliesBase { get; set; }
        public string PtasPreparerCompositeaddress { get; set; }
        public string PtasPreparerCompositeaddressOneline { get; set; }
        public int? PtasPreparerStreetdetailPostfix { get; set; }
        public int? PtasPreparerStreetdetailPrefix { get; set; }
        public string PtasPreparerapt { get; set; }
        public string PtasPreparerattention { get; set; }
        public string PtasPreparercellphone1 { get; set; }
        public string PtasPreparercellphone2 { get; set; }
        public string PtasPrepareremail1 { get; set; }
        public string PtasPrepareremail2 { get; set; }
        public string PtasPrepareremail3 { get; set; }
        public string PtasPrepareremail4 { get; set; }
        public string PtasPrepareremail5 { get; set; }
        public string PtasPreparerfax { get; set; }
        public string PtasPreparerhousenumber { get; set; }
        public int? PtasPreparerhousenumberdetail { get; set; }
        public string PtasPreparername { get; set; }
        public string PtasPreparerworkphone1 { get; set; }
        public string PtasPreparerworkphone1ext { get; set; }
        public string PtasPreparerworkphone2 { get; set; }
        public string PtasPreparerworkphone2ext { get; set; }
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
        public int? PtasPropertyType { get; set; }
        public string PtasTaxpayerCompositeaddress { get; set; }
        public string PtasTaxpayerCompositeaddressOneline { get; set; }
        public int? PtasTaxpayerStreetdetailPostfix { get; set; }
        public int? PtasTaxpayerStreetdetailPrefix { get; set; }
        public string PtasTaxpayeraccountnumber { get; set; }
        public string PtasTaxpayerapt { get; set; }
        public string PtasTaxpayerattention { get; set; }
        public string PtasTaxpayercellphone1 { get; set; }
        public string PtasTaxpayercellphone2 { get; set; }
        public string PtasTaxpayeremail1 { get; set; }
        public string PtasTaxpayeremail2 { get; set; }
        public string PtasTaxpayeremail3 { get; set; }
        public string PtasTaxpayeremail4 { get; set; }
        public string PtasTaxpayeremail5 { get; set; }
        public string PtasTaxpayerfax { get; set; }
        public string PtasTaxpayerhousenumber { get; set; }
        public int? PtasTaxpayerhousenumberdetail { get; set; }
        public string PtasTaxpayerworkphone1 { get; set; }
        public string PtasTaxpayerworkphone1ext { get; set; }
        public string PtasTaxpayerworkphone2 { get; set; }
        public string PtasTaxpayerworkphone2ext { get; set; }
        public bool? PtasValidtopost { get; set; }
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
        public Guid? PtasAccountmanageruseridValue { get; set; }
        public Guid? PtasAuditedbyidValue { get; set; }
        public Guid? PtasAuditedbyuseridValue { get; set; }
        public Guid? PtasBusinessStreetnameidValue { get; set; }
        public Guid? PtasBusinessStreettypeidValue { get; set; }
        public Guid? PtasBusinesscityidValue { get; set; }
        public Guid? PtasBusinessnaicscodeidValue { get; set; }
        public Guid? PtasBusinessstateincorporatedValue { get; set; }
        public Guid? PtasBusinesszipidValue { get; set; }
        public Guid? PtasDiscoveredbyValue { get; set; }
        public Guid? PtasDiscoveredbyidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasOriginalppassessmenthistoryidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasParentpersonalpropertyidValue { get; set; }
        public Guid? PtasPersonalpropertyidValue { get; set; }
        public Guid? PtasPreparerStreetnameidValue { get; set; }
        public Guid? PtasPreparerStreettypeidValue { get; set; }
        public Guid? PtasPreparercityidValue { get; set; }
        public Guid? PtasPreparercountryidValue { get; set; }
        public Guid? PtasPreparerstateidValue { get; set; }
        public Guid? PtasPreparerzipidValue { get; set; }
        public Guid? PtasQuickcollectidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxpayerStreetnameidValue { get; set; }
        public Guid? PtasTaxpayerStreettypeidValue { get; set; }
        public Guid? PtasTaxpayercityidValue { get; set; }
        public Guid? PtasTaxpayercountryidValue { get; set; }
        public Guid? PtasTaxpayerstateidValue { get; set; }
        public Guid? PtasTaxpayerzipidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Systemuser PtasAccountmanageruseridValueNavigation { get; set; }
        public virtual Systemuser PtasAuditedbyidValueNavigation { get; set; }
        public virtual Systemuser PtasAuditedbyuseridValueNavigation { get; set; }
        public virtual PtasStreetname PtasBusinessStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasBusinessStreettypeidValueNavigation { get; set; }
        public virtual PtasCity PtasBusinesscityidValueNavigation { get; set; }
        public virtual PtasNaicscode PtasBusinessnaicscodeidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasBusinessstateincorporatedValueNavigation { get; set; }
        public virtual PtasZipcode PtasBusinesszipidValueNavigation { get; set; }
        public virtual Systemuser PtasDiscoveredbyValueNavigation { get; set; }
        public virtual Systemuser PtasDiscoveredbyidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasPersonalpropertyhistory PtasOriginalppassessmenthistoryidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasParentpersonalpropertyidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyidValueNavigation { get; set; }
        public virtual PtasStreetname PtasPreparerStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasPreparerStreettypeidValueNavigation { get; set; }
        public virtual PtasCity PtasPreparercityidValueNavigation { get; set; }
        public virtual PtasCountry PtasPreparercountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasPreparerstateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasPreparerzipidValueNavigation { get; set; }
        public virtual PtasQuickcollect PtasQuickcollectidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasStreetname PtasTaxpayerStreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasTaxpayerStreettypeidValueNavigation { get; set; }
        public virtual PtasCity PtasTaxpayercityidValueNavigation { get; set; }
        public virtual PtasCountry PtasTaxpayercountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasTaxpayerstateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasTaxpayerzipidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> InversePtasOriginalppassessmenthistoryidValueNavigation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrectionNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollect { get; set; }
    }
}
