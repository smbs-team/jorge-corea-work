using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLandschedule
    {
        public Guid PtasLandscheduleid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasLandvalue { get; set; }
        public decimal? PtasLandvalueBase { get; set; }
        public int? PtasMaxsqft { get; set; }
        public int? PtasMinsqft { get; set; }
        public string PtasName { get; set; }
        public int? PtasYear2standardadjustment { get; set; }
        public int? PtasYear3standardadjustment { get; set; }
        public int? PtasYear4standardadjustment { get; set; }
        public int? PtasYear5standardadjustment { get; set; }
        public int? PtasYear6standardadjustment { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasAreaidValue { get; set; }
        public Guid? PtasInspectionyearidValue { get; set; }
        public Guid? PtasNeighborhoodidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasZoningidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual PtasArea PtasAreaidValueNavigation { get; set; }
        public virtual PtasYear PtasInspectionyearidValueNavigation { get; set; }
        public virtual PtasNeighborhood PtasNeighborhoodidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasZoning PtasZoningidValueNavigation { get; set; }
    }
}
