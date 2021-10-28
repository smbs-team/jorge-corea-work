namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Type Management Helper.
    /// </summary>
    public static class TypeManagementHelper
    {
        /// <summary>
        /// Determines whether the type is a numeric type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is numeric.</returns>
        public static bool IsNumericType(Type type)
        {
            return IsDecimalType(type) || IsIntegerType(type);
        }

        /// <summary>
        /// Determines whether the type is an integer type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is an integer.</returns>
        public static bool IsIntegerType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the type is a decimal type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is decimal.</returns>
        public static bool IsDecimalType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
