using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_buildingsectionfeature
    {
        public ptas_buildingsectionfeature()
        {
            Inverse_ptas_masterfeatureid_valueNavigation = new HashSet<ptas_buildingsectionfeature>();
        }

        public Guid ptas_buildingsectionfeatureid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public string ptas_directnavigation { get; set; }
        public int? ptas_featuregrosssqft { get; set; }
        public int? ptas_featurenetsqft { get; set; }
        public int? ptas_featuretype { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public string ptas_name { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public bool? ptas_showrecordchanges { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
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
        public Guid? _ptas_building_value { get; set; }
        public Guid? _ptas_buildingsectionuseid_value { get; set; }
        public Guid? _ptas_masterfeatureid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_building_valueNavigation { get; set; }
        public virtual ptas_buildingdetail_commercialuse _ptas_buildingsectionuseid_valueNavigation { get; set; }
        public virtual ptas_buildingsectionfeature _ptas_masterfeatureid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingsectionfeature> Inverse_ptas_masterfeatureid_valueNavigation { get; set; }
    }
}
