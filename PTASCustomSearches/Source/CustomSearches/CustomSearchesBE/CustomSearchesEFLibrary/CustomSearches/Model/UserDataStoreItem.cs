using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class UserDataStoreItem
    {
        public int UserDataStoreItemId { get; set; }
        public Guid UserId { get; set; }
        public string StoreType { get; set; }
        public string OwnerType { get; set; }
        public string OwnerObjectId { get; set; }
        public string ItemName { get; set; }
        public string Value { get; set; }

        public virtual OwnerType OwnerTypeNavigation { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
