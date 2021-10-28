using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_bookmark
    {
        public Guid ptas_bookmarkid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public DateTimeOffset? ptas_bookmarkdate { get; set; }
        public string ptas_bookmarknote { get; set; }
        public int? ptas_bookmarktype { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_name { get; set; }
        public string ptas_tags { get; set; }
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
        public Guid? _ptas_parceldetailid_value { get; set; }
        public Guid? _ptas_tag1_value { get; set; }
        public Guid? _ptas_tag2_value { get; set; }
        public Guid? _ptas_tag3_value { get; set; }
        public Guid? _ptas_tag4_value { get; set; }
        public Guid? _ptas_tag5_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parceldetailid_valueNavigation { get; set; }
        public virtual ptas_bookmarktag _ptas_tag1_valueNavigation { get; set; }
        public virtual ptas_bookmarktag _ptas_tag2_valueNavigation { get; set; }
        public virtual ptas_bookmarktag _ptas_tag3_valueNavigation { get; set; }
        public virtual ptas_bookmarktag _ptas_tag4_valueNavigation { get; set; }
        public virtual ptas_bookmarktag _ptas_tag5_valueNavigation { get; set; }
    }
}
