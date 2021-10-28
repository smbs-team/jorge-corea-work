namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;

    /// <summary>
    /// Helper for project variables.
    /// </summary>
    public static class ProjectVariableHelper
    {
        /// <summary>
        /// The valuation date variable name.
        /// </summary>
        private static readonly string ValuationDateName = "ValuationDate";

        /// <summary>
        /// The assessment year variable name.
        /// </summary>
        private static readonly string AssessmentYearName = "AssessmentYear";

        /// <summary>
        /// Adds the project variables to the replacement dictionary.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="replacementDictionary">The replacement dictionary.</param>
        public static void AddProjectVariables(
            UserProject userProject,
            Dictionary<string, string> replacementDictionary)
        {
            if (!replacementDictionary.ContainsKey(ProjectVariableHelper.ValuationDateName))
            {
                if (userProject != null)
                {
                    var valuationDate = GetValuationDate(userProject);
                    replacementDictionary.Add(ProjectVariableHelper.ValuationDateName, valuationDate.ToString());
                    replacementDictionary.Add(ProjectVariableHelper.AssessmentYearName, userProject.AssessmentYear.ToString());
                }
            }
        }

        /// <summary>
        /// Gets the valuation date for a project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The valuation date.</returns>
        public static DateTime GetValuationDate(UserProject project)
        {
            return new DateTime(project.AssessmentYear, 1, 1);
        }
    }
}
