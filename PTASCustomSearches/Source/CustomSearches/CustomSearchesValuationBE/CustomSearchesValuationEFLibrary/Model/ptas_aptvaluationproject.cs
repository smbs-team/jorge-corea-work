using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptvaluationproject
    {
        public ptas_aptvaluationproject()
        {
            ptas_aptvaluation = new HashSet<ptas_aptvaluation>();
        }

        public Guid ptas_aptvaluationprojectid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_apartmentneighborhood { get; set; }
        public string ptas_apartmentregressionprojecturl { get; set; }
        public int? ptas_assessmentyear { get; set; }
        public bool? ptas_comparablesalesapproachran { get; set; }
        public decimal? ptas_comparablesalesweight { get; set; }
        public bool? ptas_emvapproachran { get; set; }
        public decimal? ptas_emvweight { get; set; }
        public bool? ptas_incomeapproachran { get; set; }
        public decimal? ptas_incomeweight { get; set; }
        public string ptas_name { get; set; }
        public DateTimeOffset? ptas_rentcompenddate { get; set; }
        public DateTimeOffset? ptas_rentcompstartdate { get; set; }
        public DateTimeOffset? ptas_rentregressionenddate { get; set; }
        public DateTimeOffset? ptas_rentregressionstartdate { get; set; }
        public DateTimeOffset? ptas_salecompenddate { get; set; }
        public DateTimeOffset? ptas_salecompstartdate { get; set; }
        public DateTimeOffset? ptas_saleregressionenddate { get; set; }
        public DateTimeOffset? ptas_saleregressionstartdate { get; set; }
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

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptvaluation> ptas_aptvaluation { get; set; }
    }
}
