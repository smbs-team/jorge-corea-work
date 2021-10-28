using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPersonalpropertyasset
    {
        public PtasPersonalpropertyasset()
        {
            InversePtasMasterpersonalpropertyassetidValueNavigation = new HashSet<PtasPersonalpropertyasset>();
        }

        public Guid PtasPersonalpropertyassetid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAlternatekey { get; set; }
        public int? PtasAlternatekeyint { get; set; }
        public decimal? PtasAssessedvalue { get; set; }
        public decimal? PtasAssessedvalueBase { get; set; }
        public int? PtasAssettype { get; set; }
        public bool? PtasAvoverride { get; set; }
        public string PtasCategorycode { get; set; }
        public int? PtasChangereason { get; set; }
        public decimal? PtasCurrentcost { get; set; }
        public decimal? PtasCurrentcostBase { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasKeyapplunitid { get; set; }
        public string PtasKeyassetguid { get; set; }
        public int? PtasKeyimpid { get; set; }
        public int? PtasKeypropid { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public bool? PtasMatch { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOriginalcost { get; set; }
        public decimal? PtasOriginalcostBase { get; set; }
        public int? PtasPercent { get; set; }
        public string PtasPerspropcategory { get; set; }
        public decimal? PtasReportedcost { get; set; }
        public decimal? PtasReportedcostBase { get; set; }
        public decimal? PtasRevisedcost { get; set; }
        public decimal? PtasRevisedcostBase { get; set; }
        public int? PtasRevisedunits { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasUnits { get; set; }
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
        public Guid? PtasCategorycodeidValue { get; set; }
        public Guid? PtasDepreciationstartyearidValue { get; set; }
        public Guid? PtasMasterpersonalpropertyassetidValue { get; set; }
        public Guid? PtasOmitidValue { get; set; }
        public Guid? PtasPersonalpropertyidValue { get; set; }
        public Guid? PtasPersonalpropertylistingidValue { get; set; }
        public Guid? PtasPpassessmenthistoryidValue { get; set; }
        public Guid? PtasYearacquiredidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasPersonalpropertycategory PtasCategorycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasDepreciationstartyearidValueNavigation { get; set; }
        public virtual PtasPersonalpropertyasset PtasMasterpersonalpropertyassetidValueNavigation { get; set; }
        public virtual PtasOmit PtasOmitidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyidValueNavigation { get; set; }
        public virtual PtasPersonalpropertylisting PtasPersonalpropertylistingidValueNavigation { get; set; }
        public virtual PtasPersonalpropertyhistory PtasPpassessmenthistoryidValueNavigation { get; set; }
        public virtual PtasYear PtasYearacquiredidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> InversePtasMasterpersonalpropertyassetidValueNavigation { get; set; }
    }
}
