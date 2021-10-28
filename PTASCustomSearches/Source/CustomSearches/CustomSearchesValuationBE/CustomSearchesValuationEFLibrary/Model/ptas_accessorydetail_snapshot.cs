using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_accessorydetail_snapshot
    {
        public Guid ptas_accessorydetailid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_accessoryvalue { get; set; }
        public decimal? ptas_accessoryvalue_base { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_alternatekey { get; set; }
        public int? ptas_buildinggrade { get; set; }
        public int? ptas_buildingquality { get; set; }
        public int? ptas_commaccessorytype { get; set; }
        public int? ptas_conditionlevel { get; set; }
        public DateTimeOffset? ptas_datevalued { get; set; }
        public string ptas_description { get; set; }
        public string ptas_directnavigation { get; set; }
        public int? ptas_effectiveyear { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public int? ptas_garagearea { get; set; }
        public int? ptas_interiorfinish { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public bool? ptas_loft { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_netconditionvalue { get; set; }
        public decimal? ptas_netconditionvalue_base { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_percentnetcondition { get; set; }
        public int? ptas_quality { get; set; }
        public int? ptas_qualitylevel { get; set; }
        public int? ptas_quantity { get; set; }
        public int? ptas_resaccessorytype { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public string ptas_situsaddress { get; set; }
        public int? ptas_size { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public int? ptas_unitofmeasure { get; set; }
        public int? ptas_walltype { get; set; }
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
        public Guid? _ptas_buildingdetailid_value { get; set; }
        public Guid? _ptas_masteraccessoryid_value { get; set; }
        public Guid? _ptas_parceldetailid_value { get; set; }
        public Guid? _ptas_projectid_value { get; set; }
        public Guid? _ptas_propertytypeid_value { get; set; }
        public Guid? _ptas_sketchid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }
    }
}
