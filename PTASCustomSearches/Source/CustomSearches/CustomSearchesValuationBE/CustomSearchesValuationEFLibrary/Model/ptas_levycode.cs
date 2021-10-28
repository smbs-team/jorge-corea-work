using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_levycode
    {
        public ptas_levycode()
        {
            ptas_annexationparcelreview = new HashSet<ptas_annexationparcelreview>();
            ptas_aptadjustedlevyrate = new HashSet<ptas_aptadjustedlevyrate>();
            ptas_assessmentrollcorrection = new HashSet<ptas_assessmentrollcorrection>();
            ptas_omit_ptas_levycodeid_valueNavigation = new HashSet<ptas_omit>();
            ptas_omit_ptas_omitlevycodeid_valueNavigation = new HashSet<ptas_omit>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_taxaccount = new HashSet<ptas_taxaccount>();
        }

        public Guid ptas_levycodeid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_9998590checktotal { get; set; }
        public decimal? ptas_current1constitutionallimit { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_levydodetype { get; set; }
        public decimal? ptas_locallevytotallimit { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_overunderconstitutional { get; set; }
        public decimal? ptas_totallevyrateconstitutional { get; set; }
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
        public Guid? _ptas_firedistrictid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
        public virtual ICollection<ptas_aptadjustedlevyrate> ptas_aptadjustedlevyrate { get; set; }
        public virtual ICollection<ptas_assessmentrollcorrection> ptas_assessmentrollcorrection { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_ptas_levycodeid_valueNavigation { get; set; }
        public virtual ICollection<ptas_omit> ptas_omit_ptas_omitlevycodeid_valueNavigation { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_taxaccount> ptas_taxaccount { get; set; }
    }
}
