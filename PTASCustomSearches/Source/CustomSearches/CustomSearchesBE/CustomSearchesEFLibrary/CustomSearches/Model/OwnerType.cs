using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class OwnerType
    {
        public OwnerType()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
            CustomSearchParameter = new HashSet<CustomSearchParameter>();
            UserDataStoreItem = new HashSet<UserDataStoreItem>();
        }

        public string OwnerType1 { get; set; }

        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual ICollection<CustomSearchParameter> CustomSearchParameter { get; set; }
        public virtual ICollection<UserDataStoreItem> UserDataStoreItem { get; set; }
    }
}
