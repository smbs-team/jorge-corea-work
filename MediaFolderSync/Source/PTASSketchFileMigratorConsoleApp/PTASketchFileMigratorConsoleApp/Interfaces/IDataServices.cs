// <copyright file="IDataServices.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASketchFileMigratorConsoleApp
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>Interface for data services class.</summary>
    public interface IDataServices
    {
        /// <summary>Sets a sketch to is official. </summary>
        /// <param name="data">Data containing the sketch id and entity id.</param>
        /// <returns>Task.</returns>
        Task<HttpResponseMessage> SetIsOfficial(Dictionary<string, string> data);
    }
}