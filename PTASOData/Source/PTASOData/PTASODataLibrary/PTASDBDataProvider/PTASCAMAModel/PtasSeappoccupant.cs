using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSeappoccupant
    {
        public PtasSeappoccupant()
        {
            PtasSeapplication = new HashSet<PtasSeapplication>();
        }

        public Guid PtasSeappoccupantid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasIsapplicant { get; set; }
        public string PtasName { get; set; }
        public string PtasOccupantfirstname { get; set; }
        public string PtasOccupantlastname { get; set; }
        public string PtasOccupantmiddlename { get; set; }
        public string PtasOccupantsuffix { get; set; }
        public int? PtasOccupanttype { get; set; }
        public int? PtasOwnershippercentage { get; set; }
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
        public Guid? PtasSeapplicationidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeapplicationidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
    }
}
