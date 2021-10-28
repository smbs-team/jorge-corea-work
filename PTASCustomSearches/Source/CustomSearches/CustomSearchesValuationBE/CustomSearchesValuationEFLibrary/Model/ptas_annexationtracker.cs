using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_annexationtracker
    {
        public ptas_annexationtracker()
        {
            ptas_annexationparcelreview = new HashSet<ptas_annexationparcelreview>();
        }

        public Guid ptas_annexationtrackerid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_annexationname { get; set; }
        public int? ptas_annexationtype { get; set; }
        public string ptas_boundaryreviewboardnumber { get; set; }
        public DateTimeOffset? ptas_brbnoticeofintentdate { get; set; }
        public DateTimeOffset? ptas_brbnoticveofintentdate { get; set; }
        public bool? ptas_changelevycodes { get; set; }
        public DateTimeOffset? ptas_completedlcreview { get; set; }
        public DateTimeOffset? ptas_completedreceivedfromgis { get; set; }
        public DateTimeOffset? ptas_completedsubmittogis { get; set; }
        public bool? ptas_createlimit { get; set; }
        public bool? ptas_createnewlevycodes { get; set; }
        public DateTimeOffset? ptas_doafirstreceived { get; set; }
        public DateTimeOffset? ptas_doaprocessed { get; set; }
        public DateTimeOffset? ptas_draftreceivedfromgis { get; set; }
        public DateTimeOffset? ptas_draftsubmittogis { get; set; }
        public DateTimeOffset? ptas_effectivedate { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_numberofparcels { get; set; }
        public int? ptas_numberofparcelsverified { get; set; }
        public string ptas_resolutionordinance { get; set; }
        public DateTimeOffset? ptas_senttodor { get; set; }
        public DateTimeOffset? ptas_senttopersonalproperty { get; set; }
        public DateTimeOffset? ptas_senttorecordersoffice { get; set; }
        public DateTimeOffset? ptas_senttotreasury { get; set; }
        public decimal? ptas_signedandverifiedav { get; set; }
        public decimal? ptas_signedandverifiedav_base { get; set; }
        public decimal? ptas_signedandverifiedpercentage { get; set; }
        public decimal? ptas_totalassessedvalue { get; set; }
        public decimal? ptas_totalassessedvalue_base { get; set; }
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
        public Guid? _ptas_taxingdistrictid_value { get; set; }
        public Guid? _ptas_taxrollyeareffectiveid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_year _ptas_taxrollyeareffectiveid_valueNavigation { get; set; }
        public virtual ICollection<ptas_annexationparcelreview> ptas_annexationparcelreview { get; set; }
    }
}
