// <copyright file="Extensions.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Static extensions.
    /// </summary>
    /// <typeparam name="T">Type of generic.</typeparam>
    public static class Extensions<T>
    {
        /// <summary>
        /// All elements but first one.
        /// </summary>
        /// <param name="src">Source list.</param>
        /// <returns>ALl but last.</returns>
        public static IEnumerable<T> ButFirst(IEnumerable<T> src) => src.Skip(1).ToList();
    }
}
