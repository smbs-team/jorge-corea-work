// <copyright file="FieldDiff.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Field difference.
    /// </summary>
    public class FieldDiff
    {
        private readonly DBField src;
        private readonly DBField dest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDiff"/> class.
        /// </summary>
        /// <param name="src">Source field.</param>
        /// <param name="dest">Dest field.</param>
        public FieldDiff(DBField src, DBField dest)
        {
            this.src = src;
            this.dest = dest;
        }
    }
}
