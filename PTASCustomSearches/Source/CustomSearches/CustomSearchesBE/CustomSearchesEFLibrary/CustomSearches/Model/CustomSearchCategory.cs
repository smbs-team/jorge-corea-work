using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class CustomSearchCategory
    {
        public CustomSearchCategory()
        {
            CustomSearchCategoryDefinition = new HashSet<CustomSearchCategoryDefinition>();
        }

        public int CustomSearchCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<CustomSearchCategoryDefinition> CustomSearchCategoryDefinition { get; set; }
    }
}
