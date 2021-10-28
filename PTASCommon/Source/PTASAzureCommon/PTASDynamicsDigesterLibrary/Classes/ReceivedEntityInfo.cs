// <copyright file="ReceivedEntityInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsDigesterLibrary.Classes
{
    using System;

    /// <summary>
    /// Received Entity Information.
    /// </summary>
    public class ReceivedEntityInfo
    {
        /// <summary>
        /// Gets or sets inputParameters.
        /// </summary>
        public InputParameters[] InputParameters { get; set; }

        /// <summary>
        /// Gets or sets messageName.
        /// </summary>
        public string MessageName { get; set; }

        /// <summary>
        /// Gets or sets primaryEntityId.
        /// </summary>
        public Guid PrimaryEntityId { get; set; }

        /// <summary>
        /// Gets or sets primaryEntityName.
        /// </summary>
        public string PrimaryEntityName { get; set; }

        /// <summary>
        /// Gets or sets organizationId.
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets organization Name.
        /// </summary>
        public string OrganizationName { get; set; }
    }
}