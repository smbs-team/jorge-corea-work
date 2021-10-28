using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_land
    {
        public ptas_land()
        {
            Inverse_ptas_masterlandid_valueNavigation = new HashSet<ptas_land>();
            ptas_environmentalrestriction = new HashSet<ptas_environmentalrestriction>();
            ptas_landvaluebreakdown = new HashSet<ptas_landvaluebreakdown>();
            ptas_landvaluecalculation = new HashSet<ptas_landvaluecalculation>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
        }

        public Guid ptas_landid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public DateTimeOffset? ptas_asofvaluationdate { get; set; }
        public bool? ptas_autocalculate { get; set; }
        public decimal? ptas_baselandvalue { get; set; }
        public decimal? ptas_baselandvalue_base { get; set; }
        public bool? ptas_calculateexcessland { get; set; }
        public decimal? ptas_commerciallandvalue { get; set; }
        public decimal? ptas_commerciallandvalue_base { get; set; }
        public decimal? ptas_dollarspersquarefoot { get; set; }
        public decimal? ptas_dollarspersquarefoot_base { get; set; }
        public int? ptas_drysqft { get; set; }
        public int? ptas_drysqftactual { get; set; }
        public decimal? ptas_economicunitlandvalue { get; set; }
        public decimal? ptas_economicunitlandvalue_base { get; set; }
        public int? ptas_effectivesqft { get; set; }
        public bool? ptas_excessland { get; set; }
        public int? ptas_excesslandsqft { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public decimal? ptas_grosslandvalue { get; set; }
        public decimal? ptas_grosslandvalue_base { get; set; }
        public int? ptas_hbuifimproved { get; set; }
        public int? ptas_hbuifvacant { get; set; }
        public decimal? ptas_landacres { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_legacyrplandid { get; set; }
        public string ptas_name { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_parking { get; set; }
        public int? ptas_percentbaselandvalue { get; set; }
        public int? ptas_percentunusable { get; set; }
        public int? ptas_presentuse { get; set; }
        public int? ptas_requiredlbratio { get; set; }
        public int? ptas_roadaccess { get; set; }
        public int? ptas_sewersystem { get; set; }
        public bool? ptas_showlandvaluebreakdown { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public string ptas_situsaddress { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public bool? ptas_splitzoning { get; set; }
        public int? ptas_sqfttotal { get; set; }
        public int? ptas_streetsurface { get; set; }
        public int? ptas_submergedsqft { get; set; }
        public int? ptas_submergedsqftactual { get; set; }
        public int? ptas_taxyear { get; set; }
        public int? ptas_totalsitesperzoning { get; set; }
        public int? ptas_totalsqftactual { get; set; }
        public bool? ptas_unbuildable { get; set; }
        public DateTimeOffset? ptas_valuedate { get; set; }
        public int? ptas_watersystem { get; set; }
        public string ptas_zoning { get; set; }
        public DateTimeOffset? ptas_zoningchangedate { get; set; }
        public string ptas_zoningdescription { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _ownerid_value { get; set; }
        public Guid? _owningbusinessunit_value { get; set; }
        public Guid? _owningteam_value { get; set; }
        public Guid? _owninguser_value { get; set; }
        public Guid? _ptas_masterlandid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_land _ptas_masterlandid_valueNavigation { get; set; }
        public virtual ICollection<ptas_land> Inverse_ptas_masterlandid_valueNavigation { get; set; }
        public virtual ICollection<ptas_environmentalrestriction> ptas_environmentalrestriction { get; set; }
        public virtual ICollection<ptas_landvaluebreakdown> ptas_landvaluebreakdown { get; set; }
        public virtual ICollection<ptas_landvaluecalculation> ptas_landvaluecalculation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
    }
}
