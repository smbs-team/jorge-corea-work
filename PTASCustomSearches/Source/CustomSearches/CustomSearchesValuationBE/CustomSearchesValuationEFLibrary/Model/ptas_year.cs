using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_year
    {
        public ptas_year()
        {
            ptas_annexationparcelreview = new HashSet<ptas_annexationparcelreview>();
            ptas_annexationtracker = new HashSet<ptas_annexationtracker>();
            ptas_aptbuildingqualityadjustment = new HashSet<ptas_aptbuildingqualityadjustment>();
            ptas_aptcloseproximity = new HashSet<ptas_aptcloseproximity>();
            ptas_aptconditionadjustment = new HashSet<ptas_aptconditionadjustment>();
            ptas_aptestimatedunitsqft = new HashSet<ptas_aptestimatedunitsqft>();
            ptas_aptexpensehighend = new HashSet<ptas_aptexpensehighend>();
            ptas_aptexpenseunitsize = new HashSet<ptas_aptexpenseunitsize>();
            ptas_aptincomeexpense = new HashSet<ptas_aptincomeexpense>();
            ptas_aptneighborhood = new HashSet<ptas_aptneighborhood>();
            ptas_aptnumberofunitsadjustment = new HashSet<ptas_aptnumberofunitsadjustment>();
            ptas_aptpoolandelevatorexpense = new HashSet<ptas_aptpoolandelevatorexpense>();
            ptas_aptrentmodel = new HashSet<ptas_aptrentmodel>();
            ptas_apttrending = new HashSet<ptas_apttrending>();
            ptas_aptunittypeadjustment = new HashSet<ptas_aptunittypeadjustment>();
            ptas_aptviewqualityadjustment = new HashSet<ptas_aptviewqualityadjustment>();
            ptas_aptviewrankadjustment = new HashSet<ptas_aptviewrankadjustment>();
            ptas_aptviewtypeadjustment = new HashSet<ptas_aptviewtypeadjustment>();
            ptas_aptyearbuiltadjustment = new HashSet<ptas_aptyearbuiltadjustment>();
            ptas_assessmentrollcorrection = new HashSet<ptas_assessmentrollcorrection>();
            ptas_buildingdetail_ptas_effectiveyearid_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_ptas_yearbuiltid_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_contaminatedlandreduction = new HashSet<ptas_contaminatedlandreduction>();
            ptas_fileattachmentmetadata_ptas_rollyearid_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_ptas_taxyearid_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_fileattachmentmetadata_ptas_yearid_valueNavigation = new HashSet<ptas_fileattachmentmetadata>();
            ptas_homeimprovement_ptas_exemptionbeginyearid_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_homeimprovement_ptas_exemptionendyearid_valueNavigation = new HashSet<ptas_homeimprovement>();
            ptas_mediarepository = new HashSet<ptas_mediarepository>();
            ptas_omit_ptas_assessmentyearid_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_ptas_omittedassessmentyearid_valueNavigation = new HashSet<ptas_omit>();
            ptas_parceleconomicunit_ptas_effectiveyearid_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_parceleconomicunit_ptas_yearbuiltid_valueNavigation = new HashSet<ptas_parceleconomicunit>();
            ptas_quickcollect = new HashSet<ptas_quickcollect>();
            ptas_restrictedrent = new HashSet<ptas_restrictedrent>();
            ptas_salesaggregate_ptas_yearbuiltid_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_salesaggregate_ptas_yeareffectiveid_valueNavigation = new HashSet<ptas_salesaggregate>();
            ptas_trendfactor = new HashSet<ptas_trendfactor>();
        }

        public Guid ptas_yearid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_1constitutionalcheck { get; set; }
        public DateTimeOffset? ptas_assessmentyearend { get; set; }
        public DateTimeOffset? ptas_assessmentyearstart { get; set; }
        public bool? ptas_certified { get; set; }
        public decimal? ptas_constitutionalcheck { get; set; }
        public decimal? ptas_constitutionalcheck_base { get; set; }
        public decimal? ptas_costindexadjustmentvalue { get; set; }
        public string ptas_emailrecipients { get; set; }
        public DateTimeOffset? ptas_enddate { get; set; }
        public decimal? ptas_implicitpricedeflator { get; set; }
        public decimal? ptas_integralhomesiteimprovementsvalue { get; set; }
        public decimal? ptas_integralhomesiteimprovementsvalue_base { get; set; }
        public decimal? ptas_integralhomesiteperacrevalue { get; set; }
        public decimal? ptas_integralhomesiteperacrevalue_base { get; set; }
        public bool? ptas_iscurrentassessmentyear { get; set; }
        public bool? ptas_iscurrentcalendaryear { get; set; }
        public decimal? ptas_limitfactor { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_personalpropertyratio { get; set; }
        public decimal? ptas_realpropertyratio { get; set; }
        public DateTimeOffset? ptas_startdate { get; set; }
        public decimal? ptas_totalfarmandagriculturalacres { get; set; }
        public decimal? ptas_totalfarmandagriculturallandvalue { get; set; }
        public decimal? ptas_totalfarmandagriculturallandvalue_base { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _organizationid_value { get; set; }
        public Guid? _ptas_rollovernotificationid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _ptas_rollovernotificationid_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
        public virtual ICollection<ptas_annexationtracker> ptas_annexationtracker { get; set; }
        public virtual ICollection<ptas_aptbuildingqualityadjustment> ptas_aptbuildingqualityadjustment { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity { get; set; }
        public virtual ICollection<ptas_aptconditionadjustment> ptas_aptconditionadjustment { get; set; }
        public virtual ICollection<ptas_aptestimatedunitsqft> ptas_aptestimatedunitsqft { get; set; }
        public virtual ICollection<ptas_aptexpensehighend> ptas_aptexpensehighend { get; set; }
        public virtual ICollection<ptas_aptexpenseunitsize> ptas_aptexpenseunitsize { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense { get; set; }
        public virtual ICollection<ptas_aptneighborhood> ptas_aptneighborhood { get; set; }
        public virtual ICollection<ptas_aptnumberofunitsadjustment> ptas_aptnumberofunitsadjustment { get; set; }
        public virtual ICollection<ptas_aptpoolandelevatorexpense> ptas_aptpoolandelevatorexpense { get; set; }
        public virtual ICollection<ptas_aptrentmodel> ptas_aptrentmodel { get; set; }
        public virtual ICollection<ptas_apttrending> ptas_apttrending { get; set; }
        public virtual ICollection<ptas_aptunittypeadjustment> ptas_aptunittypeadjustment { get; set; }
        public virtual ICollection<ptas_aptviewqualityadjustment> ptas_aptviewqualityadjustment { get; set; }
        public virtual ICollection<ptas_aptviewrankadjustment> ptas_aptviewrankadjustment { get; set; }
        public virtual ICollection<ptas_aptviewtypeadjustment> ptas_aptviewtypeadjustment { get; set; }
        public virtual ICollection<ptas_aptyearbuiltadjustment> ptas_aptyearbuiltadjustment { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_ptas_effectiveyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail_ptas_yearbuiltid_valueNavigation { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_ptas_rollyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_ptas_taxyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata_ptas_yearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_ptas_exemptionbeginyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement_ptas_exemptionendyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_mediarepository> ptas_mediarepository { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_ptas_assessmentyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_ptas_omittedassessmentyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_ptas_effectiveyearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit_ptas_yearbuiltid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect { get; set; }
        public virtual ICollection<ptas_restrictedrent> ptas_restrictedrent { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_ptas_yearbuiltid_valueNavigation { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate_ptas_yeareffectiveid_valueNavigation { get; set; }
        public virtual ICollection<ptas_trendfactor> ptas_trendfactor { get; set; }
    }
}
