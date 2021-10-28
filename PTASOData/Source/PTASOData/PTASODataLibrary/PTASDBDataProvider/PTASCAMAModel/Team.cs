using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Team
    {
        public Team()
        {
            Contact = new HashSet<Contact>();
            PtasAbstractdocument = new HashSet<PtasAbstractdocument>();
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAbstractprojectsourceparcel = new HashSet<PtasAbstractprojectsourceparcel>();
            PtasAccessorydetail = new HashSet<PtasAccessorydetail>();
            PtasAddresschangehistory = new HashSet<PtasAddresschangehistory>();
            PtasAnnexationparcelreview = new HashSet<PtasAnnexationparcelreview>();
            PtasAnnexationtracker = new HashSet<PtasAnnexationtracker>();
            PtasAnnualcostdistribution = new HashSet<PtasAnnualcostdistribution>();
            PtasAppeal = new HashSet<PtasAppeal>();
            PtasAptadjustedlevyrate = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptavailablecomparablesale = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptbuildingqualityadjustment = new HashSet<PtasAptbuildingqualityadjustment>();
            PtasAptcommercialincomeexpense = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcomparablerent = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablesale = new HashSet<PtasAptcomparablesale>();
            PtasAptconditionadjustment = new HashSet<PtasAptconditionadjustment>();
            PtasAptestimatedunitsqft = new HashSet<PtasAptestimatedunitsqft>();
            PtasAptexpensehighend = new HashSet<PtasAptexpensehighend>();
            PtasAptexpenseunitsize = new HashSet<PtasAptexpenseunitsize>();
            PtasAptlistedrent = new HashSet<PtasAptlistedrent>();
            PtasAptnumberofunitsadjustment = new HashSet<PtasAptnumberofunitsadjustment>();
            PtasAptpoolandelevatorexpense = new HashSet<PtasAptpoolandelevatorexpense>();
            PtasAptrentmodel = new HashSet<PtasAptrentmodel>();
            PtasAptsalesmodel = new HashSet<PtasAptsalesmodel>();
            PtasApttrending = new HashSet<PtasApttrending>();
            PtasAptunittypeadjustment = new HashSet<PtasAptunittypeadjustment>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasAptvaluationproject = new HashSet<PtasAptvaluationproject>();
            PtasAptviewrankadjustment = new HashSet<PtasAptviewrankadjustment>();
            PtasAptyearbuiltadjustment = new HashSet<PtasAptyearbuiltadjustment>();
            PtasArcreasoncode = new HashSet<PtasArcreasoncode>();
            PtasAssessmentrollcorrection = new HashSet<PtasAssessmentrollcorrection>();
            PtasBillingclassification = new HashSet<PtasBillingclassification>();
            PtasBillingcode = new HashSet<PtasBillingcode>();
            PtasBookmark = new HashSet<PtasBookmark>();
            PtasBookmarktag = new HashSet<PtasBookmarktag>();
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingsectionfeature = new HashSet<PtasBuildingsectionfeature>();
            PtasChangereason = new HashSet<PtasChangereason>();
            PtasCondocomplex = new HashSet<PtasCondocomplex>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasContaminatedlandreduction = new HashSet<PtasContaminatedlandreduction>();
            PtasContaminationproject = new HashSet<PtasContaminationproject>();
            PtasCurrentuseapplication = new HashSet<PtasCurrentuseapplication>();
            PtasCurrentuseapplicationparcel = new HashSet<PtasCurrentuseapplicationparcel>();
            PtasCurrentusebacktaxstatement = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentusebacktaxyear = new HashSet<PtasCurrentusebacktaxyear>();
            PtasCurrentusenote = new HashSet<PtasCurrentusenote>();
            PtasCurrentuseparcel2 = new HashSet<PtasCurrentuseparcel2>();
            PtasCurrentusetask = new HashSet<PtasCurrentusetask>();
            PtasCurrentusevaluehistory = new HashSet<PtasCurrentusevaluehistory>();
            PtasDepreciationtable = new HashSet<PtasDepreciationtable>();
            PtasDesignationtype = new HashSet<PtasDesignationtype>();
            PtasEconomicunit = new HashSet<PtasEconomicunit>();
            PtasEnvironmentalrestriction = new HashSet<PtasEnvironmentalrestriction>();
            PtasExemption = new HashSet<PtasExemption>();
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasFund = new HashSet<PtasFund>();
            PtasFundallocation = new HashSet<PtasFundallocation>();
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
            PtasFundtype = new HashSet<PtasFundtype>();
            PtasHomeimprovement = new HashSet<PtasHomeimprovement>();
            PtasIndustry = new HashSet<PtasIndustry>();
            PtasInspectionhistory = new HashSet<PtasInspectionhistory>();
            PtasInspectionyear = new HashSet<PtasInspectionyear>();
            PtasJurisdictioncontact = new HashSet<PtasJurisdictioncontact>();
            PtasLand = new HashSet<PtasLand>();
            PtasLandvaluebreakdown = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluecalculation = new HashSet<PtasLandvaluecalculation>();
            PtasLevycodechange = new HashSet<PtasLevycodechange>();
            PtasLevylidliftbond = new HashSet<PtasLevylidliftbond>();
            PtasLevyordinancerequest = new HashSet<PtasLevyordinancerequest>();
            PtasLowincomehousingprogram = new HashSet<PtasLowincomehousingprogram>();
            PtasMajornumberdetail = new HashSet<PtasMajornumberdetail>();
            PtasMasspayaccumulator = new HashSet<PtasMasspayaccumulator>();
            PtasMasspayaction = new HashSet<PtasMasspayaction>();
            PtasMasspayer = new HashSet<PtasMasspayer>();
            PtasMediarepository = new HashSet<PtasMediarepository>();
            PtasNotificationconfiguration = new HashSet<PtasNotificationconfiguration>();
            PtasNuisancetype = new HashSet<PtasNuisancetype>();
            PtasOmit = new HashSet<PtasOmit>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasParceleconomicunit = new HashSet<PtasParceleconomicunit>();
            PtasParcelmetadata = new HashSet<PtasParcelmetadata>();
            PtasParkingdistrict = new HashSet<PtasParkingdistrict>();
            PtasPaymentreceipt = new HashSet<PtasPaymentreceipt>();
            PtasPermit = new HashSet<PtasPermit>();
            PtasPermitinspectionhistory = new HashSet<PtasPermitinspectionhistory>();
            PtasPermitwebsiteconfig = new HashSet<PtasPermitwebsiteconfig>();
            PtasPersonalproperty = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
            PtasPersonalpropertycategory = new HashSet<PtasPersonalpropertycategory>();
            PtasPersonalpropertyhistory = new HashSet<PtasPersonalpropertyhistory>();
            PtasPersonalpropertylisting = new HashSet<PtasPersonalpropertylisting>();
            PtasPersonalpropertynote = new HashSet<PtasPersonalpropertynote>();
            PtasPerspropbannerannouncement = new HashSet<PtasPerspropbannerannouncement>();
            PtasPhonenumber = new HashSet<PtasPhonenumber>();
            PtasPortalcontact = new HashSet<PtasPortalcontact>();
            PtasPortalemail = new HashSet<PtasPortalemail>();
            PtasPropertyreview = new HashSet<PtasPropertyreview>();
            PtasPtassetting = new HashSet<PtasPtassetting>();
            PtasQuickcollect = new HashSet<PtasQuickcollect>();
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
            PtasRecentparcel = new HashSet<PtasRecentparcel>();
            PtasRefundpetition = new HashSet<PtasRefundpetition>();
            PtasRefundpetitionlevyrate = new HashSet<PtasRefundpetitionlevyrate>();
            PtasResidentialappraiserteam = new HashSet<PtasResidentialappraiserteam>();
            PtasRestrictedrent = new HashSet<PtasRestrictedrent>();
            PtasSalepriceadjustment = new HashSet<PtasSalepriceadjustment>();
            PtasSalesaccessory = new HashSet<PtasSalesaccessory>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
            PtasSalesbuilding = new HashSet<PtasSalesbuilding>();
            PtasSalesnote = new HashSet<PtasSalesnote>();
            PtasSalesparcel = new HashSet<PtasSalesparcel>();
            PtasScheduledworkflow = new HashSet<PtasScheduledworkflow>();
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
            PtasSeapplication = new HashSet<PtasSeapplication>();
            PtasSeapplicationtask = new HashSet<PtasSeapplicationtask>();
            PtasSeappnote = new HashSet<PtasSeappnote>();
            PtasSeappoccupant = new HashSet<PtasSeappoccupant>();
            PtasSeappotherprop = new HashSet<PtasSeappotherprop>();
            PtasSectionusesqft = new HashSet<PtasSectionusesqft>();
            PtasSeeligibility = new HashSet<PtasSeeligibility>();
            PtasSeeligibilitybracket = new HashSet<PtasSeeligibilitybracket>();
            PtasSefrozenvalue = new HashSet<PtasSefrozenvalue>();
            PtasSketch = new HashSet<PtasSketch>();
            PtasStateutilityvalue = new HashSet<PtasStateutilityvalue>();
            PtasTask = new HashSet<PtasTask>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
            PtasTaxbill = new HashSet<PtasTaxbill>();
            PtasTaxdistrict = new HashSet<PtasTaxdistrict>();
            PtasTaxrollcorrection = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionvalue = new HashSet<PtasTaxrollcorrectionvalue>();
            PtasTrendfactor = new HashSet<PtasTrendfactor>();
            PtasUnitbreakdown = new HashSet<PtasUnitbreakdown>();
            PtasValuehistory = new HashSet<PtasValuehistory>();
            PtasViewtype = new HashSet<PtasViewtype>();
            PtasVisitedsketch = new HashSet<PtasVisitedsketch>();
        }

        [Key]
        public Guid Teamid { get; set; }
        public Guid? Azureactivedirectoryobjectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public string Description { get; set; }
        public string Emailaddress { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public bool? Isdefault { get; set; }
        public int? Membershiptype { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public string Name { get; set; }
        public Guid? Organizationid { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public Guid? Processid { get; set; }
        public Guid? Stageid { get; set; }
        public bool? Systemmanaged { get; set; }
        public int? Teamtype { get; set; }
        public string Traversedpath { get; set; }
        public long? Versionnumber { get; set; }
        public string Yominame { get; set; }
        public Guid? AdministratoridValue { get; set; }
        public Guid? BusinessunitidValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? QueueidValue { get; set; }
        public Guid? RegardingobjectidValue { get; set; }
        public Guid? TeamtemplateidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser AdministratoridValueNavigation { get; set; }
        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<PtasAbstractdocument> PtasAbstractdocument { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasAbstractprojectsourceparcel> PtasAbstractprojectsourceparcel { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetail { get; set; }
        public virtual ICollection<PtasAddresschangehistory> PtasAddresschangehistory { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreview { get; set; }
        public virtual ICollection<PtasAnnexationtracker> PtasAnnexationtracker { get; set; }
        public virtual ICollection<PtasAnnualcostdistribution> PtasAnnualcostdistribution { get; set; }
        public virtual ICollection<PtasAppeal> PtasAppeal { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrate { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesale { get; set; }
        public virtual ICollection<PtasAptbuildingqualityadjustment> PtasAptbuildingqualityadjustment { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpense { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerent { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesale { get; set; }
        public virtual ICollection<PtasAptconditionadjustment> PtasAptconditionadjustment { get; set; }
        public virtual ICollection<PtasAptestimatedunitsqft> PtasAptestimatedunitsqft { get; set; }
        public virtual ICollection<PtasAptexpensehighend> PtasAptexpensehighend { get; set; }
        public virtual ICollection<PtasAptexpenseunitsize> PtasAptexpenseunitsize { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrent { get; set; }
        public virtual ICollection<PtasAptnumberofunitsadjustment> PtasAptnumberofunitsadjustment { get; set; }
        public virtual ICollection<PtasAptpoolandelevatorexpense> PtasAptpoolandelevatorexpense { get; set; }
        public virtual ICollection<PtasAptrentmodel> PtasAptrentmodel { get; set; }
        public virtual ICollection<PtasAptsalesmodel> PtasAptsalesmodel { get; set; }
        public virtual ICollection<PtasApttrending> PtasApttrending { get; set; }
        public virtual ICollection<PtasAptunittypeadjustment> PtasAptunittypeadjustment { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasAptvaluationproject> PtasAptvaluationproject { get; set; }
        public virtual ICollection<PtasAptviewrankadjustment> PtasAptviewrankadjustment { get; set; }
        public virtual ICollection<PtasAptyearbuiltadjustment> PtasAptyearbuiltadjustment { get; set; }
        public virtual ICollection<PtasArcreasoncode> PtasArcreasoncode { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrection { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassification { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcode { get; set; }
        public virtual ICollection<PtasBookmark> PtasBookmark { get; set; }
        public virtual ICollection<PtasBookmarktag> PtasBookmarktag { get; set; }
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeature { get; set; }
        public virtual ICollection<PtasChangereason> PtasChangereason { get; set; }
        public virtual ICollection<PtasCondocomplex> PtasCondocomplex { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasContaminatedlandreduction> PtasContaminatedlandreduction { get; set; }
        public virtual ICollection<PtasContaminationproject> PtasContaminationproject { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplication { get; set; }
        public virtual ICollection<PtasCurrentuseapplicationparcel> PtasCurrentuseapplicationparcel { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatement { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyear { get; set; }
        public virtual ICollection<PtasCurrentusenote> PtasCurrentusenote { get; set; }
        public virtual ICollection<PtasCurrentuseparcel2> PtasCurrentuseparcel2 { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetask { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistory { get; set; }
        public virtual ICollection<PtasDepreciationtable> PtasDepreciationtable { get; set; }
        public virtual ICollection<PtasDesignationtype> PtasDesignationtype { get; set; }
        public virtual ICollection<PtasEconomicunit> PtasEconomicunit { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestriction { get; set; }
        public virtual ICollection<PtasExemption> PtasExemption { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasFund> PtasFund { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
        public virtual ICollection<PtasFundtype> PtasFundtype { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovement { get; set; }
        public virtual ICollection<PtasIndustry> PtasIndustry { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistory { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyear { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontact { get; set; }
        public virtual ICollection<PtasLand> PtasLand { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdown { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechange { get; set; }
        public virtual ICollection<PtasLevylidliftbond> PtasLevylidliftbond { get; set; }
        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequest { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogram { get; set; }
        public virtual ICollection<PtasMajornumberdetail> PtasMajornumberdetail { get; set; }
        public virtual ICollection<PtasMasspayaccumulator> PtasMasspayaccumulator { get; set; }
        public virtual ICollection<PtasMasspayaction> PtasMasspayaction { get; set; }
        public virtual ICollection<PtasMasspayer> PtasMasspayer { get; set; }
        public virtual ICollection<PtasMediarepository> PtasMediarepository { get; set; }
        public virtual ICollection<PtasNotificationconfiguration> PtasNotificationconfiguration { get; set; }
        public virtual ICollection<PtasNuisancetype> PtasNuisancetype { get; set; }
        public virtual ICollection<PtasOmit> PtasOmit { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasParceleconomicunit> PtasParceleconomicunit { get; set; }
        public virtual ICollection<PtasParcelmetadata> PtasParcelmetadata { get; set; }
        public virtual ICollection<PtasParkingdistrict> PtasParkingdistrict { get; set; }
        public virtual ICollection<PtasPaymentreceipt> PtasPaymentreceipt { get; set; }
        public virtual ICollection<PtasPermit> PtasPermit { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistory { get; set; }
        public virtual ICollection<PtasPermitwebsiteconfig> PtasPermitwebsiteconfig { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalproperty { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
        public virtual ICollection<PtasPersonalpropertycategory> PtasPersonalpropertycategory { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistory { get; set; }
        public virtual ICollection<PtasPersonalpropertylisting> PtasPersonalpropertylisting { get; set; }
        public virtual ICollection<PtasPersonalpropertynote> PtasPersonalpropertynote { get; set; }
        public virtual ICollection<PtasPerspropbannerannouncement> PtasPerspropbannerannouncement { get; set; }
        public virtual ICollection<PtasPhonenumber> PtasPhonenumber { get; set; }
        public virtual ICollection<PtasPortalcontact> PtasPortalcontact { get; set; }
        public virtual ICollection<PtasPortalemail> PtasPortalemail { get; set; }
        public virtual ICollection<PtasPropertyreview> PtasPropertyreview { get; set; }
        public virtual ICollection<PtasPtassetting> PtasPtassetting { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollect { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
        public virtual ICollection<PtasRecentparcel> PtasRecentparcel { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetition { get; set; }
        public virtual ICollection<PtasRefundpetitionlevyrate> PtasRefundpetitionlevyrate { get; set; }
        public virtual ICollection<PtasResidentialappraiserteam> PtasResidentialappraiserteam { get; set; }
        public virtual ICollection<PtasRestrictedrent> PtasRestrictedrent { get; set; }
        public virtual ICollection<PtasSalepriceadjustment> PtasSalepriceadjustment { get; set; }
        public virtual ICollection<PtasSalesaccessory> PtasSalesaccessory { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuilding { get; set; }
        public virtual ICollection<PtasSalesnote> PtasSalesnote { get; set; }
        public virtual ICollection<PtasSalesparcel> PtasSalesparcel { get; set; }
        public virtual ICollection<PtasScheduledworkflow> PtasScheduledworkflow { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtask { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupant { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherprop { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqft { get; set; }
        public virtual ICollection<PtasSeeligibility> PtasSeeligibility { get; set; }
        public virtual ICollection<PtasSeeligibilitybracket> PtasSeeligibilitybracket { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalue { get; set; }
        public virtual ICollection<PtasSketch> PtasSketch { get; set; }
        public virtual ICollection<PtasStateutilityvalue> PtasStateutilityvalue { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
        public virtual ICollection<PtasTaxbill> PtasTaxbill { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrict { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrection { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalue { get; set; }
        public virtual ICollection<PtasTrendfactor> PtasTrendfactor { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdown { get; set; }
        public virtual ICollection<PtasValuehistory> PtasValuehistory { get; set; }
        public virtual ICollection<PtasViewtype> PtasViewtype { get; set; }
        public virtual ICollection<PtasVisitedsketch> PtasVisitedsketch { get; set; }
    }
}
