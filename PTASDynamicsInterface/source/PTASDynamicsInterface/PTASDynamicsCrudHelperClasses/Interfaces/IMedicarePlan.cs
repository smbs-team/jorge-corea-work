// <copyright file="IMedicarePlan.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    /// <summary>
    /// Generic medicare plan template.
    /// </summary>
    public interface IMedicarePlan
    {
        /// <summary>
        /// Gets or sets a value indicating whether has this plan been approved?.
        /// </summary>
        bool Approved { get; set; }

        /// <summary>
        /// Gets or sets plan Id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets name of the organization that provides this plan.
        /// </summary>
        string OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets name of this particular plan.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets year Id.
        /// </summary>
        string YearId { get; set; }
    }
}