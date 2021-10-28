using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptincomeexpense
    {
        public Guid ptas_aptincomeexpenseid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_basecaprate { get; set; }
        public decimal? ptas_baseexpense { get; set; }
        public decimal? ptas_baseexpense_base { get; set; }
        public decimal? ptas_basegim { get; set; }
        public decimal? ptas_basepercentexpense { get; set; }
        public decimal? ptas_commercialgim { get; set; }
        public int? ptas_maxnumberofunits { get; set; }
        public int? ptas_maxyearbuilt { get; set; }
        public int? ptas_minnumberofunits { get; set; }
        public int? ptas_minyearbuilt { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_otherincome { get; set; }
        public decimal? ptas_otherincome_base { get; set; }
        public int? ptas_region { get; set; }
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
        public Guid? _ptas_apartmentneighborhoodid_value { get; set; }
        public Guid? _ptas_assessmentyearlookupid_value { get; set; }
        public Guid? _ptas_supergroupid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_aptneighborhood _ptas_apartmentneighborhoodid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearlookupid_valueNavigation { get; set; }
        public virtual ptas_supergroup _ptas_supergroupid_valueNavigation { get; set; }
    }
}
