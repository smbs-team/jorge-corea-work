// <copyright file="IDataAccessLibrary.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Data access library interface.
    /// </summary>
    public interface IDataAccessLibrary
    {
        /// <summary>
        /// Attempts to load data.
        /// </summary>
        /// <typeparam name="TDataType">Type of data to load.</typeparam>
        /// <typeparam name="TParams">Parameters type for the operation.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">The parameters value.</param>
        /// <returns>A list of TDataType.</returns>
        Task<List<TDataType>> LoadData<TDataType, TParams>(string sql, TParams parameters);

        /// <summary>
        /// Attempts to apply sql to database.
        /// </summary>
        /// <typeparam name="TDataType">Type of data to save.</typeparam>
        /// <param name="sql">Query to execute.</param>
        /// <param name="parameters">Object param to save.</param>
        /// <returns>Empty task.</returns>
        Task<int> SaveData<TDataType>(string sql, TDataType parameters);
    }
}