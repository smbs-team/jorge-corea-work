// <copyright file="ConvertFileResults.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Results returned after a file conversion.
    /// </summary>
    public class ConvertFileResults
    {
        /// <summary>
        /// Gets or sets array of arrays of bytes representing images.
        /// </summary>
        public IEnumerable<byte[]> Images { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are More Items.
        /// </summary>
        public bool MoreItems { get; set; }
    }
}