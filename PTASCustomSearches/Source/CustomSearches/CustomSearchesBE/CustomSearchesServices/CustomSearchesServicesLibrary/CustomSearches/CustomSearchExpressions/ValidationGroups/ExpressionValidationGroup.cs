namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions.ValidationGroups
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Contains a group of validations that need to be evaluated together.
    /// </summary>
    public class ExpressionValidationGroup : List<CustomSearchExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionValidationGroup"/> class.
        /// </summary>
        /// <param name="groupType">Type of the group.</param>
        public ExpressionValidationGroup(ExpressionValidationGroupType groupType)
        {
            this.GroupType = groupType;
        }

        /// <summary>
        /// Gets or sets the type of the group.
        /// </summary>
        public ExpressionValidationGroupType GroupType { get; protected set; }

        /// <summary>
        /// Gets the expressions with the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A list of expressions with the given role.</returns>
        public IList<CustomSearchExpression> GetExpressionsWithRole(CustomSearchExpressionRoleType role)
        {
            return (from e in this
                    where e.ExpressionRole.ToLower() == role.ToString().ToLower()
                    select e).ToList();
        }
    }
}
