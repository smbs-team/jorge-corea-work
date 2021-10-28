using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class ProjectTypeCustomSearchDefinition
    {
        public int ProjectTypeId { get; set; }
        public string DatasetRole { get; set; }
        public int CustomSearchDefinitionId { get; set; }

        public virtual CustomSearchDefinition CustomSearchDefinition { get; set; }
        public virtual ProjectType ProjectType { get; set; }
    }
}
