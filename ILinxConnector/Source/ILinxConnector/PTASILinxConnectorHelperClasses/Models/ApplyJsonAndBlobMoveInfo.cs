// <copyright file="ApplyJsonAndBlobMoveInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
    /// <summary>
    /// Object for the combined move to ilinx and apply to dynamics.
    /// </summary>
    public class ApplyJsonAndBlobMoveInfo : BlobMoveInfo
    {
        /// <summary>
        /// Gets or sets route for the apply to dynamics.
        /// </summary>
        public string Route { get; set; }
    }
}