// <copyright file="ISEAppPredefNote.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// ISEAppNote for crud.
    /// </summary>
    public interface ISEAppPredefNote
    {
        /// <summary>
        /// Gets or sets a value for SEAppPredefNotesId.
        /// ptas_seapppredefnotesid.
        /// </summary>
        string SEAppPredefNotesId { get; set; }

        /// <summary>
        /// Gets or sets a value for statecode.
        /// statecode.
        /// </summary>
        int? StateCode { get; set; }

        /// <summary>
        /// Gets or sets a value for statuscode.
        /// statuscode.
        /// </summary>
        int? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a value for Description.
        /// ptas_description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value for ShowOnPortal.
        /// ptas_showonportal.
        /// </summary>
        bool? ShowOnPortal { get; set; }

        /// <summary>
        /// Gets or sets a value for the Portal Attachment Location Option set.
        /// ptas_showonportal.
        /// </summary>
        int? PortalAttachmentLocation { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        /// ptas_name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for CreatedBy.
        /// _createdby_value.
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for createdon.
        /// createdon.
        /// </summary>
        DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value for ModifiedBy.
        /// _modifiedby_value.
        /// </summary>
        Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for ModifiedOn.
        /// modifiedon.
        /// </summary>
        DateTime? ModifiedOn { get; set; }
    }
}
