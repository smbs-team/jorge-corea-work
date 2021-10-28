using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasYear
    {
        public PtasYear()
        {
            PtasAbstractprojectPtasNpdEndrollyearidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectPtasNpdStartrollyearidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectPtasProjectrollyearidValueNavigation = new HashSet<PtasAbstractproject>();
            PtasAgriculturalusetype = new HashSet<PtasAgriculturalusetype>();
            PtasAnnexationparcelreview = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationtracker = new HashSet<PtasAnnexationtracker>();
            PtasAptadjustedlevyrate = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptbuildingqualityadjustment = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptcloseproximity = new HashSet<PtasAptcloseproximity>();
            PtasAptconditionadjustment = new HashSet<PtasAptconditionadjustment>();
            PtasAptestimatedunitsqft = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptexpensehighend = new HashSet<PtasAptexpensehighend>();
            PtasAptexpenseunitsize = new HashSet<PtasAptexpenseunitsize>();
            PtasAptincomeexpense = new HashSet<PtasAptincomeexpense>();
            PtasAptneighborhood = new HashSet<PtasAptneighborhood>();
            PtasAptnumberofunitsadjustment = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptpoolandelevatorexpense = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptrentmodel = new HashSet<PtasAptrentmodel>();
            PtasAptsalesmodel = new HashSet<PtasAptsalesmodel>();
            PtasApttrending = new HashSet<PtasApttrending>();
            PtasAptunittypeadjustment = new HashSet<PtasAptunittypeadjustment>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationproject = new HashSet<PtasAptvaluationproject>();
            PtasAptviewqualityadjustment = new HashSet<PtasAptviewqualityadjustment>();
            PtasAptviewrankadjustment = new HashSet<PtasAptviewrankadjustment>();
            PtasAptviewtypeadjustment = new HashSet<PtasAptviewtypeadjustment>();
            PtasAptyearbuiltadjustment = new HashSet<PtasAptyearbuiltadjustment>();
            PtasAssessmentrollcorrection = new HashSet<PtasAssessmentrollcorrection>();
            PtasBillingclassification = new HashSet<PtasBillingclassification>();
            PtasBillingcode = new HashSet<PtasBillingcode>();
            PtasBuildingdetailPtasEffectiveyearidValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailPtasYearbuiltidValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasCaprate = new HashSet<PtasCaprate>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasContaminatedlandreduction = new HashSet<PtasContaminatedlandreduction>();
            PtasCurrentusebacktaxyear = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusefarmyieldhistory = new HashSet<PtasCurrentusefarmyieldhistory>();
            PtasCurrentusevaluehistory = new HashSet<PtasCurrentusevaluehistory>();
            PtasFileattachmentmetadataPtasRollyearidValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataPtasTaxyearidValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFileattachmentmetadataPtasYearidValueNavigation = new HashSet<PtasFileattachmentmetadata>();
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
            PtasHomeimprovementPtasExemptionbeginyearidValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasHomeimprovementPtasExemptionendyearidValueNavigation = new HashSet<PtasHomeimprovement>();
            PtasLevycodechangePtasFromyearidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasToyearidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevylidliftbondPtasFirstyearidValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevylidliftbondPtasLastyearidValueNavigation = new HashSet<PtasLevylidliftbond>();
            PtasLevyordinancerequest = new HashSet<PtasLevyordinancerequest>();
            PtasMediarepository = new HashSet<PtasMediarepository>();
            PtasMedicareplan = new HashSet<PtasMedicareplan>();
            PtasOmitPtasAssessmentyearidValueNavigation = new HashSet<PtasOmit>();
            PtasOmitPtasOmittedassessmentyearidValueNavigation = new HashSet<PtasOmit>();
            PtasParceleconomicunitPtasEffectiveyearidValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasParceleconomicunitPtasYearbuiltidValueNavigation = new HashSet<PtasParceleconomicunit>();
            PtasPaymentreceipt = new HashSet<PtasPaymentreceipt>();
            PtasPermit = new HashSet<PtasPermit>();
            PtasPersonalpropertyPtasCreatedondateyearidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyPtasDiscoverydateyearidValueNavigation = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyassetPtasDepreciationstartyearidValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertyassetPtasYearacquiredidValueNavigation = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertycategory = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertyhistory = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertylisting = new HashSet<PtasPersonalpropertylisting>();
            PtasQuickcollect = new HashSet<PtasQuickcollect>();
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
            PtasRefundpetition = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionlevyrate = new HashSet<PtasRefundpetitionlevyrate>();
            PtasRestrictedrent = new HashSet<PtasRestrictedrent>();
            PtasSalesaggregatePtasYearbuiltidValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesaggregatePtasYeareffectiveidValueNavigation = new HashSet<PtasSalesaggregate>();
            PtasSalesbuildingPtasYearbuiltidValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSalesbuildingPtasYearrenovatedidValueNavigation = new HashSet<PtasSalesbuilding>();
            PtasSeappdetailPtasRollyearidValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeappdetailPtasYearidValueNavigation = new HashSet<PtasSeappdetail>();
            PtasSeapplicationPtasFrozenyearidValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeapplicationPtasTaxyearidValueNavigation = new HashSet<PtasSeapplication>();
            PtasSeeligibility = new HashSet<PtasSeeligibility>();
            PtasSefrozenvaluePtasFrozenyearidValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasSefrozenvaluePtasOriginationyearValueNavigation = new HashSet<PtasSefrozenvalue>();
            PtasTaxbill = new HashSet<PtasTaxbill>();
            PtasTaxdistrictPtasFireannexyearidValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictPtasLeviedtaxyearidValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxdistrictPtasLibraryannexeffectiveyearidValueNavigation = new HashSet<PtasTaxdistrict>();
            PtasTaxrollcorrectionvalue = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTimberusetype = new HashSet<PtasTimberusetype>();
            PtasTrendfactor = new HashSet<PtasTrendfactor>();
            PtasValuehistory = new HashSet<PtasValuehistory>();
        }

        public Guid PtasYearid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas1constitutionalcheck { get; set; }
        public DateTimeOffset? PtasAssessmentyearend { get; set; }
        public DateTimeOffset? PtasAssessmentyearstart { get; set; }
        public bool? PtasCertified { get; set; }
        public decimal? PtasConstitutionalcheck { get; set; }
        public decimal? PtasConstitutionalcheckBase { get; set; }
        public decimal? PtasCostindexadjustmentvalue { get; set; }
        public string PtasEmailrecipients { get; set; }
        public DateTimeOffset? PtasEnddate { get; set; }
        public decimal? PtasImplicitpricedeflator { get; set; }
        public decimal? PtasIntegralhomesiteimprovementsvalue { get; set; }
        public decimal? PtasIntegralhomesiteimprovementsvalueBase { get; set; }
        public decimal? PtasIntegralhomesiteperacrevalue { get; set; }
        public decimal? PtasIntegralhomesiteperacrevalueBase { get; set; }
        public bool? PtasIscurrentassessmentyear { get; set; }
        public bool? PtasIscurrentcalendaryear { get; set; }
        public bool? PtasIsnextassessmentyear { get; set; }
        public bool? PtasIspreviousassessmentyear { get; set; }
        public decimal? PtasLimitfactor { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPersonalpropertyratio { get; set; }
        public decimal? PtasRealpropertyratio { get; set; }
        public DateTimeOffset? PtasStartdate { get; set; }
        public decimal? PtasTotalfarmandagriculturalacres { get; set; }
        public decimal? PtasTotalfarmandagriculturallandvalue { get; set; }
        public decimal? PtasTotalfarmandagriculturallandvalueBase { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasRollovernotificationidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasRollovernotificationidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasNpdEndrollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasNpdStartrollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractprojectPtasProjectrollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasAgriculturalusetype> PtasAgriculturalusetype { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreview { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtracker { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrate { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustment { get; set; }
        public virtual ICollection<PtasAptcloseproximity> PtasAptcloseproximity { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustment { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqft { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighend { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsize { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpense { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhood { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustment { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpense { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodel { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodel { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrending { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustment { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationproject { get; set; }
        public virtual ICollection<PtasAptviewqualityadjustment> PtasAptviewqualityadjustment { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustment { get; set; }
        public virtual ICollection<PtasAptviewtypeadjustment> PtasAptviewtypeadjustment { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustment { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrection { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassification { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcode { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailPtasEffectiveyearidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetailPtasYearbuiltidValueNavigation { get; set; }
        public virtual ICollection<PtasCaprate> PtasCaprate { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreduction { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyear { get; set; }
        public virtual ICollection<PtasCurrentusefarmyieldhistory> PtasCurrentusefarmyieldhistory { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistory { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataPtasRollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataPtasTaxyearidValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadataPtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementPtasExemptionbeginyearidValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovementPtasExemptionendyearidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasFromyearidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasToyearidValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondPtasFirstyearidValueNavigation { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbondPtasLastyearidValueNavigation { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequest { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepository { get; set; }
        public virtual ICollection<PtasMedicareplan> PtasMedicareplan { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitPtasAssessmentyearidValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitPtasOmittedassessmentyearidValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitPtasEffectiveyearidValueNavigation { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunitPtasYearbuiltidValueNavigation { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceipt { get; set; }
        public virtual ICollection<PtasPermit> PtasPermit { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasCreatedondateyearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalpropertyPtasDiscoverydateyearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetPtasDepreciationstartyearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyassetPtasYearacquiredidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategory { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistory { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylisting { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollect { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetition { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrate { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrent { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregatePtasYearbuiltidValueNavigation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregatePtasYeareffectiveidValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingPtasYearbuiltidValueNavigation { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuildingPtasYearrenovatedidValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailPtasRollyearidValueNavigation { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetailPtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationPtasFrozenyearidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplicationPtasTaxyearidValueNavigation { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibility { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvaluePtasFrozenyearidValueNavigation { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvaluePtasOriginationyearValueNavigation { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbill { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictPtasFireannexyearidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictPtasLeviedtaxyearidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrictPtasLibraryannexeffectiveyearidValueNavigation { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalue { get; set; }
        public virtual ICollection<PtasTimberusetype> PtasTimberusetype { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactor { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistory { get; set; }
    }
}
