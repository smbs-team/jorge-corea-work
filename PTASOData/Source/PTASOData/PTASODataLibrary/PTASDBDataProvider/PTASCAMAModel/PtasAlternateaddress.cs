using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAlternateaddress
    {
        public Guid PtasAlternateaddressid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public int? PtasAddressfor { get; set; }
        public string PtasName { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1StreetnameidValue { get; set; }
        public Guid? PtasAddr1StreettypeidValue { get; set; }
        public Guid? PtasAddr1ZipcodeidValue { get; set; }
        public Guid? PtasBuildingidValue { get; set; }
        public Guid? PtasComplexidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasUnitidValue { get; set; }
    }
}
