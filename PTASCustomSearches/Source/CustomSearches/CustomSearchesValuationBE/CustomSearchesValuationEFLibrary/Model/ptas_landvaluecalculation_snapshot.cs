using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_landvaluecalculation_snapshot
    {
        public Guid ptas_landvaluecalculationid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public bool? ptas_accessrights { get; set; }
        public string ptas_addr1_compositeaddress_oneline { get; set; }
        public decimal? ptas_adjustedvalue { get; set; }
        public decimal? ptas_adjustedvalue_base { get; set; }
        public int? ptas_airportnoise { get; set; }
        public int? ptas_characteristictype { get; set; }
        public bool? ptas_delineationstudy { get; set; }
        public int? ptas_depthfactor { get; set; }
        public string ptas_description { get; set; }
        public int? ptas_designationtype { get; set; }
        public decimal? ptas_dollaradjustment { get; set; }
        public decimal? ptas_dollaradjustment_base { get; set; }
        public decimal? ptas_dollarperlinearft { get; set; }
        public decimal? ptas_dollarperlinearft_base { get; set; }
        public decimal? ptas_dollarpersqft { get; set; }
        public decimal? ptas_dollarpersqft_base { get; set; }
        public decimal? ptas_dollarperunit { get; set; }
        public decimal? ptas_dollarperunit_base { get; set; }
        public int? ptas_environmentalrestriction { get; set; }
        public int? ptas_envressource { get; set; }
        public decimal? ptas_grosslandvalue { get; set; }
        public decimal? ptas_grosslandvalue_base { get; set; }
        public string ptas_legacycreatedby { get; set; }
        public DateTimeOffset? ptas_legacycreatedon { get; set; }
        public string ptas_legacymigrationcode { get; set; }
        public string ptas_legacymigrationinfo { get; set; }
        public string ptas_legacymodifiedby { get; set; }
        public DateTimeOffset? ptas_legacymodifiedon { get; set; }
        public int? ptas_linearfootage { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_noiselevel { get; set; }
        public int? ptas_nuisancetype { get; set; }
        public int? ptas_numberofunits { get; set; }
        public string ptas_parcelheadername { get; set; }
        public string ptas_parcelheadertext { get; set; }
        public string ptas_parcelheadertext2 { get; set; }
        public int? ptas_percentadjustment { get; set; }
        public int? ptas_percentaffected { get; set; }
        public bool? ptas_poorquality { get; set; }
        public bool? ptas_proximityinfluence { get; set; }
        public int? ptas_quality { get; set; }
        public int? ptas_quantitytype { get; set; }
        public bool? ptas_reallocate { get; set; }
        public int? ptas_restrictedaccess { get; set; }
        public decimal? ptas_sitevalue { get; set; }
        public decimal? ptas_sitevalue_base { get; set; }
        public DateTimeOffset? ptas_snapshotdateandtime { get; set; }
        public int? ptas_snapshottype { get; set; }
        public int? ptas_squarefootage { get; set; }
        public int? ptas_tidelandorshoreland { get; set; }
        public int? ptas_transfertype { get; set; }
        public int? ptas_valuemethodcalculation { get; set; }
        public int? ptas_valuetype { get; set; }
        public int? ptas_viewtype { get; set; }
        public int? ptas_waterfrontbank { get; set; }
        public int? ptas_waterfrontlocation { get; set; }
        public int? ptas_waterfronttype { get; set; }
        public DateTimeOffset? ptas_zoningchangedate { get; set; }
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
        public Guid? _ptas_designationtypeid_value { get; set; }
        public Guid? _ptas_envrestypeid_value { get; set; }
        public Guid? _ptas_landid_value { get; set; }
        public Guid? _ptas_masterlandcharacteristicid_value { get; set; }
        public Guid? _ptas_nuisancetypeid_value { get; set; }
        public Guid? _ptas_viewtypeid_value { get; set; }
        public Guid? _ptas_zoningtypeid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }
    }
}
