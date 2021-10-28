using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_sketch
    {
        public ptas_sketch()
        {
            Inverse_ptas_templateid_valueNavigation = new HashSet<ptas_sketch>();
            ptas_accessorydetail = new HashSet<ptas_accessorydetail>();
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_visitedsketch = new HashSet<ptas_visitedsketch>();
        }

        public Guid ptas_sketchid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_bloburi { get; set; }
        public DateTimeOffset? ptas_drawdate { get; set; }
        public bool? ptas_iscomplete { get; set; }
        public bool? ptas_isofficial { get; set; }
        public bool? ptas_locked { get; set; }
        public string ptas_name { get; set; }
        public string ptas_tags { get; set; }
        public string ptas_version { get; set; }
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
        public Guid? _ptas_accessoryid_value { get; set; }
        public Guid? _ptas_buildingid_value { get; set; }
        public Guid? _ptas_drawauthorid_value { get; set; }
        public Guid? _ptas_lockedbyid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_templateid_value { get; set; }
        public Guid? _ptas_unitid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_accessorydetail _ptas_accessoryid_valueNavigation { get; set; }
        public virtual ptas_buildingdetail _ptas_buildingid_valueNavigation { get; set; }
        public virtual systemuser _ptas_drawauthorid_valueNavigation { get; set; }
        public virtual systemuser _ptas_lockedbyid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_sketch _ptas_templateid_valueNavigation { get; set; }
        public virtual ptas_condounit _ptas_unitid_valueNavigation { get; set; }
        public virtual ICollection<ptas_sketch> Inverse_ptas_templateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_accessorydetail> ptas_accessorydetail { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_visitedsketch> ptas_visitedsketch { get; set; }
    }
}
