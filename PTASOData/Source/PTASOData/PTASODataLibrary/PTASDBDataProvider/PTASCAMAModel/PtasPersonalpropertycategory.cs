using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPersonalpropertycategory
    {
        public PtasPersonalpropertycategory()
        {
            PtasPersonalpropertyasset = new HashSet<PtasPersonalpropertyasset>();
        }

        public Guid PtasPersonalpropertycategoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasCategorycode { get; set; }
        public string PtasCategorygroup { get; set; }
        public string PtasCategorygrouph { get; set; }
        public decimal? PtasCostafteroneyear { get; set; }
        public decimal? PtasCostafteroneyearBase { get; set; }
        public decimal? PtasCostaftertwoyears { get; set; }
        public decimal? PtasCostaftertwoyearsBase { get; set; }
        public decimal? PtasDepreciationrate { get; set; }
        public string PtasExamples { get; set; }
        public string PtasLegacycategorycode { get; set; }
        public decimal? PtasMinpercentgoodfactor { get; set; }
        public string PtasName { get; set; }
        public string PtasPerspropcategory { get; set; }
        public int? PtasTrendtable { get; set; }
        public decimal? PtasYearacquiredcost { get; set; }
        public decimal? PtasYearacquiredcostBase { get; set; }
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
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyasset> PtasPersonalpropertyasset { get; set; }
    }
}
