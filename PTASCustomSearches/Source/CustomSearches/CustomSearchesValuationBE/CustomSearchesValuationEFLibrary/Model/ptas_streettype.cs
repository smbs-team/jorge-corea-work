﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_streettype
    {
        public ptas_streettype()
        {
            ptas_buildingdetail = new HashSet<ptas_buildingdetail>();
            ptas_condounit = new HashSet<ptas_condounit>();
            ptas_parceldetail = new HashSet<ptas_parceldetail>();
        }

        public Guid ptas_streettypeid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_alternate1 { get; set; }
        public string ptas_alternate2 { get; set; }
        public string ptas_alternate3 { get; set; }
        public string ptas_alternate4 { get; set; }
        public string ptas_alternate5 { get; set; }
        public string ptas_alternate6 { get; set; }
        public string ptas_alternate7 { get; set; }
        public string ptas_alternate8 { get; set; }
        public string ptas_description { get; set; }
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

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual ICollection<ptas_buildingdetail> ptas_buildingdetail { get; set; }
        public virtual ICollection<ptas_condounit> ptas_condounit { get; set; }
        public virtual ICollection<ptas_parceldetail> ptas_parceldetail { get; set; }
    }
}
