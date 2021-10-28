using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptcloseproximity
    {
        public Guid ptas_aptcloseproximityid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_assessmentyear { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_proximitycode { get; set; }
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
        public Guid? _ptas_salerentneighborhoodid_value { get; set; }
        public Guid? _ptas_subjectneighborhoodid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearlookupid_valueNavigation { get; set; }
        public virtual ptas_aptneighborhood _ptas_salerentneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_aptneighborhood _ptas_subjectneighborhoodid_valueNavigation { get; set; }
    }
}
