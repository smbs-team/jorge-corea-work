using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasParceleconomicunit
    {
        public Guid PtasParceleconomicunitid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasGrosssqft { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegacyrpparceleconomicunitid { get; set; }
        public string PtasName { get; set; }
        public int? PtasNetsqft { get; set; }
        public int? PtasType { get; set; }
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
        public Guid? PtasEconomicunitidValue { get; set; }
        public Guid? PtasEffectiveyearidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasYearbuiltidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasEconomicunit PtasEconomicunitidValueNavigation { get; set; }
        public virtual PtasYear PtasEffectiveyearidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasYear PtasYearbuiltidValueNavigation { get; set; }
    }
}
