using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_portaladdress
    {
        public Guid ptas_portaladdressid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addresstitle { get; set; }
        public string ptas_apt { get; set; }
        public string ptas_attention { get; set; }
        public string ptas_housenumber { get; set; }
        public int? ptas_housenumberdetail { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_streetdetailpostfix { get; set; }
        public int? ptas_streetdetailprefix { get; set; }
        public string ptas_streetname { get; set; }
        public int? ptas_streettype { get; set; }
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
        public Guid? _ptas_cityid_value { get; set; }
        public Guid? _ptas_countryid_value { get; set; }
        public Guid? _ptas_portalcontactid_value { get; set; }
        public Guid? _ptas_stateid_value { get; set; }
        public Guid? _ptas_zipcodeid_value { get; set; }
    }
}
