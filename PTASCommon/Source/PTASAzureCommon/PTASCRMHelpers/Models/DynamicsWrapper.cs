﻿// <copyright file="DynamicsWrapper.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Models
{
    /// <summary>
    /// Dynamics wrapper, used to decode json.
    /// </summary>
    public class DynamicsWrapper
    {
        /// <summary>
        /// Gets or sets list of entity references.
        /// </summary>
        public EntityReference[] Value { get; set; }
    }
}
