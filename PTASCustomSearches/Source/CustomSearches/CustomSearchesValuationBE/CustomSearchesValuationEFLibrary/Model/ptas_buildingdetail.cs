using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_buildingdetail
    {
        public ptas_buildingdetail()
        {
            Inverse_ptas_masterbuildingid_valueNavigation = new HashSet<ptas_buildingdetail>();
            ptas_accessorydetail = new HashSet<ptas_accessorydetail>();
            ptas_buildingdetail_commercialuse = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_buildingsectionfeature = new HashSet<ptas_buildingsectionfeature>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_salesaggregate = new HashSet<ptas_salesaggregate>();
            ptas_sketch = new HashSet<ptas_sketch>();
            ptas_unitbreakdown = new HashSet<ptas_unitbreakdown>();
        }

        public Guid ptas_buildingdetailid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_12baths { get; set; }
        public int? ptas_1stflr_sqft { get; set; }
        public int? ptas_2ndflr_sqft { get; set; }
        public int? ptas_34baths { get; set; }
        public decimal? ptas_additionalcost { get; set; }
        public decimal? ptas_additionalcost_base { get; set; }
        public int? ptas_additionallivingquarters { get; set; }
        public int? ptas_addl_fireplace { get; set; }
        public string ptas_addr1_compositeaddress { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_addr1_directionprefix { get; set; }
        public int? ptas_addr1_directionsuffix { get; set; }
        public string ptas_addr1_line2 { get; set; }
        public string ptas_addr1_streetnumber { get; set; }
        public string ptas_addr1_streetnumberfraction { get; set; }
        public int? ptas_alternatekey { get; set; }
        public int? ptas_attachedgarage_sqft { get; set; }
        public int? ptas_basementgarage_sqft { get; set; }
        public int? ptas_bedroomnbr { get; set; }
        public string ptas_buildingdescription { get; set; }
        public int? ptas_buildinggrade { get; set; }
        public int? ptas_buildinggross_sqft { get; set; }
        public int? ptas_buildingnbr { get; set; }
        public int? ptas_buildingnet_sqft { get; set; }
        public string ptas_buildingnumber { get; set; }
        public int? ptas_buildingobsolescence { get; set; }
        public int? ptas_buildingquality { get; set; }
        public bool? ptas_builtgreen { get; set; }
        public int? ptas_clearheightft { get; set; }
        public int? ptas_constructionclass { get; set; }
        public bool? ptas_cranecapacity { get; set; }
        public bool? ptas_craneloading { get; set; }
        public bool? ptas_daylightbasement { get; set; }
        public int? ptas_deck_sqft { get; set; }
        public bool? ptas_dockloading { get; set; }
        public int? ptas_effectiveyear { get; set; }
        public bool? ptas_elevators { get; set; }
        public int? ptas_enclosedporch_sqft { get; set; }
        public int? ptas_energyrating { get; set; }
        public int? ptas_finbsmt_sqft { get; set; }
        public int? ptas_fr_std_fireplace { get; set; }
        public int? ptas_fullbathnbr { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public int? ptas_gradevariance { get; set; }
        public int? ptas_halfflr_sqft { get; set; }
        public int? ptas_heatingsystem { get; set; }
        public bool? ptas_invertedfloorplan { get; set; }
        public bool? ptas_leaseholdbuildingm1 { get; set; }
        public int? ptas_leasingclass { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_migrationnote { get; set; }
        public int? ptas_multi_fireplace { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_numberofaggregate { get; set; }
        public int? ptas_numberofcommonwalls { get; set; }
        public int? ptas_numberofelevators { get; set; }
        public int? ptas_numberofstories { get; set; }
        public decimal? ptas_numberofstoriesdecimal { get; set; }
        public int? ptas_onsiteparking { get; set; }
        public int? ptas_openporch_sqft { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_percentbrickorstone { get; set; }
        public int? ptas_percentcomplete { get; set; }
        public int? ptas_percentnetcondition { get; set; }
        public int? ptas_res_basementgrade { get; set; }
        public int? ptas_res_buildingcondition { get; set; }
        public int? ptas_res_heatsource { get; set; }
        public int? ptas_residentialheatingsystem { get; set; }
        public bool? ptas_restored { get; set; }
        public bool? ptas_rooftopdeck { get; set; }
        public int? ptas_shape { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public int? ptas_single_fireplace { get; set; }
        public string ptas_situsaddress { get; set; }
        public int? ptas_slabthickness { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public bool? ptas_solarpanels { get; set; }
        public bool? ptas_sprinklers { get; set; }
        public int? ptas_style { get; set; }
        public int? ptas_totalbsmt_sqft { get; set; }
        public int? ptas_totalliving_sqft { get; set; }
        public int? ptas_unfinished_full_sqft { get; set; }
        public int? ptas_unfinished_half_sqft { get; set; }
        public string ptas_unitdescription { get; set; }
        public int? ptas_units { get; set; }
        public int? ptas_upperflr_sqft { get; set; }
        public int? ptas_viewutilizationrating { get; set; }
        public int? ptas_yearbuilt { get; set; }
        public int? ptas_yearrenovated { get; set; }
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
        public Guid? _ptas_addr1_cityid_value { get; set; }
        public Guid? _ptas_addr1_countryid_value { get; set; }
        public Guid? _ptas_addr1_stateid_value { get; set; }
        public Guid? _ptas_addr1_streetnameid_value { get; set; }
        public Guid? _ptas_addr1_streettypeid_value { get; set; }
        public Guid? _ptas_addr1_zipcodeid_value { get; set; }
        public Guid? _ptas_buildingsectionuseid_value { get; set; }
        public Guid? _ptas_effectiveyearid_value { get; set; }
        public Guid? _ptas_masterbuildingid_value { get; set; }
        public Guid? _ptas_parceldetailid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_sketchid_value { get; set; }
        public Guid? _ptas_taxaccount_value { get; set; }
        public Guid? _ptas_yearbuiltid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_city _ptas_addr1_cityid_valueNavigation { get; set; }
        public virtual ptas_country _ptas_addr1_countryid_valueNavigation { get; set; }
        public virtual ptas_stateorprovince _ptas_addr1_stateid_valueNavigation { get; set; }
        public virtual ptas_streetname _ptas_addr1_streetnameid_valueNavigation { get; set; }
        public virtual ptas_streettype _ptas_addr1_streettypeid_valueNavigation { get; set; }
        public virtual ptas_zipcode _ptas_addr1_zipcodeid_valueNavigation { get; set; }
        public virtual ptas_buildingsectionuse _ptas_buildingsectionuseid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_effectiveyearid_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_masterbuildingid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parceldetailid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_sketch _ptas_sketchid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccount_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yearbuiltid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> Inverse_ptas_masterbuildingid_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> ptas_buildingsectionfeature { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate { get; set; }
        public virtual ICollection<ptas_sketch> ptas_sketch { get; set; }
        public virtual ICollection<ptas_unitbreakdown> ptas_unitbreakdown { get; set; }
    }
}
