using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_area
    {
        public ptas_area()
        {
            ptas_inspectionyear = new HashSet<ptas_inspectionyear>();
            ptas_neighborhood = new HashSet<ptas_neighborhood>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
            ptas_subarea = new HashSet<ptas_subarea>();
        }

        public Guid ptas_areaid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_areanumber { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_district { get; set; }
        public string ptas_name { get; set; }
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
        public Guid? _ptas_appraiserid_value { get; set; }
        public Guid? _ptas_residentialappraiserteam_value { get; set; }
        public Guid? _ptas_seniorappraiser_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _ptas_appraiserid_valueNavigation { get; set; }
        public virtual ptas_residentialappraiserteam _ptas_residentialappraiserteam_valueNavigation { get; set; }
        public virtual systemuser _ptas_seniorappraiser_valueNavigation { get; set; }
        public virtual ICollection<ptas_inspectionyear> ptas_inspectionyear { get; set; }
        public virtual ICollection<ptas_neighborhood> ptas_neighborhood { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
        public virtual ICollection<ptas_subarea> ptas_subarea { get; set; }
    }
}
