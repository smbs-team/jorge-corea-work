using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_contaminatedlandreduction
    {
        public Guid ptas_contaminatedlandreductionid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_assessedvaluereduction { get; set; }
        public decimal? ptas_assessedvaluereduction_base { get; set; }
        public decimal? ptas_baselandvalue { get; set; }
        public decimal? ptas_baselandvalue_base { get; set; }
        public decimal? ptas_improvementvalue { get; set; }
        public decimal? ptas_improvementvalue_base { get; set; }
        public decimal? ptas_landreducedvalue { get; set; }
        public decimal? ptas_landreducedvalue_base { get; set; }
        public decimal? ptas_landreducedvaluerounded { get; set; }
        public decimal? ptas_landreducedvaluerounded_base { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_percentremediationcost { get; set; }
        public decimal? ptas_presentcost { get; set; }
        public decimal? ptas_presentcost_base { get; set; }
        public decimal? ptas_totalvalue { get; set; }
        public decimal? ptas_totalvalue_base { get; set; }
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
        public Guid? _ptas_contaminatedprojectid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearid_valueNavigation { get; set; }
        public virtual ptas_contaminationproject _ptas_contaminatedprojectid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
    }
}
