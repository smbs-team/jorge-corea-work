using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_mediarepository
    {
        public Guid ptas_mediarepositoryid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_accessoryguid { get; set; }
        public string ptas_blobpath { get; set; }
        public string ptas_buildingguid { get; set; }
        public string ptas_description { get; set; }
        public string ptas_fileextension { get; set; }
        public int? ptas_imagetype { get; set; }
        public string ptas_landguid { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public DateTimeOffset? ptas_mediadate { get; set; }
        public int? ptas_mediatype { get; set; }
        public int? ptas_mediauploaded { get; set; }
        public string ptas_migrationnote { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_order { get; set; }
        public string ptas_permitguid { get; set; }
        public bool? ptas_posttoweb { get; set; }
        public bool? ptas_primary { get; set; }
        public string ptas_primaryentity { get; set; }
        public string ptas_primaryguid { get; set; }
        public int? ptas_relatedobjectmediatype { get; set; }
        public string ptas_rootfolder { get; set; }
        public string ptas_unitguid { get; set; }
        public string ptas_updatedbyuser { get; set; }
        public int? ptas_usagetype { get; set; }
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
        public Guid? _ptas_saleid_value { get; set; }
        public Guid? _ptas_yearid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_sales _ptas_saleid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yearid_valueNavigation { get; set; }
    }
}
