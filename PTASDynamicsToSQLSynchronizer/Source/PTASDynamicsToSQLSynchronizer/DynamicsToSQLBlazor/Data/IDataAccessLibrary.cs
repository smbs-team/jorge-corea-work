// <copyright file="IDataAccessLibrary.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataAccessLibrary
    {
        Task<List<TDataType>> LoadData<TDataType, TParams>(string sql, TParams parameters);

        Task SaveData<T>(string sql, T parameters);
    }
}