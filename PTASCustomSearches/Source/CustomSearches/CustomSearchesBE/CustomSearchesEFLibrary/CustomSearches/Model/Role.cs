using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Role
    {
        public Role()
        {
            InverseParentroleidValueNavigation = new HashSet<Role>();
        }

        public Guid Roleid { get; set; }
        public bool? Canbedeleted { get; set; }
        public int? Componentstate { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public bool? Iscustomizable { get; set; }
        public int? Isinherited { get; set; }
        public bool? Ismanaged { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public string Name { get; set; }
        public Guid? Organizationid { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public DateTimeOffset? Overwritetime { get; set; }
        public Guid? Roleidunique { get; set; }
        public Guid? Solutionid { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? BusinessunitidValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? ParentroleidValue { get; set; }
        public Guid? ParentrootroleidValue { get; set; }
        public Guid? RoletemplateidValue { get; set; }

        public virtual Role ParentroleidValueNavigation { get; set; }
        public virtual ICollection<Role> InverseParentroleidValueNavigation { get; set; }
    }
}
