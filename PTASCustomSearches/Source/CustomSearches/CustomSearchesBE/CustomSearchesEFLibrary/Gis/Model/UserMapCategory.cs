using System;
using System.Collections.Generic;

namespace CustomSearchesEFLibrary.Gis.Model
{
    public partial class UserMapCategory
    {
        public UserMapCategory()
        {
            UserMapCategoryUserMap = new HashSet<UserMapCategoryUserMap>();
        }

        public int UserMapCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<UserMapCategoryUserMap> UserMapCategoryUserMap { get; set; }
    }
}
