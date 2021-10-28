using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxdistrictcontacts
    {
        public Guid PtasTaxdistrictcontactsid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddress1 { get; set; }
        public string PtasAddress2 { get; set; }
        public string PtasCity { get; set; }
        public string PtasEmailaddress { get; set; }
        public string PtasFirstname { get; set; }
        public string PtasJobtitle { get; set; }
        public string PtasLastname { get; set; }
        public string PtasName { get; set; }
        public string PtasOther { get; set; }
        public string PtasPhonenumber { get; set; }
        public bool? PtasPrincipalcontact { get; set; }
        public string PtasState { get; set; }
        public string PtasZipcode { get; set; }
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
        public Guid? PtasTaxdistrictidValue { get; set; }
    }
}
