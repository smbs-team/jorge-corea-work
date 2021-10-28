using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class RscriptModel
    {
        public RscriptModel()
        {
            CustomSearchExpression = new HashSet<CustomSearchExpression>();
            CustomSearchParameter = new HashSet<CustomSearchParameter>();
            DatasetPostProcess = new HashSet<DatasetPostProcess>();
        }

        public int RscriptModelId { get; set; }
        public string Rscript { get; set; }
        public string Description { get; set; }
        public string RscriptFileName { get; set; }
        public string RscriptFolderName { get; set; }
        public string RscriptModelName { get; set; }
        public string RscriptResultsDefinition { get; set; }
        public string RscriptModelRole { get; set; }
        public string PredictedTsqlExpression { get; set; }
        public string RscriptDisplayName { get; set; }
        public bool LockPrecommitExpressions { get; set; }
        public string RscriptModelTemplate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<CustomSearchExpression> CustomSearchExpression { get; set; }
        public virtual ICollection<CustomSearchParameter> CustomSearchParameter { get; set; }
        public virtual ICollection<DatasetPostProcess> DatasetPostProcess { get; set; }
    }
}
