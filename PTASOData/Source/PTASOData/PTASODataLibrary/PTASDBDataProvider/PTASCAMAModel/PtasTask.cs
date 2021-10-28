using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTask
    {
        public PtasTask()
        {
            InversePtasParenttaskidValueNavigation = new HashSet<PtasTask>();
            InversePtasSubtaskidValueNavigation = new HashSet<PtasTask>();
        }

        public Guid PtasTaskid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccessoriesindestroyedproperty { get; set; }
        public string PtasAnticipatedrepairdates { get; set; }
        public decimal? PtasAppdeterredinvalAssessedvalImps { get; set; }
        public decimal? PtasAppdeterredinvalAssessedvalImpsBase { get; set; }
        public decimal? PtasAppdeterredinvalAssessedvalLand { get; set; }
        public decimal? PtasAppdeterredinvalAssessedvalLandBase { get; set; }
        public DateTimeOffset? PtasAppdeterredinvalCalendaryear { get; set; }
        public decimal? PtasAppdeterredinvalFullmarketvalueImps { get; set; }
        public decimal? PtasAppdeterredinvalFullmarketvalueImpsBase { get; set; }
        public decimal? PtasAppdeterredinvalFullmarketvalueLand { get; set; }
        public decimal? PtasAppdeterredinvalFullmarketvalueLandBase { get; set; }
        public int? PtasAppdeterredinvalTaxrollcollectionfor { get; set; }
        public decimal? PtasAppdeterredinvalTotalamountImps { get; set; }
        public decimal? PtasAppdeterredinvalTotalamountImpsBase { get; set; }
        public decimal? PtasAppdeterredinvalTotalamountLand { get; set; }
        public decimal? PtasAppdeterredinvalTotalamountLandBase { get; set; }
        public decimal? PtasAppraisedimprovementincrease { get; set; }
        public decimal? PtasAppraisedimprovementincreaseBase { get; set; }
        public decimal? PtasAppraisedimprovementvalue { get; set; }
        public decimal? PtasAppraisedimprovementvalueBase { get; set; }
        public decimal? PtasAppraisedlandvalue { get; set; }
        public decimal? PtasAppraisedlandvalueBase { get; set; }
        public decimal? PtasAppraisedtotalvalue { get; set; }
        public decimal? PtasAppraisedtotalvalueBase { get; set; }
        public string PtasArea { get; set; }
        public decimal? PtasAssessedvalueppmhfh { get; set; }
        public decimal? PtasAssessedvalueppmhfhBase { get; set; }
        public int? PtasChangereason { get; set; }
        public string PtasCitystatezip { get; set; }
        public string PtasClaimdisqualificationreason { get; set; }
        public string PtasClaimdisqualificationreasonvalue { get; set; }
        public int? PtasClaimqualificationreason { get; set; }
        public string PtasComments { get; set; }
        public int? PtasConvertpropertytypefrom { get; set; }
        public int? PtasConvertpropertytypeto { get; set; }
        public decimal? PtasCosttocureImps { get; set; }
        public decimal? PtasCosttocureImpsBase { get; set; }
        public decimal? PtasCosttocureLand { get; set; }
        public decimal? PtasCosttocureLandBase { get; set; }
        public DateTimeOffset? PtasDateofdestruction { get; set; }
        public DateTimeOffset? PtasDatesigned { get; set; }
        public int? PtasDayofyear { get; set; }
        public int? PtasDaysremaining { get; set; }
        public string PtasDescriptionofdestroyedproperty { get; set; }
        public string PtasDestroyedpropertydescription { get; set; }
        public int? PtasDestroyedpropertytasktypecode { get; set; }
        public string PtasDestructnumber { get; set; }
        public string PtasFolionumber { get; set; }
        public decimal? PtasFullmarketvalueppmhfh { get; set; }
        public decimal? PtasFullmarketvalueppmhfhBase { get; set; }
        public int? PtasHitasktype { get; set; }
        public decimal? PtasImprovementvalueloss { get; set; }
        public decimal? PtasImprovementvaluelossBase { get; set; }
        public decimal? PtasLandvalueloss { get; set; }
        public decimal? PtasLandvaluelossBase { get; set; }
        public string PtasLossoccurringasaresultof { get; set; }
        public string PtasLossoccurringasaresultofother { get; set; }
        public string PtasLossoccurringasaresultofvalue { get; set; }
        public string PtasMailingaddress { get; set; }
        public string PtasMailingcitystatezip { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNetconditiongoodImprovements { get; set; }
        public string PtasOthercomments { get; set; }
        public bool? PtasOverrideprorateimprovementcalculation { get; set; }
        public bool? PtasOverrideproratelandvaluecalculation { get; set; }
        public int? PtasPercentyearremaining { get; set; }
        public string PtasPermitissuedby { get; set; }
        public string PtasPhonenumber { get; set; }
        public int? PtasPostadditionalrecordtorollyear { get; set; }
        public string PtasPropertyaddress { get; set; }
        public decimal? PtasProratedlandvalue { get; set; }
        public decimal? PtasProratedlandvalueBase { get; set; }
        public decimal? PtasProratedlandvalueloss { get; set; }
        public decimal? PtasProratedlandvaluelossBase { get; set; }
        public decimal? PtasProrateimprovementvalue { get; set; }
        public decimal? PtasProrateimprovementvalueBase { get; set; }
        public decimal? PtasProrateimprovementvalueloss { get; set; }
        public decimal? PtasProrateimprovementvaluelossBase { get; set; }
        public string PtasQstr { get; set; }
        public bool? PtasRepairdatesunknownatthistime { get; set; }
        public int? PtasRevaluenoticeflag { get; set; }
        public bool? PtasReviewedbyacctsectsupervisor { get; set; }
        public bool? PtasReviewedbycommsrappraiser { get; set; }
        public bool? PtasReviewedbyressrappraiser { get; set; }
        public string PtasSdf { get; set; }
        public int? PtasSelectmethod { get; set; }
        public int? PtasSelectreason { get; set; }
        public int? PtasSignedby { get; set; }
        public string PtasSplitcode { get; set; }
        public string PtasSubarea { get; set; }
        public int? PtasSubmissionsource { get; set; }
        public string PtasTaskdescription { get; set; }
        public int? PtasTasknumber { get; set; }
        public int? PtasTasktype { get; set; }
        public decimal? PtasTaxableimprovementvalue { get; set; }
        public decimal? PtasTaxableimprovementvalueBase { get; set; }
        public decimal? PtasTaxablelandvalue { get; set; }
        public decimal? PtasTaxablelandvalueBase { get; set; }
        public decimal? PtasTaxabletotalvalue { get; set; }
        public decimal? PtasTaxabletotalvalueBase { get; set; }
        public string PtasTaxaccountcity { get; set; }
        public string PtasTaxaccountstate { get; set; }
        public string PtasTaxaccountzipcode { get; set; }
        public string PtasTaxpayername { get; set; }
        public int? PtasTaxrollyear { get; set; }
        public int? PtasTaxvaluereason { get; set; }
        public decimal? PtasTotalamountoflossppmhfh { get; set; }
        public decimal? PtasTotalamountoflossppmhfhBase { get; set; }
        public decimal? PtasTotalvalue { get; set; }
        public decimal? PtasTotalvalueBase { get; set; }
        public decimal? PtasTrueandfairvalueremainingImps { get; set; }
        public decimal? PtasTrueandfairvalueremainingImpsBase { get; set; }
        public decimal? PtasTrueandfairvalueremainingLand { get; set; }
        public decimal? PtasTrueandfairvalueremainingLandBase { get; set; }
        public string PtasZoning { get; set; }
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
        public Guid? PtasAccountingsectionsupervisorValue { get; set; }
        public Guid? PtasAppraiserValue { get; set; }
        public Guid? PtasCommercialsrappraiserValue { get; set; }
        public Guid? PtasConvertpropertytypefromidValue { get; set; }
        public Guid? PtasConvertpropertytypetoidValue { get; set; }
        public Guid? PtasHomeimprovementidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasParenttaskidValue { get; set; }
        public Guid? PtasPersonalpropertyidValue { get; set; }
        public Guid? PtasPortalcontactValue { get; set; }
        public Guid? PtasResidentialsrappraiserValue { get; set; }
        public Guid? PtasResponsibilityfromValue { get; set; }
        public Guid? PtasResponsibilitytoValue { get; set; }
        public Guid? PtasSalesidValue { get; set; }
        public Guid? PtasSubtaskidValue { get; set; }
        public Guid? PtasTaxaccountnumberValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Systemuser PtasAccountingsectionsupervisorValueNavigation { get; set; }
        public virtual Systemuser PtasAppraiserValueNavigation { get; set; }
        public virtual Systemuser PtasCommercialsrappraiserValueNavigation { get; set; }
        public virtual PtasPropertytype PtasConvertpropertytypefromidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasConvertpropertytypetoidValueNavigation { get; set; }
        public virtual PtasHomeimprovement PtasHomeimprovementidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasTask PtasParenttaskidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyidValueNavigation { get; set; }
        public virtual PtasPortalcontact PtasPortalcontactValueNavigation { get; set; }
        public virtual Systemuser PtasResidentialsrappraiserValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityfromValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilitytoValueNavigation { get; set; }
        public virtual PtasSales PtasSalesidValueNavigation { get; set; }
        public virtual PtasTask PtasSubtaskidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountnumberValueNavigation { get; set; }
        public virtual PtasCondounit PtasUnitidValueNavigation { get; set; }
        public virtual ICollection<PtasTask> InversePtasParenttaskidValueNavigation { get; set; }
        public virtual ICollection<PtasTask> InversePtasSubtaskidValueNavigation { get; set; }
    }
}
