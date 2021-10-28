using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class team
    {
        public team()
        {
            ptas_accessorydetail = new HashSet<ptas_accessorydetail>();
            ptas_addresschangehistory = new HashSet<ptas_addresschangehistory>();
            ptas_annexationparcelreview = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationtracker = new HashSet<ptas_annexationtracker>();
            ptas_annualcostdistribution = new HashSet<ptas_annualcostdistribution>();
            ptas_aptadjustedlevyrate = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_aptavailablecomparablesale = new HashSet<ptas_aptavailablecomparablesale>();
            ptas_aptbuildingqualityadjustment = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptcommercialincomeexpense = new HashSet<ptas_aptcommercialincomeexpense>();
            ptas_aptcomparablerent = new HashSet<ptas_aptcomparablerent>();
            ptas_aptcomparablesale = new HashSet<ptas_aptcomparablesale>();
            ptas_aptconditionadjustment = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptestimatedunitsqft = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptexpensehighend = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpenseunitsize = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptlistedrent = new HashSet<ptas_aptlistedrent>();
            ptas_aptnumberofunitsadjustment = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptpoolandelevatorexpense = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptrentmodel = new HashSet<ptas_aptrentmodel>();
            ptas_aptsalesmodel = new HashSet<ptas_aptsalesmodel>();
            ptas_apttrending = new HashSet<ptas_apttrending>();
            ptas_aptunittypeadjustment = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptvaluation = new HashSet<ptas_aptvaluation>();
            ptas_aptvaluationproject = new HashSet<ptas_aptvaluationproject>();
            ptas_aptviewrankadjustment = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptyearbuiltadjustment = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_arcreasoncode = new HashSet<ptas_arcreasoncode>();
            ptas_assessmentrollcorrection = new HashSet<ptas_assessmentrollcorrection>();
            ptas_bookmark = new HashSet<ptas_bookmark>();
            ptas_bookmarktag = new HashSet<ptas_bookmarktag>();
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_commercialuse = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingsectionfeature = new HashSet<ptas_buildingsectionfeature>();
            ptas_changereason = new HashSet<ptas_changereason>();
            ptas_condocomplex = new HashSet<ptas_condocomplex>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_contaminatedlandreduction = new HashSet<ptas_contaminatedlandreduction>();
            ptas_contaminationproject = new HashSet<ptas_contaminationproject>();
            ptas_depreciationtable = new HashSet<ptas_depreciationtable>();
            ptas_economicunit = new HashSet<ptas_economicunit>();
            ptas_environmentalrestriction = new HashSet<ptas_environmentalrestriction>();
            ptas_fileattachmentmetadata = new HashSet<ptas_fileattachmentmetadata>();
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_industry = new HashSet<ptas_industry>();
            ptas_inspectionhistory = new HashSet<ptas_inspectionhistory>();
            ptas_inspectionyear = new HashSet<ptas_inspectionyear>();
            ptas_land = new HashSet<ptas_land>();
            ptas_landvaluebreakdown = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluecalculation = new HashSet<ptas_landvaluecalculation>();
            ptas_lowincomehousingprogram = new HashSet<ptas_lowincomehousingprogram>();
            ptas_masspayaccumulator = new HashSet<ptas_masspayaccumulator>();
            ptas_masspayaction = new HashSet<ptas_masspayaction>();
            ptas_masspayer = new HashSet<ptas_masspayer>();
            ptas_mediarepository = new HashSet<ptas_mediarepository>();
            ptas_notificationconfiguration = new HashSet<ptas_notificationconfiguration>();
            ptas_omit = new HashSet<ptas_omit>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_parceleconomicunit = new HashSet<ptas_parceleconomicunit>();
            ptas_parkingdistrict = new HashSet<ptas_parkingdistrict>();
            ptas_permit = new HashSet<ptas_permit>();
            ptas_permitinspectionhistory = new HashSet<ptas_permitinspectionhistory>();
            ptas_permitwebsiteconfig = new HashSet<ptas_permitwebsiteconfig>();
            ptas_phonenumber = new HashSet<ptas_phonenumber>();
            ptas_portalcontact = new HashSet<ptas_portalcontact>();
            ptas_portalemail = new HashSet<ptas_portalemail>();
            ptas_ptassetting = new HashSet<ptas_ptassetting>();
            ptas_quickcollect = new HashSet<ptas_quickcollect>();
            ptas_recentparcel = new HashSet<ptas_recentparcel>();
            ptas_residentialappraiserteam = new HashSet<ptas_residentialappraiserteam>();
            ptas_restrictedrent = new HashSet<ptas_restrictedrent>();
            ptas_salepriceadjustment = new HashSet<ptas_salepriceadjustment>();
            ptas_salesaggregate = new HashSet<ptas_salesaggregate>();
            ptas_salesnote = new HashSet<ptas_salesnote>();
            ptas_scheduledworkflow = new HashSet<ptas_scheduledworkflow>();
            ptas_sectionusesqft = new HashSet<ptas_sectionusesqft>();
            ptas_sketch = new HashSet<ptas_sketch>();
            ptas_task = new HashSet<ptas_task>();
            ptas_taxaccount = new HashSet<ptas_taxaccount>();
            ptas_trendfactor = new HashSet<ptas_trendfactor>();
            ptas_unitbreakdown = new HashSet<ptas_unitbreakdown>();
            ptas_visitedsketch = new HashSet<ptas_visitedsketch>();
        }

        public Guid teamid { get; set; }
        public Guid? azureactivedirectoryobjectid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public string description { get; set; }
        public string emailaddress { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public bool? isdefault { get; set; }
        public int? membershiptype { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public string name { get; set; }
        public Guid? organizationid { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public Guid? processid { get; set; }
        public Guid? stageid { get; set; }
        public bool? systemmanaged { get; set; }
        public int? teamtype { get; set; }
        public string traversedpath { get; set; }
        public long? versionnumber { get; set; }
        public string yominame { get; set; }
        public Guid? _administratorid_value { get; set; }
        public Guid? _businessunitid_value { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _queueid_value { get; set; }
        public Guid? _regardingobjectid_value { get; set; }
        public Guid? _teamtemplateid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _administratorid_valueNavigation { get; set; }
        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail { get; set; }
        public virtual ICollection<ptas_addresschangehistory> ptas_addresschangehistory { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate { get; set; }
        public virtual ICollection<ptas_aptavailablecomparablesale> ptas_aptavailablecomparablesale { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment { get; set; }
        public virtual ICollection<ptas_aptcommercialincomeexpense> ptas_aptcommercialincomeexpense { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent { get; set; }
        public virtual ICollection<ptas_aptcomparablesale> ptas_aptcomparablesale { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize { get; set; }
        public virtual ICollection<ptas_aptlistedrent> ptas_aptlistedrent { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel { get; set; }
        public virtual ICollection<ptas_aptsalesmodel> ptas_aptsalesmodel { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation { get; set; }
        public virtual ICollection<ptas_aptvaluationproject> ptas_aptvaluationproject { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment { get; set; }
        public virtual ICollection<ptas_arcreasoncode> ptas_arcreasoncode { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark { get; set; }
        public virtual ICollection<ptas_bookmarktag> ptas_bookmarktag { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature { get; set; }
        public virtual ICollection<ptas_changereason> ptas_changereason { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction { get; set; }
        public virtual ICollection<ptas_contaminationproject> ptas_contaminationproject { get; set; }
        public virtual ICollection<ptas_depreciationtable> ptas_depreciationtable { get; set; }
        public virtual ICollection<ptas_economicunit> ptas_economicunit { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_industry> ptas_industry { get; set; }
        public virtual ICollection<ptas_inspectionhistory> ptas_inspectionhistory { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear { get; set; }
        public virtual ICollection<ptas_land> ptas_land { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> ptas_lowincomehousingprogram { get; set; }
        public virtual ICollection<ptas_masspayaccumulator> ptas_masspayaccumulator { get; set; }
        public virtual ICollection<ptas_masspayaction> ptas_masspayaction { get; set; }
        public virtual ICollection<ptas_masspayer> ptas_masspayer { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository { get; set; }
        public virtual ICollection<ptas_notificationconfiguration> ptas_notificationconfiguration { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit { get; set; }
        public virtual ICollection<ptas_parkingdistrict> ptas_parkingdistrict { get; set; }
        public virtual ICollection<ptas_permit> ptas_permit { get; set; }
        public virtual ICollection<ptas_permitinspectionhistory> ptas_permitinspectionhistory { get; set; }
        public virtual ICollection<ptas_permitwebsiteconfig> ptas_permitwebsiteconfig { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber { get; set; }
        public virtual ICollection<ptas_portalcontact> ptas_portalcontact { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail { get; set; }
        public virtual ICollection<ptas_ptassetting> ptas_ptassetting { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect { get; set; }
        public virtual ICollection<ptas_recentparcel> ptas_recentparcel { get; set; }
        public virtual ICollection<ptas_residentialappraiserteam> ptas_residentialappraiserteam { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent { get; set; }
        public virtual ICollection<ptas_salepriceadjustment> ptas_salepriceadjustment { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate { get; set; }
        public virtual ICollection<ptas_salesnote> ptas_salesnote { get; set; }
        public virtual ICollection<ptas_scheduledworkflow> ptas_scheduledworkflow { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch { get; set; }
        public virtual ICollection<ptas_task> ptas_task { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch { get; set; }
    }
}
