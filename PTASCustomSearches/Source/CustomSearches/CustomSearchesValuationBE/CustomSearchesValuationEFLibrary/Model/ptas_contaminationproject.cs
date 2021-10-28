using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_contaminationproject
    {
        public ptas_contaminationproject()
        {
            ptas_annualcostdistribution = new HashSet<ptas_annualcostdistribution>();
            ptas_condocomplex = new HashSet<ptas_condocomplex>();
            ptas_contaminatedlandreduction = new HashSet<ptas_contaminatedlandreduction>();
        }

        public Guid ptas_contaminationprojectid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_10yeartreasuryrate { get; set; }
        public int? ptas_estimatetimetocure { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_presentcost { get; set; }
        public decimal? ptas_presentcost_base { get; set; }
        public decimal? ptas_totalremediationcost { get; set; }
        public decimal? ptas_totalremediationcost_base { get; set; }
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
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_annualcostdistribution> ptas_annualcostdistribution { get; set; }
        public virtual ICollection<ptas_condocomplex> ptas_condocomplex { get; set; }
        public virtual ICollection<ptas_contaminatedlandreduction> ptas_contaminatedlandreduction { get; set; }
    }
}
