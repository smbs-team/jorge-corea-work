using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_projectdock
    {
        public ptas_projectdock()
        {
            Inverse_ptas_masterdockid_valueNavigation = new HashSet<ptas_projectdock>();
            ptas_condounit = new HashSet<ptas_condounit>();
        }

        public Guid ptas_projectdockid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_alternatekey { get; set; }
        public bool? ptas_gated { get; set; }
        public string ptas_name { get; set; }
        public string ptas_securitycode { get; set; }
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
        public Guid? _organizationid_value { get; set; }
        public Guid? _ptas_condocomplexid_value { get; set; }
        public Guid? _ptas_masterdockid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ptas_condocomplex _ptas_condocomplexid_valueNavigation { get; set; }
        public virtual ptas_projectdock _ptas_masterdockid_valueNavigation { get; set; }
        public virtual ICollection<ptas_projectdock> Inverse_ptas_masterdockid_valueNavigation { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
    }
}
