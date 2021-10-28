using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Team
    {
        public Guid? Azureactivedirectoryobjectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public string Description { get; set; }
        public string Emailaddress { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public bool? Isdefault { get; set; }
        public int? Membershiptype { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public string Name { get; set; }
        public Guid? Organizationid { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public Guid? Processid { get; set; }
        public Guid? Stageid { get; set; }
        public bool? Systemmanaged { get; set; }
        public Guid Teamid { get; set; }
        public int? Teamtype { get; set; }
        public string Traversedpath { get; set; }
        public long? Versionnumber { get; set; }
        public string Yominame { get; set; }
        public Guid? AdministratoridValue { get; set; }
        public Guid? BusinessunitidValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? QueueidValue { get; set; }
        public Guid? RegardingobjectidValue { get; set; }
        public Guid? TeamtemplateidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }
    }
}
