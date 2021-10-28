// <copyright file="ISEAppOccupantManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic ISEAppOccupantManager manager.
    /// </summary>
    public interface ISEAppOccupantManager
    {
        /// <summary>
        /// Get the SEAppOccupant Info that contains sEAppId.
        /// </summary>
        /// <param name="sEAppId">Id to search.</param>
        /// <returns>SEAppOccupants look up data or null.</returns>
        Task<List<DynamicsSEAppOccupant>> GetSEAppOccupantFromSEAppId(string sEAppId);
    }
}