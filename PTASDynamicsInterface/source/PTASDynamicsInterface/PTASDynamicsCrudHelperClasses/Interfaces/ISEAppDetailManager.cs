// <copyright file="ISEAppDetailManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic SEAppDetail manager.
    /// </summary>
    public interface ISEAppDetailManager
    {
        /// <summary>
        /// Get the ISEAppDetail Info that contains senior exemption application Id.
        /// </summary>
        /// <param name="sEAppId">Id to search.</param>
        /// <returns>SEAppDetails look up data or null.</returns>
        Task<List<DynamicsSeniorExemptionApplicationDetail>> GetSEAppDetailFromSEAppId(string sEAppId);
    }
}