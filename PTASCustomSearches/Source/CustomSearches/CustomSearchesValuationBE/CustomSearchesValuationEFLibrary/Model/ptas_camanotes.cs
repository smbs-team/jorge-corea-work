using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_camanotes
    {
        public Guid ptas_camanotesid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_attachedentitydisplayname { get; set; }
        public string ptas_attachedentitypk { get; set; }
        public string ptas_attachedentityschemaname { get; set; }
        public string ptas_excisetaxnmbr { get; set; }
        public string ptas_fullsitusaddress { get; set; }
        public bool? ptas_hasmedia { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_migratednoteid { get; set; }
        public string ptas_migratednotetype { get; set; }
        public string ptas_minornumber { get; set; }
        public string ptas_name { get; set; }
        public string ptas_notetext { get; set; }
        public int? ptas_notetype { get; set; }
        public bool? ptas_pintotop { get; set; }
        public string ptas_situsaddress { get; set; }
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
        public Guid? _ptas_minorparcelid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_propertyreviewid_value { get; set; }
        public Guid? _ptas_valuationyearid_value { get; set; }
    }
}
