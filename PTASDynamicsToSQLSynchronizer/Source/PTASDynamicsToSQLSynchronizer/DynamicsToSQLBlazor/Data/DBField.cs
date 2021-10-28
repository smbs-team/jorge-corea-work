// <copyright file="DBField.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a generic database field.
    /// </summary>
    public class DBField : IComparable
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Column("COLUMN_NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets datatype.
        /// </summary>
        [Column("DATA_TYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets maximum length.
        /// </summary>
        [Column("CHARACTER_MAXIMUM_LENGTH")]
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets numericPrecision.
        /// </summary>
        [Column("NUMERIC_PRECISION")]
        public int? NumericPrecision { get; set; }

        /// <summary>
        /// Gets or sets numericScale.
        /// </summary>
        [Column("NUMERIC_SCALE")]
        public int? NumericScale { get; set; }

        /// <summary>
        /// Compares to another object.
        /// </summary>
        /// <param name="dest">dest object to compare.</param>
        /// <returns>Compare results.</returns>
        public int CompareTo(object dest)
        {
            if (!(dest is DBField t))
            {
                return -1;
            }

            if (this.DataType.Equals(t.DataType))
            {
                return 0;
            }

            return 1;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{this.Name}: {this.DataType}";
    }
}
