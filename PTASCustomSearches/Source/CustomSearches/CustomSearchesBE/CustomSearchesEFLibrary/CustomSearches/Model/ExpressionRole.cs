using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class ExpressionRole
    {
        public ExpressionRole()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
        }

        public string ExpressionRole1 { get; set; }

        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
    }
}
