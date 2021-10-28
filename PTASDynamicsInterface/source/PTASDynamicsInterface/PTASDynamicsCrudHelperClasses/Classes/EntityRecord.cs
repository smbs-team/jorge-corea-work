// <copyright file="EntityRecord.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// Entity Record.
    /// </summary>
    public class EntityRecord : EntityRequest
    {
        /// <summary>
        /// Gets or sets Residential Order.
        /// </summary>
        public int ResidentialOrder { get; set; }

        /// <summary>
        /// Gets or sets Non Residential Order .
        /// </summary>
        public int NonResidentialOrder { get; set; }
    }
}