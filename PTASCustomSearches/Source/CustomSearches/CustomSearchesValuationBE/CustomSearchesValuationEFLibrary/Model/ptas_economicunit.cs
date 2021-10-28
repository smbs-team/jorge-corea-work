using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_economicunit
    {
        public ptas_economicunit()
        {
            ptas_aptvaluation = new HashSet<ptas_aptvaluation>();
            ptas_condocomplex = new HashSet<ptas_condocomplex>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_parceleconomicunit = new HashSet<ptas_parceleconomicunit>();
        }

        public Guid ptas_economicunitid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_legacyrpeconomicunitid { get; set; }
        public string ptas_name { get; set; }
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
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_parceleconomicunit> ptas_parceleconomicunit { get; set; }
    }
}
