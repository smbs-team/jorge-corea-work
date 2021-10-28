using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_omit
    {
        public Guid ptas_omitid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_agriculturevalue { get; set; }
        public decimal? ptas_agriculturevalue_base { get; set; }
        public decimal? ptas_machineequipmentvalue { get; set; }
        public decimal? ptas_machineequipmentvalue_base { get; set; }
        public decimal? ptas_manufacturingvalue { get; set; }
        public decimal? ptas_manufacturingvalue_base { get; set; }
        public DateTimeOffset? ptas_mfinterfacedate { get; set; }
        public bool? ptas_mfinterfaceflag { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_othervalue { get; set; }
        public decimal? ptas_othervalue_base { get; set; }
        public int? ptas_penalty { get; set; }
        public DateTimeOffset? ptas_penaltydate { get; set; }
        public decimal? ptas_suppliesvalue { get; set; }
        public decimal? ptas_suppliesvalue_base { get; set; }
        public string ptas_transactionnumber { get; set; }
        public DateTimeOffset? ptas_valuationdate { get; set; }
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
        public Guid? _ptas_assessmentyearid_value { get; set; }
        public Guid? _ptas_levycodeid_value { get; set; }
        public Guid? _ptas_omitlevycodeid_value { get; set; }
        public Guid? _ptas_omittedassessmentyearid_value { get; set; }
        public Guid? _ptas_personalpropertyaccountid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearid_valueNavigation { get; set; }
        public virtual ptas_levycode _ptas_levycodeid_valueNavigation { get; set; }
        public virtual ptas_levycode _ptas_omitlevycodeid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_omittedassessmentyearid_valueNavigation { get; set; }
    }
}
