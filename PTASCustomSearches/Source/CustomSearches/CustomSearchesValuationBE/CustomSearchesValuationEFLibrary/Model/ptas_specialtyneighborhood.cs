using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_specialtyneighborhood
    {
        public ptas_specialtyneighborhood()
        {
            ptas_buildingdetail_commercialuse = new HashSet<ptas_buildingdetail_commercialuse>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_gradestratificationmapping = new HashSet<ptas_gradestratificationmapping>();
            ptas_inspectionyear = new HashSet<ptas_inspectionyear>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_salesaggregate = new HashSet<ptas_salesaggregate>();
        }

        public Guid ptas_specialtyneighborhoodid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_description { get; set; }
        public string ptas_name { get; set; }
        public string ptas_nbhdnumber { get; set; }
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
        public Guid? _ptas_appraiserid_value { get; set; }
        public Guid? _ptas_specialtyareaid_value { get; set; }
        public Guid? _ptas_supergroupid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _ptas_appraiserid_valueNavigation { get; set; }
        public virtual ptas_specialtyarea _ptas_specialtyareaid_valueNavigation { get; set; }
        public virtual ptas_supergroup _ptas_supergroupid_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail_commercialuse> ptas_buildingdetail_commercialuse { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_gradestratificationmapping> ptas_gradestratificationmapping { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_salesaggregate> ptas_salesaggregate { get; set; }
    }
}
