using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPortaladdress
    {
        public Guid PtasPortaladdressid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddresstitle { get; set; }
        public string PtasApt { get; set; }
        public string PtasAttention { get; set; }
        public string PtasHousenumber { get; set; }
        public int? PtasHousenumberdetail { get; set; }
        public string PtasName { get; set; }
        public int? PtasStreetdetailpostfix { get; set; }
        public int? PtasStreetdetailprefix { get; set; }
        public string PtasStreetname { get; set; }
        public int? PtasStreettype { get; set; }
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
        public Guid? PtasCityidValue { get; set; }
        public Guid? PtasCountryidValue { get; set; }
        public Guid? PtasPortalcontactidValue { get; set; }
        public Guid? PtasStateidValue { get; set; }
        public Guid? PtasZipcodeidValue { get; set; }
    }
}
