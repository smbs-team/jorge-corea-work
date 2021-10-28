// <copyright file="ISEAppOtherProp.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// SEAppOccupant for crud.
    /// </summary>
    public interface ISEAppOtherProp
    {
        /// <summary>
        /// Gets or sets a value for SEAppOtherPropId.
        /// ptas_seappotherpropid.
        /// </summary>
        string SEAppOtherPropId { get; set; }

        /// <summary>
        /// Gets or sets a value for SEApplicationId.
        /// _ptas_seapplicationid_value.
        /// </summary>
        string SEApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        /// ptas_name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for Address.
        /// ptas_address.
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Gets or sets a value for Purpose.
        /// ptas_purpose.
        /// </summary>
        int? Purpose { get; set; }

        /// <summary>
        /// Gets or sets a value for CreatedOn.
        /// createdon.
        /// </summary>
        DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value for CreatedBy.
        /// _createdby_value.
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for ModifiedOn.
        /// modifiedon.
        /// </summary>
        DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets a value for ModifiedBy.
        /// _modifiedby_value.
        /// </summary>
        Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets county name.
        /// </summary>
        string CountyName { get; set; }

        /// <summary>
        /// Gets or sets state name.
        /// </summary>
        string StateName { get; set; }
    }
}
