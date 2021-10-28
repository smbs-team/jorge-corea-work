using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_buildingdetail_commercialuse_snapshot
    {
        public Guid ptas_buildingdetail_commercialuseid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public int? ptas_anchorstore { get; set; }
        public string ptas_description { get; set; }
        public string ptas_floornumbertxt { get; set; }
        public int? ptas_grosssqft { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_netsqft { get; set; }
        public int? ptas_numberofstories { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public int? ptas_storyheight { get; set; }
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
        public Guid? _ptas_buildingsectionuseid_value { get; set; }
        public Guid? _ptas_mastersectionuseid_value { get; set; }
        public Guid? _ptas_projectid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _ptas_specialtynbhdid_value { get; set; }
        public Guid? _ptas_unitid_value { get; set; }
    }
}
