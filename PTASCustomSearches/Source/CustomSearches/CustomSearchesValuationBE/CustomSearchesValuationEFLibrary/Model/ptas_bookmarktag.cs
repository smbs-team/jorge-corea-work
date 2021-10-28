using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_bookmarktag
    {
        public ptas_bookmarktag()
        {
            ptas_bookmark_ptas_tag1_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_ptas_tag2_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_ptas_tag3_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_ptas_tag4_valueNavigation = new HashSet<ptas_bookmark>();
            ptas_bookmark_ptas_tag5_valueNavigation = new HashSet<ptas_bookmark>();
        }

        public Guid ptas_bookmarktagid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
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
        public virtual ICollection<ptas_bookmark> ptas_bookmark_ptas_tag1_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_ptas_tag2_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_ptas_tag3_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_ptas_tag4_valueNavigation { get; set; }
        public virtual ICollection<ptas_bookmark> ptas_bookmark_ptas_tag5_valueNavigation { get; set; }
    }
}
