using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_portalcontact
    {
        public ptas_portalcontact()
        {
            ptas_homeimprovement = new HashSet<ptas_homeimprovement>();
            ptas_phonenumber = new HashSet<ptas_phonenumber>();
            ptas_portalemail = new HashSet<ptas_portalemail>();
            ptas_task = new HashSet<ptas_task>();
        }

        public Guid ptas_portalcontactid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_firstname { get; set; }
        public string ptas_lastname { get; set; }
        public string ptas_middlename { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_suffix { get; set; }
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

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_homeimprovement> ptas_homeimprovement { get; set; }
        public virtual ICollection<ptas_phonenumber> ptas_phonenumber { get; set; }
        public virtual ICollection<ptas_portalemail> ptas_portalemail { get; set; }
        public virtual ICollection<ptas_task> ptas_task { get; set; }
    }
}
