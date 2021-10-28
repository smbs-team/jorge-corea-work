//-----------------------------------------------------------------------
// <copyright file="ErrorViewModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using System;

    /// <summary>
    /// The Error View Model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to the show request identifier.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}