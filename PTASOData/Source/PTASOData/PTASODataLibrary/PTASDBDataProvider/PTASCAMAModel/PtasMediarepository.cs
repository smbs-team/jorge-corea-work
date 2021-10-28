using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasMediarepository
    {
        public Guid PtasMediarepositoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccessoryguid { get; set; }
        public string PtasBlobpath { get; set; }
        public string PtasBuildingguid { get; set; }
        public string PtasDescription { get; set; }
        public string PtasFileextension { get; set; }
        public int? PtasImagetype { get; set; }
        public string PtasLandguid { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public DateTimeOffset? PtasMediadate { get; set; }
        public int? PtasMediatype { get; set; }
        public int? PtasMediauploaded { get; set; }
        public string PtasMigrationnote { get; set; }
        public string PtasName { get; set; }
        public int? PtasOrder { get; set; }
        public string PtasPermitguid { get; set; }
        public bool? PtasPosttoweb { get; set; }
        public bool? PtasPrimary { get; set; }
        public string PtasPrimaryentity { get; set; }
        public string PtasPrimaryguid { get; set; }
        public int? PtasRelatedobjectmediatype { get; set; }
        public string PtasRootfolder { get; set; }
        public string PtasUnitguid { get; set; }
        public string PtasUpdatedbyuser { get; set; }
        public int? PtasUsagetype { get; set; }
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
        public Guid? PtasSaleidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasSales PtasSaleidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
    }
}
