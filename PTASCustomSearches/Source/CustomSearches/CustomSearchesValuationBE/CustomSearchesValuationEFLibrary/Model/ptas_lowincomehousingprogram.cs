using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_lowincomehousingprogram
    {
        public ptas_lowincomehousingprogram()
        {
            Inverse_ptas_masterlowincomehousingid_valueNavigation = new HashSet<ptas_lowincomehousingprogram>();
        }

        public Guid ptas_lowincomehousingprogramid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_name { get; set; }
        public DateTimeOffset? ptas_programenddate { get; set; }
        public DateTimeOffset? ptas_programstartdate { get; set; }
        public DateTimeOffset? ptas_snapshotdatetime { get; set; }
        public int? ptas_snapshottype { get; set; }
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
        public Guid? _ptas_condocomplexid_value { get; set; }
        public Guid? _ptas_housingprogramid_value { get; set; }
        public Guid? _ptas_masterlowincomehousingid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_condocomplex _ptas_condocomplexid_valueNavigation { get; set; }
        public virtual ptas_housingprogram _ptas_housingprogramid_valueNavigation { get; set; }
        public virtual ptas_lowincomehousingprogram _ptas_masterlowincomehousingid_valueNavigation { get; set; }
        public virtual ICollection<ptas_lowincomehousingprogram> Inverse_ptas_masterlowincomehousingid_valueNavigation { get; set; }
    }
}
