using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_buildingsectionuse
    {
        public ptas_buildingsectionuse()
        {
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_buildingdetail_commercialuse = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_salesaggregate = new HashSet<ptas_salesaggregate>();
            ptas_sectionusesqft = new HashSet<ptas_sectionusesqft>();
        }

        public Guid ptas_buildingsectionuseid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_abbreviation { get; set; }
        public string ptas_itemid { get; set; }
        public int? ptas_mainframecode { get; set; }
        public string ptas_marshallswiftdescription { get; set; }
        public string ptas_name { get; set; }
        public string ptas_typeid { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _organizationid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate { get; set; }
        public virtual ICollection<ptas_sectionusesqft> ptas_sectionusesqft { get; set; }
    }
}
