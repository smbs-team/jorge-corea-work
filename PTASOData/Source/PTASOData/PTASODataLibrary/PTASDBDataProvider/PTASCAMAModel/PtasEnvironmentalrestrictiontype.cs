using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasEnvironmentalrestrictiontype
    {
        public PtasEnvironmentalrestrictiontype()
        {
            PtasEnvironmentalrestriction = new HashSet<PtasEnvironmentalrestriction>();
            PtasLandvaluecalculation = new HashSet<PtasLandvaluecalculation>();
        }

        public Guid PtasEnvironmentalrestrictiontypeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAbbreviation { get; set; }
        public string PtasItemid { get; set; }
        public string PtasLongdescription { get; set; }
        public string PtasName { get; set; }
        public string PtasTypeid { get; set; }
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

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestriction { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculation { get; set; }
    }
}
