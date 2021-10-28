// <copyright file="ISEAppOtherPropManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic ISEAppOtherPropManager manager.
    /// </summary>
    public interface ISEAppOtherPropManager
    {
        /// <summary>
        /// Get the SEAppOccupant Info that contains sEAppId.
        /// </summary>
        /// <param name="sEAppId">Id to search.</param>
        /// <returns>SEAppOtherProp look up data or null.</returns>
        Task<List<DynamicsSEAppOtherProp>> GetSEAppOtherPropFromSEAppId(string sEAppId);
    }
}