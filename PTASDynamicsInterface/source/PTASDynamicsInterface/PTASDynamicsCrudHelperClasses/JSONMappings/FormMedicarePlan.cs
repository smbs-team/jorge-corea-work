// <copyright file="FormMedicarePlan.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Medicare plan to be read from a form.
    /// </summary>
    public class FormMedicarePlan : FormInput, IMedicarePlan
    {
        /// <inheritdoc/>
        public bool Approved { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string OrganizationName { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string YearId { get; set; }

        /// <inheritdoc/>
        public override void SetId()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                this.Id = Guid.NewGuid().ToString();
            }
        }
    }
}
