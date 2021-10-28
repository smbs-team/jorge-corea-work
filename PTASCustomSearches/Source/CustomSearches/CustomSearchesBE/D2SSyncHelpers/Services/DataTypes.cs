// <copyright file="DataTypes.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Services
{
    /// <summary>
    /// DatatTypes for SQL.
    /// </summary>
    public class DataTypes
    {
        /// <summary>
        /// Bit.
        /// </summary>
        public const string Bit = "bit";

        /// <summary>
        /// Datetime.
        /// </summary>
        public const string Datetime = "datetime";

        /// <summary>
        /// DTOffset.
        /// </summary>
        public const string DtOffset = "datetimeoffset(7)";

        /// <summary>
        /// Money or decimal.
        /// </summary>
        public const string Money = "money";

        /// <summary>
        /// Float.
        /// </summary>
        public const string Float = "float";

        /// <summary>
        /// Guid.
        /// </summary>
        public const string UniqueIdentifier = "uniqueidentifier";

        /// <summary>
        /// Integer.
        /// </summary>
        public const string Int = "int";

        /// <summary>
        /// Long integer.
        /// </summary>
        public const string BigInt = "bigint";

        /// <summary>
        /// Varchar.
        /// </summary>
        public const string Nvarchar = "nvarchar(max)";

        /// <summary>
        /// Varbinary.
        /// </summary>
        public const string VarBinary = "varbinary(max)";
    }
}