// <copyright file="Helper.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector.Exceptions
{
    using System;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>Class that helps creating an complete SQL exception message.</summary>
    public static class Helper
    {
        /// <summary>  Builds a detailed SQL exception.</summary>
        /// <param name="ex">  The SQL exception object.</param>
        /// <returns>A string containing the error message.</returns>
        public static string SqlExceptionBuilder(SqlException ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "Error Number: " + ex.Errors[i].Number + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n");
            }

            return errorMessages.ToString();
        }
    }
}
