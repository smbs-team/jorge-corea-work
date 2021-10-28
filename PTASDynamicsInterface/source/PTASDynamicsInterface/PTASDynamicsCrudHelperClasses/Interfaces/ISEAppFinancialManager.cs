// <copyright file="ISEAppFinancialManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic ISEAppFinancialManager manager.
    /// </summary>
    public interface ISEAppFinancialManager
    {
        /// <summary>
        /// Get the Senior Application Financial Info that contains senior exemption application detail Id.
        /// </summary>
        /// <param name="sEAppDetailId">Id to search.</param>
        /// <returns>SEAppFinancials look up data or null.</returns>
        Task<List<DynamicsSeniorExemptionApplicationFinancial>> GetSEAppFinancialFromSEAppDetailId(string sEAppDetailId);
    }
}