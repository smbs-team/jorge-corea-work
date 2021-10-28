// <copyright file="ISEAppOccupant.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// SEAppOccupant for crud.
    /// </summary>
    public interface ISEAppOccupant
    {
        /// <summary>
        /// Gets or sets a value for SEAppOccupantId.
        /// ptas_seappoccupantid.
        /// </summary>
        string SEAppOccupantId { get; set; }

        /// <summary>
        /// Gets or sets a value for SEApplicationId.
        /// _ptas_seapplicationid_value.
        /// </summary>
        string SEApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value for OccupantType.
        /// ptas_occupanttype.
        /// </summary>
        int? OccupantType { get; set; }

        /// <summary>
        /// Gets or sets a value for OccupantLastName.
        /// ptas_occupantlastname.
        /// </summary>
        string OccupantLastName { get; set; }

        /// <summary>
        /// Gets or sets a value for OccupantFirstName.
        /// ptas_occupantfirstname.
        /// </summary>
        string OccupantFirstName { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        /// ptas_name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for OccupantMiddleName.
        /// ptas_occupantmiddlename.
        /// </summary>
        string OccupantMiddleName { get; set; }

        /// <summary>
        /// Gets or sets a value for OccupantSuffix.
        /// ptas_occupantsuffix.
        /// </summary>
        string OccupantSuffix { get; set; }

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
    }
}
