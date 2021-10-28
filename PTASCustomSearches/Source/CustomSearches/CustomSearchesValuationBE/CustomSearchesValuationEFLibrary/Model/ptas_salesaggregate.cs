using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_salesaggregate
    {
        public Guid ptas_salesaggregateid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_acres { get; set; }
        public string ptas_address { get; set; }
        public int? ptas_avgsqftofunits { get; set; }
        public int? ptas_buildinggrosssqft { get; set; }
        public int? ptas_buildingnetsqft { get; set; }
        public int? ptas_buildingquality { get; set; }
        public int? ptas_constructionclass { get; set; }
        public string ptas_description { get; set; }
        public string ptas_folio { get; set; }
        public int? ptas_lotsizesqft { get; set; }
        public string ptas_major { get; set; }
        public int? ptas_maxfloors { get; set; }
        public string ptas_minor { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_noofunitssold { get; set; }
        public decimal? ptas_percentview { get; set; }
        public int? ptas_presentuse { get; set; }
        public string ptas_primarybuilding { get; set; }
        public int? ptas_projectappeal { get; set; }
        public int? ptas_projectlocation { get; set; }
        public string ptas_propertyname { get; set; }
        public decimal? ptas_sqftlotgrossbuildingarea { get; set; }
        public decimal? ptas_sqftlotnetbuildingarea { get; set; }
        public decimal? ptas_sqftlotunit { get; set; }
        public int? ptas_totalbuildings { get; set; }
        public bool? ptas_vacantland { get; set; }
        public decimal? ptas_verifiedsalepricesqftlot { get; set; }
        public bool? ptas_views { get; set; }
        public decimal? ptas_vsppergra { get; set; }
        public decimal? ptas_vsppernra { get; set; }
        public decimal? ptas_vspperunit { get; set; }
        public string ptas_zoning { get; set; }
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
        public Guid? _ptas_buildingsectionuseid_value { get; set; }
        public Guid? _ptas_districtid_value { get; set; }
        public Guid? _ptas_geoareaid_value { get; set; }
        public Guid? _ptas_geoneighborhoodid_value { get; set; }
        public Guid? _ptas_presentuseid_value { get; set; }
        public Guid? _ptas_primarybuildingid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_qstrid_value { get; set; }
        public Guid? _ptas_responsibilityid_value { get; set; }
        public Guid? _ptas_saleid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _ptas_specialtyneighborhoodid_value { get; set; }
        public Guid? _ptas_yearbuiltid_value { get; set; }
        public Guid? _ptas_yeareffectiveid_value { get; set; }
        public Guid? _ptas_zoningid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_buildingsectionuse _ptas_buildingsectionuseid_valueNavigation { get; set; }
        public virtual ptas_district _ptas_districtid_valueNavigation { get; set; }
        public virtual ptas_geoarea _ptas_geoareaid_valueNavigation { get; set; }
        public virtual ptas_geoneighborhood _ptas_geoneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_landuse _ptas_presentuseid_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_primarybuildingid_valueNavigation { get; set; }
        public virtual ptas_propertytype _ptas_propertytypeid_valueNavigation { get; set; }
        public virtual ptas_qstr _ptas_qstrid_valueNavigation { get; set; }
        public virtual ptas_responsibility _ptas_responsibilityid_valueNavigation { get; set; }
        public virtual ptas_sales _ptas_saleid_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyareaid_valueNavigation { get; set; }
        public virtual ptas_specialtyneighborhood _ptas_specialtyneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yearbuiltid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yeareffectiveid_valueNavigation { get; set; }
        public virtual ptas_zoning _ptas_zoningid_valueNavigation { get; set; }
    }
}
