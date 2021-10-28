// <copyright file="ISEAppNoteManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic ISEAppNoteManager manager.
    /// </summary>
    public interface ISEAppNoteManager
    {
        /// <summary>
        /// Get the SEAppNote Info that contains sEAppId.
        /// </summary>
        /// <param name="sEAppId">Id to search.</param>
        /// <returns>SEAppNotes look up data or null.</returns>
        Task<List<DynamicsSEAppNote>> GetSEAppNoteFromSEAppId(string sEAppId);
    }
}