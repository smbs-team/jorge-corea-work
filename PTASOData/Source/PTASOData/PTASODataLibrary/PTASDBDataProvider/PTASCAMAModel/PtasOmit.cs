using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasOmit
    {
        public PtasOmit()
        {
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
        }

        public Guid PtasOmitid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAssessedvalue { get; set; }
        public decimal? PtasAssessedvalueBase { get; set; }
        public DateTimeOffset? PtasMfinterfacedate { get; set; }
        public bool? PtasMfinterfaceflag { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOmitvalue { get; set; }
        public decimal? PtasOmitvalueBase { get; set; }
        public int? PtasPenalty { get; set; }
        public decimal? PtasSuppliesvalue { get; set; }
        public decimal? PtasSuppliesvalueBase { get; set; }
        public string PtasTransactionnumber { get; set; }
        public DateTimeOffset? PtasValuationdate { get; set; }
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
        public Guid? PtasAssessmentyearidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasOmitlevycodeidValue { get; set; }
        public Guid? PtasOmittedassessmentyearidValue { get; set; }
        public Guid? PtasPersonalpropertyaccountidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasLevycode PtasOmitlevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasOmittedassessmentyearidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyaccountidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
    }
}
