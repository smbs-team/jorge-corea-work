using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class role
    {
        public role()
        {
            Inverse_parentroleid_valueNavigation = new HashSet<role>();
            Inverse_parentrootroleid_valueNavigation = new HashSet<role>();
        }

        public Guid roleid { get; set; }
        public bool? canbedeleted { get; set; }
        public int? componentstate { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public bool? iscustomizable { get; set; }
        public int? isinherited { get; set; }
        public bool? ismanaged { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public string name { get; set; }
        public Guid? organizationid { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public DateTimeOffset? overwritetime { get; set; }
        public Guid? roleidunique { get; set; }
        public Guid? solutionid { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _businessunitid_value { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _parentroleid_value { get; set; }
        public Guid? _parentrootroleid_value { get; set; }
        public Guid? _roletemplateid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual role _parentroleid_valueNavigation { get; set; }
        public virtual role _parentrootroleid_valueNavigation { get; set; }
        public virtual ICollection<role> Inverse_parentroleid_valueNavigation { get; set; }
        public virtual ICollection<role> Inverse_parentrootroleid_valueNavigation { get; set; }
    }
}
