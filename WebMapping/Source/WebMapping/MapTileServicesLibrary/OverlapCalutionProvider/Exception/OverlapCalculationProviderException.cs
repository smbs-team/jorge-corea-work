﻿namespace PTASMapTileServicesLibrary.OverlapCalutionProvider.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Different types of exceptions that OverlapCalculation provider can generate.
    /// </summary>
    public enum OverlapCalculationProviderExceptionCategory
    {
        /// <summary>
        /// There was an error reading data from SQL Server.
        /// </summary>
        SqlServerError = 0,

        /// <summary>
        /// Unkown error.
        /// </summary>
        Unkown = 1
    }

    /// <summary>
    /// Exception generated by the Overlap Calculation Provider.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OverlapCalculationProviderException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverlapCalculationProviderException" /> class.
        /// </summary>
        /// <param name="exceptionCategory">Category of the exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OverlapCalculationProviderException(OverlapCalculationProviderExceptionCategory exceptionCategory, string message, System.Exception innerException)
            : base(message, innerException)
        {
            this.OverlapCalculationProviderExceptionCategory = exceptionCategory;
        }

        /// <summary>
        /// Gets the category of the exception.
        /// </summary>
        public OverlapCalculationProviderExceptionCategory OverlapCalculationProviderExceptionCategory { get; }
    }
}
