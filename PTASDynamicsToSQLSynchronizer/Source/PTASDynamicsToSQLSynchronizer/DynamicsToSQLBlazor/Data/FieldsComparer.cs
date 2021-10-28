// <copyright file="FieldsComparer.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Compares fields on tables.
    /// </summary>
    public static class FieldsComparer
    {
        /// <summary>
        /// Get a list of new fields.
        /// </summary>
        /// <param name="src">Source table.</param>
        /// <param name="dest">Dest table.</param>
        /// <returns>List of new fields.</returns>
        public static IEnumerable<DBField> NewFields(DBTable src, DBTable dest)
        {
            // which ones are in src and not in dest.
            return src.Fields.Where(f => !dest.Fields.Any(f2 => f2.Name == f.Name)).ToArray();
        }

        /// <summary>
        /// List of fields that were deleted in the source.
        /// </summary>
        /// <param name="src">Source table.</param>
        /// <param name="dest">Dest table.</param>
        /// <returns>Deleted fields list.</returns>
        public static IEnumerable<DBField> DeletedFields(DBTable src, DBTable dest)
        {
            return NewFields(dest, src);
        }

        /// <summary>
        /// List of fields that have changed.
        /// </summary>
        /// <param name="src">Source table.</param>
        /// <param name="dest">Dest table.</param>
        /// <returns>Changed fields.</returns>
        public static IEnumerable<FieldDiff> ChangedFields(DBTable src, DBTable dest)
        {
            return src.Fields
                .Join(dest.Fields, f => f.Name, a => a.Name, (a, b) => (a, b))
                .Where(((DBField a, DBField b) r) => !r.a.Equals(r.b))
                .Select(((DBField src, DBField dest) r) => new FieldDiff(r.src, r.dest)).ToArray();
        }
    }
}
