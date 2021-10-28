﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_restrictedrent
    {
        public Guid ptas_restrictedrentid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_setaside_100pct { get; set; }
        public decimal? ptas_setaside_100pct_base { get; set; }
        public decimal? ptas_setaside_120pct { get; set; }
        public decimal? ptas_setaside_120pct_base { get; set; }
        public decimal? ptas_setaside_20pct { get; set; }
        public decimal? ptas_setaside_20pct_base { get; set; }
        public decimal? ptas_setaside_30pct { get; set; }
        public decimal? ptas_setaside_30pct_base { get; set; }
        public decimal? ptas_setaside_35pct { get; set; }
        public decimal? ptas_setaside_35pct_base { get; set; }
        public decimal? ptas_setaside_40pct { get; set; }
        public decimal? ptas_setaside_40pct_base { get; set; }
        public decimal? ptas_setaside_45pct { get; set; }
        public decimal? ptas_setaside_45pct_base { get; set; }
        public decimal? ptas_setaside_50pct { get; set; }
        public decimal? ptas_setaside_50pct_base { get; set; }
        public decimal? ptas_setaside_60pct { get; set; }
        public decimal? ptas_setaside_60pct_base { get; set; }
        public decimal? ptas_setaside_70pct { get; set; }
        public decimal? ptas_setaside_70pct_base { get; set; }
        public decimal? ptas_setaside_80pct { get; set; }
        public decimal? ptas_setaside_80pct_base { get; set; }
        public int? ptas_unittype { get; set; }
        public int? ptas_utilityprogram { get; set; }
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
        public Guid? _ptas_assessmentyear_value { get; set; }
        public Guid? _ptas_lowincomehousingprogram_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyear_valueNavigation { get; set; }
        public virtual ptas_housingprogram _ptas_lowincomehousingprogram_valueNavigation { get; set; }
    }
}
