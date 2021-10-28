using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentuseapplicationparcel
    {
        public PtasCurrentuseapplicationparcel()
        {
            PtasCurrentusebacktaxstatement = new HashSet<PtasCurrentusebacktaxstatement>();
            PtasCurrentuselanduse = new HashSet<PtasCurrentuselanduse>();
            PtasCurrentusetask = new HashSet<PtasCurrentusetask>();
            PtasCurrentusevaluehistory = new HashSet<PtasCurrentusevaluehistory>();
        }

        public Guid PtasCurrentuseapplicationparcelid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAcresappliedfor { get; set; }
        public decimal? PtasAcresappliedforfarmandagriculture { get; set; }
        public decimal? PtasAcresappliedforopenspace { get; set; }
        public decimal? PtasAcresappliedfortimberandforestland { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public DateTimeOffset? PtasBacktaxesdue { get; set; }
        public decimal? PtasBacktaxesowed { get; set; }
        public decimal? PtasBacktaxesowedBase { get; set; }
        public string PtasBacktaxesreason { get; set; }
        public string PtasCouncilfile { get; set; }
        public DateTimeOffset? PtasDateparcelacquired { get; set; }
        public string PtasEmailaddress { get; set; }
        public bool? PtasFarmandagriculture { get; set; }
        public string PtasFolio { get; set; }
        public DateTimeOffset? PtasLastsitevisit { get; set; }
        public DateTimeOffset? PtasMonitoringlettersenton { get; set; }
        public bool? PtasMonitoringlettersentwaitingfortaxpayer { get; set; }
        public string PtasName { get; set; }
        public bool? PtasOpenspace { get; set; }
        public string PtasParcelowner { get; set; }
        public int? PtasPbrstype { get; set; }
        public string PtasPhonenumber { get; set; }
        public bool? PtasSitevisitrequired { get; set; }
        public bool? PtasTimberandforestland { get; set; }
        public decimal? PtasTotalacres { get; set; }
        public DateTimeOffset? PtasUpdatedforestmanagementplandue { get; set; }
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
        public Guid? PtasCrossreferenceparcelidValue { get; set; }
        public Guid? PtasCurrentuseapplicationValue { get; set; }
        public Guid? PtasCurrentusegroupidValue { get; set; }
        public Guid? PtasParcelValue { get; set; }
        public Guid? PtasSitevisittaskidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1CityidValueNavigation { get; set; }
        public virtual PtasCountry PtasAddr1CountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1StateidValueNavigation { get; set; }
        public virtual PtasStreetname PtasAddr1StreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasAddr1StreettypeidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1ZipcodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasCrossreferenceparcelidValueNavigation { get; set; }
        public virtual PtasCurrentuseapplication PtasCurrentuseapplicationValueNavigation { get; set; }
        public virtual PtasCurrentusegroup PtasCurrentusegroupidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelValueNavigation { get; set; }
        public virtual PtasCurrentusetask PtasSitevisittaskidValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxstatement> PtasCurrentusebacktaxstatement { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduse { get; set; }
        public virtual ICollection<PtasCurrentusetask> PtasCurrentusetask { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistory { get; set; }
    }
}
