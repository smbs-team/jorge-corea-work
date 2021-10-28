using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_aptlistedrent
    {
        public ptas_aptlistedrent()
        {
            ptas_aptcomparablerent = new HashSet<ptas_aptcomparablerent>();
        }

        public Guid ptas_aptlistedrentid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public int? ptas_airportnoise { get; set; }
        public int? ptas_buildingquality { get; set; }
        public int? ptas_condition { get; set; }
        public string ptas_description { get; set; }
        public string ptas_detailcomment { get; set; }
        public DateTimeOffset? ptas_effectivestartdate { get; set; }
        public bool? ptas_hasview { get; set; }
        public string ptas_informationsource { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_netareasqft { get; set; }
        public int? ptas_numberofbathrooms { get; set; }
        public int? ptas_numberofbedrooms { get; set; }
        public int? ptas_numberofunits { get; set; }
        public int? ptas_percentwithview { get; set; }
        public bool? ptas_pool { get; set; }
        public int? ptas_region { get; set; }
        public string ptas_renttermperiod { get; set; }
        public string ptas_renttermunit { get; set; }
        public DateTimeOffset? ptas_trenddate { get; set; }
        public decimal? ptas_trendedrent { get; set; }
        public decimal? ptas_trendedrent_base { get; set; }
        public decimal? ptas_typicalrent { get; set; }
        public decimal? ptas_typicalrent_base { get; set; }
        public int? ptas_unitbreakdowntype { get; set; }
        public string ptas_unituse { get; set; }
        public int? ptas_yearbuilt { get; set; }
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
        public Guid? _ptas_neighborhoodid_value { get; set; }
        public Guid? _ptas_parceld_value { get; set; }
        public Guid? _ptas_projectid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_neighborhood _ptas_neighborhoodid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parceld_valueNavigation { get; set; }
        public virtual ptas_condocomplex _ptas_projectid_valueNavigation { get; set; }
        public virtual ICollection<ptas_aptcomparablerent> ptas_aptcomparablerent { get; set; }
    }
}
