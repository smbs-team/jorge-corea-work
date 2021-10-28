using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_stateorprovince
    {
        public ptas_stateorprovince()
        {
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_condocomplex = new HashSet<ptas_condocomplex>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_quickcollect_ptas_newinformation_addr_locationstateid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_ptas_newinformation_addr_stateid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_ptas_personalpropinfo_addr_locationstateid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_ptas_personalpropinfo_addr_stateid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_quickcollect_ptas_requestorinfo_addr_stateid_valueNavigation = new HashSet<ptas_quickcollect>();
            ptas_taxaccount = new HashSet<ptas_taxaccount>();
        }

        public Guid ptas_stateorprovinceid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_abbreviation { get; set; }
        public string ptas_alpha3 { get; set; }
        public string ptas_name { get; set; }
        public string ptas_uncode { get; set; }
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
        public Guid? _ptas_countryid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_country _ptas_countryid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_newinformation_addr_locationstateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_newinformation_addr_stateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_personalpropinfo_addr_locationstateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_personalpropinfo_addr_stateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect_ptas_requestorinfo_addr_stateid_valueNavigation { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount { get; set; }
    }
}
