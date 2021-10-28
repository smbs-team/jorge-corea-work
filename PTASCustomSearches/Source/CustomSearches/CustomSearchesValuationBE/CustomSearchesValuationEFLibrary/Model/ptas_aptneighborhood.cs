using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptneighborhood
    {
        public ptas_aptneighborhood()
        {
            ptas_aptcloseproximity_ptas_salerentneighborhoodid_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptcloseproximity_ptas_subjectneighborhoodid_valueNavigation = new HashSet<ptas_aptcloseproximity>();
            ptas_aptincomeexpense = new HashSet<ptas_aptincomeexpense>();
            ptas_aptvaluation = new HashSet<ptas_aptvaluation>();
        }

        public Guid ptas_aptneighborhoodid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_parkingcoveredsecuredrent { get; set; }
        public decimal? ptas_parkingcoveredsecuredrent_base { get; set; }
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
        public Guid? _ptas_assessmentyearlookupid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearlookupid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_ptas_salerentneighborhoodid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcloseproximity> ptas_aptcloseproximity_ptas_subjectneighborhoodid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptincomeexpense> ptas_aptincomeexpense { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation { get; set; }
    }
}
